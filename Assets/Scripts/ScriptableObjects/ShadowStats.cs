using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShadowStats : ScriptableObject
{
    [Header("Color")] 
    public Color color; 
    
    [Header("Offsets")] 
    public float xOffset = 0.05f;
    public float yOffset = -0.05f;

}
