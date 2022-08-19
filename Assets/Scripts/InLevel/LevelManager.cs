using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager scr;
    public int inContador = 10,
        inPuntos = 0,
        inCarasX = 3,
        inEquivoc = 0;

    public Sprite emptySprite;
    public Sprite[] colr, colorHit, hats, eyes, mouths;

    public FaceScript caraMScr;
    public TextMeshPro txtContador, txtPuntaje;
    public SpriteRenderer filtroMorado;
    public Animator animChange;
    Camera mainCam;

    public struct character
    {
        public Sprite sColor, sColorHit, sHat, sEyes, sMouth;
    }
    public character chrCriminal;
    public character chrEmpty;

    public Color clrNormal, clrLight;
    public GameObject cnvsLose;

    [HideInInspector]public bool contando = false,
        ocupado = true;

    GameObject caraO;

    GameObject[] carasX;

    bool blPerdido = false;

    Vector3 randomPosition()
    {
        float cam = Mathf.RoundToInt(Camera.main.orthographicSize) - 1.2f,
            x = Random.Range(-cam, cam),
            y = Random.Range(-cam, cam);

        return new Vector3(x, y, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        // AJUSTAR ALGUNOS VALORES
        scr = this;
        chrEmpty.sColor = chrEmpty.sColorHit = chrEmpty.sEyes = chrEmpty.sHat = chrEmpty.sMouth = emptySprite;
        txtContador.GetComponent<MeshRenderer>().sortingLayerName = "VirtualHud";
        caraO = GameObject.FindGameObjectWithTag("caraO");
        carasX = GameObject.FindGameObjectsWithTag("caraX");

        // EMPEZAR PARTIDA

        StartCoroutine(ienStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (inContador <= 0)
        {
            vPerder();
        }
    }

    public void ActualizarPos()
    {
        StartCoroutine(ienAcertar());
    }

    void Filtro(bool ponerF)
    {
        Color colorcin = filtroMorado.color;
        float alph = (ponerF) ? 1f : 0f;
        GameManager.scr.PlaySE(GameManager.scr.sounds[0]);

        filtroMorado.color = new Color(colorcin.r, colorcin.g, colorcin.b, alph);
    }

    public void ActualizarContador(int valor = 0, bool mostrarC = false)
    {
        inContador += valor;
        if (inContador < 0)
        {
            inContador = 0;
        }
        if (mostrarC)
        {
            MostrarCambioTiempo(valor);
        }

        txtContador.text = inContador + "";
    }

    void ActualizarPuntaje(int value = 0)
    {
        inPuntos += value;
        string str = "x" + ((inPuntos < 10) ? "0" : "") + inPuntos;
        txtPuntaje.text = str;
    }

    void vPerder()
    {
        if (!blPerdido)
        {
            blPerdido = true;
            StartCoroutine(ienAcertar(false));
        }
    }

    void OrganizarCaras(GameObject[] caras)
    {
        if (caras.Length >= 4)
        {
            caras[0].transform.position = new Vector3(1, 1);
            caras[1].transform.position = new Vector3(-1, 1);
            caras[2].transform.position = new Vector3(1, -1);
            caras[3].transform.position = new Vector3(-1, -1);
        }
        if (caras.Length >= 8)
        {
            caras[4].transform.position = new Vector3(3, 1);
            caras[5].transform.position = new Vector3(-3, 1);
            caras[6].transform.position = new Vector3(3, -1);
            caras[7].transform.position = new Vector3(-3, -1);
        }
    }

    void MostrarCambioTiempo(int cambio)
    {
        string simbolo = (cambio < 0) ? "-" : "+";
        string mostrar = simbolo + (Mathf.Abs(cambio));
        animChange.GetComponent<TextMeshPro>().text = mostrar;
        animChange.Play("index");
    }

    public IEnumerator ienStart()
    {
        ocupado = true;
        yield return new WaitForSeconds(1f);
        mainCam = Camera.main;
        StartCoroutine(ienActualizarPosiciones());
        yield return new WaitForSeconds(1.6f);
        GameManager.scr.PlayBGM(GameManager.scr.musGame);
    }

    public IEnumerator ienTiempo()
    {
        contando = true;
        ActualizarContador();
        yield return new WaitForSeconds(1f);
        while ((inContador > 1) && (contando))
        {
            ActualizarContador(-1);
            GameManager.scr.PlaySE(GameManager.scr.sounds[1]);
            yield return new WaitForSeconds(1f);
        }

        if (contando)
        {
            ActualizarContador(-1);
            GameManager.scr.PlaySE(GameManager.scr.sounds[1]);
            vPerder();
        }
    }

    public IEnumerator ienAcertar(bool ganar = true)
    {
        mainCam.backgroundColor = clrLight;
        ocupado = true;
        foreach (var item in carasX)
        {
            item.GetComponent<FaceScript>().Quitarse();
        }
        
        if (ganar)
        {
            GameManager.scr.PlaySE(GameManager.scr.sounds[2]);
            ActualizarPuntaje(1);
            yield return new WaitForSeconds(1.25f);
            StartCoroutine(ienActualizarPosiciones());
            mainCam.backgroundColor = clrNormal;
        }
        else
        {
            GameManager.scr.audSrcMusic.Stop();
            GameManager.scr.audSrc.PlayOneShot(GameManager.scr.sounds[3]);
            yield return new WaitForSeconds(0.9f);
            int hs = PlayerPrefs.GetInt("hs", 0);
            bool rec = false;
            if (hs < inPuntos)
            {
                PlayerPrefs.SetInt("hs", inPuntos);
                hs = inPuntos;
                rec = true;
            }

            yield return new WaitForSeconds(0.1f);
            string puntajes = "<b>" + inPuntos + "</b>\n" + inEquivoc + "\n" + hs;

            GameObject cnvsL = Instantiate(cnvsLose);
            yield return new WaitForSeconds(0.1f);
            TextMeshProUGUI txtPuntos = GameObject.Find("txtPuntosLose").GetComponent<TextMeshProUGUI>(),
                txtRecore = GameObject.Find("txtRECORD").GetComponent<TextMeshProUGUI>();

            if (rec)
            {
                yield return new WaitForSeconds(0.1f);
                GameManager.scr.audSrc.PlayOneShot(GameManager.scr.sounds[5]);
                yield return new WaitForSeconds(0.1f);
                GameManager.scr.audSrc.PlayOneShot(GameManager.scr.sounds[6]);
            }
            else
            {
                txtRecore.text = "";
                yield return new WaitForSeconds(0.1f);
                GameManager.scr.audSrc.PlayOneShot(GameManager.scr.sounds[5]);
            }
            txtPuntos.text = puntajes;
        }
    }

    public IEnumerator ienTemblor()
    {
        yield return null;
    }

    public IEnumerator ienActualizarPosiciones()
    {
        float inVariable = 1 + ((10 - inPuntos) / 10);
        inVariable = (inVariable <= 0) ? 0 : inVariable;

        contando = false;
        Filtro(true);
        caraMScr.IgualarCara(chrEmpty);
        yield return new WaitForSeconds(0.5f);

        caraO.GetComponent<FaceScript>().RostroAleatorio();
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < 10; i++)
        {
            chrCriminal = caraO.GetComponent<FaceScript>().myChar;
            caraMScr.IgualarCara(chrCriminal);
        }
        yield return new WaitForSeconds(0.4f * inVariable);

        int carasPuestas = 0;
        if (inPuntos < 2)
        {
            List<GameObject> listaCaras = new List<GameObject>();
            int inCarasX2 = inCarasX + 1;
            int dado = Random.Range(0, inCarasX);

            foreach (var item in carasX)
            {
                if (carasPuestas < inCarasX2)
                {
                    if (carasPuestas == dado)
                    {
                        carasPuestas += 1;
                        yield return new WaitForSeconds(0.05f * inVariable);
                        listaCaras.Add(caraO);
                    }
                    else
                    {
                        item.GetComponent<FaceScript>().RostroAleatorio();
                        carasPuestas += 1;
                        yield return new WaitForSeconds(0.05f * inVariable);
                        listaCaras.Add(item);
                    }
                }
            }
            OrganizarCaras(listaCaras.ToArray());
        }
        else
        {
            caraO.transform.position = randomPosition();

            foreach (var item in carasX)
            {
                if (carasPuestas < inCarasX)
                {
                    item.transform.position = randomPosition();
                    item.GetComponent<FaceScript>().RostroAleatorio();
                    carasPuestas += 1;
                    yield return new WaitForSeconds(0.01f * inVariable);

                }

            }
        }

        Filtro(false);
        ocupado = false;
        StartCoroutine(ienTiempo());
    }
}
