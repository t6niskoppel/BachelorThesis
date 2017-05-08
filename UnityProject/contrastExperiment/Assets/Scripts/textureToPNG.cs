using UnityEngine;
using UnityEngine.UI;
using System.IO;
/**
 *
 * This class is used to save a Texture2D component as .JPG when using the Gabor generation application
 * 
 */
public class textureToPNG : MonoBehaviour {

    public RawImage rawImage;
    public InputField fileName;
    public Button saveButton;
    private Texture2D texture;

    public void Start()
    {
        texture = rawImage.mainTexture as Texture2D;
        saveButton.onClick.AddListener(saveGabor);
    }

    
    public void saveGabor()
    {
        Debug.Log("Saving gabor as " + fileName.text);
        Save(fileName.text);
    }

    private void Save(string filename)
    {
        Texture2D temp = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
        temp.SetPixels(texture.GetPixels());
        temp.Apply();
        byte[] bytes = temp.EncodeToJPG();
        File.WriteAllBytes(Application.dataPath + "/GaborPictures/" + filename + ".png",bytes);
    }
}
