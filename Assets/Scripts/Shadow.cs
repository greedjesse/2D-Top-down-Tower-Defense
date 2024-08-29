using UnityEngine;

[ExecuteInEditMode]
public class Shadow : MonoBehaviour
{
    [SerializeField] private ShadowStats stats;

    private SpriteRenderer _spriteRenderer;
    private GameObject _parent;
    
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _parent = transform.parent.gameObject;
    }

    void Start()
    {
        _spriteRenderer.sprite = _parent.GetComponent<SpriteRenderer>().sprite;
        _spriteRenderer.color = stats.color;
    }

    void Update()
    {
        HandleShadow();
    }

    #region Shadow

    private void HandleShadow()
    {
        transform.localRotation = _parent.transform.localRotation;
        transform.position = _parent.transform.position + new Vector3(stats.xOffset, stats.yOffset, 0);
    }

    #endregion
}
