using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ChangeTag : MonoBehaviour
{
    public GameObject Magnet;
    private Material blue_emission;
    private Material red_emission;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        blue_emission = Magnet.GetComponent<MeshRenderer>().materials[0];
        red_emission = Magnet.GetComponent<MeshRenderer>().materials[1];
    }
    public void change_tag()
    {
        if(Magnet.tag!="UI")
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