using NodeEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class BTNodeEditorWindow: NodeEditorWindow
{
    [MenuItem("Window/行为树编辑器")]
    private static void OpenWindow()
    {
        BTNodeEditorWindow window = GetWindow<BTNodeEditorWindow>();
        window.titleContent = new GUIContent("节点编辑器");
    }

    #region 创建节点
    protected override void CreateEditorNode(ENodeType nodeType, Vector2 mousePosition)
    {
        CreateEditorNode(BTNodeManager.Instance.CreateNode(nodeType), mousePosition);
    }
    protected override EditorNode CreateEditorNode(Node node, Vector2 mousePosition)
    {
        if (node.NodeType == ENodeType.Root)
        {
            var rootNode = BTNodeManager.Instance.GetRootNode();
            if (rootNode != null)
            {
                if (rootNode != node)
                {
                    BTNodeManager.Instance.RemoveNode(node);
                    ShowNotification(new GUIContent("根节点只能有1个！"));
                    return null;
                }
            }
        }
        var editorNode = new BTEditorNode(mousePosition, 200, 50, m_nodeStyle, m_selectedNodeStyle, m_inPointStyle, m_outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);
        editorNode.InstanceNode = node as BTNode;
        m_nodes.Add(editorNode);
        var childs = node.Childs;
        if (childs.Count > 0)
        {
            for (int i = 0; i < childs.Count; i++)
            {
                var childNode = CreateEditorNode(childs[i], childs[i].Position);
                CreateConnection(childNode.m_inPoint, editorNode.m_outPoint);
            }
        }
        return editorNode;
    }
    #endregion

    #region 创建连线
    protected override void CreateConnection()
    {
        if (m_connections == null)
        {
            m_connections = new List<Connection>();
        }
        m_connections.Add(new BTConnection(m_selectedInPoint, m_selectedOutPoint, OnClickRemoveConnection));
    }
    protected override void CreateConnection(ConnectionPoint inPoint, ConnectionPoint outPoint)
    {
        if (m_connections == null)
        {
            m_connections = new List<Connection>();
        }
        m_connections.Add(new BTConnection(inPoint, outPoint, OnClickRemoveConnection));
    }
    #endregion

    #region 加载配置
    protected override void LoadNode()
    {
        var rootNode = LoadConfig(Common.BinarySerializer.DefaultBinPath + "/BTBin/BTNode.bin");
        if (rootNode != null)
        {
            CreateEditorNode(rootNode, rootNode.Position);
        }
    }
    protected override Node LoadConfig(string path)
    {
        BTNodeManager.Instance.Nodes.Clear();
        Node rootNode = null;
        if (File.Exists(path))
        {
            using (Stream configfs = File.Open(path, FileMode.Open))
            {
                using (var r = new BinaryReader(configfs))
                {
                    var nodeType = Common.BinarySerializer.Read_Int32(r);
                    rootNode = BTNodeManager.Instance.CreateNode((ENodeType)nodeType);
                    rootNode.Deserialize(r);
                }
            }
        }
        return rootNode;
    }
    #endregion
}
