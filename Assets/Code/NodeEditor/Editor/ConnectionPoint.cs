using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace NodeEditor
{
    public enum ConnectionPointType
    {
        In,
        Out
    }
    public class ConnectionPoint
    {
        public Rect m_rect;
        public ConnectionPointType m_type;
        public EditorNode m_node;
        public GUIStyle m_style;
        public Action<ConnectionPoint> OnClickConnectionPoint;

        public ConnectionPoint(EditorNode node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> onClickConnectionPoint)
        {
            this.m_node = node;
            this.m_type = type;
            this.m_style = style;
            this.OnClickConnectionPoint = onClickConnectionPoint;
            m_rect = new Rect(0, 0, 10f, 20f);
        }

        public virtual void Draw()
        {
            m_rect.y = m_node.m_rect.y + (m_node.m_rect.height * 0.5f) - m_rect.height * 0.5f;
            switch (m_type)
            {
                case ConnectionPointType.In:
                    m_rect.x = m_node.m_rect.x - m_rect.width + 8f;
                    break;
                case ConnectionPointType.Out:
                    m_rect.x = m_node.m_rect.x + m_node.m_rect.width - 8f;
                    break;
                default:
                    break;
            }
            if (GUI.Button(m_rect, "", m_style))
            {
                if (OnClickConnectionPoint != null)
                {
                    OnClickConnectionPoint(this);
                }
            }
        }
    }
}
