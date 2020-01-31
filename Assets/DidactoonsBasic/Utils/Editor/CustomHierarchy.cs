using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[InitializeOnLoad]
public static class CustomHierarchy 
{
    private static Vector2 offsetVector = new Vector2(0, 0);
	
	static Texture2D UITexture;

    static CustomHierarchy (){
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

	private static int gameObjectNumber;
	private static Color darkGreyColor = new Color(0.22f,0.22f,0.22f);
	private static int rightSpotsUsed=0;

    private static void HandleHierarchyWindowItemOnGUI(int instanceID,Rect selectionRect){
		rightSpotsUsed = 0;
		GameObject obj = (GameObject) EditorUtility.InstanceIDToObject(instanceID);
		if(obj == null) {
			gameObjectNumber = 0;
			return;
		}
		gameObjectNumber++;

        // EditorGUI.DrawRect(GetRectForColumn(1,selectionRect),Color.red);
		// Rect offsetRect = new Rect(selectionRect.position + offsetVector, selectionRect.size);
		// offsetRect.x  = selectionRect.width;

		// if(gameObjectNumber % 2 == 0){
			
		// 	selectionRect.x = 0;
		// 	selectionRect.width = Screen.width;
		// 	//383838
		// 	EditorGUI.DrawRect(selectionRect,darkGreyColor);
		// }else{
		// 	// EditorGUI.DrawRect(selectionRect,Color.blue);
		// }
		// DrawName(GetOffsetedSelectionRect(selectionRect),new GUIContent(obj.name));
		// DrawToogleActive(GetRectForColumnFromLeft(0,selectionRect),obj);

		if( obj.GetComponent<Canvas>() != null){
			DrawMainComponentIcon(GetRectForColumnFromRight(rightSpotsUsed,selectionRect),EditorGUIUtility.IconContent("d_Canvas Icon"));
			rightSpotsUsed++;
		}

		if( obj.GetComponent<Animator>() != null){
			DrawMainComponentIcon(GetRectForColumnFromRight(rightSpotsUsed,selectionRect),EditorGUIUtility.IconContent("Animator Icon"));
			rightSpotsUsed++;
		}

		
    }

	private static Rect GetOffsetedSelectionRect( Rect selectionRect){
		return new Rect(selectionRect.position + offsetVector, selectionRect.size);
	}

	private static Rect GetRectForColumnFromRight(int index, Rect selectionRect){

		//In version 2018.3, Unity added a column at the right most side of the hierarchy to add the arrow for prefab
        #if UNITY_2018_3_OR_NEWER
        #else
            index++;
        #endif

		Rect offsetRect = GetOffsetedSelectionRect(selectionRect);

		float myRectPosX = offsetRect.x + offsetRect.width - 20 * index;
		float myRectPosY= offsetRect.y;
		float myRectPosWidth= 20;
		float myRectPosHeight= offsetRect.height;

		return new Rect(myRectPosX,myRectPosY,myRectPosWidth,myRectPosHeight);
	}

	private static Rect GetRectForColumnFromLeft(int index, Rect selectionRect){

		// //In version 2018.3, Unity added a column at the right most side of the hierarchy to add the arrow for prefab
        // #if UNITY_2018_3_OR_NEWER
        // #else
        //     index++;
        // #endif

		Rect offsetRect = GetOffsetedSelectionRect(selectionRect);

		float myRectPosX = 0 + 20 * index;
		float myRectPosY= offsetRect.y;
		float myRectPosWidth= 20;
		float myRectPosHeight= offsetRect.height;

		return new Rect(myRectPosX,myRectPosY,myRectPosWidth,myRectPosHeight);
	}

	private static void DrawMainComponentIcon(Rect posRect,GUIContent gUIContent){
		EditorGUI.LabelField(posRect, gUIContent);
	}

	private static void DrawBG(Rect posRect,GUIContent gUIContent){
		posRect.x += 15f;
		EditorGUI.LabelField(posRect, gUIContent);
	}

	private static void DrawName(Rect posRect,GUIContent gUIContent){
		posRect.x += 15f;
		EditorGUI.LabelField(posRect, gUIContent);
	}

	private static void DrawToogleActive(Rect posRect, GameObject go){
		bool toggleStatus = GUI.Toggle(posRect, go.activeSelf, "");
		Undo.RecordObject(go, "Active status change");
		go.SetActive(toggleStatus);
	}
}


// GUIContent gUIContent = EditorGUIUtility.IconContent("d_Canvas Icon");
			// EditorGUI.LabelField(offsetRect, gUIContent);
			// EditorGUI.LabelField(offsetRect, "", new GUIStyle()
			// {
			// 	style.normal.background = gUIContent;
			// 	normal = new GUIStyleState() { textColor = Color.blue },
			// 	fontStyle = FontStyle.Normal
			// }
			// );