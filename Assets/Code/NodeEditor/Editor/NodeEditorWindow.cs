using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace NodeEditor
{
    public class NodeEditorWindow : EditorWindow
    {
        [MenuItem("Window/Node Editor")]
        private static void OpenWindow()
        {
            NodeEditorWindow window = GetWindow<NodeEditorWindow>();
            window.titleContent = new GUIContent("节点编辑器");
        }

        protected List<EditorNode> m_nodes = new List<EditorNode>();
        protected List<Connection> m_connections;
        protected GUIStyle m_nodeStyle;
        protected GUIStyle m_selectedNodeStyle;
        protected GUIStyle m_inPointStyle;
        protected GUIStyle m_outPointStyle;
        protected ConnectionPoint m_selectedInPoint;
        protected ConnectionPoint m_selectedOutPoint;

        protected Vector2 m_offset;
        protected Vector2 m_drag;

        protected virtual void OnEnable()
        {
            m_nodeStyle = new GUIStyle();
            m_nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            m_nodeStyle.border = new RectOffset(12, 12, 12, 12);
            m_selectedNodeStyle = new GUIStyle();
            m_selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
            m_selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);
            m_inPointStyle = new GUIStyle();
            m_inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
            m_inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
            m_inPointStyle.border = new RectOffset(4, 4, 12, 12);
            m_outPointStyle = new GUIStyle();
            m_outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
            m_outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
            m_outPointStyle.border = new RectOffset(4, 4, 12, 12);

            LoadNode();
        }
        protected virtual void OnDisable()
        {
            m_nodes.Clear();
            ShowIndex = false;
            NodeManager.Instance.Nodes.Clear();
        }
        protected virtual void OnGUI()
        {
            DrawGrid();
            DrawTopBar();
            DrawNodes();
            DrawConnections();
            DrawConnectionLine(Event.current);
            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);
            if (GUI.changed)
            {
                Repaint();
            }
        }
        #region 字段定义
        public static GUISkin Skin
        {
            get
            {
                return GUI.skin;
            }
        }
        #endregion
        #region 点击事件
        protected virtual void ProcessEvents(Event e)
        {
            m_drag = Vector2.zero;
            switch (e.type)
            {
                case EventType.MouseDown:
                    {
                        if (e.button == 0)
                        {
                            ClearConnectionSelection();
                        }
                        //点击右键弹出菜单
                        if (e.button == 1)
                        {
                            ProcessContextMenu(e.mousePosition);
                        }
                    }
                    break;
                case EventType.MouseDrag:
                    {
                        if (e.button == 0)
                        {
                            OnDrag(e.delta);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        protected virtual void ProcessNodeEvents(Event e)
        {
            for (int i = m_nodes.Count - 1; i >= 0; i--)
            {
                bool guiChanged = m_nodes[i].ProcessEvents(e);
                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }
        #endregion
        #region 绘制节点
        protected virtual void DrawNodes()
        {
            for (int i = 0; i < m_nodes.Count; i++)
            {
                m_nodes[i].Draw();
            }
        }
        #endregion
        #region 绘制连线
        protected virtual void DrawConnections()
        {
            if (m_connections != null)
            {
                for (int i = 0; i < m_connections.Count; i++)
                {
                    m_connections[i].Draw();
                }
            }
        }
        #endregion
        #region 绘制右键菜单
        protected virtual void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("添加根节点"), false, () => OnClickAddRootNode(mousePosition));
            genericMenu.ShowAsContext();
        }
        private void OnClickAddRootNode(Vector2 mousePosition)
        {
            CreateEditorNode(ENodeType.Root, mousePosition);
        }
        protected virtual void OnClickInPoint(ConnectionPoint inPoint)
        {
            m_selectedInPoint = inPoint;
            if (m_selectedOutPoint != null)
            {
                if (m_selectedOutPoint.m_node != m_selectedInPoint.m_node)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }
        protected virtual void OnClickOutPoint(ConnectionPoint outPoint)
        {
            m_selectedOutPoint = outPoint;
            if (m_selectedInPoint != null)
            {
                if (m_selectedInPoint.m_node != m_selectedOutPoint.m_node)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
            }
        }
        protected virtual void OnClickRemoveNode(EditorNode node)
        {
            node.InstanceNode.Remove();
            if (m_connections != null)
            {
                List<Connection> connectionsToRemove = new List<Connection>();
                for (int i = 0; i < m_connections.Count; i++)
                {
                    if (m_connections[i].m_inPoint == node.m_inPoint || m_connections[i].m_outPoint == node.m_outPoint)
                    {
                        connectionsToRemove.Add(m_connections[i]);
                    }
                }
                for (int i = 0; i < connectionsToRemove.Count; i++)
                {
                    connectionsToRemove[i].OnRemove();
                    m_connections.Remove(connectionsToRemove[i]);
                }
            }
            m_nodes.Remove(node);
        }
        protected virtual void OnClickRemoveConnection(Connection connection)
        {
            connection.OnRemove();
            m_connections.Remove(connection);
        }
        #endregion
        #region 拖动画布
        protected virtual void OnDrag(Vector2 delta)
        {
            m_drag = delta;
            for (int i = 0; i < m_nodes.Count; i++)
            {
                m_nodes[i].Drag(delta);
            }
            GUI.changed = true;
        }
        #endregion
        #region 从连接点绘制贝塞尔曲线到鼠标位置
        protected virtual void DrawConnectionLine(Event e)
        {
            if (m_selectedInPoint != null && m_selectedOutPoint == null)
            {
                Handles.DrawBezier(m_selectedInPoint.m_rect.center,
                    e.mousePosition,
                    m_selectedInPoint.m_rect.center + Vector2.left * 50f,
                    e.mousePosition - Vector2.left * 50f,
                    Color.white,
                    null,
                    2f);
                GUI.changed = true;
            }
            if (m_selectedOutPoint != null && m_selectedInPoint == null)
            {
                Handles.DrawBezier(m_selectedOutPoint.m_rect.center,
                    e.mousePosition,
                    m_selectedOutPoint.m_rect.center - Vector2.left * 50f,
                    e.mousePosition + Vector2.left * 50f,
                    Color.white,
                    null,
                    2f);
                GUI.changed = true;
            }
        }
        #endregion
        #region 绘制网格
        protected virtual void DrawGrid()
        {
            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);
        }
        protected virtual void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);
            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);
            m_offset += m_drag * 0.5f;
            Vector3 newOffset = new Vector3(m_offset.x % gridSpacing, m_offset.y % gridSpacing, 0);
            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset,
                    new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }
            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset,
                    new Vector3(position.width, gridSpacing * j, 0) + newOffset);
            }
            Handles.color = Color.white;
            Handles.EndGUI();
        }
        #endregion
        #region 绘制顶条
        protected const float TOP_WIDTH = 280.0f;
        protected const float TOP_HEIGHT = 18.0f;
        public static bool ShowIndex { get; set; }
        public Rect TopBarArea { get; set; }
        protected virtual void DrawTopBar()
        {
            GUILayout.Box("", Skin.GetStyle("Toolbar"), GUILayout.Width(TOP_WIDTH), GUILayout.Height(TOP_HEIGHT));
            if (Event.current.type == EventType.Repaint)
            {
                if (TopBarArea != GUILayoutUtility.GetLastRect())
                {
                    TopBarArea = GUILayoutUtility.GetLastRect();
                }
            }
            GUILayout.BeginArea(TopBarArea);
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("保存", EditorStyles.toolbarButton, GUILayout.Width(60), GUILayout.ExpandHeight(true)))
                {
                    SaveConfig();
                }
                ShowIndex = GUILayout.Toggle(ShowIndex, "显示索引", EditorStyles.toolbarButton, GUILayout.Width(60));
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }
        #endregion

        #region 创建节点
        protected virtual void CreateEditorNode(ENodeType nodeType, Vector2 mousePosition)
        {
            CreateEditorNode(NodeManager.Instance.CreateNode(nodeType), mousePosition);
        }
        protected virtual EditorNode CreateEditorNode(Node node, Vector2 mousePosition)
        {
            if (node.NodeType == ENodeType.Root)
            {
                var rootNode = NodeManager.Instance.GetRootNode();
                if(rootNode!= null)
                {
                    if (rootNode != node)
                    {
                        NodeManager.Instance.RemoveNode(node);
                        ShowNotification(new GUIContent("根节点只能有1个！"));
                        return null;
                    }
                }
            }
            var editorNode = new EditorNode(mousePosition, 200, 50, m_nodeStyle, m_selectedNodeStyle, m_inPointStyle, m_outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);
            editorNode.InstanceNode = node;
            m_nodes.Add(editorNode);
            var childs = node.Childs;
            if (childs.Count > 0)
            {
                for (int i = 0; i < childs.Count; i++)
                {
                    var childNode = CreateEditorNode(childs[i], childs[i].Position);
                    CreateConnection(childNode.m_inPoint, editorNode.m_outPoint);
                }
            }
            return editorNode;
        }
        #endregion
        #region 创建连线
        protected virtual void CreateConnection()
        {
            if (m_connections == null)
            {
                m_connections = new List<Connection>();
            }
            m_connections.Add(new Connection(m_selectedInPoint, m_selectedOutPoint, OnClickRemoveConnection));
        }
        protected virtual void CreateConnection(ConnectionPoint inPoint, ConnectionPoint outPoint)
        {
            if (m_connections == null)
            {
                m_connections = new List<Connection>();
            }
            m_connections.Add(new Connection(inPoint, outPoint, OnClickRemoveConnection));
        }
        protected virtual void ClearConnectionSelection()
        {
            m_selectedInPoint = null;
            m_selectedOutPoint = null;
        }
        #endregion
        #region 加载配置

        protected virtual void LoadNode()
        {
            var rootNode = LoadConfig(Common.BinarySerializer.DefaultBinPath + "//Node.bin");
            if (rootNode != null)
            {
                CreateEditorNode(rootNode, rootNode.Position);
            }
        }
        protected virtual void SaveConfig()
        {
            var allNode = NodeManager.Instance.Nodes;
            var rootNode = NodeManager.Instance.GetRootNode();
            if (rootNode == null)
            {
                ShowNotification(new GUIContent("必须有根节点！"));
                return;
            }
            var binPath = Common.BinarySerializer.DefaultBinPath + "//Node.bin";
            if (File.Exists(binPath))
            {
                File.Delete(binPath);
            }
            var bw = new BinaryWriter(new FileStream(binPath, FileMode.Create));
            rootNode.Serialize(bw);
            bw.Close();
        }
        protected virtual Node LoadConfig(string path)
        {
            NodeManager.Instance.Nodes.Clear();
            Node rootNode = null;
            if (File.Exists(path))
            {
                using (Stream configfs = File.Open(path, FileMode.Open))
                {
                    using (var r = new BinaryReader(configfs))
                    {
                        var nodeType = Common.BinarySerializer.Read_Int32(r);
                        rootNode = NodeManager.Instance.CreateNode((ENodeType)nodeType);
                        rootNode.Deserialize(r);
                    }
                }
            }
            return rootNode;
        }
        #endregion
    }
}
