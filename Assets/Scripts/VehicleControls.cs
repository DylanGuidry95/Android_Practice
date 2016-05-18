using UnityEngine;
using System.Collections;

public class VehicleControls : MonoBehaviour
{
    // Use this for initialization
    public Vector3 initialRotation;
    public GameObject Wheel;

    public Vector3 Acceleration;
    public Vector3 Velocity;
    float Speed; 
    void Start()
    {
        initialRotation = Input.acceleration;
    }

    bool temp = true;
    bool noGas = true;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (temp)
        {
            initialRotation = Input.acceleration;
            temp = false;
        }

        if (Input.acceleration.x < -.05 || Input.acceleration.x > .05)
        {
            Wheel.transform.Rotate(Wheel.transform.forward, -Input.acceleration.x);
            gameObject.transform.Rotate(transform.up, Input.acceleration.x * (Mathf.PI * Speed));
        }

        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began || t.phase == TouchPhase.Stationary)
            {
                noGas = false;
                if (Speed < 1 && t.position.x > 375)
                    Speed += .2f * Time.deltaTime;
                if (Speed < 1 && t.position.x < 375)
                    Speed -= .2f * Time.deltaTime;
            }
            if (t.phase == TouchPhase.Ended)
            {
                noGas = true;
            }
        }

        if (noGas)
        {
            if (Speed > 0)
                Speed -= .4f * Time.deltaTime;
            if (Speed < .1f && Speed > -.1f)
                Speed = 0;
            if (Speed < 0)
                Speed += .4f * Time.deltaTime;
        }

        transform.position += transform.forward * Speed;

        //Movement();
    }

    void Movement()
    {
        foreach(Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began || t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Moved)
            {
                if (t.position.x > 375)
                {
                    Acceleration += new Vector3(0, 0, transform.forward.z);
                }
                //if (t.position.x < 375)
                //{
                //    Acceleration += new Vector3(0, 0, -transform.forward.z);
                //}
            }
            if(t.phase == TouchPhase.Ended)
            {
                noGas = true;
            }
        }


        if(Acceleration.z > 0)
        {
            Velocity = Acceleration + Velocity.normalized;
            transform.position += Velocity * Time.deltaTime;
        }

    }
}
