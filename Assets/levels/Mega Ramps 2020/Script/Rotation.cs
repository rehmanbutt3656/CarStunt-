using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    // Start is called before the first frame update
    
public float var=1;
    // Update is called once per frame
    void Update()
    {
       transform.Rotate(0,0,360*Time.deltaTime*var);
    }
}
