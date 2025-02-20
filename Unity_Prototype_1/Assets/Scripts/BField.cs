using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
// using UnityEditor.Events;
using System.Linq;

public class BField : MonoBehaviour
{
    public GameObject magnetPrefab;
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
    public List<GameObject> Magnets = new List<GameObject>();
    public List<Vector3> MagPositions = new List<Vector3>();
    public int mag_num;
    public Vector3 total_b_field;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generate_field(field_size,arrow_gap,Arrows); //generates the field of arrows
        generate_magnet(magnetPrefab,Magnets);
        generate_magnet(magnetPrefab,Magnets);
        mag_num = Magnets.Count;
        min_radius_of_influence = arrow_gap/2;
        for (int i=0; i<Arrows.Count;i++) //iterates through all arrows //equal to line 37-44
        {
            arrow_position = Arrows[i].transform.position;
            Arrows[i].SetActive(false);
            for(int j=0; j<Magnets.Count;j++)
            {
                magnet_position = MagPositions[j];
                float distance = Vector3.Distance(arrow_position, magnet_position);
                if (distance < radius_of_influence && distance > min_radius_of_influence)
                {
                    Arrows[i].SetActive(true);
                }
            }
            if(Arrows[i].activeSelf==true)
            {
                for(int j=0; j<Magnets.Count;j++)
                {
                    magnet_position = Magnets[j].transform.position;
                    float distance = Vector3.Distance(arrow_position, magnet_position);
                    Vector3 b_field = calculate_b_field(magnet_position, arrow_position, distance);
                    total_b_field += b_field;
                }
                Arrows[i].transform.rotation = Quaternion.LookRotation(b_factor * total_b_field); // gets arrow to point in b direction. increase the coeffeient also increases the effective range
                if(total_b_field.magnitude>max_B_field_value)
                {
                max_B_field_value = total_b_field.magnitude;
                // print(max_B_field_value);
                }
                colorscale = 60*(total_b_field.magnitude/max_B_field_value);
                if(colorscale>1)
                {
                    colorscale = 1f;
                }  
                //print(colorscale);//debug
                Arrows[i].GetComponent<MeshRenderer>().material.color = new Color(1,1-colorscale,0,colorscale);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool magnets_new = false;
        if (Magnets.Count!=mag_num)
        {
            mag_num=Magnets.Count;
            magnets_new = true;
        }
        else
        {
            for (int j=0; j<Magnets.Count;j++)
            {
                if (MagPositions[j] != Magnets[j].transform.position)
                {
                    magnets_new = true;
                }
            }
        }
        if((magnets_new==true))
        {
            for (int i=0; i<Arrows.Count;i++) //iterates through all arrows //equal to line 37-44
            {
                total_b_field = Vector3.zero;
                arrow_position = Arrows[i].transform.position;
                Arrows[i].SetActive(false);
                for(int j=0; j<Magnets.Count;j++)
                {
                    magnet_position = Magnets[j].transform.position;
                    MagPositions[j] = magnet_position;
                    float distance = Vector3.Distance(arrow_position, magnet_position);
                    if (distance < radius_of_influence && distance > min_radius_of_influence)
                    {
                        Arrows[i].SetActive(true);
                    }
                }
                if(Arrows[i].activeSelf==true)
                {
                    for(int j=0; j<Magnets.Count;j++)
                    {
                        magnet_position = MagPositions[j];
                        float distance = Vector3.Distance(arrow_position, magnet_position);
                        Vector3 b_field = calculate_b_field(magnet_position, arrow_position, distance);
                        total_b_field += b_field;
                    }
                    Arrows[i].transform.rotation = Quaternion.LookRotation(b_factor * total_b_field); // gets arrow to point in b direction. increase the coeffeient also increases the effective range
                    colorscale = 60*(total_b_field.magnitude/max_B_field_value);
                    if(colorscale>1)
                    {
                        colorscale = 1f;
                    }  
                    //print(colorscale);//debug
                    Arrows[i].GetComponent<MeshRenderer>().material.color = new Color(1,1-colorscale,0,colorscale);
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
                    //BoxCollider arrowCollider = arrow.AddComponent(typeof(BoxCollider)) as BoxCollider;
                    //arrowCollider.excludeLayers = ~0; //Exclude Layers: Everything - will not have any physics interactions
                    //XRSimpleInteractable scanInteractor = arrow.AddComponent(typeof(XRSimpleInteractable)) as XRSimpleInteractable;
                    //scanInteractor.interactionLayers = InteractionLayerMask.GetMask("Scannables");
                    //UnityEventTools.AddPersistentListener(scanInteractor.selectEntered, scan);

                    Arrows.Add(arrow); //Adds arrow to list of arrows
                }
            }
        }
    }

    void generate_magnet(GameObject magnetPrefab, List<GameObject> Magnets)
    {
        GameObject magnet = Instantiate(magnetPrefab);
        magnet.transform.position = Vector3.zero;
        Magnets.Add(magnet);
        MagPositions.Add(magnet.transform.position);
    }

    // void scan(SelectEnterEventArgs args)
    // {
    //     Vector3 b_field = calculate_b_field(magnet.transform.position, args.interactableObject.transform.position, 
    //                                         Vector3.Distance(magnet.transform.position, args.interactableObject.transform.position));
    //     Debug.Log(b_field.x + ", " + b_field.y + ", " + b_field.z);
    // }
}