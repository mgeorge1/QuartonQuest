using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class observerCam: MonoBehaviour
{

    public Transform player;
    public Vector3 offset;
    public Transform axisObject;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset;
        transform.localEulerAngles = player.localEulerAngles;
        transform.LookAt(axisObject);
    }

}
