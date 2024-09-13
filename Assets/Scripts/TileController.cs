using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [SerializeField] private StatsHolder stateHolder;
    
    [Header("Prefab")]
    [SerializeField] private GameObject tilePrefab;

    [Header("Instantiate")]
    public Vector2 centerPos;
    public Vector2 startPos;
    [Tooltip("Even values only.")] public Vector2 tileCount;
    public Vector2 tileSize;

    [Header("Colors")] 
    [SerializeField] private Color normalColor;
    [SerializeField] private Color offsetColor;
    
    [Header("Opacity")] 
    [SerializeField] private float normalOpacity = 0.2f;
    [SerializeField] private float offsetOpacity = 0.3f;
    [Range(0.1f, 0.5f)] public float opacityOffsetRange = 0.1f; 
    public float opacitySpeedFactor = 6;

    [Header("Highlight")] 
    [Tooltip("Squared Distance")]
    [SerializeField] private float cursorHighlightRange;
    
    private Camera _camera;
    [HideInInspector] public List<Vector3> highlightPosAndRange;

    void Awake()
    {
        _camera = Camera.main;
    }
    
    void Start()
    {
        highlightPosAndRange = new List<Vector3>();
        highlightPosAndRange.Add(Vector2.zero);
        
        InstantiateTiles();
    }

    private void InstantiateTiles()
    {
        for (int c = 1; c < tileCount.x + 1; c++)
        {
            for (int r = 1; r < tileCount.y + 1; r++)
            {
                GameObject instance = Instantiate(tilePrefab, new Vector3(startPos.x + c * tileSize.x - tileSize.x / 2, startPos.y + r * tileSize.y - tileSize.y / 2, 0), Quaternion.identity);
                
                bool offset = (c % 2 == 0 && r % 2 == 1) || (c % 2 == 1 && r % 2 == 0);
                instance.GetComponent<Tile>().Init(stateHolder, this, tileCount, tileSize, opacityOffsetRange, opacitySpeedFactor,
                    offset ? offsetOpacity : normalOpacity, offset ? offsetColor : normalColor);
                instance.name = $"Tile {(c-1) * tileCount.y + r-1}";
                instance.transform.parent = transform;
            }
        }
    }

    void Update()
    {
        Vector3 vec3 = _camera.ScreenToWorldPoint(Input.mousePosition);
        vec3.z = cursorHighlightRange;
        highlightPosAndRange[0] = vec3;
    }
}
