using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using TMPro;
public class GenerateMagnet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    BField bscript;
    public GameObject magnetPrefab;      
    public Slider auxslider;
    public TextMeshProUGUI auxsliderText;
    public TextMeshProUGUI magsliderText;
    public Slider magslider;
    private Vector3 pos;
    void Start()
    {
        bscript = GameObject.Find("System").GetComponent<BField>(); //gets bfield script, so new magnet can be added to list
    }

    void Update()
    {
        auxsliderText.text = auxslider.value.ToString();
        magsliderText.text = magslider.value.ToString();
    }
    
    public void button_press() //called when generate button is pressed in unity
    {
        Vector3 aux = new Vector3(0, 0, (Mathf.Abs(auxslider.value) * 5.0f) * Mathf.Sign(auxslider.value)); //sets auxillary field
        pos = new Vector3(Random.Range(-1.8f, -2.2f), 1, Random.Range(-0.5f, 0.5f)); //set to random position on table
        MagnetClass.Generate_magnet(magnetPrefab, bscript.magnets, pos, aux, (magslider.value+2) * 1.75f); //generates a magnet with properties on sliders
    }
}
