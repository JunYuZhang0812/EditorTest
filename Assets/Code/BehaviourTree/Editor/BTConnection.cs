using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NodeEditor;
using UnityEditor;
using UnityEngine;

public class BTConnection : Connection
{
    public BTConnection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> onClickRemoveConnection):base(inPoint,outPoint,onClickRemoveConnection)
    {

    }
    public override void Draw()
    {
        var color = Color.white;
        if( Application.isPlaying && m_inPoint.m_node != null )
        {
            var node = m_inPoint.m_node.InstanceNode as BTNode;
            var lastRunTime = node.LastRunTime;
            if(lastRunTime > 0 && Time.time - lastRunTime <= 0.2 )
            {
                color = Color.green;
            }
        }
        //绘制贝塞尔曲线（起始位置，结束位置，起始切线，终止切线，颜色，图片，宽度）
        Handles.DrawBezier(
            m_inPoint.m_rect.center,
            m_outPoint.m_rect.center,
            m_inPoint.m_rect.center + Vector2.left * 50f,
            m_outPoint.m_rect.center - Vector2.left * 50f,
            color,
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
