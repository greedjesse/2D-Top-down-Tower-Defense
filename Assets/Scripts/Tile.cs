using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Camera _camera;
    private SpriteRenderer _sr;
    private TileController _tc;
    
    private Vector2 _girdSize;

    public Vector2 coordinate;

    void Awake()
    {
        _camera = Camera.main;
        _sr = GetComponent<SpriteRenderer>();
    }

    public void Init(TileController tc, Vector2 count, Vector2 size, float opacityOffsetRange, float opacitySpeedFactor, float opacity, Color color)
    {
        _tc = tc;
        
        color.a = opacity;
        _sr.color = color;
        
        _baseOpacity = opacity;
        _opacity = 0;
        _opacityOffsetRange = opacityOffsetRange;
        _opacitySpeedFactor = opacitySpeedFactor;
        _girdSize = count * size;
    }

    void Update()
    {
        HandleMovement();
        HandleOpacity();
        
        GetCoordinate();
    }

    #region Movement

    private void HandleMovement()
    {
        // Follows camera.
        Vector2 relativePos = transform.position - _camera.transform.position;
        relativePos += _girdSize / 2;
        relativePos = new Vector2(Mod(relativePos.x, _girdSize.x), Mod(relativePos.y, _girdSize.y));
        relativePos -= _girdSize / 2;
        transform.position = (Vector2) _camera.transform.position + relativePos;
    }

    #endregion

    #region Opacity

    private float _baseOpacity;
    private float _opacity;
    private float _opacityOffsetRange; 
    private float _opacitySpeedFactor;

    private void HandleOpacity()
    {
        // Dynamic opacity.
        List<Vector3> posAndRanges = _tc.highlightPosAndRange;
        float opacity = 0;
        foreach (Vector3 a in posAndRanges)
        {
            opacity = Mathf.Max(opacity, (a.z - SquaredDist(a, transform.position)) / a.z * (_baseOpacity + _opacityOffsetRange));
        }
        
        _opacity += (opacity - _opacity) / _opacitySpeedFactor;
        
        Color color = _sr.color;
        color.a = _opacity;
        _sr.color = color;
    }

    #endregion
    
    #region Coordinate

    private void GetCoordinate()
    {
        Vector2 relativePos = (Vector2) transform.position - _tc.centerPos;
        coordinate = new Vector2(Mathf.Round(relativePos.x / _tc.tileSize.x), Mathf.Round(relativePos.y / _tc.tileSize.y));
    }
    
    #endregion
    
    #region Tools
    
    private float Mod(float x, float m) {
        return (x % m + m) % m;
    }
    
    private float SquaredDist(Vector2 a, Vector2 b)
    {
        return Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2);        
    }
    
    #endregion
}
