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
    public float motorFoce;

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
    }

    void UpdateMotorForce()
    {
        // --
        // Remove this
        if (Input.GetKey(KeyCode.D))
        {
            // Move forward OR brake
        }
        else if (Input.GetKey(KeyCode.A))
        {
            // Move backwards OR brake
        }
        // --

        SetMotorSpeed(motorFoce);
    }

    // Negative speed should move the player backwards, but moves forward
    void SetMotorSpeed(float speed)
    {
        JointMotor2D motor = _backWheel.motor;
        motor.motorSpeed = speed;
        _backWheel.motor = motor;
    }
}
