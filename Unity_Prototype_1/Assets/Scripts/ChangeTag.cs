using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ChangeTag : MonoBehaviour
{
    public GameObject Magnet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void change_tag()
    {
        if(Magnet.tag!="UI")
        {
            Magnet.tag="UI";
        }
        else
        {
            Magnet.tag="NotUI";
        }
    }
}