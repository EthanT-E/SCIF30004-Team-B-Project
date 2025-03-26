using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.AR;
using Unity.VisualScripting;

namespace Assets.Scripts
{

    public class Magnet_class : MonoBehaviour
    {
        public GameObject Magnet;
        public Vector3 MagnetPosition;
        public Quaternion MagnetRotation;
        public float magnetic_susceptibility;
        public Vector3 auxilary_field;
        public Vector3 dipole_moment;
        public bool UI_value_change = false;
        public bool holding=false;

        public Vector3 max_B_field;
        public float max_B_field_value;
        public float Radius_of_influence;
        public Magnet_class(GameObject prefab, Vector3 start_pos, Vector3 iAux, float iSus = 1)
        {
            Magnet = Instantiate(prefab);
            Magnet.transform.position = start_pos;
            magnetic_susceptibility = iSus;
            auxilary_field = iAux;
            dipole_moment = Dipole_moment();
            
            Magnet.GetComponent<Magnet_class>().MagnetPosition = Magnet.transform.position;
            Magnet.GetComponent<Magnet_class>().magnetic_susceptibility = iSus;

            Magnet.GetComponent<Magnet_class>().auxilary_field = iAux;

            Magnet.GetComponent<Magnet_class>().dipole_moment = Dipole_moment();
            Debug.Log($"DIpole: {Magnet.GetComponent<Magnet_class>().dipole_moment.x},{Magnet.GetComponent<Magnet_class>().dipole_moment.y},{Magnet.GetComponent<Magnet_class>().dipole_moment.z}");
        }

        private Vector3 Dipole_moment()
        {
            return auxilary_field * magnetic_susceptibility;
        }

        public void set_suscept(float Ichi)
        {
            magnetic_susceptibility = Ichi;
            update_dipole();
        }

        public void set_auxiliary(Vector3 iAux)
        {
            auxilary_field = iAux;
            update_dipole();
        }

        public void new_pos()
        {
            MagnetPosition = Magnet.transform.position;
            Magnet.GetComponent<Magnet_class>().MagnetPosition = Magnet.transform.position;
        }

        public void new_rot()
        {
            MagnetRotation = Magnet.transform.rotation;
        }
        
        public void update_dipole()
        {
            dipole_moment = Dipole_moment();
            dipole_moment =  MagnetRotation*dipole_moment;
        }

        public void influence_force(Vector3 force)
        {
            float scale_factor = 1.0f - Mathf.Exp(-force.magnitude / 1E10f);
            float adjust_magnitude = Mathf.Pow(force.magnitude, 0.65f);
            Magnet.GetComponent<Rigidbody>().AddForce(force.normalized * scale_factor * adjust_magnitude, ForceMode.Acceleration);
        }

        public void influence_torque(Vector3 torque)
        {
            Magnet.GetComponent<Rigidbody>().AddTorque(torque.normalized * Mathf.Clamp(torque.magnitude, 0.0f, 1E5f), ForceMode.Acceleration);
        }

        public static void Generate_magnet(GameObject prefab, List<Magnet_class> Magnet_list, Vector3 start_pos, Vector3 iAux, float iSus = 1)
        {
            Magnet_class mag = new Magnet_class(prefab, start_pos, iAux, iSus);
            Magnet_list.Add(mag);
        }

        public static void Generate_magnet(GameObject prefab, List<Magnet_class> Magnet_list, Vector3 start_pos, float iSus = 1)
        {
            Vector3 DefaultAux = new Vector3(1, 0, 0);
            Magnet_class mag = new Magnet_class(prefab, start_pos, DefaultAux, iSus);
            Magnet_list.Add(mag);
        }

        public void Send_Self(SelectEnterEventArgs args)
        {
            if (args.interactorObject is NearFarInteractor controller)
            {
                RecieveAndSendToSystem reciever = controller.GetComponent<RecieveAndSendToSystem>();
                Debug.Log($"DIpoe: {this.GetComponent<Magnet_class>().dipole_moment.x},{this.GetComponent<Magnet_class>().dipole_moment.y},{this.GetComponent<Magnet_class>().dipole_moment.z}");
                reciever.get_magnet(this.GetComponent<Magnet_class>());
                ChangeTag holding = Magnet.GetComponent<ChangeTag>();
                holding.not_hovering();
            }
        }

        public Vector3 closest_arrow(float arrow_gap)
        {
        Vector3 closest_arrow = new Vector3(arrow_gap,
                                        arrow_gap,
                                        arrow_gap);
        return MagnetRotation*closest_arrow;
        }

        public Vector3 calculate_r(Vector3 position)
        {
        Vector3 r = new Vector3(Magnet.transform.position.x - position.x,
                                        Magnet.transform.position.y - position.y,
                                        Magnet.transform.position.z - position.z);
        return r;
        }


        public void unsend_Self(SelectExitEventArgs args)
        {
            if (args.interactorObject is NearFarInteractor controller)
            {
                RecieveAndSendToSystem reciever = controller.GetComponent<RecieveAndSendToSystem>();
                reciever.get_magnet(null);
                ChangeTag holding = Magnet.GetComponent<ChangeTag>();
                holding.not_hovering();
            }
        }

    }
}
