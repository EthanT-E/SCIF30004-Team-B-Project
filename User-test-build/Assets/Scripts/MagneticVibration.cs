using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using TMPro;
using Assets.Scripts;
public class MagneticVibration : MonoBehaviour
{

    public InputDevice leftController;
    public InputDevice rightController;

    private Magnet_class leftMagnet;
    private Magnet_class rightMagnet;

    public TextMeshProUGUI textmesh;
    void Start()
    {
        leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }


    void Update()
    {
        textmesh.text = $"Left Magnet: {leftMagnet}, Right Magnet: {rightMagnet}";
        if (leftMagnet == null || rightMagnet == null)
        {
            leftController.SendHapticImpulse(0, 0, 0.1f);
            rightController.SendHapticImpulse(0, 0, 0.1f);
        }
        else
        {
            Debug.Log(Calculate_force(leftMagnet, rightMagnet));
            textmesh.text = $"Magnetic FOrce {Calculate_force(leftMagnet, rightMagnet)} N";
            float clampedForce = Mathf.Clamp(Calculate_force(leftMagnet, rightMagnet), 400.0f, 2000.0f);
            float intensity = Mathf.InverseLerp(400.0f, 2000.0f, clampedForce);
            leftController.SendHapticImpulse(0, intensity, 0.1f);
            rightController.SendHapticImpulse(0, intensity, 0.1f);
        }
    }
    float Calculate_force(Magnet_class magnet1, Magnet_class magnet2)
    {
        Vector3 dist = magnet1.MagnetPosition - magnet2.MagnetPosition;
        Vector3 dist_norm = dist.normalized;

        Vector3 dipole_moment1 = magnet1.dipole_moment;
        Vector3 dipole_moment2 = magnet2.dipole_moment;

        float dip1_dot_dip2 = Vector3.Dot(dipole_moment1, dipole_moment2);
        float distn_dot_dip1 = Vector3.Dot(dist_norm, dipole_moment1);
        float distn_dot_dip2 = Vector3.Dot(dist_norm, dipole_moment2);

        Vector3 force = (3 * 1 * dipole_moment1.magnitude * dipole_moment2.magnitude / (4 * Mathf.PI * Mathf.Pow(dist.magnitude, 4))) *
            ((dist_norm * dip1_dot_dip2) + (dipole_moment1 * distn_dot_dip1) + (dipole_moment2 * distn_dot_dip2) - (5 * dist_norm * distn_dot_dip1 * distn_dot_dip2));

        return force.magnitude;
    }

    public void set_left_magnet(Magnet_class magnet)
    {
        leftMagnet = magnet;
    }

    public void set_right_magnet(Magnet_class magnet)
    {
        rightMagnet = magnet;
    }

}
