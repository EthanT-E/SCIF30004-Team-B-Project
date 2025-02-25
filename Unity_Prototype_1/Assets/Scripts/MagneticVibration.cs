using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class MagneticVibration : MonoBehaviour
{
    public GameObject leftMagnet;
    public GameObject rightMagnet;

    public InputDevice leftController;
    public InputDevice rightController;

    float Calculate_force(GameObject magnet1, GameObject magnet2)
    {
        Vector3 dist = magnet1.transform.position - magnet1.transform.position;
        Vector3 dist_norm = dist.normalized;

        Vector3 dipole_moment1 = new Vector3(1, 5, 1); // Must replace if magnet properties wanted to be customised.
        Vector3 dipole_moment2 = new Vector3(1, 5, 1); // Must replace if magnet properties wanted to be customised.

        float dip1_dot_dip2 = Vector3.Dot(dipole_moment1, dipole_moment2);
        float distn_dot_dip1 = Vector3.Dot(dist_norm, dipole_moment1);
        float distn_dot_dip2 = Vector3.Dot(dist_norm, dipole_moment2);

        Vector3 force = (3 * 1 * dipole_moment1.magnitude * dipole_moment2.magnitude / (4 * Mathf.PI * Mathf.Pow(dist.magnitude, 4))) * 
            ((dist_norm * dip1_dot_dip2) + (dipole_moment1 * distn_dot_dip1) + (dipole_moment2 * distn_dot_dip2) - (5 * dist_norm * distn_dot_dip1 * distn_dot_dip2));

        return force.magnitude;

    }

    void Update()
    {
        Debug.Log(Calculate_force(leftMagnet, rightMagnet));
        if (leftMagnet == null || rightMagnet == null)
        {
            Debug.Log("aint no magnet held enough");
            leftController.SendHapticImpulse(0,0, 0.1f);
            rightController.SendHapticImpulse(0,0, 0.1f);
        }
        else
        {
            float intensity = Mathf.InverseLerp(10.0f, 0.001f, Calculate_force(leftMagnet, rightMagnet));
            leftController.SendHapticImpulse(0, intensity, 0.1f);
            rightController.SendHapticImpulse(0, intensity, 0.1f);
        }
    }

}
