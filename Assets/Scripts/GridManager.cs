using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Izgara Ayarları")]
    public int width = 6;
    public int height = 6;
    public GameObject[] tilePrefabs;

    [Header("Görsel Efektler")]
    public GameObject explosionPrefab;

    [Header("Oyun Mantığı")]
    public int maxMoves = 5;
    private int currentMoves = 0;
    private bool isBoomPhase = false;

    [Header("UI Elemanları")]
    public TextMeshProUGUI movesText;

    private GameObject[,] gridObjects;

    private void Awake()
    {
        Instance = this;
    }

void Start()
    {
        // Eğer LevelManager varsa rastgele oluşturulan maxMoves değerini al
        if (LevelManager.Instance != null)
        {
            maxMoves = LevelManager.Instance.maxMoves;
        }

        gridObjects = new GameObject[width, height];
        GenerateGrid();
        UpdateUI();
    }

    void UpdateUI()
    {
        if (movesText != null)
        {
            int remainingMoves = Mathf.Max(0, maxMoves - currentMoves);
            movesText.text = "Moves: " + remainingMoves + " / " + maxMoves;
        }
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int randomIndex = Random.Range(0, tilePrefabs.Length);
                GameObject tile = Instantiate(tilePrefabs[randomIndex], new Vector3(x, y, 0), Quaternion.identity, transform);
                
                Tile tileScript = tile.GetComponent<Tile>();
                if (tileScript == null) tileScript = tile.AddComponent<Tile>();
                tileScript.gridPosition = new Vector2Int(x, y);

                gridObjects[x, y] = tile;
            }
        }

        Camera.main.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10f);
    }

    public void SwapTiles(Vector2Int a, Vector2Int b)
    {
        if (isBoomPhase || currentMoves >= maxMoves) return;

        GameObject tileA = gridObjects[a.x, a.y];
        GameObject tileB = gridObjects[b.x, b.y];

        gridObjects[a.x, a.y] = tileB;
        gridObjects[b.x, b.y] = tileA;

        tileA.GetComponent<Tile>().gridPosition = b;
        tileB.GetComponent<Tile>().gridPosition = a;

        StartCoroutine(MoveToPosition(tileA, new Vector3(b.x, b.y, 0), 0.15f));
        StartCoroutine(MoveToPosition(tileB, new Vector3(a.x, a.y, 0), 0.15f));

        currentMoves++;
        UpdateUI();

        if (currentMoves >= maxMoves)
        {
            TriggerBigBoom();
        }
    }

    private IEnumerator MoveToPosition(GameObject obj, Vector3 targetPos, float duration)
    {
        if (obj == null) yield break;
        Vector3 startPos = obj.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (obj == null) yield break;
            obj.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (obj != null) obj.transform.position = targetPos;
    }

    public void TriggerBigBoom()
    {
        if (isBoomPhase) return;
        isBoomPhase = true;
        StartCoroutine(CheckAndDestroyMatches());
    }

    private IEnumerator CheckAndDestroyMatches()
    {
        yield return new WaitForSeconds(0.2f);

        // 1. AŞAMA: Temel 3'lü eşleşmeleri tespit et (Yatay ve Dikey)
        HashSet<GameObject> baseMatches = new HashSet<GameObject>();

        // Yatay Kontrol
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width - 2; x++)
            {
                GameObject t1 = gridObjects[x, y];
                GameObject t2 = gridObjects[x + 1, y];
                GameObject t3 = gridObjects[x + 2, y];

                if (t1 != null && t2 != null && t3 != null)
                {
                    if (t1.name == t2.name && t2.name == t3.name)
                    {
                        baseMatches.Add(t1);
                        baseMatches.Add(t2);
                        baseMatches.Add(t3);
                    }
                }
            }
        }

        // Dikey Kontrol
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height - 2; y++)
            {
                GameObject t1 = gridObjects[x, y];
                GameObject t2 = gridObjects[x, y + 1];
                GameObject t3 = gridObjects[x, y + 2];

                if (t1 != null && t2 != null && t3 != null)
                {
                    if (t1.name == t2.name && t2.name == t3.name)
                    {
                        baseMatches.Add(t1);
                        baseMatches.Add(t2);
                        baseMatches.Add(t3);
                    }
                }
            }
        }

        // 2. AŞAMA: YAYILMA (FLOOD FILL) ALGORİTMASI
        HashSet<GameObject> tilesToDestroy = new HashSet<GameObject>();
        Queue<GameObject> queue = new Queue<GameObject>();

        foreach (GameObject tile in baseMatches)
        {
            tilesToDestroy.Add(tile);
            queue.Enqueue(tile);
        }

        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        while (queue.Count > 0)
        {
            GameObject current = queue.Dequeue();
            if (current == null) continue;

            Tile tileScript = current.GetComponent<Tile>();
            if (tileScript == null) continue;

            Vector2Int pos = tileScript.gridPosition;

            foreach (Vector2Int dir in directions)
            {
                Vector2Int nPos = pos + dir;

                if (nPos.x >= 0 && nPos.x < width && nPos.y >= 0 && nPos.y < height)
                {
                    GameObject neighbor = gridObjects[nPos.x, nPos.y];

                    if (neighbor != null && !tilesToDestroy.Contains(neighbor))
                    {
                        if (neighbor.name == current.name)
                        {
                            tilesToDestroy.Add(neighbor);
                            queue.Enqueue(neighbor);
                        }
                    }
                }
            }
        }

        // 3. AŞAMA: PATLATMA VE LEVEL MANAGER'A HABER VERME
        if (tilesToDestroy.Count > 0)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayExplode();
            }

            foreach (GameObject tile in tilesToDestroy)
            {
                Tile tileScript = tile.GetComponent<Tile>();
                gridObjects[tileScript.gridPosition.x, tileScript.gridPosition.y] = null;
                
                if (explosionPrefab != null)
                {
                    GameObject vfx = Instantiate(explosionPrefab, tile.transform.position, Quaternion.identity);
                    Destroy(vfx, 1f);
                }

                // LEVEL MANAGER GOAL BİLGİSİNİ GÜNCELLEME SATIRI:
                if (LevelManager.Instance != null)
                {
                    LevelManager.Instance.AddDestroyedBlock(1);
                }

                Destroy(tile);
            }

            yield return new WaitForSeconds(0.25f);
            StartCoroutine(ApplyGravityAndRefill());
        }
        else
        {
            isBoomPhase = false;
            UpdateUI();
        }
    }

    private IEnumerator ApplyGravityAndRefill()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (gridObjects[x, y] == null)
                {
                    for (int aboveY = y + 1; aboveY < height; aboveY++)
                    {
                        if (gridObjects[x, aboveY] != null)
                        {
                            gridObjects[x, y] = gridObjects[x, aboveY];
                            gridObjects[x, aboveY] = null;

                            StartCoroutine(MoveToPosition(gridObjects[x, y], new Vector3(x, y, 0), 0.2f));
                            gridObjects[x, y].GetComponent<Tile>().gridPosition = new Vector2Int(x, y);
                            break;
                        }
                    }
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (gridObjects[x, y] == null)
                {
                    int randomIndex = Random.Range(0, tilePrefabs.Length);
                    float spawnY = height + 3f + (height - y);
                    
                    GameObject newTile = Instantiate(tilePrefabs[randomIndex], new Vector3(x, spawnY, 0), Quaternion.identity, transform);
                    
                    Tile tileScript = newTile.GetComponent<Tile>();
                    if (tileScript == null) tileScript = newTile.AddComponent<Tile>();
                    tileScript.gridPosition = new Vector2Int(x, y);

                    gridObjects[x, y] = newTile;
                    StartCoroutine(MoveToPosition(newTile, new Vector3(x, y, 0), 0.3f));
                }
            }
        }

        yield return new WaitForSeconds(0.35f);
        StartCoroutine(CheckAndDestroyMatches());
    }
}