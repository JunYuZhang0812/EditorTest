using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorStylesWindow : EditorWindow {

    [MenuItem("Window/EditorStyles", false, 100)]
    public static void ShowWindow(MenuCommand menuCommand)
    {
        var window = EditorWindow.GetWindow(typeof(EditorStylesWindow));
        window.titleContent.text = "EditorStyles";
        window.minSize = new Vector2(500, 200);
        window.Show();
    }

    void OnGUI()
	{
		EditorGUILayout.BeginVertical();

		EditorGUILayout.LabelField("样式：EditorStyles.label", EditorStyles.label);
        EditorGUILayout.LabelField("样式：EditorStyles.miniLabel", EditorStyles.miniLabel);
        EditorGUILayout.LabelField("样式：EditorStyles.largeLabel", EditorStyles.largeLabel);
        EditorGUILayout.LabelField("样式：EditorStyles.boldLabel", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("样式：EditorStyles.miniBoldLabel", EditorStyles.miniBoldLabel);
        EditorGUILayout.LabelField("样式：EditorStyles.centeredGreyMiniLabel", EditorStyles.centeredGreyMiniLabel);
        EditorGUILayout.LabelField("样式：EditorStyles.wordWrappedMiniLabel", EditorStyles.wordWrappedMiniLabel);
        EditorGUILayout.LabelField("样式：EditorStyles.wordWrappedLabel", EditorStyles.wordWrappedLabel);
        EditorGUILayout.LabelField("样式：EditorStyles.whiteLabel", EditorStyles.whiteLabel);
        EditorGUILayout.LabelField("样式：EditorStyles.whiteMiniLabel", EditorStyles.whiteMiniLabel);
        EditorGUILayout.LabelField("样式：EditorStyles.whiteLargeLabel", EditorStyles.whiteLargeLabel);
        EditorGUILayout.LabelField("样式：EditorStyles.whiteBoldLabel", EditorStyles.whiteBoldLabel);
        EditorGUILayout.LabelField("样式：EditorStyles.radioButton", EditorStyles.radioButton);
        EditorGUILayout.LabelField("样式：EditorStyles.miniButton", EditorStyles.miniButton);
        EditorGUILayout.LabelField("样式：EditorStyles.miniButtonLeft", EditorStyles.miniButtonLeft);
        EditorGUILayout.LabelField("样式：EditorStyles.miniButtonMid", EditorStyles.miniButtonMid);
        EditorGUILayout.LabelField("样式：EditorStyles.miniButtonRight", EditorStyles.miniButtonRight);
        EditorGUILayout.LabelField("样式：EditorStyles.textField", EditorStyles.textField);
        EditorGUILayout.LabelField("样式：EditorStyles.textArea", EditorStyles.textArea);
        EditorGUILayout.LabelField("样式：EditorStyles.miniTextField", EditorStyles.miniTextField);
        EditorGUILayout.LabelField("样式：EditorStyles.numberField", EditorStyles.numberField);
        EditorGUILayout.LabelField("样式：EditorStyles.popup", EditorStyles.popup);
        EditorGUILayout.LabelField("样式：EditorStyles.objectField", EditorStyles.objectField);
        EditorGUILayout.LabelField("样式：EditorStyles.objectFieldThumb", EditorStyles.objectFieldThumb);
        EditorGUILayout.LabelField("样式：EditorStyles.objectFieldMiniThumb", EditorStyles.objectFieldMiniThumb);
        EditorGUILayout.LabelField("样式：EditorStyles.colorField", EditorStyles.colorField);
        EditorGUILayout.LabelField("样式：EditorStyles.layerMaskField", EditorStyles.layerMaskField);
        EditorGUILayout.LabelField("样式：EditorStyles.toggle", EditorStyles.toggle);
        EditorGUILayout.LabelField("样式：EditorStyles.foldout", EditorStyles.foldout);
        EditorGUILayout.LabelField("样式：EditorStyles.foldoutPreDrop", EditorStyles.foldoutPreDrop);
        EditorGUILayout.LabelField("样式：EditorStyles.toggleGroup", EditorStyles.toggleGroup);
        EditorGUILayout.LabelField("样式：EditorStyles.toolbar", EditorStyles.toolbar);
        EditorGUILayout.LabelField("样式：EditorStyles.toolbarButton", EditorStyles.toolbarButton);
        EditorGUILayout.LabelField("样式：EditorStyles.toolbarPopup", EditorStyles.toolbarPopup);
        EditorGUILayout.LabelField("样式：EditorStyles.toolbarDropDown", EditorStyles.toolbarDropDown);
        EditorGUILayout.LabelField("样式：EditorStyles.toolbarTextField", EditorStyles.toolbarTextField);
        EditorGUILayout.LabelField("样式：EditorStyles.inspectorDefaultMargins", EditorStyles.inspectorDefaultMargins);
        EditorGUILayout.LabelField("样式：EditorStyles.inspectorFullWidthMargins", EditorStyles.inspectorFullWidthMargins);
        EditorGUILayout.LabelField("样式：EditorStyles.helpBox", EditorStyles.helpBox);


        EditorGUILayout.EndVertical();
	}
	
}
