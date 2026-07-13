using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class GridOverlay : MonoBehaviour
{
    public int gridSize = 64;
    public int lineThickness = 2;
    public Color lineColor = Color.gray;
    public Color backgroundColor = Color.black;

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        Texture2D tex = new Texture2D(gridSize, gridSize);
        Color[] pixels = new Color[gridSize * gridSize];

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                bool isBorder = x < lineThickness || y < lineThickness;
                pixels[y * gridSize + x] = isBorder ? lineColor : backgroundColor;
            }
        }

        tex.SetPixels(pixels);
        tex.Apply();
        tex.wrapMode = TextureWrapMode.Repeat;

        RawImage img = GetComponent<RawImage>();
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
        img.texture = tex;

        float uvWidth = img.rectTransform.rect.width / gridSize;
        float uvHeight = img.rectTransform.rect.height / gridSize;
        img.uvRect = new Rect(0, 0, uvWidth, uvHeight);
    }
}