using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;

public class CustomInspectorWindow : EditorWindow
{
    static ExposableMonobehavior m_Instance;
    static PropertyField[] m_fields;

    static Transform selectedObject;
    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Custom Inspector")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        CustomInspectorWindow window = (CustomInspectorWindow)EditorWindow.GetWindow(typeof(CustomInspectorWindow));
        window.Show();
    }

    bool addComponents = false;
    static List<bool> ActiveComponentsView;

    public static void GetActiveObjectComponents(Transform active)
    {
        ActiveComponentsView = new List<bool>();
        temps = new List<string>();
        if (active != null)
        {
            Debug.Log("passed");
            if (active.GetComponent<RectTransform>() == null && active.gameObject != null)
            {
                Debug.Log("hit");
                selectedObject = active;
                if (selectedObject.GetType().IsAssignableFrom(typeof(ExposableMonobehavior)))
                {
                    Debug.Log("Get Info");
                    m_Instance = selectedObject.GetComponent<ExposableMonobehavior>();
                    m_fields = ExposeProperties.GetProperties(m_Instance);
                    foreach (PropertyField p in m_fields)
                    {
                        temps.Add(p.Name);
                    }
                }
            }
            //if (active.GetComponent<RectTransform>() == null && active.gameObject != null)
            //{
            //    selectedObject = active;;
            //    foreach (Component c in selectedObject.GetComponents(typeof(Component)))
            //    {
            //        ActiveComponentsView.Add(false);
            //    }
            //}
        }
    }

    int t;
    static List<string> temps;
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
            t = EditorGUILayout.Popup(t, temps.ToArray());
            //if (ActiveComponentsView.Count != 0 || ActiveComponentsView != null)
            //{
            //    for (int i = 0; i < ActiveComponentsView.Count; i++)
            //    {
            //        ActiveComponentsView[i] = EditorGUILayout.Foldout(ActiveComponentsView[i], selectedObject.GetComponents(typeof(Component))[i].GetType().ToString());
            //        GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
            //    }
            //}

            //addComponents = GUI.Button(new Rect(10, position.height - 30, position.width - 20, 25), "Add Component");
            //if (addComponents)
            //{
            //    ComponentSelection.CreateWindow(new Rect(position.x + 5, position.y + 50, position.width - 20 / 2, 25), ref selectedObject);
            //}
        }
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }
}
