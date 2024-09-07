using TMPro.EditorUtilities;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    #region Components

    [SerializeField] private PieceStats stats;
    private Camera _camera;

    #endregion
    
    void Start()
    {
        _camera = Camera.main;
    }
    
    void Update() 
    {
        GatherInputs();
    }

    #region Inputs

    private Inputs _inputs;
    
    private void GatherInputs()
    {
        _inputs = new Inputs()
        {
            LeftMouseDown = Input.GetMouseButtonDown(0)
        };

        if (_inputs.LeftMouseDown)
        {
            _possibleDestination = _camera.ScreenToWorldPoint(Input.mousePosition);
            _moveToConsume = true;
        }
    }

    #endregion
    
    void FixedUpdate()
    {
        HandleGroundCheck();
        
        HandleMovement();
    }
    
    #region Ground Check

    [SerializeField] private bool _grounded;
    
    private void HandleGroundCheck()
    {
        _grounded = false;
        Debug.Log(SquaredDist(shadowPos, _destination));
        if (SquaredDist(shadowPos, _destination) < stats.groundingThreshold)
        {
            _grounded = true;
            _source = _destination;
            shadowPos = _destination;
            _currentPos = _destination;
        }
    }
    #endregion
    
    #region Movement
    [SerializeField] private Vector2 _source;
    [SerializeField] private Vector2 _destination;
    [SerializeField] private Vector2 _currentPos;
    public Vector2 shadowPos;

    private bool _moveToConsume;
    private Vector2 _possibleDestination;
    
    [SerializeField] private float _distanceSD;  // TODO -> Remember to change this after determined the new source.

    private float _speed;
    private Vector2 _velocity;
    
    private void HandleMovement()
    {
        if (_grounded && _moveToConsume)
        {
            _destination = _possibleDestination;
            _distanceSD = Vector2.Distance(_source, _destination);
            _grounded = false;
        }
        _moveToConsume = false;
        
        _speed = Vector2.Distance(shadowPos, _destination) / stats.speedFactor;
        _velocity = (_destination - _source).normalized * _speed;

        shadowPos += _velocity;
        _currentPos = shadowPos + new Vector2(0f,
            Offset(Vector2.Distance(_source, shadowPos), stats.maxYOffset, _distanceSD));

        transform.position = _currentPos;
    }

    private float Offset(float x, float max, float end)
    {
        if (_distanceSD == 0) return 0;
        
        // TODO -> Can be optimized.
        float h = end / 2;
        float k = max;
        float a = - k / Mathf.Pow(h, 2);
        
        return a * Mathf.Pow(x - h, 2) + k;
    }

    #endregion

    private float SquaredDist(Vector2 a, Vector2 b)
    {
        return Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2);        
    }

    struct Inputs
    {
        public bool LeftMouseDown;
    }
}
