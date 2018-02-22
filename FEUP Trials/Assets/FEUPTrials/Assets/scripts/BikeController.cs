using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeController : MonoBehaviour {
    [SerializeField]
    private WheelJoint2D _frontWheel;
    [SerializeField]
    private WheelJoint2D _backWheel;

    public float maxSpeed = 1000;
    // Acceleration (in force) per second
    public float forceAcceleration = 500;
    // Motor force
    public float motorForce;

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
        UpdateMotorForce();
        //UpdateRotation();
    }

    void UpdateMotorForce()
    {
        if (Input.GetKey(KeyCode.W))
        {
            // Move forward OR brake
            motorForce += forceAcceleration * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            // Move backwards OR brake
            motorForce -= forceAcceleration * Time.deltaTime;
        }
        /*else
        {
            float signal = (motorForce / Mathf.Abs(motorForce));
            float force = Mathf.Abs(motorForce) - forceAcceleration;
            if((force >= 0 && motorForce < 0) || (force <= 0 && motorForce > 0))
            {
                force = 0;
            }
            else
            {
                motorForce = signal * force;
            }
        }*/

        SetMotorSpeed(motorForce);
    }

    void UpdateRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            // Rotate with positive angle
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // Rotate with negative angle
        }

    }

    // Negative speed should move the player backwards, but moves forward
    void SetMotorSpeed(float speed)
    {
        JointMotor2D motor = _backWheel.motor;
        motor.motorSpeed = speed;
        _backWheel.motor = motor;
    }
}
