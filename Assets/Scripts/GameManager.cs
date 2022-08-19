using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    /*
     ---< GAME MANAGER >---
     Aquí solamente van transiciones de room, sonidos y otras
     cosas que voy a necesitar usar en todas las escenas.

    PlayerPrefs
    hs = Puntaje
    idioma = El idioma (0 = eng, 1 = esp)
    volMusic
    volSound

         */
    public static GameManager scr;
    public AudioSource audSrc, audSrcMusic;
    public AudioClip[] sounds;
    public AudioClip musGame;

    public GameObject obTrans;

    float volSound = 0f;
    float volMusic = 0f;
    [HideInInspector]public float multMusic = 1f;

    bool BorrarDatos()
    {
        return (Input.GetKey(KeyCode.LeftShift)) && (Input.GetKey(KeyCode.RightShift));
    }

    private void Start()
    {
        scr = this;
        Application.runInBackground = true;
    }

    private void Update()
    {
        volSound = PlayerPrefs.GetFloat("volSound", 1f);
        BGMVolume();

        if (BorrarDatos())
        {
            PlayerPrefs.SetInt("hs", 0);
        }
    }

    public void CargarEscena(string escena = "a")
    {
        if (escena == "a")
        {
            escena = SceneManager.GetActiveScene().name;
        }
        Transition transition = Instantiate(obTrans).GetComponent<Transition>();
        transition.escen = escena;
    }

    public void PlaySE(AudioClip clip)
    {
        audSrc.PlayOneShot(clip, volSound);
    }

    public void PlayBGM(AudioClip clip)
    {
        audSrcMusic.Stop();
        BGMVolume();
        audSrcMusic.clip = clip;
        audSrcMusic.Play();
    }

    public void BGMVolume()
    {
        volMusic = PlayerPrefs.GetFloat("volMusic", 1f);
        audSrcMusic.volume = volMusic * multMusic;
    }

    public IEnumerator ienTemblor(float intensity = 5f, float time = 0.25f)
    {
        CinemachineBasicMultiChannelPerlin cbmcp =
        GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = intensity;
        float numActual = intensity;

        yield return new WaitForSeconds(time);
        cbmcp.m_AmplitudeGain = 0f;
    }
}
