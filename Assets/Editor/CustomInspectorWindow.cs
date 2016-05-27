using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;

public class CustomInspectorWindow : EditorWindow
{
    static List<ExposableMonobehavior> Components;
    static List<PropertyField[]> ComponentFields;
    
    static List<bool> ActiveComponentsView;

    bool addComponents = false;
    static Transform selectedObject;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Custom Inspector")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        CustomInspectorWindow window = (CustomInspectorWindow)EditorWindow.GetWindow(typeof(CustomInspectorWindow));
        window.Show();
    }

    public static void GetActiveObjectComponents(Transform active)
    {
        ComponentFields = new List<PropertyField[]>();
        Components = new List<ExposableMonobehavior>();
        ActiveComponentsView = new List<bool>();
        if (active != null)
        {
            if (active.GetComponent<RectTransform>() == null && active.gameObject != null)
            {
                selectedObject = active; ;
                foreach (ExposableMonobehavior c in selectedObject.GetComponents(typeof(ExposableMonobehavior)))
                {
                    Components.Add(c);
                    PropertyField[] fields = ExposeProperties.GetProperties(c);
                    ComponentFields.Add(fields);
                    ActiveComponentsView.Add(true);
                }
            }
        }
    }

    void OnGUI()
    {
        Transform a = Selection.activeTransform;
        if (a != selectedObject && Selection.activeGameObject != null)
        {
            GetActiveObjectComponents(a);
        }

        if (selectedObject != null)
        {
            if (a != selectedObject)
            {
                GetActiveObjectComponents(a);
            }

            if(Components != null && ComponentFields != null)
            {
                if (Event.current.Equals(Event.KeyboardEvent("^left")))
                    CollapseComponents(false);
                if (Event.current.Equals(Event.KeyboardEvent("^right")))
                    CollapseComponents(true);

                if (GUI.Button(new Rect(10, position.height - 65, position.width / 2, 25), "Collapse"))
                {
                    CollapseComponents(false);
                }
                if (GUI.Button(new Rect(position.width / 2, position.height - 65, (position.width / 2) - 10, 25), "Expand"))
                {
                    CollapseComponents(true);
                }

                    for (int i = 0; i < Components.Count; i++)
                {
                    ActiveComponentsView[i] = EditorGUILayout.Foldout(ActiveComponentsView[i],Components[i].GetType().ToString());
                    if(ActiveComponentsView[i])
                        ExposeProperties.Expose(ComponentFields[i]);
                    GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
                }
            }

            addComponents = GUI.Button(new Rect(10, position.height - 30, position.width - 20, 25), "Add Component");
            if (addComponents)
            {
                ComponentSelection.CreateWindow(new Rect(position.x + 5, position.y + 50, position.width - 20 / 2, 25), ref selectedObject);
            }
        }
    }

    void CollapseComponents(bool state)
    {
        for (int i = 0; i < ActiveComponentsView.Count; i++)
        {
            ActiveComponentsView[i] = state;
        }
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }
}
