/*using System.IO;

namespace NodeEditor
{
    [EditorNode("RootNode")]
    public class RootNode : Node
    {
        [NodeField("名字")]
        public string Name { get; set; }

        public RootNode() : base()
        {
            NodeType = ENodeType.Root;
        }
        public override void Serialize(BinaryWriter w)
        {
            base.Serialize(w);
            Common.BinarySerializer.Write_String(w, Name);
        }
        public override void Deserialize(BinaryReader r)
        {
            base.Deserialize(r);
            Name = Common.BinarySerializer.Read_String(r);
        }
    }
}
*/
