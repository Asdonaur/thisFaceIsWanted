using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Idiomas : MonoBehaviour
{
    public int IDFrase = 0;
    int idioma = 0;
    string[] frasesEng =
    {
        "Start",
        "Options",
        "Exit",
        "Language",
        "ENGLISH",
        "VOLUME",
        "CREDITS",
        "<b>Main developer:</b>\nOscar Moreno\n(Asdonaur)",
        "<b>Sounds:</b>\nCreative Commons (CC)",
        "<b>Used software:</b>\nUnity 2020\nInkscape\nAudacity\nPaint.NET",
        "Back",
        "RESULTS",
        "<b>Caught criminals:</b>\nMistakes:\nCaught record:",
        "Retry",
        "Main Menu",
        "PAUSE",
        "Resume",
    };

    string[] frasesEsp =
    {
        "Jugar",
        "Opciones",
        "Salir",
        "Idioma",
        "ESPAÑOL",
        "VOLUMEN",
        "CRÉDITOS",
        "<b>Desarrollado por:</b>\nOscar Moreno\n(Asdonaur)",
        "<b>Sonidos:</b>\nCreative Commons (CC)",
        "<b>Programas usados:</b>\nUnity 2020\nInkscape\nAudacity\nPaint.NET",
        "Volver",
        "RESULTADOS",
        "<b>Criminales atrapados:</b>\nEquivicaciones:\nRecord de criminales:",
        "Reintentar",
        "Menú principal",
        "PAUSA",
        "Continuar",
    };

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.tag = "fraseIdioma";
        
    }

    private void OnEnable()
    {
        AjustarIdioma();
    }

    public void AjustarIdioma()
    {
        idioma = PlayerPrefs.GetInt("idioma", 0);

        TextMeshPro tmp = GetComponent<TextMeshPro>();
        TextMeshProUGUI tmpGUI = GetComponent<TextMeshProUGUI>();
        string fraseAMostrar = "";

        // DECIDIR CUAL FRASE SE VA A MOSTRAR
        switch (idioma)
        {
            case 0:
                fraseAMostrar = frasesEng[IDFrase];
                break;
            case 1:
                fraseAMostrar = frasesEsp[IDFrase];
                break;
        }

        // BUSCAR COMPONENTE PARA PONER LA FRASE
        if (tmp)
        {
            tmp.text = fraseAMostrar;
        }
        else if (tmpGUI)
        {
            tmpGUI.text = fraseAMostrar;
        }
    }
}
