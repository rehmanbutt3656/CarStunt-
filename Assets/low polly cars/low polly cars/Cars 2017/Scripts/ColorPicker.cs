using UnityEngine;
using System.Collections;

// relies on: http://forum.unity3d.com/threads/12031-create-random-colors?p=84625&viewfull=1#post84625

public class ColorPicker : MonoBehaviour
{


    public bool showPicker = false;
    public Color setColor;
    // the solid texture which everything is compared against
    public Texture2D colorPicker;

    [HideInInspector]
    public bool clickDown = false;
    [HideInInspector]
    public Color lastSetColor;

    // the picker being displayed
    private Texture2D displayPicker;

    private int positionLeft = 0;
    private int positionTop = 0;

    private int textureWidth = 0;
    private int textureHeight = 0;

    private Texture2D saturationTexture;
    private Texture2D styleTexture;


    void Awake()
    {

        colorPicker.Apply();
        displayPicker = colorPicker;

        textureWidth = colorPicker.width;
        textureHeight = colorPicker.height;

        float v = 0.0F;
        float diff = 1.0f / textureHeight;
        saturationTexture = new Texture2D(20, textureHeight);
        for (int i = 0; i < saturationTexture.width; i++)
        {
            for (int j = 0; j < saturationTexture.height; j++)
            {
                saturationTexture.SetPixel(i, j, new Color(v, v, v));
                v += diff;
            }
            v = 0.0F;
        }
        saturationTexture.Apply();

    }


    void OnGUI()
    {

        positionLeft = (Screen.width) - (textureWidth);
        positionTop = (Screen.height) - (textureHeight);

        if (!showPicker) return;

        if (GUI.RepeatButton(new Rect(positionLeft, positionTop, textureWidth, textureHeight), displayPicker))
        {
            int a = (int)Input.mousePosition.x;
            int b = Screen.height - (int)Input.mousePosition.y;
            clickDown = true;
            setColor = displayPicker.GetPixel(a - positionLeft, -(b - positionTop));
            lastSetColor = setColor;
        }
    }

}