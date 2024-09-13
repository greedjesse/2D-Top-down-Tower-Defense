using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PieceStats : ScriptableObject
{
    [Header("Movement")]
    public float speedFactor = 8f;
    public float moveTime = 0.8f;
    public float moveBuffer = 0.1f;
    public float maxYOffset = 1f;
    public float minYOffset = 0.4f;
    public float dMaxYOffset = 10f;
    public float dMinYOffset = 1f;

    [Header("Moving Pattern")] 
    public List<Vector2> patterns = new List<Vector2>();

    [Header("Grounding")] 
    public float earlyGroundingTimeOffset = 0.05f;
}
