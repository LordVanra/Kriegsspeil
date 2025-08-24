using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class FogMask : MonoBehaviour
{
    private int textureSize = 2048;     
    public float worldSize = 0.001f;

    public List<GameObject> visionSourcesRed; 
    public List<GameObject> visionSourcesBlue; 

    private Slider mode;
    private SpriteRenderer fogRenderer;
    private Texture2D fogTexture;
    private Color32[] fogColors;
    private Material fogMaterial;
    public Image button1;
    public Image button2;

    private float lastVal = 0f;

    private bool isDragging = false;

    private Color normalColor = Color.white;
    private Color selectedColor = Color.gray;

    void Start()
    {
        fogRenderer = GetComponent<SpriteRenderer>();
        fogRenderer.color = Color.white;

        // Create the fog texture
        fogTexture = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);
        fogTexture.filterMode = FilterMode.Bilinear;
        fogColors = new Color32[textureSize * textureSize];
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

        DeselectAllButtons();

        button1.gameObject.GetComponent<Button>().onClick.AddListener(() => ToggleButton1());
        button2.gameObject.GetComponent<Button>().onClick.AddListener(() => ToggleButton2());

        mode = GameObject.Find("FogMode").GetComponent<Slider>();
        lastVal = mode.value;
        HideFog();

        GetComponent<BoxCollider2D>().enabled = false;
    }

    void Update()
    {   
        if(mode.value != lastVal){
            if(mode.value != 1){
                ClearFog();
                GetComponent<BoxCollider2D>().enabled = true;
            }
            else{
                HideFog();
                DeselectAllButtons();
                GetComponent<BoxCollider2D>().enabled = false;
            }
            if (mode.value == 0){
                foreach (GameObject source in visionSourcesRed)
                {
                    Vector2 texturePos = WorldToTextureCoords(source.transform.position);

                    DrawVisionCircle((int)texturePos.x, (int)texturePos.y, -source.transform.eulerAngles.z);
                }
                ApplyTexture();
            }
            else if (mode.value == 2){
                foreach (GameObject source in visionSourcesBlue)
                {
                    Vector2 texturePos = WorldToTextureCoords(source.transform.position);

                    DrawVisionCircle((int)texturePos.x, (int)texturePos.y, -source.transform.eulerAngles.z);
                }     
                ApplyTexture();
            }
        }
        lastVal = mode.value;
        if(mode.value != 1){
            GameObject.Find("Spawn").GetComponent<Slider>().value = 0;
            GameObject.Find("Combat").GetComponent<Slider>().value = 0;
        }

        if (button1.color == selectedColor)
        {
            modifyFog(true);
            ApplyTexture();
        }
        if (button2.color == selectedColor)
        {
            modifyFog(false);
            ApplyTexture();
        }
    }

    public void ToggleButton1()
    {
        if (button1.color == selectedColor)
        {
            button1.color = normalColor;
        }
        else
        {
            button1.color = selectedColor; 
            button2.color = normalColor;
        }
    }
    
    public void ToggleButton2()
    {
        if (button2.color == selectedColor)
        {
            button2.color = normalColor;
        }
        else
        {
            button1.color = normalColor; 
            button2.color = selectedColor;
        }
    }
    
    public void DeselectAllButtons()
    {
        button1.color = normalColor;
        button2.color = normalColor;
    }

    public void HideFog(){
        for (int i = 0; i < fogColors.Length; i++)
        {
            fogColors[i] = new Color32(0, 0, 0, 0);
        }
        ApplyTexture();
    }

    public void ClearFog()
    {
        Color32 start = new Color32(180, 180, 180, 255);
        Color32 min = new Color32(100, 100, 100, 255);
        Color32 max = new Color32(240, 240, 240, 255);
        byte n = 0;
        for (int i = 0; i < fogColors.Length; i++)
        {
            fogColors[i] = new Color32(0, 0, 0, 255);
            if (i < 2048 * 2048)
            {
                fogColors[i] = start;
                n = (byte)UnityEngine.Random.Range(-3, 3);
                start.r += n;
                start.g += n;
                start.b += n;
                if (start.r < min.r)
                {
                    start = min;
                }
                if (start.r > max.r)
                {
                    start = max;
                }
            }
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

                if (Mathf.Pow((x) * Mathf.Cos(Mathf.Deg2Rad * theta) - y * Mathf.Sin(Mathf.Deg2Rad * theta), 2) * 2 + Mathf.Pow(y * Mathf.Cos(Mathf.Deg2Rad * theta) + (x) * Mathf.Sin(Mathf.Deg2Rad * theta) + 20, 2) <= radiusSquared)
                {
                    if ((x * Mathf.Sin(Mathf.Deg2Rad * -theta) + 50) / Mathf.Cos(Mathf.Deg2Rad * -theta) > y && (x * Mathf.Sin(Mathf.Deg2Rad * -theta) - 125) / Mathf.Cos(Mathf.Deg2Rad * -theta) < y && theta >= -90 && theta < 90)
                    {
                        int index = pixelY * textureSize + pixelX;
                        fogColors[index] = new Color32(0, 0, 0, 0);
                    }
                    else if ((x * Mathf.Sin(Mathf.Deg2Rad * -theta) + 50) / Mathf.Cos(Mathf.Deg2Rad * -theta) < y && (x * Mathf.Sin(Mathf.Deg2Rad * -theta) - 125) / Mathf.Cos(Mathf.Deg2Rad * -theta) > y) 
                    {
                        int index = pixelY * textureSize + pixelX;
                        fogColors[index] = new Color32(0, 0, 0, 0);
                    }
                }
            }
        }
    }

    public void modifyFog(bool add){
        int radiusInt = 20;
        int radiusSquared = radiusInt * radiusInt;
        Color32 left;
        Color32 start = new Color32(180, 180, 180, 255);
        Color32 min = new Color32(100, 100, 100, 255);
        Color32 max = new Color32(240, 240, 240, 255);

        for (int y = -radiusInt-20; y <= radiusInt+20; y++)
        {
            for (int x = -radiusInt-20; x <= radiusInt+20; x++)
            {
                int pixelX = (int) WorldToTextureCoords(GetMousePos()).x + x;
                int pixelY = (int) WorldToTextureCoords(GetMousePos()).y + y;

                if (Mathf.Pow(x, 2) + Mathf.Pow(y, 2) <= radiusSquared && isDragging)
                {
                    byte n = 0;
                    int index = pixelY * textureSize + pixelX;

                    if(add && !EventSystem.current.IsPointerOverGameObject()){
                        fogColors[index] = new Color32(0, 0, 0, 0);
                    }
                    else if (!EventSystem.current.IsPointerOverGameObject()){
                        left = fogColors[(pixelY * textureSize + pixelX)/2];
                        n = (byte)UnityEngine.Random.Range(-3, 3);
                        left.r += n;
                        left.g += n;
                        left.b += n;

                        if (left.r < min.r)
                        {
                            left = min;
                        }
                        if (left.r > max.r)
                        {
                            left = max;
                        }

                        fogColors[index] = left;
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
    public void ApplyTexture()
    {
        fogTexture.SetPixels32(fogColors);
        fogTexture.Apply();
    }

    void OnMouseDown(){
        isDragging = true;
    }

    void OnMouseUp(){
        isDragging = false;
    }

    private Vector3 GetMousePos(){
        Vector2 mousePos = Input.mousePosition;
        
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    Vector2 WorldToTextureCoords(Vector2 worldPos)
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
}