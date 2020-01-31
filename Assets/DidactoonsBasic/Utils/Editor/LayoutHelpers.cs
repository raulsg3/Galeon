using UnityEngine;
using UnityEditor;
using System;


public static class LayoutHelpers{
    public static void EditorAddSpace(float numberOfSpace = 1){
        for (int i = 0; i < numberOfSpace; i++)
            EditorGUILayout.Space();
    }
} 
public class HorizontalBlock : System.IDisposable
{

    public HorizontalBlock(params GUILayoutOption[] options)
    {
        GUILayout.BeginHorizontal(options);
    }

    public HorizontalBlock(GUIStyle style, params GUILayoutOption[] options)
    {
        GUILayout.BeginHorizontal(style, options);
    }

    public void Dispose()
    {
        GUILayout.EndHorizontal();
    }
}
public class VerticalBlock : IDisposable
{
    public VerticalBlock(params GUILayoutOption[] options)
    {
        GUILayout.BeginVertical(options);
    }

    public VerticalBlock(GUIStyle style, params GUILayoutOption[] options)
    {
        GUILayout.BeginVertical(style, options);
    }

    public void Dispose()
    {
        GUILayout.EndVertical();
    }
}
