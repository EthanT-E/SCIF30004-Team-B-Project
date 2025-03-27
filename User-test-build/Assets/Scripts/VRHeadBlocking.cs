using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//See https://metaanomie.blogspot.com/2020/04/unity-vr-head-blocking-steam-vr-v2.html for more information
public class VRHeadBlocking : MonoBehaviour
{
    public GameObject player;

    private int layerMask;
    private Collider[] objs = new Collider[10];
    private Vector3 prevHeadPos;
    private float backupCap = .2f;

    private void Start()
    {
        layerMask = 1 << 8;
        layerMask = ~layerMask;

        prevHeadPos = transform.position;
    }

    /**
    /* Detects whether the collider at the camera has hit anything
    /* @param[in] loc - Vector3 - position vector of camera
    */
    private int DetectHit(Vector3 loc)
    {
        int hits = 0;
        int size = Physics.OverlapSphereNonAlloc(loc, backupCap, objs, layerMask, QueryTriggerInteraction.Ignore);
        for (int i = 0; i < size; i++)
        {
            //Make sure anything in the VR rig that could collide with the camera is set to use tag Player
            if (objs[i].tag != "Player" && objs[i].tag != "UI" && objs[i].tag != "NotUI" )
            {
                hits++;
            }
        }
        return hits;
    }

    public void Update()
    {
        if (player != null)
        {

            int hits = DetectHit(transform.position);

            // No collision
            if (hits == 0) prevHeadPos = transform.position;

            // Collision
            else
            {
                // Player pushback
                Vector3 headDiff = transform.position - prevHeadPos;
                if (Mathf.Abs(headDiff.x) > backupCap)
                {
                    if (headDiff.x > 0) headDiff.x = backupCap;
                    else headDiff.x = backupCap * -1;
                }
                if (Mathf.Abs(headDiff.z) > backupCap)
                {
                    if (headDiff.z > 0) headDiff.z = backupCap;
                    else headDiff.z = backupCap * -1;
                }
                Vector3 adjHeadPos = new Vector3(player.transform.position.x - headDiff.x,
                                                 player.transform.position.y,
                                                 player.transform.position.z - headDiff.z);
                player.transform.SetPositionAndRotation(adjHeadPos, player.transform.rotation);
            }
        }
    }
}