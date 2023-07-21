/*using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NodeEditor;

[EditorNode("Type2")]
public class NodeType2 : Node
{
    [NodeField("文本")]
    public string Text { get; set; }
    [NodeField("EType", typeof(EType))]
    public int EType { get; set; }
    [NodeField("ENumber", typeof(ENumber))]
    public int ENumber { get; set; }
    public NodeType2() : base()
    {
        NodeType = ENodeType.NodeType2;
    }
    public override void Serialize(BinaryWriter w)
    {
        base.Serialize(w);
        Common.BinarySerializer.Write_String(w, Text);
        Common.BinarySerializer.Write_Int32(w, EType);
        Common.BinarySerializer.Write_Int32(w, ENumber);
    }
    public override void Deserialize(BinaryReader r)
    {
        base.Deserialize(r);
        Text = Common.BinarySerializer.Read_String(r);
        EType = Common.BinarySerializer.Read_Int32(r);
        ENumber = Common.BinarySerializer.Read_Int32(r);
    }
}*/
