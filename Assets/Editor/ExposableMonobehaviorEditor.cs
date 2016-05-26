using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ExposableMonobehavior), true)] //Da fuq is the true for
public class ExposableMonobehaviorEditor : Editor
{
    ExposableMonobehavior m_Instance;
    PropertyField[] m_fields;
	
    public virtual void OnEnable()
    {
        m_Instance = target as ExposableMonobehavior;
        m_fields = ExposeProperties.GetProperties(m_Instance);
    }

    public override void OnInspectorGUI()
    {
        if (m_Instance == null)
            return;

        DrawDefaultInspector();
        ExposeProperties.Expose(m_fields);
    }
}
