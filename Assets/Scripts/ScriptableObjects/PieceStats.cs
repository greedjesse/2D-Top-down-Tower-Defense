using UnityEngine;

[CreateAssetMenu]
public class PieceStats : ScriptableObject
{
    [Header("Movement")]
    public float speedFactor = 8f;
    public float moveTime = 0.8f;
    public float moveBuffer = 0.1f;
    public float maxYOffset = 0.5f;

    [Header("Grounding")] 
    public float earlyGroundingTimeOffset = 0.05f;
}
