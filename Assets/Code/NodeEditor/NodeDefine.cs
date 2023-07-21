
using System;
namespace NodeEditor
{
    #region 枚举
    public enum ENodeType
	{
		/// <summary>
		/// 根节点
		/// </summary>
		Root,
		/// <summary>
		/// 选择节点
		/// </summary>
		Selector,
		/// <summary>
		/// 顺序节点
		/// </summary>
		Sequence,
		/// <summary>
		/// 修饰节点
		/// </summary>
		Decorator,
		/// <summary>
		/// 随机节点
		/// </summary>
		Random,
		/// <summary>
		/// 并行节点
		/// </summary>
		Parallel,
		/// <summary>
		/// 条件节点
		/// </summary>
		Condition,
		/// <summary>
		/// 行为节点
		/// </summary>
		Action,
	}
    #endregion

    #region 属性
    public class EditorNodeAttribute : Attribute
	{
		public string Name { get; set; }
		public EditorNodeAttribute(string name)
		{
			Name = name;
		}
	}
	public class NodeFieldAttribute : Attribute
	{
		public string Name { get; set; }
		public Type RelationType { get; set; }
		public NodeFieldAttribute(string name)
		{
			Name = name;
		}
		public NodeFieldAttribute(string name, Type type)
		{
			Name = name;
			RelationType = type;
		}
	}

	public class DescribeAttribute : Attribute
	{
		public string Des { get; set; }
		public DescribeAttribute(string des)
		{
			Des = des;
		}
	}
    #endregion
}