using NodeEditor;
//运行状态
public enum EStatus
{
    [Describe("成功")]
    Success,
    [Describe("失败")]
    Failure,
    [Describe("运行中")]
    Running,
    [Describe("打断")]
    Break,
    [Describe("退出")]
    Exit,
    [Describe("未运行")]
    None,
};
