using UnityEngine;

[ExecuteInEditMode]
public class Shadow : MonoBehaviour
{
    [SerializeField] private ShadowStats stats;

    private SpriteRenderer _spriteRenderer;
    private GameObject _parent;
    private PieceController _parentController;
    
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _parent = transform.parent.gameObject;
        _parentController = _parent.GetComponent<PieceController>();
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
        transform.position = _parentController.shadowPos + new Vector2(stats.xOffset, stats.yOffset);
    }

    #endregion
}
