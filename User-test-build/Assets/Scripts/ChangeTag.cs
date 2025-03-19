using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class ChangeTag : MonoBehaviour
{
    public GameObject Magnet;
    private Material blue_emission;
    private Material red_emission;
    public InputActionReference right_button;
    public InputActionReference left_button;
    private bool UI_on = false;
    private bool hovering = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        blue_emission = Magnet.GetComponent<MeshRenderer>().materials[0];
        red_emission = Magnet.GetComponent<MeshRenderer>().materials[1];
    }

    void Update()
    {
        if (hovering)
        {
        left_button.action.started += change_tag;
        right_button.action.started += change_tag;
        }
    }

    public void is_hovering()
    {
        hovering = true;
    }

    public void not_hovering()
    {
        hovering = false;
        left_button.action.started -= change_tag;
        right_button.action.started -= change_tag;
    }
    void change_tag(InputAction.CallbackContext context)
    {
        UI_on = !UI_on;
        if(UI_on)
        {
            red_emission.EnableKeyword("_EMISSION");
            red_emission.SetColor("_EmissionColor",Color.red*200);

            blue_emission.EnableKeyword("_EMISSION");
            blue_emission.SetColor("_EmissionColor",Color.blue*200);
            Magnet.tag="UI";
        }
        else
        {
            red_emission.DisableKeyword("_EMISSION");
            red_emission.SetColor("_EmissionColor",Color.black*0);

            blue_emission.DisableKeyword("_EMISSION");
            blue_emission.SetColor("_EmissionColor",Color.black*0);
            Magnet.tag="NotUI";
        }
    }
}