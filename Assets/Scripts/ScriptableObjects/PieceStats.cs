using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class PieceStats : ScriptableObject
{
    [Header("Movement")]
    public float speedFactor = 8f;
    public float moveBuffer = 0.1f;
    public float maxMoveTime = 0.8f;
    public float minMoveTime = 0.6f;
    public float dMaxMoveTime = 16;
    public float dMinMoveTime = 6;
    public float maxYOffset = 1f;
    public float minYOffset = 0.4f;
    public float dMaxYOffset = 10f;
    public float dMinYOffset = 1f;

    [Header("Moving Pattern")] 
    public List<Vector2> patterns = new List<Vector2>();

    [Header("Grounding")] 
    public float earlyGroundingTimeOffset = 0.05f;
}
