using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PieceController : MonoBehaviour
{
    #region Components

    [SerializeField] private PieceStats stats;
    [FormerlySerializedAs("_sh")] [SerializeField] private StatsHolder sh;
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
        HandleSelection();
    }

    #region Inputs

    private Inputs _inputs;

    private float _timeLeftMouseWasPressed;
    
    private void GatherInputs()
    {
        // Gather inputs.
        _inputs = new Inputs()
        {
            LeftMouseDown = Input.GetMouseButtonDown(0)
        };

        // Fed the received inputs back to the game.
        if (_inputs.LeftMouseDown)
        {
            _timeLeftMouseWasPressed = _time;
            _possibleDestination = _camera.ScreenToWorldPoint(Input.mousePosition);
            _moveToConsume = true;
        }
    }

    #endregion
    
    void FixedUpdate()
    {
        HandleMovement();
    }
    
    #region Ground Check

    private bool Grounded => !Moving || _time > _timeLastMoveExecuted + stats.moveTime - stats.earlyGroundingTimeOffset;
    
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
    private bool _moveBufferUsable;

    private Vector2 _possibleDestination;
    private float _distanceSD;
    private float _timeLastMoveExecuted;

    private bool Moving => _time < _timeLastMoveExecuted + stats.moveTime;
    private bool HaveBufferedMove => _moveBufferUsable && _time < _timeLeftMouseWasPressed + stats.moveBuffer;
    
    
    private void HandleMovement()
    {
        _moveBufferUsable = Moving;
        
        // Check grounded.
        if (!Moving)
        {
            _source = _destination;
            shadowPos = _destination;
            _currentPos = _destination;
        }

        // Execute move?
        if (Grounded && (_moveToConsume || HaveBufferedMove)) ExecuteMove();
        _moveToConsume = false;
        
        // The actual movement.
        if (Moving)
        {
            _speed = Vector2.Distance(shadowPos, _destination) / stats.speedFactor;
            _velocity = (_destination - _source).normalized * _speed;

            shadowPos += _velocity;
            _distanceToSource = Vector2.Distance(_source, shadowPos);
            _currentPos = shadowPos + new Vector2(0f, Offset(_distanceToSource, Sigmoid(_distanceSD, 
                new Vector2(stats.minYOffset, stats.maxYOffset), new Vector2(stats.dMinYOffset, stats.dMaxYOffset)), _distanceSD));

            transform.position = _currentPos;
        }
    }

    private void ExecuteMove()
    {
        _timeLastMoveExecuted = _time;
        _source = shadowPos;
        _destination = _possibleDestination;
        _distanceSD = Vector2.Distance(_source, _destination);
    }
    
    // Used for the y offset.
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
    
    #region Selection

    private bool _mouseOver;

    private void HandleSelection()
    {
        if (_mouseOver) return;
        
        if (_inputs.LeftMouseDown)
        {
            if (sh.selectedPiece == gameObject)
            {
                sh.selectedPieceQueue.Remove(gameObject);
            }
        }    
    }
    
    // Collider needed.
    void OnMouseOver()
    {
        _mouseOver = true;
        if (_inputs.LeftMouseDown)
        {
            if (!sh.selectedPieceQueue.Contains(gameObject))
            {
                sh.selectedPieceQueue.Add(gameObject);
            }
        }
    }

    void OnMouseExit()
    {
        _mouseOver = false;
    }

    #endregion

    #region Tools
    
    private float SquaredDist(Vector2 a, Vector2 b)
    {
        return Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2);        
    }

    private float Sigmoid(float x, Vector2 yRange, Vector2 xRange)
    {
        float xDist = xRange.y - xRange.x;
        float yDist = yRange.y - yRange.x;
        return yRange.x + (yDist / (1 + Mathf.Exp(-5f / xDist * (x - xDist / 2))));
    }
    
    #endregion

    // Holds the inputs.
    struct Inputs
    {
        public bool LeftMouseDown;
    }
}
