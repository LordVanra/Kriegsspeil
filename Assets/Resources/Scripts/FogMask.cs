using UnityEngine;
using System.Collections.Generic;

public class FogMask : MonoBehaviour
{
    private int textureSize = 2048;     
    public float worldSize = 0.001f;        
    public List<GameObject> visionSources; 
    private SpriteRenderer fogRenderer;
    private Texture2D fogTexture;
    private Color32[] fogColors;
    private Material fogMaterial;

    void Start()
    {
        fogRenderer = GetComponent<SpriteRenderer>();

        // Create the fog texture
        fogTexture = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);
        fogTexture.filterMode = FilterMode.Bilinear;
        fogColors = new Color32[textureSize * textureSize];

        // Get material instance (this creates a copy so we don't affect the original)
        fogMaterial = fogRenderer.material;

        // Create a sprite from the texture with proper pixels per unit
        float pixelsPerUnit = textureSize / worldSize;
        Sprite fogSprite = Sprite.Create(
            fogTexture, 
            new Rect(0, 0, textureSize, textureSize), 
            new Vector2(0.5f, 0.5f), 
            pixelsPerUnit
        );
        fogRenderer.sprite = fogSprite;

        // Set the texture on the material
        fogMaterial.SetTexture("_MainTex", fogTexture);

        // Initial fog setup - start with full fog
        ClearFog();
        ApplyTexture();
    }

    void Update()
    {
        foreach (GameObject source in visionSources)
        {

            Vector2 texturePos = WorldToTextureCoords(source.transform.position);

            DrawVisionCircle((int)texturePos.x, (int)texturePos.y, -source.transform.eulerAngles.z);
        }

        ApplyTexture();
    }

    public void ClearFog()
    {
        for (int i = 0; i < fogColors.Length; i++)
        {
            fogColors[i] = new Color32(0, 0, 0, 255);
        }
    }

    void DrawVisionCircle(int centerX, int centerY, float theta)
    {
        // Clamp radius to reasonable size
        int radiusInt = 120;
        int radiusSquared = radiusInt * radiusInt;

        for (int y = -radiusInt-20; y <= radiusInt+20; y++)
        {
            for (int x = -radiusInt-20; x <= radiusInt+20; x++)
            {
                int pixelX = centerX + x;
                int pixelY = centerY + y;

                if (Mathf.Pow((x) * Mathf.Cos(Mathf.Deg2Rad * theta) - (y + 20) * Mathf.Sin(Mathf.Deg2Rad * theta), 2) * 2 + Mathf.Pow((y + 20) * Mathf.Cos(Mathf.Deg2Rad * theta) + (x) * Mathf.Sin(Mathf.Deg2Rad * theta), 2) <= radiusSquared)
                {
                    if ((x * Mathf.Sin(Mathf.Deg2Rad * -theta) + 50) / Mathf.Cos(Mathf.Deg2Rad * -theta) < y && theta >= -270 && theta < -90 && (x * Mathf.Sin(Mathf.Deg2Rad * -theta) - 125) / Mathf.Cos(Mathf.Deg2Rad * -theta) > y)
                    {
                        int index = pixelY * textureSize + pixelX;
                        fogColors[index] = new Color32(0, 0, 0, 0);
                    }
                    else if ((x * Mathf.Sin(Mathf.Deg2Rad * -theta) + 50) / Mathf.Cos(Mathf.Deg2Rad * -theta) > y && (x * Mathf.Sin(Mathf.Deg2Rad * -theta) - 125) / Mathf.Cos(Mathf.Deg2Rad * -theta) < y)
                    {
                        int index = pixelY * textureSize + pixelX;
                        fogColors[index] = new Color32(0, 0, 0, 0);
                    }
                }
            }
        }
    }

    Vector2 WorldToTextureCoords(Vector3 worldPos)
    {
        // Get the actual world bounds of the sprite as it appears in the scene
        Bounds spriteBounds = fogRenderer.bounds;
        
        // Calculate relative position within the sprite bounds (0-1)
        float normalizedX = (worldPos.x - spriteBounds.min.x) / spriteBounds.size.x;
        float normalizedY = (worldPos.y - spriteBounds.min.y) / spriteBounds.size.y;
        
        // Convert to texture coordinates
        float texX = normalizedX * textureSize;
        float texY = normalizedY * textureSize;
        
        // Clamp to valid texture bounds
        texX = Mathf.Clamp(texX, 0, textureSize - 1);
        texY = Mathf.Clamp(texY, 0, textureSize - 1);
        
        return new Vector2(texX, texY);
    }
    void ApplyTexture()
    {
        fogTexture.SetPixels32(fogColors);
        fogTexture.Apply();
    }
}