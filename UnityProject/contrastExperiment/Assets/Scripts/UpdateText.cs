using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateText : MonoBehaviour {

    public InputField inputField;

    public void ChangeValue()
    {
        inputField.text = ""+this.GetComponent<Slider>().value;
    }
}
