using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int gridPosition;
    private static Tile selectedTile;

    private void OnMouseDown()
    {
        // Eğer LevelManager varsa ve oyun bittiyse, tıklamaları tamamen engelle
        if (LevelManager.Instance != null && LevelManager.Instance.isGameEnded)
        {
            return;
        }

        if (selectedTile == null)
        {
            selectedTile = this;
            if (SoundManager.Instance != null) SoundManager.Instance.PlaySelect();
        }
        else
        {
            if (selectedTile == this)
            {
                selectedTile = null;
                return;
            }

            if (IsAdjacent(selectedTile.gridPosition, gridPosition))
            {
                GridManager.Instance.SwapTiles(selectedTile.gridPosition, gridPosition);
                if (SoundManager.Instance != null) SoundManager.Instance.PlaySwap();
                selectedTile = null;
            }
            else
            {
                selectedTile = this;
                if (SoundManager.Instance != null) SoundManager.Instance.PlaySelect();
            }
        }
    }

    private bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        return (Mathf.Abs(a.x - b.x) == 1 && a.y == b.y) || (Mathf.Abs(a.y - b.y) == 1 && a.x == b.x);
    }
}