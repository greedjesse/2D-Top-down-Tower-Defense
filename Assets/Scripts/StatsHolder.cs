
using System.Collections.Generic;
using UnityEngine;

public class StatsHolder : MonoBehaviour
{
    public GameObject selectedPiece;
    public List<GameObject> selectedPieceQueue;

    private void Update()
    {
        GameObject currPiece = null;
        foreach (GameObject piece in selectedPieceQueue)
        {
            currPiece = piece; 
            break;
        }
        selectedPiece = currPiece;
    }
}
