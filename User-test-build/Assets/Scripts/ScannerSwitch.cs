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
        bscript = GameObject.Find("System").GetComponent<BField>(); //accesses b field calculation
    }
    void Update()
    {
        button.action.started += switch_layer; //if secondary button pressed then switch_layer function called
        
        trigger.action.started += calc_Bfield; //if trigger is pressed then calculates bfield
        
    }

    void switch_layer(InputAction.CallbackContext context)
    {
        Scanner = !Scanner; //toggles between scanner device and controller
        if (Scanner)
        {
            interactor.interactionLayers = scanner_layer; //changes to scanner interaction layer
            ScannerUI.SetActive(true); //disables and enables relevant controller objects
            scanner_object.SetActive(true);
            Controller.SetActive(false);
            RayInteractor.SetActive(false);
            PokeInteractor.SetActive(false);
        }
        else
        {
            interactor.interactionLayers = mag_layer; //changes to magnet interaction layer
            ScannerUI.SetActive(false);
            scanner_object.SetActive(false);
            Controller.SetActive(true);
            RayInteractor.SetActive(true);
            PokeInteractor.SetActive(true);
        }
    }

    void calc_Bfield(InputAction.CallbackContext context)
    {
        if (Scanner) //if in scanner layer and the trigger is pressed recalculates bfield and outputs magnitude to the UI
        {
            Vector3 b_field = bscript.calculate_b_field(scanner_sphere.transform.position);
            float magvalue = Vector3.Magnitude(b_field);
            ScannerUI.transform.Find("BFieldMag").GetComponent<TMP_Text>().text = magvalue.ToString("E2");
        }
    }
}

