using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;

public class CustomInspectorWindow : EditorWindow
{
    static List<ExposableMonobehavior> Components; //All script components attached to the object that inherit/ implement the class ExposableMonobehavior
    static List<PropertyField[]> ComponentFields; //All Properties/Fields of each Component attached to the object
    
    static List<bool> ActiveComponentsView; //Toggles for each component foldouts to be collapsed or expanded

    static Transform selectedObject; //Active item being viewed in the inspector

    /// <summary>
    /// Adds a new item to the Window tab with the name Custom Inspector
    /// Creates a new window if not window of this type is found and sets focus to it
    /// If a window is found of this type set focus to it
    /// </summary>
    [MenuItem("Window/Custom Inspector")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        CustomInspectorWindow window = (CustomInspectorWindow)EditorWindow.GetWindow(typeof(CustomInspectorWindow));
        window.Show(); //Shows the window
    }

    /// <summary>
    /// Checks to make sure the item clicked on in the Editor is a valid gameobject
    /// that can be viewed in the new inspector window. Called when Selection.activeTransform is changed.
    /// </summary>
    /// <param name="active">Transform selected we want to check if is valid to be displayed in our new inspector window</param>
    public static void GetActiveObjectComponents(Transform active)
    {
        ComponentFields = new List<PropertyField[]>(); //Blows out the ComponentFields list
        Components = new List<ExposableMonobehavior>(); //Blows out the Components list
        ActiveComponentsView = new List<bool>(); //Blows out the ActiveComponentsView list
        if (active != null) //If active is not null
        {
            if (active.GetComponent<RectTransform>() == null && active.gameObject != null) //If active does not have a RectTransform Component and active is a gameobject
            {
                selectedObject = active; ; //Sets selected object to the value of active
                foreach (ExposableMonobehavior c in selectedObject.GetComponents(typeof(ExposableMonobehavior))) //Loops through each Exposable Monobehavior in all components of type ExposableMonobehavior attached to our active gameobject
                {
                    Components.Add(c); //Adds the ExposableMonobehavior to out Components list
                    PropertyField[] fields = ExposeProperties.GetProperties(c); //Creates a new array of PropertyField with the properties of the components we are looking at
                    ComponentFields.Add(fields); //Adds the new array to the list of ComponentFields
                    ActiveComponentsView.Add(true); //Adds a new boolean to the ActiveComponentsView list of true so when the items load into the window the foldout containing all the information is expanded
                }
            }
        }
    }

    /// <summary>
    /// Called multiple times per-frame when the GUI is updated.
    /// Handles most of the drawing and format of the main Editor Window.
    /// </summary>
    void OnGUI()
    {
        Transform a = Selection.activeTransform; //Creates a new Transform object and set it equal to the value of the activeTransform from the Selection class
        if (a != selectedObject && Selection.activeGameObject != null) //If the new Transform object is not the same as the selectedObject and activeGameObject from the Selection class is not null
        {
            GetActiveObjectComponents(a); //Calls the GetActiveObjectComponents and passess the value of the new Transform created as an argument
        }

        if (selectedObject != null) //If selectedObject not null
        {
            if (a != selectedObject) //If the new Transform not equal to the selectedObject
            {
                GetActiveObjectComponents(a); //Calls the GetActiveObjectComponents and passess the value of the new Transform created as an argument
            }

            if(Components != null && ComponentFields != null) //If Components not null and ComponentFiels not null
            {
                if (Event.current.Equals(Event.KeyboardEvent("^left"))) //If keyboard event left control and left arrow are pressed at the same time
                    CollapseComponents(false); //Calls CollapseComponents and passes in false as the boolean argument
                if (Event.current.Equals(Event.KeyboardEvent("^right"))) //If keyboard event left control and right arrow are pressed at the same time
                    CollapseComponents(true); //Calls CollapseComponents and passes in true as the boolean argument

                if (GUI.Button(new Rect(10, position.height - 65, position.width / 2, 25), "Collapse")) //If the Collapse button is pressed
                    CollapseComponents(false); //Calls CollapseComponents and passes in false as the boolean argument
                if (GUI.Button(new Rect(position.width / 2, position.height - 65, (position.width / 2) - 10, 25), "Expand")) //If the Expand button is pressesd
                    CollapseComponents(true); //Calls CollapseComponents and passes in true as the boolean argument

                for (int i = 0; i < Components.Count; i++) //Iterates through indexes in the Components list.
                {
                    ActiveComponentsView[i] = EditorGUILayout.Foldout(ActiveComponentsView[i],Components[i].GetType().ToString()); //At the index of "i" we create a new Foldout UI element and set the value of ActiveComponentsView at the index of "i" to the state of the foldout(boolean value) 
                    if(ActiveComponentsView[i]) //If ActiveComponentsView at the index of "i" is true
                        ExposeProperties.Expose(ComponentFields[i]); //Calls the static function Expose from the ExposeProperties class and passes the arguement value of ComponentFields at the index of "i" 
                    GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) }); //Creates a GUI box as a divider to seperate the different components for readiblity. 
                }
            }

            if (GUI.Button(new Rect(10, position.height - 30, position.width - 20, 25), "Add Component")) //If the Add Component Button is pressed
            {
                ComponentSelection.CreateWindow(new Rect(position.x + 5, position.y + 50, position.width - 20 / 2, 25), ref selectedObject); //Calls the static CreateWindow fucntion from the ComponentSelection class and passes the argument values of the position we want to instantiate the new window at and a ref our selectedObject
            }
        }
    }

    /// <summary>
    /// Called when certian event triggers happen
    /// Event Triggers
    /// Left Control + Left Arrow
    /// Left Control + Right Arrow
    /// Collapse Button Click
    /// Expand Button Click
    /// </summary>
    /// <param name="state">State we want the component foldout to be in. (False = collapse, True = expand)</param>
    void CollapseComponents(bool state)
    {
        for (int i = 0; i < ActiveComponentsView.Count; i++) // Iterates through the values of the ActiveCompoentsView list
        {
            ActiveComponentsView[i] = state; //Sets the value at AcitiveComponentsView at the index of "i" to the value of the state passed in
        }
    }

    /// <summary>
    /// Called when the Inspector UI recives on update.
    /// </summary>
    void OnInspectorUpdate()
    {
        Repaint(); //Refreshes the window
    }
}
