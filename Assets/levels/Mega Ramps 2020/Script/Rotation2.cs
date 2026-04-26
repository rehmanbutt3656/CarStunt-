using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation2 : MonoBehaviour
{
    // Start is called before the first frame update
    public float var=1f;
  

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,360*Time.deltaTime*var,0);
    }
}
