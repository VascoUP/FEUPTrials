using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectToBike : MonoBehaviour {
    [SerializeField]
    private Vector2 _anchor;
    [SerializeField]
    private Vector2 _connectedAnchor;
    [SerializeField]
    private float _breakForce;
    [SerializeField]
    private float _breakTorque;

    private void Start()
    {
        GameObject playerManagerGameObject = Utils.FilterTaggedObjectByParent("PlayerManager", transform.parent.parent.name);
        if (playerManagerGameObject == null)
            Debug.LogError("Null player manager game object");

        PlayerManager playerManager = playerManagerGameObject.GetComponent<PlayerManager>();
        if (playerManager == null)
            Debug.LogError("Null player manager");
        
        AddNewHingeJoint(playerManager.activeBike);
    }

    private void AddNewHingeJoint(GameObject bike)
    {
        HingeJoint2D hj = gameObject.AddComponent<HingeJoint2D>();
        hj.connectedBody = bike.GetComponent<Rigidbody2D>();
        hj.autoConfigureConnectedAnchor = true;
        hj.anchor = new Vector2(_anchor.x, _anchor.y);
        hj.connectedAnchor = new Vector2(_connectedAnchor.x, _connectedAnchor.y);
        hj.breakForce = _breakForce;
        hj.breakTorque = _breakTorque;
    }
}
