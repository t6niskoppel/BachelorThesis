using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSlider : MonoBehaviour {

    public Slider slider;

    public void ChangeValue()
    {
        float value = slider.value;
        float.TryParse(this.GetComponent<InputField>().text, out value);
        slider.value = value;
    }
}
