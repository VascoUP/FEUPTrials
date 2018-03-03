using UnityEngine;
using UnityEditor;

public class Utils
{
    public static GameObject FilterTaggedObjectByParent(string tag, string parentName)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject o in objects)
        {
            if (o.transform.parent.name == parentName)
            {
                return o;
            }
        }
        return null;
    }

    public static GameObject FilterTaggedObjectByParentAndName(string tag, string objectName, string parentName)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject o in objects)
        {
            if (o.transform.parent.name == parentName && o.gameObject.name == objectName)
            {
                return o;
            }
        }
        return null;
    }

    public static void SetLayer(Transform trans, int layer) {
        trans.gameObject.layer = layer;
        foreach (Transform child in trans)
            SetLayer(child, layer);
    }
}