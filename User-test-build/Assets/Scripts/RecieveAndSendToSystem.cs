using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.AR;
using Unity.VisualScripting;
using Assets.Scripts;

// This class was required to send information to the system GameObject
public class RecieveAndSendToSystem : MonoBehaviour
{
    public GameObject magnet_held;
    public GameObject sample_system;
    private InteractorHandedness which_hand;
    private MagneticVibration system;

    // Gets controller Object, handedness attribute from self NearFarInteractor and system
    void Start()
    {
        NearFarInteractor controller = this.GetComponent<NearFarInteractor>();
        which_hand = controller.handedness;
        
        system = sample_system.GetComponent<MagneticVibration>(); // For MagnetVibration component
    }

    /**
    /* obtains magnet sent from MagnetClass and sens to system's MagnetVibration component
    /* @param[in] magnet - MagnetClass - magnet to send
    */
    public void get_magnet(MagnetClass magnet)
    {
        // Checking if controller hand is either left or right (it's a InteractorHandedness object)
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
