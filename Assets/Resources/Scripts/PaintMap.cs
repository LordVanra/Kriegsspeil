using UnityEngine;
using UnityEngine.UI;

public class PaintMap : MonoBehaviour
{
    public int textureSize = 2048;   // resolution of the paintable texture

    private Texture2D tex;
    private SpriteRenderer rend;

    // Six paint colors
    private Color[] colors = new Color[6]
    {
        Color.green,                               // Green
        new Color(0.36f, 0.25f, 0.20f, 1f),        // Brown with green tint
        new Color(0.82f, 0.71f, 0.55f, 1f),        // Tan with green tint
        Color.blue,                                // Blue
        Color.black,                               // Black
        new Color(0f, 0.2f, 0f, 1f)                // Dark Green
    };

    private int currentColorIndex = 0;

    private Slider sizeSlider;
    private int brushSize;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();

        // Create a new white texture
        tex = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;   // smooth blending
        tex.wrapMode  = TextureWrapMode.Clamp;  // no tiling

        Color[] fillColor = new Color[textureSize * textureSize];
        for (int i = 0; i < fillColor.Length; i++)
            fillColor[i] = Color.white;
        tex.SetPixels(fillColor);
        tex.Apply();

        // Assign to sprite
        Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, textureSize, textureSize), new Vector2(0.5f, 0.5f), 2048);
        rend.sprite = newSprite;

        sizeSlider = GameObject.Find("BrushSize").GetComponent<Slider>();
    }

    void Update()
    {
        brushSize = (int) sizeSlider.value;

        // Switch colors with 1–6
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentColorIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentColorIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) currentColorIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) currentColorIndex = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) currentColorIndex = 4;
        if (Input.GetKeyDown(KeyCode.Alpha6)) currentColorIndex = 5;

        // Paint with left mouse button
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // Convert hit point into local sprite coordinates
                Vector2 localPos = transform.InverseTransformPoint(hit.point);

                // Sprite goes from -0.5 to +0.5 in local space, so normalize to [0,1]
                float uvX = localPos.x + 0.5f;
                float uvY = localPos.y + 0.5f;

                // Convert UVs to pixel coordinates
                int px = Mathf.RoundToInt(uvX * tex.width);
                int py = Mathf.RoundToInt(uvY * tex.height);

                // Paint in a circular brush
                for (int x = -brushSize/3; x <= brushSize/3; x++)
                {
                    for (int y = -brushSize/2; y <= brushSize/2; y++)
                    {
                        if (x * x * 9 + y * y * 4 <= brushSize * brushSize)
                        {
                            int tx = px + x;
                            int ty = py + y;

                            if (tx >= 0 && tx < tex.width && ty >= 0 && ty < tex.height)
                            {
                                tex.SetPixel(tx, ty, colors[currentColorIndex]);
                            }
                        }
                    }
                }

                tex.Apply();
            }
        }
    }
}
