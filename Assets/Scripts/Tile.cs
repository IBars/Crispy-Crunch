using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int gridPosition;
    private static Tile selectedTile;

    private void OnMouseDown()
    {
        if (selectedTile == null)
        {
            // İlk taşı seç
            selectedTile = this;
            transform.localScale *= 1.15f; // Seçildiğini belirtmek için hafif büyüt
        }
        else if (selectedTile == this)
        {
            // Aynı taşa tekrar tıklanırsa seçimi iptal et
            transform.localScale /= 1.15f;
            selectedTile = null;
        }
        else
        {
            // İkinci taşı seç ve komşu mu kontrol et
            if (IsAdjacent(selectedTile.gridPosition, gridPosition))
            {
                selectedTile.transform.localScale /= 1.15f;
                GridManager.Instance.SwapTiles(selectedTile.gridPosition, gridPosition);
                selectedTile = null;
            }
            else
            {
                // Komşu değilse yeni seçimi bu taş yap
                selectedTile.transform.localScale /= 1.15f;
                selectedTile = this;
                transform.localScale *= 1.15f;
            }
        }
    }

    private bool IsAdjacent(Vector2Int posA, Vector2Int posB)
    {
        return Mathf.Abs(posA.x - posB.x) + Mathf.Abs(posA.y - posB.y) == 1;
    }
}