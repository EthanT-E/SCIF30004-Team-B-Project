using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
public class scannerswitch : MonoBehaviour
{
    BField bscript;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor;
    public InputActionReference button;
    public InputActionReference trigger;
    public InteractionLayerMask mag_layer;
    public InteractionLayerMask scanner_layer;
    public GameObject ScannerUI;
    public GameObject scanner_object;
    public GameObject scanner_sphere;
    public GameObject Controller;
    public GameObject RayInteractor;
    public GameObject PokeInteractor;
    // Update is called once per frame
    private bool Scanner = false;
    void Start()
    {
        bscript = GameObject.Find("System").GetComponent<BField>();
    }
    void Update()
    {
        button.action.started += switch_layer;
        
        trigger.action.started += calc_Bfield;
        
    }

    void switch_layer(InputAction.CallbackContext context)
    {
        Scanner = !Scanner;
        if (Scanner)
        {
            interactor.interactionLayers = scanner_layer;
            ScannerUI.SetActive(true);
            scanner_object.SetActive(true);
            Controller.SetActive(false);
            RayInteractor.SetActive(false);
            PokeInteractor.SetActive(false);
        }
        else
        {
            interactor.interactionLayers = mag_layer;
            ScannerUI.SetActive(false);
            scanner_object.SetActive(false);
            Controller.SetActive(true);
            RayInteractor.SetActive(true);
            PokeInteractor.SetActive(true);
        }
    }

    void calc_Bfield(InputAction.CallbackContext context)
    {
        if (Scanner)
        {
            Vector3 b_field = bscript.calculate_b_field(scanner_sphere.transform.position);
            float magvalue = Vector3.Magnitude(b_field);
            ScannerUI.transform.Find("BFieldMag").GetComponent<TMP_Text>().text = magvalue.ToString("E2");
        }
    }
}

