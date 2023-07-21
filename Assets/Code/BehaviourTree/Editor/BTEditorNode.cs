using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditor;
using System;

public class BTEditorNode : EditorNode
{
    private new BTNode m_node;
    public new BTNode InstanceNode
    {
        get { return m_node; }
        set
        {
            m_node = value;
            InitInstanceNode();
        }
    }
    public BTEditorNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint, Action<EditorNode> onRemoveNode) : base(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, onClickInPoint, onClickOutPoint, onRemoveNode)
    {

    }

    protected override void DrawInstance()
    {
        base.DrawInstance();
        DrawRunResult();
    }
    protected void DrawRunResult()
    {
        switch (InstanceNode.RunStatus)
        {
            case EStatus.Success:
                {
                    GUI.Label(new Rect(m_rect.x, m_rect.y, 15, 15), "", Skin.GetStyle("WinBtnMaxMac"));
                }
                break;
            case EStatus.Failure:
                {
                    GUI.Label(new Rect(m_rect.x, m_rect.y, 15, 15), "", Skin.GetStyle("WinBtnCloseMac"));
                }
                break;
            case EStatus.Running:
                {
                    GUI.Label(new Rect(m_rect.x, m_rect.y, 15, 15), "", Skin.GetStyle("U2D.pivotDotActive"));
                }
                break;
            case EStatus.Break:
                {
                    GUI.Label(new Rect(m_rect.x, m_rect.y, 15, 15), "", Skin.GetStyle("WinBtnMinMac"));
                }
                break;
            case EStatus.Exit:
                {
                    GUI.Label(new Rect(m_rect.x, m_rect.y, 15, 15), "", Skin.GetStyle("SearchCancelButton"));
                }
                break;
            default:
                break;
        }
    }
}
