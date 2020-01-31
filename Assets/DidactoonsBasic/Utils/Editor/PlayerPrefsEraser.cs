using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PlayerPrefsEraser : EditorWindow
{

    [MenuItem("Didactoons/Clear PlayerPrefs")]
    public static void clearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}