using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.AR;
using Unity.VisualScripting;
using Assets.Scripts;

public class RecieveAndSendToSystem : MonoBehaviour
{
    public GameObject magnet_held;
    public GameObject sample_system;
    private InteractorHandedness which_hand;
    private MagneticVibration system;

    void Start()
    {
        NearFarInteractor controller = this.GetComponent<NearFarInteractor>();
        which_hand = controller.handedness;

        system = sample_system.GetComponent<MagneticVibration>();
    }

    public void get_magnet(Magnet_class magnet)
    {
        if (which_hand == InteractorHandedness.Left)
        {
            system.set_left_magnet(magnet);
        }
        else if (which_hand == InteractorHandedness.Right)
        {
            system.set_right_magnet(magnet);
        }
    }
}
