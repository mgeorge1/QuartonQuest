using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planet_rotation : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform planet;
    void FixedUpdate()
    {
        planet.Rotate(0, (1/2), 1 * Time.deltaTime);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
