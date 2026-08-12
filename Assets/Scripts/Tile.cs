using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int gridPosition;
    private static Tile selectedTile;

    private void OnMouseDown()
    {
        if (selectedTile == null)
        {
            selectedTile = this;
            if (SoundManager.Instance != null) SoundManager.Instance.PlaySelect();
        }
        else
        {
            if (IsAdjacent(selectedTile.gridPosition, gridPosition))
            {
                GridManager.Instance.SwapTiles(selectedTile.gridPosition, gridPosition);
                if (SoundManager.Instance != null) SoundManager.Instance.PlaySwap();
            }
            selectedTile = null;
        }
    }

    private bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        return (Mathf.Abs(a.x - b.x) == 1 && a.y == b.y) || (Mathf.Abs(a.y - b.y) == 1 && a.x == b.x);
    }
}