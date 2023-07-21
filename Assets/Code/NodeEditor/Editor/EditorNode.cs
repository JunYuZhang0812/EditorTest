using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Text;

namespace NodeEditor
{
    public class EditorNode
    {
        protected Node m_node;
        public Node InstanceNode
        {
            get { return m_node; }
            set
            {
                m_node = value;
                InitInstanceNode();
            }
        }
        public static GUISkin Skin
        {
            get
            {
                return GUI.skin;
            }
        }
        public Rect m_rect;
        public bool m_isDragged;
        public ConnectionPoint m_inPoint;
        public ConnectionPoint m_outPoint;
        public bool m_isSelected;
        public GUIStyle m_style;
        public GUIStyle m_defaultNodeStyle;
        public GUIStyle m_selectedNodeStyle;
        public Action<EditorNode> OnRemoveNode;

        private float DEFAULT_HEIGHT = 60f;

        private Action<string> m_valueChange;
        protected event Action<string> ValueChange
        {
            add { m_valueChange += value; }
            remove { m_valueChange -= value; }
        }
        public EditorNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint, Action<EditorNode> onRemoveNode)
        {
            m_rect = new Rect(position.x, position.y, width, height);
            m_style = nodeStyle;
            m_inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, onClickInPoint);
            m_outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, onClickOutPoint);
            m_defaultNodeStyle = nodeStyle;
            m_selectedNodeStyle = selectedStyle;
            if (InstanceNode != null)
            {
                InstanceNode.Position = m_rect.position;
            }
            OnRemoveNode = onRemoveNode;
        }

        public void Drag(Vector2 delta)
        {
            m_rect.position += delta;
            if (InstanceNode != null)
            {
                InstanceNode.Position = m_rect.position;
            }
        }

        public void Draw()
        {
            m_inPoint.Draw();
            m_outPoint.Draw();
            DrawInstance();
        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    {
                        if (e.button == 0)
                        {
                            if (m_rect.Contains(e.mousePosition))
                            {
                                m_isDragged = true;
                                GUI.changed = true;
                                m_isSelected = true;
                                m_style = m_selectedNodeStyle;
                            }
                            else
                            {
                                GUI.changed = true;
                                m_isSelected = false;
                                m_style = m_defaultNodeStyle;
                            }
                        }
                        if (e.button == 1 && m_isSelected && m_rect.Contains(e.mousePosition))
                        {
                            ProcessContextMenu();
                            e.Use();
                        }
                    }
                    break;
                case EventType.MouseUp:
                    {
                        m_isDragged = false;
                    }
                    break;
                case EventType.MouseDrag:
                    {
                        if (e.button == 0 && m_isDragged)
                        {
                            Drag(e.delta);
                            e.Use();
                            return true;
                        }
                    }
                    break;
                default:
                    break;
            }
            return false;
        }
        #region 删除节点
        protected virtual void ProcessContextMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("删除节点"), false, OnClickRemoveNode);
            genericMenu.ShowAsContext();
        }
        private void OnClickRemoveNode()
        {
            if (OnRemoveNode != null)
            {
                OnRemoveNode(this);
            }
        }
        #endregion

        #region 外观
        protected virtual void DrawInstance()
        {
            var fontSize = EditorStyles.helpBox.fontSize;
            var alignment = EditorStyles.helpBox.alignment;
            var color = EditorStyles.helpBox.normal.textColor;
            EditorStyles.helpBox.alignment = TextAnchor.UpperCenter;
            EditorStyles.helpBox.fontSize = 12;
            EditorStyles.helpBox.normal.textColor = Color.white;
            var name = GetNodeName();
            var size = EditorStyles.helpBox.CalcSize(new GUIContent(name));
            m_rect.width = Math.Max(size.x + 40, m_rect.width);
            GUI.Box(m_rect, "", m_style);
            GUI.Box(new Rect(m_rect.x + 20, m_rect.y + 8, m_rect.width - 40, 20), name, EditorStyles.helpBox);
            EditorStyles.helpBox.fontSize = fontSize;
            EditorStyles.helpBox.alignment = alignment;
            EditorStyles.helpBox.normal.textColor = color;
            DrawProperty();
        }
        protected virtual void DrawProperty()
        {
            var propers = InstanceNode.GetType().GetProperties();
            if (propers != null && propers.Length != 0)
            {
                int index = 0;
                for (int i = 0; i < propers.Length; i++)
                {
                    var pro = propers[i];
                    if (!NodeEditorWindow.ShowIndex && pro.Name == "Index")
                    {
                        continue;
                    }
                    var tempRect = new Rect(m_rect.x + 20, m_rect.y + 40 + index * 20, m_rect.width / 2 - 16, 16);
                    var attr = Attribute.GetCustomAttribute(pro, typeof(NodeFieldAttribute), true) as NodeFieldAttribute;
                    if (attr != null)
                    {
                        index++;
                        GUI.Label(tempRect, attr.Name);
                        tempRect.x = m_rect.x + 8 + m_rect.width / 2 - 8;
                        DrawPropertyValue(tempRect, pro, attr);
                    }
                }
                m_rect.height = DEFAULT_HEIGHT + index * 20;
            }
        }
        private List<int> listIntValue = new List<int>();
        private StringBuilder builder = new StringBuilder();
        private string listStr = string.Empty;
        private void DrawPropertyValue(Rect rect, PropertyInfo pro, NodeFieldAttribute attr)
        {
            #region Bool类型
            if (pro.PropertyType == typeof(bool))
            {
                EditorGUI.BeginChangeCheck();
                pro.SetValue(InstanceNode, EditorGUI.Toggle(rect, (bool)pro.GetValue(InstanceNode, null)), null);
                if (EditorGUI.EndChangeCheck())
                {
                    if (m_valueChange != null)
                    {
                        m_valueChange(pro.Name);
                    }
                }
            }
            #endregion
            #region List<int>类型
            if (pro.PropertyType == typeof(List<int>))
            {
                EditorGUI.BeginChangeCheck();
                listIntValue = (List<int>)pro.GetValue(InstanceNode, null);
                for (int i = 0; i < listIntValue.Count; i++)
                {
                    builder.Append(listIntValue[i]).Append(",");
                }
                listStr = builder.ToString();
                builder.Clear();
                listStr = EditorGUI.TextField(rect, listStr);
                if (EditorGUI.EndChangeCheck())
                {
                    string[] array = listStr.Split(',');
                    listIntValue.Clear();
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(array[i]))
                        {
                            listIntValue.Add(int.Parse(array[i]));
                        }
                    }
                    pro.SetValue(InstanceNode, listIntValue, null);
                    if (m_valueChange != null)
                    {
                        m_valueChange(pro.Name);
                    }
                }
            }
            #endregion
            #region String类型
            if (pro.PropertyType == typeof(string))
            {
                EditorGUI.BeginChangeCheck();
                pro.SetValue(InstanceNode, EditorGUI.TextField(rect, (string)pro.GetValue(InstanceNode, null)), null);
                if (EditorGUI.EndChangeCheck())
                {
                    if (m_valueChange != null)
                    {
                        m_valueChange(pro.Name);
                    }
                }
            }
            #endregion
            #region Float类型
            if (pro.PropertyType == typeof(float))
            {
                EditorGUI.BeginChangeCheck();
                pro.SetValue(InstanceNode, EditorGUI.FloatField(rect, (float)pro.GetValue(InstanceNode, null)), null);
                if (EditorGUI.EndChangeCheck())
                {
                    if (m_valueChange != null)
                    {
                        m_valueChange(pro.Name);
                    }
                }
            }
            #endregion
            #region Int类型
            if (pro.PropertyType == typeof(int))
            {
                if (attr.RelationType != null)
                {
                    var type = attr.RelationType;
                    var list = new List<int>();
                    var strList = new List<string>();
                    if (type.IsEnum)
                    {
                        var fields = Enum.GetValues(type);
                        foreach (var item in fields)
                        {
                            var fieldName = Enum.GetName(type, item);
                            var field = type.GetField(fieldName);
                            var fieldAtt = Attribute.GetCustomAttribute(field, typeof(DescribeAttribute), true) as DescribeAttribute;
                            if (fieldAtt != null)
                            {
                                var fieldValue = field.GetValue(null);
                                var value = Convert.ToInt64(fieldValue);
                                if (value <= Int32.MaxValue && value >= Int32.MinValue)
                                {
                                    list.Add((int)value);
                                    strList.Add(fieldAtt.Des);
                                }
                            }
                        }
                    }
                    if (type.IsClass)
                    {
                        var fields = type.GetFields();
                        for (int i = 0; i < fields.Length; i++)
                        {
                            var field = fields[i];
                            var fieldAtt = Attribute.GetCustomAttribute(field, typeof(DescribeAttribute), true) as DescribeAttribute;
                            if (fieldAtt != null)
                            {
                                var fieldValue = field.GetValue(null);
                                var value = Convert.ToInt64(fieldValue);
                                if (value <= Int32.MaxValue && value >= Int32.MinValue)
                                {
                                    list.Add((int)value);
                                    strList.Add(fieldAtt.Des);
                                }
                            }
                        }
                    }
                    EditorGUI.BeginChangeCheck();
                    var proValue = (int)pro.GetValue(InstanceNode, null);
                    if (proValue <= Int32.MaxValue && proValue >= Int32.MinValue)
                    {
                        pro.SetValue(InstanceNode, EditorGUI.IntPopup(rect, proValue, strList.ToArray(), list.ToArray()), null);
                        if (EditorGUI.EndChangeCheck())
                        {
                            if (m_valueChange != null)
                            {
                                m_valueChange(pro.Name);
                            }
                        }
                    }
                    return;
                }
                EditorGUI.BeginChangeCheck();
                pro.SetValue(InstanceNode, EditorGUI.IntField(rect, (int)pro.GetValue(InstanceNode, null)), null);
                if (EditorGUI.EndChangeCheck())
                {
                    if (m_valueChange != null)
                    {
                        m_valueChange(pro.Name);
                    }
                }
            }
            #endregion
        }

        #endregion
        #region 获取属性
        protected virtual void InitInstanceNode()
        {
            InstanceNode.Position = m_rect.position;
            var propers = InstanceNode.GetType().GetProperties();
            if (propers != null)
            {
                for (int i = 0; i < propers.Length; i++)
                {
                    var pro = propers[i];
                    var att = Attribute.GetCustomAttribute(pro, typeof(NodeFieldAttribute), true) as NodeFieldAttribute;
                    if (att != null)
                    {
                        ValueChange += (s) =>
                        {
                            Debug.Log(string.Format("属性改变：{0} 值：{1}", s, pro.GetValue(InstanceNode, null)));
                        };
                    }
                }
            }
        }
        protected EditorNodeAttribute InstanceNodeAttribute
        {
            get
            {
                var attrs = InstanceNode.GetType().GetCustomAttributes(typeof(EditorNodeAttribute), false);
                if (attrs.Length > 0)
                {
                    var attr = attrs[0] as EditorNodeAttribute;
                    if (attr != null)
                    {
                        return attr;
                    }
                }
                return null;
            }
        }
        protected string GetNodeName()
        {
            return InstanceNodeAttribute.Name;
        }
        #endregion
    }
}
