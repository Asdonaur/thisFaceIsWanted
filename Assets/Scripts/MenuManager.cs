using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public bool esMenu;
    public GameObject menuInicio,
        menuOpt,
        cubePause,
        menuPause;

    public Image Purpanel;
    Color colorcin;

    public Slider sliSound, sliMusic;

    bool sliderSonar = false;
    // Start is called before the first frame update
    void Start()
    {
        if (esMenu)
        {
            colorcin = Purpanel.color;
            Purpanel.color = new Color(colorcin.r, colorcin.g, colorcin.b, 1f);

            
            StartCoroutine(ienStart());
        }
    }

    public void ButtonPress(string index)
    {
        GameManager.scr.PlaySE(GameManager.scr.sounds[5]);
        StartCoroutine(ienButton(index));
    }

    public void SliderMove(string index)
    {
        switch (index)
        {
            case "sound":
                float valor = (sliSound.value * 1) / 5;
                PlayerPrefs.SetFloat("volSound", valor);
                break;

            case "music":
                float valor2 = (sliMusic.value * 1) / 5;
                PlayerPrefs.SetFloat("volMusic", valor2);
                GameManager.scr.BGMVolume();
                break;
        }
        StartCoroutine(ienSlider());
    }

    void ActivarPurpanel(bool activar)
    {
        Purpanel.enabled = activar;
        GameManager.scr.PlaySE(GameManager.scr.sounds[0]);
    }

    void Pausar(bool p)
    {
        GameManager.scr.multMusic = (p) ? 0.25f : 1f;
        GameManager.scr.BGMVolume();

        Time.timeScale = (p) ? 0f : 1f;
        cubePause.SetActive(p);
        menuPause.SetActive(p);
    }

    IEnumerator ienButton(string index2)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        switch (index2)
        {
            case "start":
                GameManager.scr.CargarEscena("SampleScene");
                break;

            case "menu":
                Time.timeScale = 1f;
                yield return new WaitForSecondsRealtime(0.1f);
                GameManager.scr.CargarEscena("MainMenu");
                break;

            case "menuOpt":
                StartCoroutine(CambiarMenu(true));
                break;

            case "optRet":
                StartCoroutine(CambiarMenu(false));
                break;

            case "optLan":
                int idiomaOr = PlayerPrefs.GetInt("idioma", 0);
                PlayerPrefs.SetInt("idioma", (idiomaOr == 0) ? 1 : 0);
                GameObject[] textos = GameObject.FindGameObjectsWithTag("fraseIdioma");
                foreach (var item in textos)
                {
                    item.GetComponent<Idiomas>().AjustarIdioma();
                }
                break;

            case "quit":
                Application.Quit();
                break;

            case "pPause":
                Pausar(true);
                break;

            case "pResume":
                Pausar(false);
                break;

            default:
                break;
        }
    }

    IEnumerator ienSlider()
    {
        yield return new WaitForSeconds(0.15f);
        if (sliderSonar)
        {
            GameManager.scr.PlaySE(GameManager.scr.sounds[5]);
        }
    }

    IEnumerator CambiarMenu(bool opt)
    {
        ActivarPurpanel(true);
        yield return new WaitForSeconds(0.75f);
        menuInicio.SetActive(!opt);
        menuOpt.SetActive(opt);
        ActivarPurpanel(false);
    }

    IEnumerator ienStart()
    {
        yield return new WaitForSeconds(0.1f);
        sliMusic.value = (PlayerPrefs.GetFloat("volMusic", 1) * 5);
        yield return new WaitForSeconds(0.1f);
        sliSound.value = (PlayerPrefs.GetFloat("volSound", 1) * 5);
        yield return new WaitForSeconds(0.5f);
        ActivarPurpanel(false);
        sliderSonar = true;
    }
}
