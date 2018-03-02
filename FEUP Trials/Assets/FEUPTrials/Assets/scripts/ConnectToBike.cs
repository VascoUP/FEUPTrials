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
        GameObject gameMan = GameObject.Find("Game Manager");
        if (gameMan == null)
            return;

        GameManager gameManScript = gameMan.GetComponent<GameManager>();
        AddNewHingeJoint(gameManScript.activeBike);
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
