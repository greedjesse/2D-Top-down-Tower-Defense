using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StatsHolder : MonoBehaviour
{
    [SerializeField] private TileController tileController;
    
    [Header("Piece Selection")]
    public GameObject selectedPiece;
    public List<GameObject> selectedPieceQueue;

    [Header("Tiles")] 
    public Vector2 lastClickedTileCoord;
    public Vector2 lastClickedTilePos;
    
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

    public Vector2 GetTilePos(Vector2 worldPos)
    {
        // TODO -> replace 0.5f and 2 with var.
        return new Vector2(
            Mathf.Floor(1 / tileController.tileSize.x * (worldPos.x - tileController.centerPos.x + 0.5f)) / (1 / tileController.tileSize.x) +
            1 / tileController.tileSize.x * 2 - 2,
            Mathf.Floor(1 / tileController.tileSize.y * (worldPos.y - tileController.centerPos.y + 0.5f)) / (1 / tileController.tileSize.y) +
            1 / tileController.tileSize.y * 2 - 2);
    }
}
