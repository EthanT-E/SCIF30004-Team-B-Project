using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using TMPro;
public class MagUI : MonoBehaviour
{
    BField bscript;
    public Slider auxslider;
    public Slider magslider;

    public TextMeshProUGUI auxsliderText;
    public TextMeshProUGUI magsliderText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bscript = GameObject.Find("System").GetComponent<BField>();
    }

    // Update is called once per frame
    void Update()
    {
        auxsliderText.text = auxslider.value.ToString();
        magsliderText.text = magslider.value.ToString();
        for(int i = 0; i < bscript.magnets.Count; i++)
        {
            if (bscript.magnets[i].Magnet.tag=="UI")
            {
                if (auxslider.value!=bscript.magnets[i].auxilary_field.y)
                {
                    bscript.magnets[i].set_auxiliary(new Vector3(0,0,(Mathf.Abs(auxslider.value) * 5.0f) * Mathf.Sign(auxslider.value)));
                    bscript.magnets[i].UI_value_change = true;
                }
                if (magslider.value!=bscript.magnets[i].magnetic_susceptibility)
                {
                    bscript.magnets[i].set_suscept((magslider.value+2) * 1.75f);
                    bscript.magnets[i].UI_value_change = true;
                }
            }
        }
    }
    public void delete_magnet()
    {
    for(int i = 0; i < bscript.magnets.Count; i++)
        {
            if (bscript.magnets[i].Magnet.tag=="UI")
            {
                Destroy(bscript.magnets[i].Magnet);
                bscript.magnets.RemoveAt(i);
                bscript.magnets[i].UI_value_change = true;
            }
        }
    }
}