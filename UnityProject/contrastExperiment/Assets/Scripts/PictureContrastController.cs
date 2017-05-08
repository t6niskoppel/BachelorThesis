using UnityEngine;
using UnityEngine.UI;


public class PictureContrastController : MonoBehaviour {

    public RawImage rawImageLeft;
    public RawImage rawImageRight;
    public Texture2D textureLeft;
    public Texture2D textureRight;

	void Start () {
        rawImageLeft.texture = textureLeft;
        rawImageRight.texture = textureRight;
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float rightContrast = Random.Range(0f, 1f);
            float leftContrast = Random.Range(0f, 1f);
            ChangeContrast(rawImageRight,textureRight, rightContrast);
            ChangeContrast(rawImageLeft,textureLeft, leftContrast);
        }
	}

    void ChangeContrast(RawImage rw,Texture2D texture, float contrastValue)
    {
        Color32[] pixels = texture.GetPixels32();
        for (int i = 0; i < pixels.Length; i++)
        {
            Color32 c = pixels[i];
            byte red = toByte((int)(((((c.r / 255f) - 0.5f) * contrastValue) + 0.5f) * 255));
            byte green = toByte((int)(((((c.g / 255f) - 0.5f) * contrastValue) + 0.5f) * 255));
            byte blue = toByte((int)(((((c.b / 255f) - 0.5f) * contrastValue) + 0.5f) * 255));
            pixels[i] = new Color32(red, green, blue, (c.a));
        }
        Texture2D temp = new Texture2D(texture.width,texture.height);
        temp.SetPixels32(pixels);
        temp.Apply();
        rw.texture = temp;
    }

    byte toByte(int n)
    {
        if (n > 255)
        { return (byte)255; }
        else if (n < 0)
        { return (byte)0; }
        else return (byte)n;
    }
}
