using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
public class MagUI : MonoBehaviour
{
    BField bscript;
    public Slider auxslider;
    public Slider magslider;
    public GameObject Mag_UI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Mag_UI.SetActive(true);
        bscript = GameObject.Find("System").GetComponent<BField>();
    }

    // Update is called once per frame
    void Update()
    {
        bool UI_on = false;
        for(int i = 0; i < bscript.magnets.Count; i++)
        {
            if (bscript.magnets[i].Magnet.tag=="UI")
            {
                UI_on = true;
                if (auxslider.value!=bscript.magnets[i].auxilary_field.y)
                {
                    bscript.magnets[i].set_auxiliary(new Vector3(auxslider.value,0,0));
                    bscript.magnets[i].UI_value_change = true;
                }
                if (magslider.value!=bscript.magnets[i].magnetic_susceptibility)
                {
                    bscript.magnets[i].set_suscept(magslider.value);
                    bscript.magnets[i].UI_value_change = true;
                }
            }
        }
        if (UI_on==true)
        {
            Mag_UI.SetActive(true);
        }
        else
        {
            Mag_UI.SetActive(false);
        }
    }
}