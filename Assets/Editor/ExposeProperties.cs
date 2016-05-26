using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;

public static class ExposeProperties
{
    public static void Expose(PropertyField[] properties)
    {
        var emptyOptions = new GUILayoutOption[0];
        EditorGUILayout.BeginVertical(emptyOptions);

        foreach(PropertyField field in properties)
        {
            EditorGUILayout.BeginHorizontal(emptyOptions);
            if(field.Type == SerializedPropertyType.Integer)
            {
                var oldValue = (int)field.Getvalue();
                var newValue = EditorGUILayout.IntField(field.Name, oldValue, emptyOptions);
                if (oldValue != newValue)
                    field.SetValue(newValue);
            }

            else if(field.Type == SerializedPropertyType.Float)
            {
                var oldValue = (float)field.Getvalue();
                var newValue = EditorGUILayout.FloatField(field.Name, oldValue, emptyOptions);
                if (oldValue != newValue)
                    field.SetValue(newValue);
            }

            else if (field.Type == SerializedPropertyType.Boolean)
            {
                var oldValue = (bool)field.Getvalue();
                var newValue = EditorGUILayout.Toggle(field.Name, oldValue, emptyOptions);
                if (oldValue != newValue)
                    field.SetValue(newValue);
            }

            else if (field.Type == SerializedPropertyType.String)
            {
                var oldValue = (string)field.Getvalue();
                var newValue = EditorGUILayout.TextField(field.Name, oldValue, emptyOptions);
                if (oldValue != newValue)
                    field.SetValue(newValue);
            }

            else if (field.Type == SerializedPropertyType.Vector2)
            {
                var oldValue = (Vector2)field.Getvalue();
                var newValue = EditorGUILayout.Vector2Field(field.Name, oldValue, emptyOptions);
                if (oldValue != newValue)
                    field.SetValue(newValue);
            }

            else if (field.Type == SerializedPropertyType.Vector3)
            {
                var oldValue = (Vector3)field.Getvalue();
                var newValue = EditorGUILayout.Vector3Field(field.Name, oldValue, emptyOptions);
                if (oldValue != newValue)
                    field.SetValue(newValue);
            }

            else if (field.Type == SerializedPropertyType.Enum)
            {
                var oldValue = (Enum)field.Getvalue();
                var newValue = EditorGUILayout.EnumPopup(field.Name, oldValue, emptyOptions);
                if (oldValue != newValue)
                    field.SetValue(newValue);
            }

            else if (field.Type == SerializedPropertyType.ObjectReference)
            {
                UnityEngine.Object oldValue = (UnityEngine.Object)field.Getvalue();
                var newValue = EditorGUILayout.ObjectField(field.Name, oldValue, field.Info.PropertyType, true,emptyOptions);
                if (oldValue != newValue)
                    field.SetValue(newValue);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    public static PropertyField[] GetProperties(object obj)
    {
        var fields = new List<PropertyField>();
        PropertyInfo[] infos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach(PropertyInfo info in infos)
        {
            if (!(info.CanRead && info.CanWrite)) //Da fuq
                continue;

            object[] attributes = info.GetCustomAttributes(true); //Da fuq

            bool isExposed = false;
            foreach(object o in attributes)
            {
                if(o.GetType() == typeof(ExposePropertyAttribute)) //Da fuq
                {
                    isExposed = true;
                    break;
                }
            }
            if(!isExposed)
                continue;

            var type = SerializedPropertyType.Integer;
            if(PropertyField.GetPropertyType(info, out type))
            {
                var field = new PropertyField(obj, info, type);
                fields.Add(field);
            }
        }
        return fields.ToArray();
    }
}

public class PropertyField
{
    object m_obj;
    PropertyInfo m_info;
    SerializedPropertyType m_type; //Da fuq is this

    MethodInfo getter; //Da fuq is this type
    MethodInfo setter;

    public PropertyInfo Info { get { return m_info; } }
    public SerializedPropertyType Type { get { return m_type; } }
    public string Name { get { return ObjectNames.NicifyVariableName(m_info.Name); } } //Da fuq this function do

    public PropertyField(object obj, PropertyInfo info, SerializedPropertyType type)
    {
        m_obj = obj;
        m_info = info;
        m_type = type;

        getter = m_info.GetGetMethod(); //Da fuq is this
        setter = m_info.GetSetMethod(); //Da fug is this
    }

    public object Getvalue()
    {
        return getter.Invoke(m_obj, null);
    }

    public void SetValue(object val)
    {
        setter.Invoke(m_obj, new[] { val });
    }

    public static bool GetPropertyType(PropertyInfo info, out SerializedPropertyType propertyType)
    {
        Type type = info.PropertyType;
        propertyType = SerializedPropertyType.Generic;

        if (type == typeof(int))
            propertyType = SerializedPropertyType.Integer;

        else if (type == typeof(float))
            propertyType = SerializedPropertyType.Float;

        else if (type == typeof(bool))
            propertyType = SerializedPropertyType.Boolean;

        else if (type == typeof(string))
            propertyType = SerializedPropertyType.String;

        else if (type == typeof(Vector2))
            propertyType = SerializedPropertyType.Vector2;

        else if (type == typeof(Vector3))
            propertyType = SerializedPropertyType.Vector3;

        else if (type.IsEnum)
            propertyType = SerializedPropertyType.Enum;

        else if (typeof(MonoBehaviour).IsAssignableFrom(type))
            propertyType = SerializedPropertyType.ObjectReference;

        return propertyType != SerializedPropertyType.Generic;
    }
}
