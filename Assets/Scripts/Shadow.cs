using UnityEngine;

public class Shadow : MonoBehaviour
{
    [SerializeField] private ShadowStats stats;

    private SpriteRenderer _spriteRenderer;
    private Transform _parent;
        
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _parent = transform.parent;
    }

    void Start()
    {
        _spriteRenderer.color = stats.color;
    }

    void Update()
    {
        transform.localRotation = _parent.transform.localRotation;
        transform.position = _parent.position + new Vector3(stats.xOffset, stats.yOffset, 0);
    }
}
