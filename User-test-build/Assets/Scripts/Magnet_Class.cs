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
            dipole_moment = Dipole_moment();
        }

        public void set_auxiliary(Vector3 iAux)
        {
            auxilary_field = iAux;
            dipole_moment = Dipole_moment();
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
            Magnet.GetComponent<Rigidbody>().AddForce(force * Mathf.Clamp(force.magnitude, 15.0f, 100.0f), ForceMode.Impulse);
        }

        public void influence_torque(Vector3 torque)
        {
            Magnet.GetComponent<Rigidbody>().AddTorque(torque * Mathf.Clamp(torque.magnitude, 1.0f, 30.0f), ForceMode.Impulse);
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
            }
        }


        public void unsend_Self(SelectExitEventArgs args)
        {
            if (args.interactorObject is NearFarInteractor controller)
            {
                RecieveAndSendToSystem reciever = controller.GetComponent<RecieveAndSendToSystem>();
                reciever.get_magnet(null);
            }
        }

    }
}
