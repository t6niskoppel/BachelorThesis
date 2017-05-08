using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateGabor : MonoBehaviour {

	private RawImage rawImage;
    [Tooltip("Tekstuuri suurus pikslites")]
    [SerializeField]  public int size;
    [Tooltip("Siinuslaine pikkus")]
    [SerializeField] public float lambda;
    [Tooltip("Siinuslaine orientatsioon")]
    [SerializeField]  public float theta;
    [Tooltip("Normaaljaotuse standardhälve")]
    [SerializeField]  public float sigma;
    [Tooltip("Siinuslaine faas")]
    [SerializeField] public float phase;
    [Tooltip("Sellest väiksema alpha väärtusega pikslid jäetakse välja")]
    [SerializeField]  public float trim;
    [Tooltip("Gabori läbipaistvus., vahemik [0,1]")]
    [SerializeField]  public float alpha;

	void Start () {
		rawImage = GetComponent<RawImage> ();
        rawImage.texture = gaborMaker(size, lambda, theta, sigma, phase, trim); //teen default gabori
        Color c = rawImage.color;
        rawImage.color = new Color(c.r, c.g, c.b, alpha);
    }

	void Update(){
        if (Input.GetKeyDown(KeyCode.Space)) { 
            rawImage.texture = gaborMaker(size, lambda, theta, sigma, phase, trim);
            Color c = rawImage.color;
            rawImage.color = new Color(c.r, c.g, c.b, alpha);
        }
    }
    

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
                f = (f + 1) * .5f; //f vahemikku [0,1]
                
                float a = Mathf.Exp(-(X*X+Y*Y)/2*s*s);

                if (a <= trim) a = 0;
                
                mesh[(i * size) + j] = new Color(f, f, f, a); ;
            }
        }
        
        Texture2D temp = new Texture2D(size, size);
        temp.SetPixels(mesh);
        temp.Apply();
        return temp;
    }
}
