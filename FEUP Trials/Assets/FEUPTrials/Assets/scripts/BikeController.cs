using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeController : MonoBehaviour {
    [SerializeField]
    private GameObject _frontWheel;

    [SerializeField]
    private GameObject _backWheel;

    public float MAX_SPEED = 1000;
    public float rotationSpeed;    

	void Update () {
        if(Input.GetKey(KeyCode.W))
        {
            if (rotationSpeed + 10f > MAX_SPEED)
                rotationSpeed = MAX_SPEED;
            else
                rotationSpeed += 10f;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            if (Mathf.Abs(rotationSpeed) + 10f > MAX_SPEED)
                rotationSpeed = -MAX_SPEED;
            else
                rotationSpeed -= 10f;
        }
        else if(rotationSpeed != 0)
        {
            if(Mathf.Abs(rotationSpeed) - 5f < 0)
                rotationSpeed = 0;
            else if (rotationSpeed > 0)
                rotationSpeed -= 5f;
            else if(rotationSpeed < 0)
                rotationSpeed += 5f;
        }

        _frontWheel.transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
        _backWheel.transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
    }
}
