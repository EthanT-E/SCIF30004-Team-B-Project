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
    // Store left controller and right controller used in game (only tested on Quest 2)
    public InputDevice leftController;
    public InputDevice rightController;

    // store MagnetClass instances for held magnets
    private MagnetClass leftMagnet;
    private MagnetClass rightMagnet;

    public TextMeshProUGUI textmesh; // for displaying force - only was for debugging

    // Stores information about left and right controllers
    void Start()
    {
        leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }


    void Update()
    {
        textmesh.text = $"Left Magnet: {leftMagnet}, Right Magnet: {rightMagnet}"; //sets text to be what is held
        
        // DO NOT APPLY HAPTIC IMPULSE IF BOTH HANDS ARE NOT HOLDING MAGNET
        if (leftMagnet == null || rightMagnet == null)
        {
            leftController.SendHapticImpulse(0, 0, 0.1f);
            rightController.SendHapticImpulse(0, 0, 0.1f);
        }
        
        else
        {
            Debug.Log(Calculate_force(leftMagnet, rightMagnet));
            textmesh.text = $"Magnetic FOrce {Calculate_force(leftMagnet, rightMagnet)} N";
            float clampedForce = Mathf.Clamp(Calculate_force(leftMagnet, rightMagnet)/10000.0f, 400.0f, 2000.0f); // RESTRICT FORCE SO IT'S NOT TOO STRONG
            float intensity = Mathf.InverseLerp(400.0f, 2000.0f, clampedForce); // Normalise force to be between ranges

            // Apply haptic feedback (vibration) to controllers
            leftController.SendHapticImpulse(0, intensity, 0.1f);
            rightController.SendHapticImpulse(0, intensity, 0.1f);
        }
    }

    /**
    /* Calculates force vector between two magnets
    /* Is dependant on magnet properties encased in MagnetClass
    /* @param[in] magnet1 - MagnetClass - magnet to calculate force with
    /* @param[in] magnet2 - MagnetClass - magnet to calculate force with
    /* @return - float of Force vector between two magnets - only need to return magnitude
    */
    float Calculate_force(MagnetClass magnet1, MagnetClass magnet2)
    {
        Vector3 dist = magnet1.magnet_position - magnet2.magnet_position;
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

    // assigns leftMagnet to be what was sent
    public void set_left_magnet(MagnetClass magnet)
    {
        leftMagnet = magnet;
    }

    // assigns righttMagnet to be what was sent
    public void set_right_magnet(MagnetClass magnet)
    {
        rightMagnet = magnet;
    }

}
