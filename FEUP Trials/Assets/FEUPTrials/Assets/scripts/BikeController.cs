using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeController : MonoBehaviour {
    private Rigidbody2D _bikeRB;
    private WheelJoint2D _frontWheel;
    private Rigidbody2D _frontWheelRB;
    private WheelJoint2D _backWheel;
    private Rigidbody2D _backWheelRB;

    public float maxForce = 500;
    // Acceleration (in force) per second
    public float forceAcceleration = 500;
    // Motor force
    public float motorForce;
    // Velocity at which the bike needs to be to brake
    public float velocityThereshold;

    // Angular drag value for when the bike is braking
    public float brakeForce;

    public float angularDrag;

    private bool _braking = false;

    private void Start()
    {
        _bikeRB = GetComponent<Rigidbody2D>();

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

        _frontWheelRB = _frontWheel.GetComponent<Rigidbody2D>();
        _frontWheelRB.angularDrag = angularDrag;
        _backWheelRB = _backWheel.GetComponent<Rigidbody2D>();
        _backWheelRB.angularDrag = angularDrag;

        _backWheel.useMotor = false;
    }
    
    private float DirectionalVelocity()
    {
        float velocity = Mathf.Cos(transform.rotation.z) * _bikeRB.velocity.x + Mathf.Sin(transform.rotation.z) * _bikeRB.velocity.y;
        return velocity;
    }

    private void Brake()
    {
        _braking = true;

        _backWheel.useMotor = false;
        motorForce = 0;
        
        _frontWheelRB.drag = brakeForce;
        _backWheelRB.drag = brakeForce;
    }

    private void StopBrake()
    {
        _braking = false;

        _backWheel.useMotor = true;

        _frontWheelRB.drag = angularDrag;
        _backWheelRB.drag = angularDrag;
    }

    private void UpdateMotorSpeed(float speed)
    {
        if (!_backWheel.useMotor)
            return;

        _frontWheelRB.drag = angularDrag;
        _backWheelRB.drag = angularDrag;

        JointMotor2D motor = _backWheel.motor;
        motor.motorSpeed = -speed;
        _backWheel.motor = motor;
    }

    void Update() {
        UpdateMotorForce();
        //UpdateRotation();
    }

    void UpdateMotorForce()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _backWheel.useMotor = true;

            if (_braking)
                StopBrake();

            // Move forward
            motorForce += forceAcceleration * Time.deltaTime;

            if (motorForce > maxForce)
                motorForce = maxForce;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _backWheel.useMotor = true;

            //Brake
            if (DirectionalVelocity() > velocityThereshold)
            {
                if (!_braking)
                    Brake();
            }
            else
            {
                if (_braking)
                    StopBrake();

                // Move backwards
                motorForce = -100;
            }
        }
        else
        {
            if (_braking)
                StopBrake();

            _backWheel.useMotor = false;
            motorForce = 0;
        }

        UpdateMotorSpeed(motorForce);
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
}
