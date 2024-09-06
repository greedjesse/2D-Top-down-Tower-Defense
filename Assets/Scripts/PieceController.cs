using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class PieceController : MonoBehaviour
{
    [SerializeField] private PieceStats stats;
    
    void FixedUpdate()
    {
        HandleMovement();
    }
    
    #region Movement
    [SerializeField] private Vector2 _source;
    [SerializeField] private Vector2 _destination;
    [SerializeField] private Vector2 _currentPos;
    [SerializeField] private Vector2 _shadowPos;

    [SerializeField] private float _distanceSD;  // TODO -> Remember to change this after determined the new source.

    private float _speed;
    private Vector2 _velocity;
    
    private void HandleMovement()
    {
        // TODO -> Remove.
        _distanceSD = Vector2.Distance(_source, _destination);
        
        _speed = Vector2.Distance(_shadowPos, _destination) / stats.speedFactor;
        _velocity = (_destination - _source).normalized * _speed;

        _shadowPos += _velocity;
        _currentPos = _shadowPos + new Vector2(0f,
            Offset(Vector2.Distance(_source, _shadowPos), stats.maxYOffset, _distanceSD));

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
        return Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y * b.y, 2);        
    }
}
