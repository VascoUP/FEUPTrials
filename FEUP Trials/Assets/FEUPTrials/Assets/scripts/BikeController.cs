using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeController : MonoBehaviour {
    [SerializeField]
    private WheelJoint2D _frontWheel;

    [SerializeField]
    private WheelJoint2D _backWheel;

    public float maxSpeed = 1000;
    public float speed;

    private void Start()
    {
        WheelJoint2D[] wheelJoints = GetComponents<WheelJoint2D>();
        foreach(WheelJoint2D w in wheelJoints)
        {
            if (w.connectedBody.tag == "FrontWheel")
                _frontWheel = w;
            else if (w.connectedBody.tag == "BackWheel")
                _backWheel = w;
        }

        if (_backWheel == null || _frontWheel == null)
            Debug.LogError("Null wheel");
    }

    void Update () {
        // --
        // Remove this
        if(Input.GetKey(KeyCode.D))
        {
            if (speed + 10f > maxSpeed)
                speed = maxSpeed;
            else
                speed += 10f;
        }
        else if(Input.GetKey(KeyCode.A))
        {
            if (Mathf.Abs(speed) + 10f > maxSpeed)
                speed = -maxSpeed;
            else
                speed -= 10f;
        }
        else if(speed != 0)
        {
            if(Mathf.Abs(speed) - 5f < 0)
                speed = 0;
            else if (speed > 0)
                speed -= 5f;
            else if(speed < 0)
                speed += 5f;
        }
        // --

        SetMotorSpeed(speed);
    }

    void SetMotorSpeed(float speed)
    {
        JointMotor2D motor = _backWheel.motor;
        motor.motorSpeed = speed;
        _backWheel.motor = motor;
    }
}
