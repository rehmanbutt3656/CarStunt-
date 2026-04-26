using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealyPanalshow : MonoBehaviour
{
    public GameObject Panal;
    public float DelayTime;

    private void OnEnable()
    {
        Invoke("OpenPanalWithDelay",DelayTime);
    }


    public void OpenPanalWithDelay()
    {
        Panal.SetActive(true);
    }
}
