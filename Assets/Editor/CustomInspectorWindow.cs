using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class CustomInspectorWindow : EditorWindow
{
    static GameObject selectedObject;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Custom Inspector")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        CustomInspectorWindow window = (CustomInspectorWindow)EditorWindow.GetWindow(typeof(CustomInspectorWindow));
        window.Show();
    }


    void OnGUI()
    {
        int i = 0;

        selectedObject = Selection.activeGameObject;
        if (selectedObject != null)
        {
            EditorGUILayout.BeginVertical("Add Button");
            selectedIndex = EditorGUILayout.Popup(selectedIndex, Files.ToArray());
            if (GUI.Button(new Rect(10, 20, position.width - 20, 25), "Add Component"))
                InstantiatePrimitive();
            EditorGUILayout.EndVertical();
        }
    }

    List<string> Files = new List<string>();
    List<string> AssetPaths = new List<string>();

    int selectedIndex = 0;

    public void InstantiatePrimitive()
    {
        foreach (string file in Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Assets\Scripts\", "*.cs", SearchOption.AllDirectories))
        {
            string[] temp = file.Split('\\');
            if (temp[temp.Length - 1].Contains(".cs") && !temp[temp.Length - 1].Contains(".meta"))
            {
                Files.Add(temp[temp.Length - 1]);
                AssetPaths.Add(file);
            }
        }


        Component newComp = new Component();
        newComp.name = Files[selectedIndex];
        selectedObject.AddComponent(newComp.GetType());
        Debug.Log(Files[selectedIndex].GetType());
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }
}
