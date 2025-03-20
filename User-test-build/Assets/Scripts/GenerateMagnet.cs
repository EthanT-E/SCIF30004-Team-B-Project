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
    private Vector3 pos;
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
        Vector3 aux = new Vector3(0, 0, (Mathf.Abs(auxslider.value) * 5.0f) * Mathf.Sign(auxslider.value));
        pos = new Vector3(Random.Range(-1.8f, -2.2f), 1, Random.Range(-0.5f, 0.5f));
        Magnet_class.Generate_magnet(magnetPrefab, bscript.magnets, pos, aux, (magslider.value+2) * 1.75f);
    }
}
