using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
public class GenerateMagnet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    BField bscript;
    public GameObject magnetPrefab;
    public Slider auxslider;
    public Slider magslider;
    Vector3 pos = new Vector3(-2, 1, 0);
    void Start()
    {
        bscript = GameObject.Find("System").GetComponent<BField>();
    }

    void Update()
    {
    }

    // Update is called once per frame
    public void button_press()
    {
        Vector3 aux = new Vector3(1,1,auxslider.value);
        Magnet_class.Generate_magnet(magnetPrefab, bscript.magnets, pos,aux,magslider.value);
    }
}
