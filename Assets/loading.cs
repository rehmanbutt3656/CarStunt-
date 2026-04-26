using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loading : MonoBehaviour
{

    private void Start()
    {
        Invoke("LoadingStart",3);
    }

    public void LoadingStart()
    {
        SceneManager.LoadSceneAsync("SplashScreen");
    }


}
