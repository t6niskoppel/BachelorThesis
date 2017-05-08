using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/**
 * 
 * Used for enabling/diabling the fixation point and showing the cue
 * 
 */

public class FixationPointController : MonoBehaviour {

    public Image dot;
    public float visibleDuration;
    public Color flashColor = Color.red;
    public Color mainColor = Color.black;
    public Color disabledColor = Color.gray;
	
	
    public void EnableFixationPoint()
    {
        dot.color = mainColor;
    }

    public void DisableFixationPoint()
    {
        dot.color = disabledColor;
    }

    public IEnumerator ShowCue()
    {
        dot.color = flashColor;
        yield return new WaitForSeconds(visibleDuration);
        dot.color = mainColor;
    }

}
