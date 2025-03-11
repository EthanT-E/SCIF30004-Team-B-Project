using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class scannerswitch : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor;
    public InputActionReference button;
    public InteractionLayerMask mag_layer;
    public InteractionLayerMask scanner_layer;
    public GameObject ScannerUI;
    // Update is called once per frame
    private bool Scanner = false;
    void Update()
    {
        button.action.started += switch_layer;
    }

    void switch_layer(InputAction.CallbackContext context)
    {
        Scanner = !Scanner;
        if (Scanner ==true)
        {
            interactor.interactionLayers = scanner_layer;
            ScannerUI.SetActive(true);
        }
        else
        {
            interactor.interactionLayers = mag_layer;
            ScannerUI.SetActive(false);
        }
    }
}
