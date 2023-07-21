using UnityEngine;
using NodeEditor;

public abstract class BTNode:Node
{
    public abstract EStatus Tick(float dt);

    private EStatus m_runStatus = EStatus.None;
    public EStatus RunStatus{ get { return m_runStatus; } }
    public float m_lastRuneTime = 0f;
    public float LastRunTime { get { return m_lastRuneTime; } }

    protected EStatus SetRunResult(EStatus result)
    {
        m_lastRuneTime = Time.time;
        m_runStatus = result;
        return result;
    }
}

public class BTNodeManager:NodeManager
{
    public override Node CreateNode(ENodeType nodeType)
    {
        switch (nodeType)
        {
            default:
                break;
        }
        return null;
    }
}
