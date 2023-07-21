using System.Collections.Generic;

namespace NodeEditor
{
    public class NodeManager : Singleton<NodeManager>
    {
        protected List<Node> m_nodes = new List<Node>();
        public List<Node> Nodes
        {
            get
            {
                return m_nodes;
            }
        }
        public virtual int GetInstanceId(Node node)
        {
            var parentIndex = -1;
            if (node.Parent != null)
            {
                var id = node.Parent.InstanceId;
                parentIndex = (int)((id & 0xFFFF0000) >> 16);
            }
            parentIndex++;
            return ((int)node.NodeType << 32) + (parentIndex << 16) + node.Index;
        }
        public virtual Node CreateNode(ENodeType nodeType)
        {
            switch (nodeType)
            {
                /*case ENodeType.Root:
                    return new RootNode();
                case ENodeType.NodeType1:
                    return new NodeType1();
                case ENodeType.NodeType2:
                    return new NodeType2();*/
                default:
                    break;
            }
            return null;
        }
        public virtual Node GetRootNode()
        {
            for (int i = 0; i < m_nodes.Count; i++)
            {
                if (m_nodes[i].NodeType == ENodeType.Root)
                {
                    return m_nodes[i];
                }
            }
            return null;
        }
        public virtual void AddNode(Node node)
        {
            m_nodes.Add(node);
        }
        public virtual void RemoveNode(Node node)
        {
            m_nodes.Remove(node);
        }
    }
}
