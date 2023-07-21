using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Common;

namespace NodeEditor
{
    public class Node
    {
        protected int m_index = -1;
        [NodeField("索引")]
        public int Index
        {
            get
            {
                return m_index;
            }
            set
            {
                if (value != m_index)
                {
                    if (Parent == null)
                    {
                        value = 0;
                        RawSetIndex(value);
                    }
                    else
                    {
                        var childs = Parent.Childs;
                        childs.Remove(this);
                        if (value < 0)
                        {
                            value = 0;
                        }
                        else if (value >= childs.Count)
                        {
                            value = childs.Count;
                        }
                        childs.Insert(value, this);
                        for (int i = 0; i < childs.Count; i++)
                        {
                            childs[i].RawSetIndex(i);
                        }
                    }
                }
            }
        }
        public ENodeType NodeType { get; set; }
        public int InstanceId { get; set; }
        public Vector2 Position { get; set; }
        private Node m_parent;
        public Node Parent
        {
            get
            {
                return m_parent;
            }
            set
            {
                m_parent = value;
                InitIndex();
            }
        }
        public List<Node> Childs { get; set; }

        public Node()
        {
            Childs = new List<Node>();
            InitIndex();
            NodeManager.Instance.AddNode(this);
        }

        private void InitIndex()
        {
            if (Parent == null)
            {
                Index = 0;
            }
            else
            {
                Index = Parent.Childs.Count;
            }
        }
        public virtual void Remove()
        {
            NodeManager.Instance.RemoveNode(this);
            if (Parent != null)
            {
                var childs = Parent.Childs;
                childs.Remove(this);
                for (int i = 0; i < childs.Count; i++)
                {
                    childs[i].RawSetIndex(i);
                }
            }
        }
        public virtual void RawSetIndex(int index)
        {
            m_index = index;
            InstanceId = NodeManager.Instance.GetInstanceId(this);
        }
        public virtual void Serialize(BinaryWriter w)
        {
            BinarySerializer.Write_Int32(w, (int)NodeType);
            BinarySerializer.Write_Int32(w, Index);
            BinarySerializer.Write_Vector2(w, Position);
            BinarySerializer.Write_Int32(w, Childs.Count);
            Childs.Sort((l, r) => { return l.Index.CompareTo(r.Index); });
            for (int i = 0; i < Childs.Count; i++)
            {
                Childs[i].Serialize(w);
            }
        }
        public virtual void Deserialize(BinaryReader r)
        {
            Index = BinarySerializer.Read_Int32(r);
            Position = BinarySerializer.Read_Vector2(r);
            var childCount = BinarySerializer.Read_Int32(r);
            for (int i = 0; i < childCount; i++)
            {
                var childNodeType = BinarySerializer.Read_Int32(r);
                var child = NodeManager.Instance.CreateNode((ENodeType)childNodeType);
                if (child != null)
                {
                    child.Parent = this;
                    child.Deserialize(r);
                    if (!Childs.Contains(child))
                    {
                        Childs.Add(child);
                    }
                }
            }
        }
    }
}

