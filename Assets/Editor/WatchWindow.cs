using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class WatchWindow : EditorWindow
{
    List<string> PropertiesWatched;

    [MenuItem("Window/Watch")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        WatchWindow window = (WatchWindow)EditorWindow.GetWindow(typeof(WatchWindow));
        window.Show(); //Shows the window
    }

    void AddWatch(string prop)
    {
        Init();
        PropertiesWatched.Add(prop);
    }


}
