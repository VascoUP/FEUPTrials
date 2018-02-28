using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerArmScript : MonoBehaviour {

    public bool addHinge = true;

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
        addHinge = false;
        HingeJoint2D hj = gameObject.AddComponent<HingeJoint2D>();
        hj.connectedBody = bike.GetComponent<Rigidbody2D>();
        hj.autoConfigureConnectedAnchor = true;
        hj.anchor = new Vector2(0.398f, 0.104f);
        hj.connectedAnchor = new Vector2(1.218f, 0.764f);
    }
}
