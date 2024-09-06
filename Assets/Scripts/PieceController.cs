using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class PieceController : MonoBehaviour
{
    [SerializeField] private PieceStats stats;
    
    #region Movement
    private Vector2 _source;
    [SerializeField] private Vector2 _destination;
    [SerializeField] private Vector2 _currentPos;
    [SerializeField] private Vector2 _shadowPos;

    private float _distanceSD;  // TODO -> Remember to change this after determined the new source.

    private float _speed;
    private Vector2 _velocity;
    
    void FixedUpdate()
    {
        HandleMovement();
    }
    
    private void HandleMovement()
    {
        // TODO -> Remove.
        _distanceSD = Vector2.Distance(_source, _destination);
        
        _speed = Vector2.Distance(_shadowPos, _destination) / stats.speedFactor;
        _velocity = (_destination - _source).normalized * _speed;

        _shadowPos += _velocity;
        _currentPos = _shadowPos + new Vector2(0f, Offset(SquaredDist(_source, _shadowPos), stats.maxYOffset, Mathf.Pow(_distanceSD, 2)));

        transform.position = _shadowPos;  // TODO -> Change.
    }

    private float Offset(float x, float max, float end)
    {
        float h = end / 2;
        float k = max;
        float a = - k / Mathf.Pow(h, 2);

        return a * Mathf.Pow(x - h, 2) + k;
    }

    #endregion

    private float SquaredDist(Vector2 a, Vector2 b)
    {
        return a.x * b.x + a.y * b.y;        
    }
}
