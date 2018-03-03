using UnityEngine;

public class BikeController : MonoBehaviour
{
    private PlayerManager _playerManager;

    private Rigidbody2D _bikeRB;
    private WheelJoint2D _frontWheel;
    private Rigidbody2D _frontWheelRB;
    private WheelJoint2D _backWheel;
    private Rigidbody2D _backWheelRB;
    
    private bool _isActive = true;

    public AnimationCurve forceCurve;
    private float _accelerationTime = 0;
    
    // Acceleration (in force) per second
    public float forceAcceleration = 500;
    // Motor force
    public float motorForce;
    // Rotation force
    public float rotationForce;
    // Velocity at which the bike needs to be to brake
    public float velocityThereshold;

    // Angular drag value for when the bike is braking
    public float brakeForce;

    public float angularDrag;

    private bool _braking = false;

    private void Start()
    {
        // Get the player manager with the same 
        GameObject playerManagerGameObject = Utils.FilterTaggedObjectByParent("PlayerManager", transform.parent.name);
        if (playerManagerGameObject == null)
            Debug.LogError("Null player manager game object");

        _playerManager = playerManagerGameObject.GetComponent<PlayerManager>();
        if (_playerManager == null)
            Debug.LogError("Null player manager");

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

        _frontWheelRB = _frontWheel.connectedBody.gameObject.GetComponent<Rigidbody2D>();
        _frontWheelRB.angularDrag = angularDrag;
        _backWheelRB = _backWheel.connectedBody.gameObject.GetComponent<Rigidbody2D>();
        _backWheelRB.angularDrag = angularDrag;

        _backWheel.useMotor = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint" && collision.gameObject.layer == gameObject.layer)
        {
            Checkpoint c = collision.gameObject.GetComponent<Checkpoint>();
            if (c != null && !c.active)
            {
                _playerManager.SetCheckpoint(c);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Body")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
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
        
        _frontWheelRB.angularDrag = brakeForce;
        _backWheelRB.angularDrag = brakeForce;
    }

    private void StopBrake()
    {
        _braking = false;

        _backWheel.useMotor = true;

        _frontWheelRB.angularDrag = angularDrag;
        _backWheelRB.angularDrag = angularDrag;
    }

    private bool ActivateMotor()
    {
        return _backWheel.useMotor || !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S));
    }

    private void UpdateUseMotor()
    {
        if (ActivateMotor())
            _backWheel.useMotor = true;
        else if (_backWheel.useMotor)
            _backWheel.useMotor = false;
    }

    private void UpdateMotorSpeed(float speed)
    {
        if (!_backWheel.useMotor)
            return;

        _frontWheelRB.angularDrag = angularDrag;
        _backWheelRB.angularDrag = angularDrag;

        JointMotor2D motor = _backWheel.motor;
        motor.motorSpeed = -speed;
        _backWheel.motor = motor;
    }

    private void CalculateMotorForce()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (_braking)
                StopBrake();

            // Move forward
            _accelerationTime += Time.deltaTime;
            float force = forceCurve.Evaluate(_accelerationTime);
            motorForce = force * _backWheel.motor.maxMotorTorque;
        }
        else
        {
            _accelerationTime = 0;
            if (Input.GetKey(KeyCode.S))
            {
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
                motorForce = 0;
            }
        }
    }

    private void UpdateMotorForce()
    {
        UpdateUseMotor();
        CalculateMotorForce();
        UpdateMotorSpeed(motorForce);
    }

    private void UpdateRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            // Rotate with positive angle
            _bikeRB.AddTorque(Time.deltaTime * rotationForce, ForceMode2D.Impulse);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // Rotate with negative angle
            _bikeRB.AddTorque(-1 * Time.deltaTime * rotationForce, ForceMode2D.Impulse);
        }

    }

    private void ResetBikeValues()
    {
        _accelerationTime = 0;
        motorForce = 0;
        _braking = false;
        UpdateMotorSpeed(0f);
    }


    private void Update()
    {
        if (!_isActive)
            return; 
        
        UpdateMotorForce();
        UpdateRotation();
    }


    public void StopMotion()
    {
        _isActive = false;
        ResetBikeValues();
    }
}
