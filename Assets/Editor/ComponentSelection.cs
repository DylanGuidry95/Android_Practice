using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;

public class ComponentSelection : EditorWindow
{
    static Transform Active;

    public static void CreateWindow(Rect p, ref Transform a)
    {
        ComponentSelection window = (ComponentSelection)EditorWindow.GetWindow(typeof(ComponentSelection));
        window.Show();
        window.position = p;
        Active = a;
        InstantiatePrimitive();
    }

    //Network Components
    static List<System.Type> NetworkComponents = new List<System.Type>();
    static List<string> NetworkComponentNames = new List<string>();
    static int networkSelectedIndex = 0;

    //Physics Components
    static List<System.Type> PhysicsComponents = new List<System.Type>();
    static List<string> PhysicsComponentNames = new List<string>();
    static int physicsSelectedIndex = 0;

    //Physics2D Components
    static List<System.Type> Physics2DComponents = new List<System.Type>();
    static List<string> Physics2DComponentNames = new List<string>();
    static int physics2DSelectedIndex = 0;

    //Mesh Components
    static List<System.Type> MeshComponents = new List<System.Type>();
    static List<string> MeshComponentNames = new List<string>();
    static List<bool> MeshSelectedComponents = new List<bool>();
    static bool MeshToggle = true;

    //Navigation Components
    static List<System.Type> NavigationComponents = new List<System.Type>();
    static List<string> NavigationComponentNames = new List<string>();
    static int navigationSelectedIndex = 0;

    //Audio Components
    static List<System.Type> AudioComponents = new List<System.Type>();
    static List<string> AudioComponentNames = new List<string>();
    static int audioSelectedIndex = 0;

    //Script Components
    static List<System.Type> ScriptComponents = new List<System.Type>();
    static List<string> ScriptComponentNames = new List<string>();
    static List<bool> ScriptSelectedComponents = new List<bool>();
    static bool ScriptToggle = true;

    void OnGUI()
    {
        GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
        //Mesh Components
        MeshToggle = EditorGUILayout.Foldout(MeshToggle, "Mesh Components");
        EditorGUILayout.BeginVertical();
        if (MeshToggle)
        {
            foreach (string t in MeshComponentNames)
            {
                MeshSelectedComponents[MeshComponentNames.IndexOf(t)] = EditorGUILayout.Toggle(t, MeshSelectedComponents[MeshComponentNames.IndexOf(t)]);
            }
        }
        GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });

        //Script Components
        ScriptToggle = EditorGUILayout.Foldout(ScriptToggle, "Script Components");
        if (ScriptToggle)
        {
            EditorGUILayout.LabelField("Num of elements", ScriptComponents.Count.ToString());
            EditorGUILayout.LabelField("Num of bools", ScriptComponents.Count.ToString());
            EditorGUILayout.LabelField("Num of names", ScriptComponents.Count.ToString());
            NameSpace = EditorGUILayout.DelayedTextField("Scripts in namespace", NameSpace);
            GetScripts(NameSpace);
            if (ScriptComponentNames.Count > 0)
            {
                foreach (string t in ScriptComponentNames)
                {
                    ScriptSelectedComponents[ScriptComponentNames.IndexOf(t)] = EditorGUILayout.Toggle(t, ScriptSelectedComponents[ScriptComponentNames.IndexOf(t)]);
                    if (ScriptSelectedComponents[ScriptComponentNames.IndexOf(t)] == true && Active.GetComponent(ScriptComponents[ScriptComponentNames.IndexOf(t)]) == null)
                    {
                        Active.gameObject.AddComponent(ScriptComponents[ScriptComponentNames.IndexOf(t)]);
                        CustomInspectorWindow.GetActiveObjectComponents(Active);
                    }
                    else if (Active.GetComponent(ScriptComponents[ScriptComponentNames.IndexOf(t)]) != null && ScriptSelectedComponents[ScriptComponentNames.IndexOf(t)] == false)
                    {
                        SafeDestroy(Active.GetComponent(ScriptComponents[ScriptComponentNames.IndexOf(t)]));
                        CustomInspectorWindow.GetActiveObjectComponents(Active);
                    }
                }
            }
        }
        GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
        EditorGUILayout.EndVertical();
        Repaint();
    }

    bool stop = false;
    string NameSpace = "";
    string CheckNameSpace = "";
    T SafeDestroy<T>(T obj) where T : Component
    {
        if (Application.isEditor)
            Component.DestroyImmediate(obj);
        else
            Component.Destroy(obj);

        return null;
    }

    void ComponentViews(List<bool> b, List<string> n, List<System.Type> c, bool viewable)
    {
        GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });

        //Script Components
        viewable = EditorGUILayout.Foldout(viewable, "Script Components");
        if (viewable)
        {
            //meshSelectedIndex = EditorGUILayout.Popup(meshSelectedIndex, MeshComponentNames.ToArray());
            foreach (string t in n)
            {
                b[n.IndexOf(t)] = EditorGUILayout.Toggle(t, b[n.IndexOf(t)]);
            }
        }
        GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
    }

    static void InstantiatePrimitive()
    {
        foreach (System.Type T in GetAllSubTypes(typeof(Component)))
        {
            //Network
            if (T.IsSubclassOf(typeof(NetworkBehaviour)) || T != typeof(NetworkScenePostProcess) && T != typeof(UnityEngine.Networking.Match.NetworkMatch) && T.Name.Contains("Network") && T != typeof(NetworkBehaviour))
            {
                NetworkComponents.Add(T);
                NetworkComponentNames.Add(T.Name);
            }

            //Ridgidbody
            if (T.IsSubclassOf(typeof(Collider)) || T.IsSubclassOf(typeof(Joint)) || T == typeof(Rigidbody) || T == typeof(ConstantForce) || T == typeof(Cloth))
            {
                PhysicsComponents.Add(T);
                PhysicsComponentNames.Add(T.Name);
            }

            //Mesh
            if (T == typeof(MeshFilter) || T == typeof(MeshRenderer) || T == typeof(TextMesh) || T == typeof(SkinnedMeshRenderer))
            {
                MeshComponents.Add(T);
                MeshComponentNames.Add(T.Name);
                if(Active.GetComponent(T) != null)
                    MeshSelectedComponents.Add(true);
                else
                    MeshSelectedComponents.Add(false);
            }

            //Physics2D 
            if (T.IsSubclassOf(typeof(Collider2D)) || T.IsSubclassOf(typeof(Joint2D)) || T == typeof(Rigidbody2D) || T == typeof(ConstantForce2D) || T.IsSubclassOf(typeof(Effector2D)))
            {
                Physics2DComponents.Add(T);
                Physics2DComponentNames.Add(T.Name);
            }

            //Navigation
            if(T == typeof(NavMeshAgent) || T == typeof(NavMeshObstacle) || T == typeof(OffMeshLink))
            {
                NavigationComponents.Add(T);
                NavigationComponentNames.Add(T.Name);
            }


            //Audio
            if(T == typeof(AudioSource) || T == typeof(AudioListener))
            {
                AudioComponents.Add(T);
                AudioComponentNames.Add(T.Name);
            }
        }
    }

    void GetScripts(string n)
    {
        ////Script
        if (GetAllSubTypes(typeof(ExposableMonobehavior)).Length == 0)
        {
            Debug.Log("hit");
            ScriptComponents = new List<System.Type>();
            ScriptComponentNames = new List<string>();
            ScriptSelectedComponents = new List<bool>();
            CheckNameSpace = NameSpace;
        }

        foreach (var T in GetAllSubTypes(typeof(ExposableMonobehavior)))
        {
            if(!ScriptComponents.Contains(T))
            {
                Debug.Log(T.BaseType + " " + T.Name);
                ScriptComponents.Add(T);
                ScriptComponentNames.Add(T.Name);
                if (Active.GetComponent(T) != null)
                    ScriptSelectedComponents.Add(true);
                else
                    ScriptSelectedComponents.Add(false);
            }
        }
    }

    static System.Type[] GetAllSubTypes(System.Type aBaseClass)
    {
        List<System.Type> result = new List<System.Type>(); //Creates a new list of types we will return this value
        System.Reflection.Assembly[] AS = System.AppDomain.CurrentDomain.GetAssemblies(); //Generates a new array of all assemblies in our project
        foreach (System.Reflection.Assembly A in AS) //Loops through all of the assemblies in the array we created
        {
            System.Type[] types = A.GetTypes(); //Foreach item in the array we get all of the types associated with item and add them to an Array of types
            foreach (System.Type T in types) //Loop through all of the types
            {
                if (T.IsSubclassOf(aBaseClass)) //If it is a subclass of the class passed in as an argument
                    result.Add(T); //Add it to the list of types we are returning. In this case it is the variable result
            }
        }
        return result.ToArray(); //Returns result in the form of an array so it matches the return type of the function
    }
}
