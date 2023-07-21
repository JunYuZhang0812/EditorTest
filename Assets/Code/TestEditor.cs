using UnityEditor;
using UnityEngine;

class TestEditorWindow : EditorWindow
{
    [MenuItem("Window/TestWindow")]
    private static void OpenWindow()
    {
        TestEditorWindow window = GetWindow<TestEditorWindow>();
        window.titleContent = new GUIContent("测试窗口");
    }
    void OnEnable()
    {
        Selection.selectionChanged += Repaint;
    }
    void OnDisable()
    {
        Selection.selectionChanged -= Repaint;
    }
    void OnGUI()
    {
        //EditorGUILayout.ObjectField(Selection.activeObject, typeof(UnityEngine.Object),false);
        if( GUILayout.Button("解析"))
        {
            Parse();
        }
    }

    void Parse()
    {

    }
}

class TestEditorWindow2:EditorWindow
{
    [MenuItem("Window/TestWindow2")]
    private static void OpenWindow()
    {
        TestEditorWindow2 window = GetWindow<TestEditorWindow2>();
        window.titleContent = new GUIContent("测试窗口2");
    }
    void OnGUI()
    {
        if( GUILayout.Button("测试") )
        {
            var window = EditorWindow.GetWindow(typeof(TestEditorWindow));
            window.Repaint();
        }
    }
}