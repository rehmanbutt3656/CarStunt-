using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Click : MonoBehaviour {
    public Material[] skies;
    Image[] images;
	// Use this for initialization
	void Start () {
        images = new Image[20];
        for (int i = 0; i < 20; i++)
        {
            GameObject sky = transform.GetChild(i).gameObject;
            images[i] = sky.GetComponent<Image>();
        }
	}

    public void SetSky(int i)
    {
        if (i < 0 || i > 19) return;
        RenderSettings.skybox = skies[i];
    }

    public void LightUp(int i)
    {
        if (i < 0 || i > 19) return;
        images[i].color = new Color(1, 1, 1, 1);
    }

    public void LightOff(int i)
    {
        if (i < 0 || i > 19) return;
        images[i].color = new Color(1, 1, 1, 0.5f);
    }
}
