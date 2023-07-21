/*using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NodeEditor;

[EditorNode("Type1")]
public class NodeType1 : Node
{
    [NodeField("开关")]
    public bool Toggle { get; set; }
    [NodeField("数字")]
    public int Number { get; set; }

    public NodeType1() : base()
    {
        NodeType = ENodeType.NodeType1;
    }
    public override void Serialize(BinaryWriter w)
    {
        base.Serialize(w);
        Common.BinarySerializer.Write_Boolean(w, Toggle);
        Common.BinarySerializer.Write_Int32(w, Number);
    }
    public override void Deserialize(BinaryReader r)
    {
        base.Deserialize(r);
        Toggle = Common.BinarySerializer.Read_Boolean(r);
        Number = Common.BinarySerializer.Read_Int32(r);
    }
}*/
