using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class CustomInspectorWindow : EditorWindow
{
    GameObject selectedObject;
    
    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Custom Inspector")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        CustomInspectorWindow window = (CustomInspectorWindow)EditorWindow.GetWindow(typeof(CustomInspectorWindow));
        window.Show();
    }

    bool addComponents = false;

    void OnGUI()
    {
        selectedObject = Selection.activeGameObject;
        if (selectedObject != null)
        {
            EditorGUILayout.BeginVertical("Button");
            addComponents = GUI.Button(new Rect(10, 20, position.width - 20, 25), "Add Component");
            if (addComponents)
            {
                ComponentSelection.CreateWindow(new Rect(position.x + 5, position.y + 50, position.width - 20 / 2, 25), ref selectedObject);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("Active");
            GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });

            EditorGUILayout.EndVertical();
        }
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }
}
