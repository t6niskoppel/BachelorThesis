using UnityEngine;
using UnityEngine.UI;

/**
 * 
 * Used to change stimulus contrast in the Gabor generation application
 * 
 */

public class rawImageContrast : MonoBehaviour {

    public RawImage rawImage;
    public Slider slider;
    public Text contrastValue;

	void Start () {
        slider.onValueChanged.AddListener(delegate { contrastValueUpdate(); });
	}

    void contrastValueUpdate()
    {
        contrastValue.text = "" + slider.value * 100 + "%";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeContrast();
        }
    }

    void ChangeContrast()
    {
        float contrastValue = slider.value;
        Texture2D mesh = rawImage.texture as Texture2D;
        Color32[] pixels = mesh.GetPixels32();
        for(int i = 0; i < pixels.Length; i++)
        {
            Color32 c = pixels[i];
            byte red =  toByte((int) ((((( (c.r +1) / 256f) - 0.5f) * contrastValue) + 0.5f)* 256)-1);
            byte green = toByte((int)((((((c.g +1) / 256f) - 0.5f) * contrastValue) + 0.5f) * 256)-1);
            byte blue = toByte((int)((((((c.b +1) / 256f) - 0.5f) * contrastValue) + 0.5f) * 256)-1);
            pixels[i] = new Color32(red, green, blue, (c.a));
        }
        mesh.SetPixels32(pixels);
        mesh.Apply();
    }

    byte toByte(int n)
{
    if (n >= 255)
    { return (byte)255; }
    else if (n <= 0)
    { return (byte)0; }
    else return (byte)n;
}
	
	
}
