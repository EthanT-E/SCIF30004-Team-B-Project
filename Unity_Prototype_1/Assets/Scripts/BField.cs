using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEditor.Events;

public class BField : MonoBehaviour
{
    public GameObject magnet;
    Vector3 magnet_position;
    Vector3 arrow_position;
    public GameObject arrowPrefab;
    public float colorscale;
    public float min_radius_of_influence;
    public List<GameObject> Arrows = new List<GameObject>();
    public Vector3 field_size = new Vector3(2,2,2);
    public float arrow_gap = 0.5f;
    public float b_factor = 1000;
    public float radius_of_influence = 2.0f;
    public float max_B_field_value = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generate_field(field_size,arrow_gap,Arrows); //generates the field of arrows
        magnet_position = magnet.transform.position; //gets magnet position
        min_radius_of_influence = arrow_gap/2;
        for (int i=0; i<Arrows.Count;i++) //iterates through all arrows //equal to line 37-44
        {
            arrow_position = Arrows[i].transform.position;
            float distance = Vector3.Distance(arrow_position, magnet_position);
            if (distance < radius_of_influence && distance > min_radius_of_influence)
            {
                Vector3 b_field = calculate_b_field(magnet_position, arrow_position, distance);
                Arrows[i].transform.rotation = Quaternion.LookRotation(b_factor * b_field); // gets arrow to point in b direction. increase the coeffeient also increases the effective range
                Arrows[i].SetActive(true);
                if(b_field.magnitude>max_B_field_value)
                {
                    max_B_field_value = b_field.magnitude;
                    print(max_B_field_value);
                }
            }
            else
            {
                Arrows[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 new_magnet_position = magnet.transform.position;
        if(magnet_position != new_magnet_position)
        {
            magnet_position = new_magnet_position;
            for (int i=0; i<Arrows.Count;i++)
            {

                arrow_position = Arrows[i].transform.position;
                float distance = Vector3.Distance(arrow_position, magnet_position);
                if (distance < radius_of_influence && distance > min_radius_of_influence)
                {
                    Vector3 b_field = calculate_b_field(magnet_position, arrow_position, distance);
                    Arrows[i].transform.rotation = Quaternion.LookRotation(b_factor * b_field); // gets arrow to point in b direction. increase the coeffeient also increases the effective range 
                    Arrows[i].SetActive(true);
                    colorscale = 60*(b_field.magnitude/max_B_field_value);
                    if(colorscale>1){
                        colorscale = 1f;
                    }
                    
                    print(colorscale);//debug
                    Arrows[i].GetComponent<MeshRenderer>().material.color = new Color(1,1-colorscale,0,colorscale);
                }
                else
                {
                    Arrows[i].SetActive(false);
                }
            } 
        }
    }

    Vector3 calculate_b_field(Vector3 magnet_pos, Vector3 arrow_pos, float distance)
    {
        Vector3.Dot(magnet_pos, arrow_pos);
        Vector3 dipole_moment = new Vector3(1, 2, 3);
        Vector3 vector_distance = new Vector3(arrow_pos.x - magnet_pos.x,
                                              arrow_pos.y - magnet_pos.y,
                                              arrow_pos.z - magnet_pos.z).normalized;
        return( (float)1e-7*(3 * (Vector3.Dot(dipole_moment, vector_distance)) * vector_distance - dipole_moment)
                                                                                            / Mathf.Pow(distance, 3));
    }

    void generate_field(Vector3 field_size, float arrow_gap, List<GameObject> Arrows)
    {
        // iterates over each axes to form a grid of points
        for (float x = -field_size.x; x <= field_size.x; x += arrow_gap)
        {
            for (float y = -field_size.y; y <= field_size.y; y += arrow_gap)
            {
                for (float z = -field_size.z; z <= field_size.z; z += arrow_gap)
                {
                    GameObject arrow = Instantiate(arrowPrefab, transform); //creates arrow
                    arrow.transform.position = new Vector3(x, y, z); //gives arrow coordinates corresponding to grid point

                    //Allow left controller to interact with arrow
                    BoxCollider arrowCollider = arrow.AddComponent(typeof(BoxCollider)) as BoxCollider;
                    arrowCollider.excludeLayers = ~0; //Exclude Layers: Everything - will not have any physics interactions
                    XRSimpleInteractable scanInteractor = arrow.AddComponent(typeof(XRSimpleInteractable)) as XRSimpleInteractable;
                    scanInteractor.interactionLayers = InteractionLayerMask.GetMask("Scannables");
                    UnityEventTools.AddPersistentListener(scanInteractor.selectEntered, scan);

                    Arrows.Add(arrow); //Adds arrow to list of arrows
                }
            }
        }
    }
    void scan(SelectEnterEventArgs args)
    {
        Vector3 b_field = calculate_b_field(magnet.transform.position, args.interactableObject.transform.position, 
                                            Vector3.Distance(magnet.transform.position, args.interactableObject.transform.position));
        Debug.Log(b_field.x + ", " + b_field.y + ", " + b_field.z);
    }
}