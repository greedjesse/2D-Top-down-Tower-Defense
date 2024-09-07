using UnityEngine;

[CreateAssetMenu]
public class PieceStats : ScriptableObject
{
    [Header("Movement")]
    public float speedFactor = 6f;
    public float moveTime = 0.8f;
    public float maxYOffset = 0.5f;

    [Header("Grounding")] 
    public float groundingThreshold = 0.05f;
}
