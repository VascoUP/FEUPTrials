using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerSetter : MonoBehaviour {
    [SerializeField]
    private string _sortingLayerName;

	// Use this for initialization
	void Start () {
        MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();
        if(mesh != null)
        {
            mesh.sortingLayerName = _sortingLayerName;
        }
        else
        {
            Debug.LogError("No mesh");
        }
	}
}
