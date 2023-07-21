using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NodeEditor
{
    public class Connection
    {
        public ConnectionPoint m_inPoint;
        public ConnectionPoint m_outPoint;
        public Action<Connection> OnClickRemoveConnection;

        public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> onClickRemoveConnection)
        {
            this.m_inPoint = inPoint;
            this.m_outPoint = outPoint;
            this.OnClickRemoveConnection = onClickRemoveConnection;
            OnCreate();
        }

        public virtual void OnCreate()
        {
            if (m_inPoint == null) return;
            if (m_outPoint != null)
            {
                var inNode = m_inPoint.m_node.InstanceNode;
                var outNode = m_outPoint.m_node.InstanceNode;
                if (inNode.Parent != outNode)
                {
                    inNode.Parent = outNode;
                }
                if (!outNode.Childs.Contains(inNode))
                {
                    outNode.Childs.Add(inNode);
                }
            }
            else
            {
                m_inPoint.m_node.InstanceNode.Parent = null;
            }
        }
        public virtual void OnRemove()
        {
            if (m_inPoint == null) return;
            if (m_outPoint != null)
            {
                var inNode = m_inPoint.m_node.InstanceNode;
                var outNode = m_outPoint.m_node.InstanceNode;
                inNode.Parent = null;
                outNode.Childs.Remove(inNode);
            }
            else
            {
                m_inPoint.m_node.InstanceNode.Parent = null;
            }
        }
        public virtual void Draw()
        {
            //绘制贝塞尔曲线（起始位置，结束位置，起始切线，终止切线，颜色，图片，宽度）
            Handles.DrawBezier(
                m_inPoint.m_rect.center,
                m_outPoint.m_rect.center,
                m_inPoint.m_rect.center + Vector2.left * 50f,
                m_outPoint.m_rect.center - Vector2.left * 50f,
                Color.white,
                null,
                2f);
            if (Handles.Button((m_inPoint.m_rect.center + m_outPoint.m_rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
            {
                if (OnClickRemoveConnection != null)
                {
                    OnClickRemoveConnection(this);
                }
            }
        }
    }
}
