using UnityEngine;
using System.Collections;
namespace Scripts
{

    public class MathTest : ExposableMonobehavior
    {
        [HideInInspector, SerializeField]
        float TotalRotation = 0;
        [HideInInspector, SerializeField]
        MonoBehaviour Wheel;

        [ExposeProperty]
        public float Rotation { get { return TotalRotation; }set { TotalRotation = value; } }
        [ExposeProperty]
        public MonoBehaviour TheWheel { get { return Wheel; } set { Wheel = value; } }

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