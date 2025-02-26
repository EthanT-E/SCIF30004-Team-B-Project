using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using TMPro;

public class MagneticVibration : MonoBehaviour
{

    public InputDevice leftController;
    public InputDevice rightController;

    public XRDirectInteractor leftInteractor;
    public XRDirectInteractor rightInteractor;

    private GameObject leftMagnet;
    private GameObject rightMagnet;

    public TextMeshProUGUI textmesh;
    void Start()
    {
        leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        leftInteractor.selectEntered.AddListener(On_left_grabbed);
        rightInteractor.selectEntered.AddListener(On_right_grabbed);
        
        leftInteractor.selectExited.AddListener(On_left_released);
        rightInteractor.selectExited.AddListener(On_right_released);
    }


    void Update()
    {
        textmesh.text = $"Left Magnet: {leftMagnet}, Right Magnet: {rightMagnet}";
        if (leftMagnet == null || rightMagnet == null)
        {
            leftController.SendHapticImpulse(0,0, 0.1f);
            rightController.SendHapticImpulse(0,0, 0.1f);
        }
        else
        {
            Debug.Log(Calculate_force(leftMagnet, rightMagnet));
            textmesh.text = $"Magnetic FOrce {Calculate_force(leftMagnet, rightMagnet)} N";
            float intensity = Mathf.InverseLerp(0.1f, 0.001f, Calculate_force(leftMagnet, rightMagnet));
            leftController.SendHapticImpulse(0, 1, 0.1f);
            rightController.SendHapticImpulse(0, 1, 0.1f);
        }
    }
        float Calculate_force(GameObject magnet1, GameObject magnet2)
    {
        Vector3 dist = magnet1.transform.position - magnet2.transform.position;
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

    // Listener functions to check for controller events
    void On_left_grabbed(SelectEnterEventArgs args)
    {
        leftMagnet = args.interactableObject.transform.gameObject;
    }

    void On_right_grabbed(SelectEnterEventArgs args)
    {
        rightMagnet = args.interactableObject.transform.gameObject;
    }

    void On_left_released(SelectExitEventArgs args)
    {
        leftMagnet = null;
    }

    void On_right_released(SelectExitEventArgs args)
    {
        rightMagnet = null;
    }



}
