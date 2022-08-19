using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public string escen;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(ienStart());
    }

    IEnumerator ienStart()
    {
        GameManager.scr.PlaySE(GameManager.scr.sounds[0]);
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(escen);
        if (escen == "MainMenu")
        {
            yield return new WaitForSeconds(0.25f);
            Destroy(this.gameObject);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            GameManager.scr.PlaySE(GameManager.scr.sounds[0]);
            Destroy(this.gameObject);
        }
    }
}
