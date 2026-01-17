using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class HelpfulScript : MonoBehaviour
{
    public string text;
    public bool isReadonly;
}

#if UNITY_EDITOR
[CustomEditor(typeof(HelpfulScript))]
public class HelpfulScriptEditor : Editor
{
    private HelpfulScript _targetHelpfulScript;

    private void OnEnable()
    {
        _targetHelpfulScript = (HelpfulScript)target;
    }

    public override void OnInspectorGUI()
    {
        GUI.enabled = false;
        MonoScript targetScript = MonoScript.FromMonoBehaviour((HelpfulScript)target);
        EditorGUILayout.ObjectField(new GUIContent("스크립트"), targetScript, typeof(HelpfulScript), false);
        GUI.enabled = true;

        serializedObject.Update();

        GUIStyle textFieldStyle = new GUIStyle(GUI.skin.textArea) { wordWrap = true };
        float textFieldHeight = textFieldStyle.CalcHeight(new GUIContent(_targetHelpfulScript.text), EditorGUIUtility.currentViewWidth);

        EditorGUILayout.BeginHorizontal();
        _targetHelpfulScript.isReadonly = EditorGUILayout.Toggle(new GUIContent("전체 선택 방지"), _targetHelpfulScript.isReadonly);
        
        if (_targetHelpfulScript.isReadonly == true)
            EditorGUILayout.LabelField(new GUIContent("텍스트 입력 불가"), EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();

        if (_targetHelpfulScript.isReadonly == false)
            _targetHelpfulScript.text = EditorGUILayout.TextArea(_targetHelpfulScript.text, textFieldStyle, GUILayout.Height(textFieldHeight));
        else EditorGUILayout.LabelField(_targetHelpfulScript.text, textFieldStyle, GUILayout.Height(textFieldHeight));

        serializedObject.ApplyModifiedProperties();
    }
}
#endif