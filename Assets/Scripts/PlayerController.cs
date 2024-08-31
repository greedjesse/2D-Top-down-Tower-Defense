using ScriptableObjects;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Components
    
    private Camera _camera;
    private Rigidbody2D _rb;

    #endregion

    #region Timers

    private float _time;

    #endregion
    
    private void Awake()
    {
        _camera = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _currentWeapon = 1;

        _bullet1ToConsume = false;
        _bullet2ToConsume = false;
        _timeBullet1WasFired = 0;
        _timeBullet2WasFired = 0;
    }

    void Update()
    {
        _time += Time.deltaTime;
        
        GatherInputs();
        
        HandleWeapons();
        HandleBullets();
    }

    #region Inputs

    private FrameInputs _inputs;
    
    private void GatherInputs()
    {
        _inputs.OneDown = Input.GetKeyDown(KeyCode.Alpha1);
        _inputs.TwoDown = Input.GetKeyDown(KeyCode.Alpha2);
        _inputs.LeftMouseDown = Input.GetMouseButtonDown(0);
        _inputs.LeftMouseHeld = Input.GetMouseButton(0);
    }

    #endregion

    #region Weapon

    [SerializeField] private WeaponStats weaponStats1;
    [SerializeField] private WeaponStats weaponStats2;
    
    private int _currentWeapon;
    
    private void HandleWeapons()
    {
        if (_inputs.OneDown)
        {
            _currentWeapon = 1;
        }

        if (_inputs.TwoDown)
        {
            _currentWeapon = 2;
        }
    }

    #endregion

    #region Bullets

    [SerializeField] private GameObject bullet1;
    [SerializeField] private GameObject bullet2;
    
    private float _timeBullet1WasFired;
    private float _timeBullet2WasFired;

    private bool _bullet1ToConsume;
    private bool _bullet2ToConsume;

    private bool CanFireBullet1 => _currentWeapon == 1 && _time > _timeBullet1WasFired + weaponStats1.baseTimeBetweenShots;
    private bool CanFireBullet2 => _currentWeapon == 2 && _time > _timeBullet2WasFired + weaponStats2.baseTimeBetweenShots;
    
    private void HandleBullets()
    {
        if (_inputs.LeftMouseHeld)
        {
            if (_currentWeapon == 1)
            {
                _bullet1ToConsume = true;
            }

            if (_currentWeapon == 2)
            {
                _bullet2ToConsume = true;
            }
        }
        
        HandleBullet1();
        HandleBullet2();
    }

    private void HandleBullet1()
    {
        if (CanFireBullet1 && _bullet1ToConsume) FireBullet1();
        _bullet1ToConsume = false;
    }
    
    private void HandleBullet2()
    {
        if (CanFireBullet2 && _bullet2ToConsume) FireBullet2();
        _bullet2ToConsume = false;
    }

    private void FireBullet1()
    {
        _timeBullet1WasFired = _time;

        Instantiate(bullet1, transform.position, transform.localRotation);
        
        // _shrinkage = weaponStats1.playerShrinkage;
        _velocity += -GetDirectionOfPlayer(0f) * weaponStats1.recoilForce;
    }
    
    private void FireBullet2() 
    {
        _timeBullet2WasFired = _time;

        // _shrinkage = weaponStats2.playerShrinkage;
        _velocity += -GetDirectionOfPlayer(0f) * weaponStats2.recoilForce;
    }

    #endregion
    
    private void FixedUpdate()
    {
        HandleRotation();
        HandleMovement();
        // HandleShrinkage();
        
        ApplyMovement();
    }

    #region Shrinkage
    //
    // [Header("Shrinkage")]
    // [Range(0.4f, 0.9f)] [SerializeField] private float softness;
    // private float _shrinkage;
    //
    // private float _shrinkageAdjustment;
    //
    // private void HandleShrinkage()
    // {
    //     _shrinkageAdjustment = (0 - _shrinkage) * softness + _shrinkageAdjustment * softness;
    //     _shrinkage += _shrinkageAdjustment;
    //
    //     float xOffset = Mathf.Clamp(_shrinkage, -1, 0);
    //     float yOffset = -Mathf.Clamp(_shrinkage, 0, 1);
    //
    //     Vector3 scale = new Vector3(1, 1, 1);
    //     scale.x += xOffset;
    //     scale.y += yOffset;
    //
    //     // transform.localScale = scale;
    //     transform.localScale = new Vector3(1, 1 - (_velocity.x * _velocity.x + _velocity.y * _velocity.y)/2000, 1);
    // }

    #endregion

    #region Rotation

    [Header("Rotation")] 
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float rotateThreshold;
    
    private void HandleRotation()
    {
        // Calculate target target rot z.
        Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 difference = mousePos - transform.position;
        float rotz = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        if (rotz < 0) rotz += 360;

        if (Mathf.Abs(rotz - transform.eulerAngles.z) > rotateThreshold)
        {
            if (Mathf.Abs(rotz - transform.eulerAngles.z) < 360 - Mathf.Abs(rotz - transform.eulerAngles.z))
            {
                _rb.AddTorque(Mathf.Sign(rotz - transform.eulerAngles.z) * rotateSpeed);
            }
            else
            {
                _rb.AddTorque(-Mathf.Sign(rotz - transform.eulerAngles.z) * rotateSpeed);
            }
        }
    }

    #endregion

    #region Move
    
    [Header("Move")]
    [SerializeField] private float force;
    [SerializeField] private float friction;

    private Vector2 _velocity;

    private void HandleMovement()
    {
        _velocity *= friction;
    }
    
    #endregion

    private void ApplyMovement() => _rb.velocity = _velocity;

    private Vector2 GetDirectionOfPlayer(float degOffset)
    {
        float rad = (transform.eulerAngles.z + degOffset) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
    }

    private struct FrameInputs
    {
        public bool OneDown;
        public bool TwoDown;
        public bool LeftMouseDown;
        public bool LeftMouseHeld;
    }
}
