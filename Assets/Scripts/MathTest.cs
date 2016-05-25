using UnityEngine;
using System.Collections;
namespace Scripts
{

    public class MathTest : MonoBehaviour
    {
        public float TotalRotation = 0;
        public GameObject Wheel;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            TotalRotation = Quaternion.Angle(Quaternion.Euler(Wheel.transform.localEulerAngles), Wheel.transform.rotation);
            if (Input.acceleration.x > 0)
            {
                Debug.Log("Right");
                //Do Stuff
                //Wheel.transform.Rotate(Wheel.transform.forward, -Input.acceleration.x);
                TotalRotation += Wheel.transform.rotation.z;
            }
            if (Input.acceleration.x < 0)
            {
                Debug.Log("Left");
                //Do Stuff
                //Wheel.transform.Rotate(Wheel.transform.forward, -Input.acceleration.x);
                TotalRotation -= Wheel.transform.rotation.z;
            }
        }
    }
}