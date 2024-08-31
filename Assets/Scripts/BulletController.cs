using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private BulletStats stats;

    private Rigidbody2D _rb;

    private float _time;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _time = 0;
        _velocity = Vector2.zero;
    }

    void Update()
    {
        _time += Time.deltaTime;
    }
    
    private void FixedUpdate()
    {
        HandleMovement();
        
        ApplyMovement();
    }

    #region Move

    private Vector2 _velocity;
    
    private void HandleMovement()
    {
        if (_time > stats.bulletLastTime) Destroy(gameObject);

        float rotz = transform.localEulerAngles.z;
        Vector2 direction = new Vector2(Mathf.Cos(rotz * Mathf.Deg2Rad), Mathf.Sin(rotz * Mathf.Deg2Rad)).normalized;
        _velocity = stats.baseBulletSpeed * direction;
    }

    #endregion

    private void ApplyMovement() => _rb.velocity = _velocity;
}
