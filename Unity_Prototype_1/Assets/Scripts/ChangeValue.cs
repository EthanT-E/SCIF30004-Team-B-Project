using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChangeValue : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI sliderText;
    
    void Start()
    {

    }

    void Update()
    {
        sliderText.text = slider.value.ToString();
    }
}
