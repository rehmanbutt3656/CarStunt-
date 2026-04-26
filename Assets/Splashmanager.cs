using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Splashmanager : MonoBehaviour
{

    public string NextScenName;


    public void Start()
    {
        Time.timeScale = 1f;
        Invoke("LoadScen",3f);
    }


    public void LoadScen()
    {
        SceneManager.LoadSceneAsync(NextScenName);
    }
}
