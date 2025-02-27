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
    bool buttonpress=false;
    public Slider auxslider;
    public Slider magslider;
    void Start()
    {
        bscript = GameObject.Find("ScriptAs ObjectName").GetComponent<BField>();
    }

    // Update is called once per frame
    public void button_press()
    {
        GameObject magnet = Instantiate(magnetPrefab);
        Magnet_class newmag = new Magnet_class(magnet,new Vector3(-2.5f,1,0));
        newmag.set_suscept(magslider.value);
        newmag.set_auxiliary(new Vector3(0,auxslider.value,0));
        bscript.magnets.Add(newmag);
    }
}
