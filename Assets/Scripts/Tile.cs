using UnityEngine;
using UnityEngine.InputSystem; 

public class Tile : MonoBehaviour
{
    public Vector2Int gridPosition;
    private static Tile selectedTile;

    // Sürükleme için gereken değişkenler
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private float swipeThreshold = 30f; 
    
    private bool selectedJustNow = false;

    private void OnMouseDown()
    {
        if (LevelManager.Instance != null && LevelManager.Instance.isGameEnded) return;

        // ÇÖZÜM 1: Mouse.current yerine evrensel Pointer.current kullanıyoruz.
        // Bu sayede hem PC'de tıklamayı hem Mobilde parmak dokunuşunu anında algılar.
        if (Pointer.current != null)
        {
            firstTouchPosition = Pointer.current.position.ReadValue();
        }

        selectedJustNow = false;

        if (selectedTile == null)
        {
            selectedTile = this;
            if (SoundManager.Instance != null) SoundManager.Instance.PlaySelect();
            selectedJustNow = true; 
        }
        else if (selectedTile != this && !IsAdjacent(selectedTile.gridPosition, gridPosition))
        {
            selectedTile = this;
            if (SoundManager.Instance != null) SoundManager.Instance.PlaySelect();
            selectedJustNow = true;
        }
    }

    private void OnMouseUp()
    {
        if (LevelManager.Instance != null && LevelManager.Instance.isGameEnded) return;

        // ÇÖZÜM 1 DEVAMI: Parmağı ekrandan çektiğimiz konumu alıyoruz
        if (Pointer.current != null)
        {
            finalTouchPosition = Pointer.current.position.ReadValue();
        }

        float distance = Vector2.Distance(firstTouchPosition, finalTouchPosition);

        if (distance > swipeThreshold)
        {
            CalculateSwipe();
        }
        else
        {
            if (!selectedJustNow) 
            {
                if (selectedTile == this)
                {
                    selectedTile = null;
                }
                else if (selectedTile != null && IsAdjacent(selectedTile.gridPosition, gridPosition))
                {
                    GridManager.Instance.SwapTiles(selectedTile.gridPosition, gridPosition);
                    if (SoundManager.Instance != null) SoundManager.Instance.PlaySwap();
                    selectedTile = null;
                }
            }
        }
    }

    private void CalculateSwipe()
    {
        float swipeX = finalTouchPosition.x - firstTouchPosition.x;
        float swipeY = finalTouchPosition.y - firstTouchPosition.y;

        Vector2Int direction = Vector2Int.zero;

        if (Mathf.Abs(swipeX) > Mathf.Abs(swipeY))
        {
            direction = swipeX > 0 ? Vector2Int.right : Vector2Int.left; 
        }
        else
        {
            direction = swipeY > 0 ? Vector2Int.up : Vector2Int.down; 
        }

        Vector2Int targetPos = gridPosition + direction;

        if (targetPos.x >= 0 && targetPos.x < GridManager.Instance.width &&
            targetPos.y >= 0 && targetPos.y < GridManager.Instance.height)
        {
            GridManager.Instance.SwapTiles(gridPosition, targetPos);
            if (SoundManager.Instance != null) SoundManager.Instance.PlaySwap();
            
            selectedTile = null; 
        }
    }

    private bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        return (Mathf.Abs(a.x - b.x) == 1 && a.y == b.y) || (Mathf.Abs(a.y - b.y) == 1 && a.x == b.x);
    }
}