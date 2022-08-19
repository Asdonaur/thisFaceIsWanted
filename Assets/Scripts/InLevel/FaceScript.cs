using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceScript : MonoBehaviour
{
    public LevelManager.character myChar;

    /*
     0 = CARA
     1 = SOMBRERO
     2 = LENTES
     3 = BOCA
         */
    public SpriteRenderer[] sprRens;
    Animator animator;
    public float flWalkSpeed = 0;
    Vector3 v3Dir;

    bool clicked = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(ienStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.tag != "caraM")
        {
            VerifyPos(true);
            VerifyPos(false);
            transform.Translate(v3Dir * flWalkSpeed * Time.deltaTime);

            if (Input.touches.Length > 0)
            {
                Tocado();
            }
        }
    }

    private void OnMouseDown() //                                                                            <--  DETECTA CUANDO HACES CLIC
    {
        Tocado();
    }

    void Tocado()
    {
        if ((!LevelManager.scr.ocupado) && (!clicked))
        {
            clicked = true;
            sprRens[0].sprite = myChar.sColorHit;
            GameManager.scr.PlaySE(GameManager.scr.sounds[4]);
            switch (this.gameObject.tag)
            {
                case "caraO":
                    animator.Play("HitR");
                    LevelManager.scr.ActualizarContador(5, true);
                    LevelManager.scr.inCarasX += (LevelManager.scr.inPuntos < 2) ? 4 : (1 + Random.Range(0, 3));
                    LevelManager.scr.contando = false;
                    LevelManager.scr.ActualizarPos();
                    break;

                case "caraX":
                    GameManager.scr.PlaySE(GameManager.scr.sounds[3]);
                    LevelManager.scr.ActualizarContador(-5, true);
                    LevelManager.scr.inEquivoc += 1;
                    StartCoroutine(ienWrong());
                    break;

                default:
                    break;
            }
            flWalkSpeed = 0f;
        }
    }

    void Cambiarse()
    {
        int numColor = Random.Range(0, LevelManager.scr.colr.Length);
        myChar.sColor = LevelManager.scr.colr[numColor];
        myChar.sColorHit = LevelManager.scr.colorHit[numColor];
        sprRens[0].sprite = myChar.sColor;

        myChar.sHat = LevelManager.scr.hats[Random.Range(0, LevelManager.scr.hats.Length)];
        sprRens[1].sprite = myChar.sHat;

        myChar.sEyes = LevelManager.scr.eyes[Random.Range(0, LevelManager.scr.eyes.Length)];
        sprRens[2].sprite = myChar.sEyes;
    }

    public void RostroAleatorio()
    {
        bool EsIgualAlCriminal()
        {
            LevelManager.character crim = LevelManager.scr.chrCriminal;

            if (this.gameObject.tag == "caraX")
            {
                return (crim.sColor == myChar.sColor) && (crim.sHat == myChar.sHat) && (crim.sEyes == myChar.sEyes) && (crim.sMouth == myChar.sMouth);
            }
            else
            {
                return false;
            }
        }

        foreach (var item in sprRens)
        {
            float posProm = (transform.position.y + ((this.gameObject.tag == "caraO") ? 10 : 0)) * 100 - 3000;
            item.sortingOrder = Mathf.CeilToInt(posProm);
        }

        Cambiarse();

        while (EsIgualAlCriminal())
        {
            Cambiarse();
        }

        flWalkSpeed = Random.Range(0, (LevelManager.scr.inPuntos * 0.5f));
        if (flWalkSpeed < 2)
        {
            flWalkSpeed = 0;
        }
        if (flWalkSpeed > 5)
        {
            flWalkSpeed = flWalkSpeed / 2;
        }
        v3Dir = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
        clicked = false;
    }

    public void IgualarCara(LevelManager.character origin) // EXCLUSIVO DE LA CARA M
    {
        myChar.sColor = origin.sColor;
        myChar.sColorHit = origin.sColorHit;
        sprRens[0].sprite = myChar.sColor;

        myChar.sHat = origin.sHat;
        sprRens[1].sprite = myChar.sHat;

        myChar.sEyes = origin.sEyes;
        sprRens[2].sprite = myChar.sEyes;

        myChar.sMouth = origin.sMouth;
        sprRens[3].sprite = myChar.sMouth;
    }

    public void Quitarse()
    {
        transform.position = new Vector3(0, 0, -100);
    }

    void VerifyPos(bool ex)
    {
        float valor = 0f;
        switch (ex)
        {
            case true:
                valor = Mathf.Abs(transform.position.x);
                break;

            case false:
                valor = Mathf.Abs(transform.position.y);
                break;
        }
        if (valor >= 6f)
        {
            switch (ex)
            {
                case true:
                    transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
                    break;

                case false:
                    transform.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);
                    break;
            }
        }
    }

    IEnumerator ienWrong()
    {
        animator.Play("Hit");
        StartCoroutine(GameManager.scr.ienTemblor());
        yield return new WaitForSeconds(0.75f);
        Quitarse();
    }

    IEnumerator ienStart()
    {
        yield return new WaitForSeconds(0.5f);
        
        if (gameObject.tag == "caraM")
        {
            IgualarCara(LevelManager.scr.chrEmpty);
        }
        else
        {
            Cambiarse();
        }
    }
}
