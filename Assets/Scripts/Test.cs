using UnityEngine;
using System.Collections;

public class Test : ExposableMonobehavior
{
    [HideInInspector, SerializeField] int m_someInt;
    [HideInInspector, SerializeField] float m_someFloat;
    [HideInInspector, SerializeField] bool m_someBool;
    [HideInInspector, SerializeField] string m_someString;
    [HideInInspector, SerializeField] MonoBehaviour m_Obj;

    [ExposeProperty]
    public int someInt
    {
        get { return m_someInt; }
        set { m_someInt = value; }
    }

    [ExposeProperty]
    public float someFloat
    {
        get { return m_someFloat; }
        set { m_someFloat = value; }
    }

    [ExposeProperty]
    public bool someBool
    {
        get { return m_someBool; }
        set { m_someBool = value; }
    }

    [ExposeProperty]
    public string someString
    {
        get { return m_someString; }
        set { m_someString = value; }
    }

    [ExposeProperty]
    public MonoBehaviour SomeScript
    {
        get { return m_Obj; }
        set { m_Obj = value; }
    }
}