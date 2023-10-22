using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallRemoving : MonoBehaviour
{
    public Tilemap foregoundWallTilemap;
    private float startTransparency;
    public float transitionDuration = 0.7f;  // Duration of the transition in seconds

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Player")
        {
            StartCoroutine(ChangeTransparencySmoothly(0.0f, transitionDuration));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            StartCoroutine(ChangeTransparencySmoothly(1.0f, transitionDuration));
        }
    }

    void Start()
    {
        startTransparency = GetCurrentTransparency();
    }

    float GetCurrentTransparency()
    {
        TilemapRenderer tilemapRenderer = foregoundWallTilemap.GetComponent<TilemapRenderer>();
        return tilemapRenderer.material.color.a;
    }

    IEnumerator ChangeTransparencySmoothly(float targetAlpha, float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float normalizedTime = (Time.time - startTime) / duration;
            float currentAlpha = Mathf.Lerp(startTransparency, targetAlpha, normalizedTime);

            SetTilemapTransparency(currentAlpha);
            yield return null;
        }

        SetTilemapTransparency(targetAlpha); // Ensure the target transparency is set precisely
    }

    void SetTilemapTransparency(float alpha)
    {
        TilemapRenderer tilemapRenderer = foregoundWallTilemap.GetComponent<TilemapRenderer>();
        Material tilemapMaterial = tilemapRenderer.material;
        Color materialColor = tilemapMaterial.color;
        materialColor.a = alpha;
        tilemapMaterial.color = materialColor;
    }
}
