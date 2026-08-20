using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeRainManager : MonoBehaviour
{
    [Header("Slime Ayarları")]
    public Sprite[] slimeSprites;
    public int slimeCount = 15;
    public float minSpeed = 150f;
    public float maxSpeed = 350f;
    public float minSize = 60f;
    public float maxSize = 120f;

    [Header("Referanslar")]
    public RectTransform canvasRect;

    private List<RectTransform> slimes = new List<RectTransform>();
    private List<float> speeds = new List<float>();

    void Start()
    {
        if (slimeSprites == null || slimeSprites.Length == 0) return;

        for (int i = 0; i < slimeCount; i++)
        {
            SpawnSlime(true);
        }
    }

    void SpawnSlime(bool randomYStart)
    {
        GameObject obj = new GameObject("SlimeRain");
        obj.transform.SetParent(transform, false);

        Image img = obj.AddComponent<Image>();
        img.sprite = slimeSprites[Random.Range(0, slimeSprites.Length)];
        img.preserveAspect = true;

        RectTransform rt = obj.GetComponent<RectTransform>();
        float size = Random.Range(minSize, maxSize);
        rt.sizeDelta = new Vector2(size, size);

        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        float randomX = Random.Range(-canvasWidth / 2f, canvasWidth / 2f);
        float startY = randomYStart
            ? Random.Range(-canvasHeight / 2f, canvasHeight / 2f)
            : canvasHeight / 2f + size;

        rt.anchoredPosition = new Vector2(randomX, startY);

        slimes.Add(rt);
        speeds.Add(Random.Range(minSpeed, maxSpeed));
    }

    void Update()
    {
        float canvasHeight = canvasRect.rect.height;
        float canvasWidth = canvasRect.rect.width;

        for (int i = 0; i < slimes.Count; i++)
        {
            if (slimes[i] == null) continue;

            Vector2 pos = slimes[i].anchoredPosition;
            pos.y -= speeds[i] * Time.deltaTime;
            slimes[i].anchoredPosition = pos;

            // Ekranın altına çıkınca tekrar yukarıdan başlat
            float halfHeight = canvasHeight / 2f;
            float halfSize = slimes[i].sizeDelta.y / 2f;

            if (pos.y < -halfHeight - halfSize)
            {
                float randomX = Random.Range(-canvasWidth / 2f, canvasWidth / 2f);
                slimes[i].anchoredPosition = new Vector2(randomX, halfHeight + halfSize);

                // Yeni bir slime sprite'ı seç
                slimes[i].GetComponent<Image>().sprite =
                    slimeSprites[Random.Range(0, slimeSprites.Length)];
            }
        }
    }
}