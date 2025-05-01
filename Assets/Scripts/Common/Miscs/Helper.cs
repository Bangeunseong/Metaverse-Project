using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    public static T GetComponent_Helper<T>(GameObject obj) where T : Component
    {
        if(obj == null) { Debug.LogError("Fatal error! : GameObject is null!"); return null; }
        T component = obj.GetComponent<T>();
        if(component == null) { Debug.LogError($"Fatal error! : {typeof(T)} is missing in {obj.name}!"); return null; }
        return component;
    }

    public static T GetComponentInChildren_Helper<T>(GameObject obj) where T : Component
    {
        if (obj == null) { Debug.LogError("Fatal error! : GameObject is null!"); return null; }
        T component = obj.GetComponentInChildren<T>();
        if (component == null) { Debug.LogError($"Fatal error! : {typeof(T)} is missing in children of {obj.name}!"); return null; }
        return component;
    }

    public static T GetComponentInParent_Helper<T>(GameObject obj) where T : Component
    {
        if (obj == null) { Debug.LogError("Fatal error! : GameObject is null!"); return null; }
        T component = obj.GetComponentInParent<T>();
        if (component == null) { Debug.LogError($"Fatal error! : {typeof(T)} is missing in parent of {obj.name}!"); return null; }
        return component;
    }
}
