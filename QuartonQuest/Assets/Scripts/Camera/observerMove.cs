using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class observerMove : MonoBehaviour
{

    public Rigidbody rb;
    public Transform player;
    public Transform axisObject;
    private float sidewaysForce = 40000f;
    private float forwardForce =  40000f;
    private float flyingForce =   4000f;
    public string nextScene;
    private Vector3 zero = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        player.LookAt(axisObject);
        rb.velocity = zero;
        if (Input.GetKey("w"))
        {
            rb.AddRelativeForce(0, flyingForce, 0);
            //rb.AddRelativeForce(0, 0, forwardForce * Time.deltaTime);
        }

        if (Input.GetKey("a"))
        {
            rb.AddRelativeForce(-sidewaysForce * Time.deltaTime, 0, 0);
        }

        if (Input.GetKey("s"))
        {
            //rb.AddRelativeForce(0, 0, -forwardForce * Time.deltaTime);
            rb.AddRelativeForce(0, -flyingForce, 0);
        }

        if (Input.GetKey("d"))
        {
            rb.AddRelativeForce(sidewaysForce * Time.deltaTime, 0, 0);
        }

        if (Input.GetKey("q"))
        {
            rb.AddRelativeForce(0, 0, forwardForce * Time.deltaTime);
        }
        if (Input.GetKey("e"))
        {
            rb.AddRelativeForce(0, 0, -forwardForce * Time.deltaTime);
        }
        //if (Input.GetKey("d"))
        //{
        //    Vector3 eulerAngle = new Vector3(0, 120, 0);
        //    Quaternion deltaRotation = Quaternion.Euler(eulerAngle * Time.deltaTime);
        //    rb.MoveRotation(rb.rotation * deltaRotation);
        //}

        //if (Input.GetKey("a"))
        //{
        //    Vector3 eulerAngle = new Vector3(0, -120, 0);
        //    Quaternion deltaRotation = Quaternion.Euler(eulerAngle * Time.deltaTime);
        //    rb.MoveRotation(rb.rotation * deltaRotation);
        //}

        if (Input.GetKey("f"))
        {
            rb.AddRelativeForce(0, flyingForce, 0);
        }

        if (Input.GetKey("n"))
        {
            SceneManager.LoadScene(nextScene);
        }

    }
}
