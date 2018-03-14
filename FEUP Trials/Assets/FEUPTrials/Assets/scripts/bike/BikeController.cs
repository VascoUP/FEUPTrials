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

    [SerializeField]
    private AnimationCurve _forceCurve;
    private float _accelerationTime = 0;

    [SerializeField]
    private float _motorBackwardsForce = 100;
    
    // Motor force
    private float _motorForce;
    // Rotation force
    [SerializeField]
    private float _rotationForce;
    // Velocity at which the bike needs to be to brake
    [SerializeField]
    private float _velocityThereshold;

    // Angular drag value for when the bike is braking
    [SerializeField]
    public float _brakeForce;

    [SerializeField]
    public float _angularDrag;

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
        _frontWheelRB.angularDrag = _angularDrag;
        _backWheelRB = _backWheel.connectedBody.gameObject.GetComponent<Rigidbody2D>();
        _backWheelRB.angularDrag = _angularDrag;

        _backWheel.useMotor = false;
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Body")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
    }

    
    public void FinishedGame(PlayerStats stats)
    {
        StopMotion();
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
        _motorForce = 0;
        
        _frontWheelRB.angularDrag = _brakeForce;
        _backWheelRB.angularDrag = _brakeForce;
    }

    private void StopBrake()
    {
        _braking = false;

        _backWheel.useMotor = true;

        _frontWheelRB.angularDrag = _angularDrag;
        _backWheelRB.angularDrag = _angularDrag;
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

        _frontWheelRB.angularDrag = _angularDrag;
        _backWheelRB.angularDrag = _angularDrag;

        JointMotor2D motor = _backWheel.motor;
        motor.motorSpeed = -speed;
        _backWheel.motor = motor;
    }

    private void CalculateMotorForce()
    {
        float value = InputManager.GetAxis(this.transform, "Horizontal");
        //float value = Input.GetAxis("Horizontal");
        if (value > 0)
        {
            if (_braking)
                StopBrake();

            // Move forward
            _accelerationTime += Time.deltaTime;
            float force = _forceCurve.Evaluate(_accelerationTime);
            _motorForce = force * value * _backWheel.motor.maxMotorTorque;
        }
        else
        {
            _accelerationTime = 0;
            if (value < 0)
            {
                //Brake
                if (DirectionalVelocity() > _velocityThereshold)
                {
                    if (!_braking)
                        Brake();
                }
                else
                {
                    if (_braking)
                        StopBrake();
                    // Move backwards
                    _motorForce = _motorBackwardsForce * value;
                }
            }
            else
            {
                if (_braking)
                    StopBrake();
                _motorForce = 0;
            }
        }
    }

    private void UpdateMotorForce()
    {
        UpdateUseMotor();
        CalculateMotorForce();
        UpdateMotorSpeed(_motorForce);
    }

    private void UpdateRotation()
    {
        float value = InputManager.GetAxis(this.transform, "Vertical");
        //float value = Input.GetAxis("Vertical");
        _bikeRB.AddTorque(value * Time.deltaTime * _rotationForce, ForceMode2D.Impulse);
    }

    private void ResetBikeValues()
    {
        _accelerationTime = 0;
        _motorForce = 0;
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
