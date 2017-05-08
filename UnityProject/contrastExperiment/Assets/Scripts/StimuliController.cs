using UnityEngine;
using UnityEngine.UI;

/**
 * 
 * Used to show or hide RawImages, assigning Texture2D to RawImage and changing Texture2D contrast
 * 
 */

public class StimuliController : MonoBehaviour
{
    public RawImage rawImageLeft;
    public RawImage rawImageRight;
    public HandController handController;

    public void hideStimuli()
    {
        rawImageLeft.enabled = false;
        rawImageRight.enabled = false;
    }

    public void showStimuli()
    {
        rawImageLeft.transform.LookAt(handController.centerEyeAnchor);
        rawImageRight.transform.LookAt(handController.centerEyeAnchor);
        rawImageLeft.enabled = true;
        rawImageRight.enabled = true;
    }

    public void prepareStimuli(float leftContrast, float rightContrast, Texture2D leftTexture, Texture2D rightTexture)
    {
        ChangeContrast(rawImageRight, leftTexture, rightContrast);
        ChangeContrast(rawImageLeft, rightTexture, leftContrast);
    }

    //applies texture with specified contrastValue to the RawImage rw
    void ChangeContrast(RawImage rw, Texture2D texture, float contrastValue)
    {
        Color32[] pixels = texture.GetPixels32();
        for (int i = 0; i < pixels.Length; i++)
        {
            Color32 c = pixels[i];
            byte red = toByte(Mathf.RoundToInt((((((c.r / 255f) - 0.5f) * contrastValue) + 0.5f) * 255)));
            byte green = toByte(Mathf.RoundToInt((((((c.g / 255f) - 0.5f) * contrastValue) + 0.5f) * 255)));
            byte blue = toByte(Mathf.RoundToInt((((((c.b / 255f) - 0.5f) * contrastValue) + 0.5f) * 255)));
            pixels[i] = new Color32(red, green, blue, (c.a));
        }
        Texture2D temp = new Texture2D(texture.width, texture.height);
        temp.SetPixels32(pixels);
        temp.Apply();
        rw.texture = temp;
    }

    byte toByte(int n)
    {
        if (n > 255)  return 255;
        else if (n < 0) return 0; 
        else return (byte) n;
    }
}
