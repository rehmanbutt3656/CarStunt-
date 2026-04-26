using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource LevelStart;
    public AudioSource Respawn;
    public AudioSource CheckPoint;
    public AudioSource LevelComplete;
    public AudioSource LevelFail;
    public AudioSource LevelFail2;

    public AudioSource[] BGMusic;

    // Use this for initialization
    void Start () { setSounds(); }

    void setSounds ()
    {
        float SoundVol = PlayerPrefs.GetFloat("GameSounds", 0.6f);
        float MusicVol = PlayerPrefs.GetFloat("GameMusic", 0.75f);
        //foreach (GameObject x in GameObject.FindGameObjectsWithTag("Bird"))
        //{
        //    if (x.gameObject.GetComponent<AudioSource>() != null)
        //        x.gameObject.GetComponent<AudioSource>().volume = SoundVol;
        //}
        //foreach (GameObject x in GameObject.FindGameObjectsWithTag("Heli"))
        //{
        //    if (x.gameObject.GetComponent<AudioSource>() != null)
        //        x.gameObject.GetComponent<AudioSource>().volume = SoundVol;
        //}
        foreach (AudioSource x in BGMusic) { x.volume = MusicVol; }

        LevelStart.volume = MusicVol;
        Respawn.volume = SoundVol;
        CheckPoint.volume = SoundVol;
        LevelComplete.volume = SoundVol;
        LevelFail.volume = SoundVol;
        LevelFail2.volume = SoundVol;

        //PlayerPrefs.GetInt ("EngineSound", 1);		
        //PlayerPrefs.GetInt ("OtherSounds", 1);		
        //PlayerPrefs.GetInt ("BGSounds", 1);		


        ////BG Music
        //if (PlayerPrefs.GetInt ("BGSounds", 1) == 1) {
        //	foreach (AudioSource x in BGMusic) {
        //		x.enabled = true;
        //	}
        //} else if (PlayerPrefs.GetInt ("BGSounds", 1) == 0) {
        //	foreach (AudioSource x in BGMusic) {
        //		x.enabled = false;
        //	}
        //}

        //Other Sounds
        //if (PlayerPrefs.GetInt ("OtherSounds", 1) == 1) {
        //	foreach (AudioSource x in OtherSounds) {
        //		x.enabled = true;
        //	}
        //	LevelStart.enabled = true;
        //	Respawn.enabled = true;
        //	CheckPoint.enabled = true;
        //	LevelComplete.enabled = true;
        //	LevelComplete2.enabled = true;
        //	LevelFail.enabled = true;


        //} else if (PlayerPrefs.GetInt ("OtherSounds", 1) == 0) {
        //	foreach (AudioSource x in OtherSounds) {
        //		x.enabled = false;
        //	}
        //	LevelStart.enabled = false;
        //	Respawn.enabled = false;
        //	CheckPoint.enabled = false;
        //	LevelComplete.enabled = false;
        //	LevelComplete2.enabled = false;
        //	LevelFail.enabled = false;

        //	foreach (GameObject x in GameObject.FindGameObjectsWithTag("Bird")) {
        //		if (x.gameObject.GetComponent<AudioSource> () != null)
        //			x.gameObject.GetComponent<AudioSource> ().enabled = false;
        //	}
        //	foreach (GameObject x in GameObject.FindGameObjectsWithTag("Heli")) {
        //		if (x.gameObject.GetComponent<AudioSource> () != null)
        //			x.gameObject.GetComponent<AudioSource> ().enabled = false;
        //	}
        //}
    }


    public void PlayLevelStartSound () { LevelStart.Play(); }

    public void PlayCheckPointSound () { CheckPoint.Play(); }

    public void PlayLevelCompleteSound () { LevelComplete.Play(); }

    public void PlayLevelFail ()
    {
        LevelFail.Play();
        LevelFail2.Play();
    }

    public void PlayRespawn () { Respawn.Play(); }






    // Update is called once per frame
    void Update ()
    {

    }
}
