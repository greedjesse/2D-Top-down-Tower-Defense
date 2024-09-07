using System.Collections;
using TMPro.EditorUtilities;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    #region Components

    [SerializeField] private PieceStats stats;
    private Camera _camera;

    #endregion

    private float _time;
    
    void Start()
    {
        _time = 0;
        _camera = Camera.main;
    }
    
    void Update()
    {
        _time += Time.deltaTime;
        
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
        
    }
    #endregion
    
    #region Movement
    private float _speed;
    private Vector2 _velocity;
    
    private Vector2 _source;
    private Vector2 _destination;
    private Vector2 _currentPos;
    private float _distanceToSource;
    public Vector2 shadowPos;

    private bool _moveToConsume;

    private Vector2 _possibleDestination;
    private float _distanceSD;
    private float _timeLastMoveExecuted;

    private bool Moving => _time < _timeLastMoveExecuted + stats.moveTime; 
    
    private void HandleMovement()
    {
        if (!Moving)
        {
            _grounded = true;
            _source = _destination;
            shadowPos = _destination;
            _currentPos = _destination;
        }
        
        if (_grounded && _moveToConsume) ExecuteMove();
        _moveToConsume = false;
        
        if (Moving)
        {
            _speed = Vector2.Distance(shadowPos, _destination) / stats.speedFactor;
            _velocity = (_destination - _source).normalized * _speed;

            shadowPos += _velocity;
            _distanceToSource = Vector2.Distance(_source, shadowPos);
            _currentPos = shadowPos + new Vector2(0f, Offset(_distanceToSource, stats.maxYOffset, _distanceSD));

            transform.position = _currentPos;
        }
    }

    private void ExecuteMove()
    {
        _timeLastMoveExecuted = _time;
        _destination = _possibleDestination;
        _distanceSD = Vector2.Distance(_source, _destination);
        _grounded = false;
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
