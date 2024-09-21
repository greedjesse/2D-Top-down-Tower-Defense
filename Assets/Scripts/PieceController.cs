using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PieceController : MonoBehaviour
{
    #region Components

    [SerializeField] private PieceStats stats;
    [SerializeField] private StatsHolder statsHolder;
    private PieceStatsHolder _pieceStatsHolder;
    private Collider2D _col;
    private Camera _camera;

    #endregion

    private float _time;
    
    void Start()
    {
        _time = 0;
        _col = GetComponent<Collider2D>();
        _pieceStatsHolder = GetComponent<PieceStatsHolder>();
        _camera = Camera.main;

        _source = startPos;
        _destination = startPos;
        shadowPos = startPos;
        _currentPos = startPos;
        _pieceStatsHolder.destination = _destination;
        
        statsHolder.existingPieces.Add(_pieceStatsHolder);
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
        if (_selected && _inputs.LeftMouseDown)
        {
            _timeLeftMouseWasPressed = _time;
            _possibleDestination = statsHolder.GetTilePos(_camera.ScreenToWorldPoint(Input.mousePosition));

            if (CheckMovementValidation(_possibleDestination)) _moveToConsume = true;
            else _moveBufferUsable = false;
        }
    }

    #endregion
    
    void FixedUpdate()
    {
        HandleMovement();
    }
    
    #region Ground Check

    private float MoveTime => Sigmoid(_distanceSD, new Vector2(stats.dMinMoveTime, stats.dMaxMoveTime), new Vector2(stats.minMoveTime, stats.maxMoveTime));
    private bool Grounded => !Moving || _time > _timeLastMoveExecuted + MoveTime - stats.earlyGroundingTimeOffset;
    
    #endregion
    
    #region Movement
    private float _speed;
    private Vector2 _velocity;

    [SerializeField] private Vector2 startPos;
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

    private bool Moving => _time < _timeLastMoveExecuted + MoveTime;
    private bool HaveBufferedMove => _moveBufferUsable && _time < _timeLeftMouseWasPressed + stats.moveBuffer;
    
    
    private void HandleMovement()
    {
        // Check grounded.
        if (!Moving)
        {
            _moveBufferUsable = false;
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
            _currentPos = shadowPos + new Vector2(0f,
                Offset(_distanceToSource,
                    Sigmoid(_distanceSD, new Vector2(stats.dMinYOffset, stats.dMaxYOffset),
                        new Vector2(stats.minYOffset, stats.maxYOffset)), _distanceSD));

            transform.position = _currentPos;
        }
    }

    private void ExecuteMove()
    {
        _timeLastMoveExecuted = _time;
        _moveBufferUsable = true;
        _source = shadowPos;
        _destination = _possibleDestination;
        _pieceStatsHolder.destination = _destination;
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

    private bool _selected;

    private void HandleSelection()
    {
        _selected = false;
        if (statsHolder.selectedPiece == gameObject)
        {
            _selected = true;
        }
        
        Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        if (_col.OverlapPoint(mousePos))
        {
            if (_inputs.LeftMouseDown)
            {
                if (!statsHolder.selectedPieceQueue.Contains(gameObject))
                {
                    statsHolder.selectedPieceQueue.Add(gameObject);
                }
            }
        }
        else
        {
            if (!_moveToConsume && !HaveBufferedMove && _inputs.LeftMouseDown)
            {
                if (statsHolder.selectedPiece == gameObject)
                {
                    statsHolder.selectedPieceQueue.Remove(gameObject);
                }
            }                
        }
    }
    
    #endregion

    #region Tools
    
    private float SquaredDist(Vector2 a, Vector2 b)
    {
        return Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2);        
    }

    private float Sigmoid(float x, Vector2 xRange, Vector2 yRange)
    {
        float xDist = xRange.y - xRange.x;
        float yDist = yRange.y - yRange.x;
        return yRange.x + (yDist / (1 + Mathf.Exp(-5f / xDist * (x - xDist / 2))));
    }

    private bool CheckMovementValidation(Vector2 destination)
    {
        bool valid = false;
        foreach (Vector2 pattern in stats.patterns)
        {
            if (_destination + pattern == destination)
            {
                valid = true;
            }
        }

        if (!valid) return false;

        foreach (PieceStatsHolder piece in statsHolder.existingPieces)
        {
            if (destination == piece.destination)
            {
                return false;
            }
        }
        
        return true;
    }
    
    #endregion

    // Holds the inputs.
    struct Inputs
    {
        public bool LeftMouseDown;
    }
}
