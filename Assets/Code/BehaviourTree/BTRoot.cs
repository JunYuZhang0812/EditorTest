using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 根节点，其实就是拿选择节点改个类型，但是类型上要做区分，方便做工具以及根节点判断
/// </summary>
public class BTRoot : BTNode
{
    private int m_id;
    public int ID
    {
        get { return m_id; }
        set { m_id = value; }
    }
    public override EStatus Tick(float dt)
    {
        throw new NotImplementedException();
    }
}
