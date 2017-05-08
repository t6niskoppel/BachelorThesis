using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class GaborGenerator : MonoBehaviour {

	public RawImage rawImage;
    public Text sizeText;
    public Text lambdaText;
    public Text thetaText;
    public Text sigmaText;
    public Text phaseText;
    public Text trimText;
    public Text fileName;
    public Button saveButton;
    public Button drawButton;

    public Slider slider;
    public Text contrastValue;
    private Texture2D current;

	void Start () {
        current = gaborMaker(80, 12f, 45f, 10f, 0f, 0.01f);
        rawImage.texture = current;
        
        saveButton.onClick.AddListener(Save);
        drawButton.onClick.AddListener(createGabor);
        slider.onValueChanged.AddListener(delegate { contrastValueUpdate(); }); 
    }

	void createGabor(){
        int size = int.Parse(sizeText.text);//size of the picture (size x size)
        float lambda = float.Parse(lambdaText.text); //length of the sine wave
        float theta = float.Parse(thetaText.text); //orientation of the gabor grating
        float sigma = float.Parse(sigmaText.text); //size of gaussian standard deviation in pixels
        float phase = float.Parse(phaseText.text); //the phase of the wave in interval [0,1]
        float trim = float.Parse(trimText.text); //trim off gaussian values smaller than this

        current = gaborMaker(size, lambda, theta, sigma, phase, trim);
        rawImage.texture = current;
        ChangeContrast();
    }

    //Generates a gabor patch on a Texture2D element
    //http://www.icn.ucl.ac.uk/courses/MATLAB-Tutorials/Elliot_Freeman/html/gabor_tutorial.html
    public Texture2D gaborMaker(int size, float lambda, float theta, float sigma, float phase, float trim) 
    {
        float pi = Mathf.PI;

        Color[] mesh = new Color[size * size];

        float freq = size / lambda;
        float thataRad = (theta / 360) * 2 * pi;
        float s = size/sigma;
        float radPhase = phase * 2 * pi;

        for (int i = 0; i < size; i++)
        {
            float Y = i * 1f / (size-1) - .5f;

            for (int j = 0; j < size; j++)
            {
                float X = j * 1f / (size-1) - .5f;

                float xprim = X * Mathf.Cos(thataRad);
                float yprim = Y * Mathf.Sin(thataRad);

                float radXY = (xprim + yprim) * freq * 2 * pi;

                float f = Mathf.Sin(radXY+radPhase);
                f = (f + 1) * .5f; //f to the interval [0,1]

                float a = Mathf.Exp(-(X*X+Y*Y)/2*s*s); //The value of distribution at the point (x,y)
                if (a <= trim) a = 0;
                f = ((f - .5f) * a + .5f);

                mesh[(i * size) + j] = new Color(f, f, f, 1.0f); ;
            }
        }
        
        Texture2D temp = new Texture2D(size, size);
        temp.SetPixels(mesh);
        temp.Apply();
        return temp;
    }

    private void Save() //Saves the Texture2D as .PNG
    {
        byte[] bytes = current.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/GaborPictures/" + fileName.text + ".png", bytes);
    }

    void contrastValueUpdate() //Updates the text value of the slider
    {
        contrastValue.text = "" + slider.value * 100 + "%";
    }

    void ChangeContrast() //Used for changing Texture2D contrast
    {
        float contrastValue = slider.value;
        Texture2D mesh = rawImage.texture as Texture2D;
        Color32[] pixels = mesh.GetPixels32();
        for (int i = 0; i < pixels.Length; i++)
        {
            Color32 c = pixels[i];
            byte red = toByte((int)((((((c.r+1) / 256f) - 0.5f) * contrastValue) + 0.5f) * 256) -1);
            byte green = toByte((int)((((((c.g+1)  / 256f) - 0.5f) * contrastValue) + 0.5f) * 256) -1);
            byte blue = toByte((int)((((((c.b+1)  / 256f) - 0.5f) * contrastValue) + 0.5f) * 256) -1);
            pixels[i] = new Color32(red, green, blue, (c.a));
        }
        mesh.SetPixels32(pixels);
        mesh.Apply();
    }

    byte toByte(int n) // int to byte
    {
        if (n > 255)
        { return 255; }
        else if (n < 0)
        { return 0; }
        else return (byte)n;
    }
}
