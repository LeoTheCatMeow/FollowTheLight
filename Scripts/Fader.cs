using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    static Fader instance;
    Image fader;

    void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }

        fader = GetComponent<Image>();
        fader.color = new Color(0f, 0f, 0f, 1f);
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public static void FadeIn()
    {
        instance.StartCoroutine(instance.Fade(true));
    }

    public static void FadeOut()
    {
        instance.StartCoroutine(instance.Fade(false));
    }

    IEnumerator Fade (bool fadeIn)
    {
        if (fadeIn)
        {
            fader.color = new Color(0f, 0f, 0f, 1f);
            for (int i = 79; i >= 0; i--)
            {
                fader.color = new Color(0f, 0f, 0f, i / 80f);
                yield return new WaitForSeconds(0.025f);
            }
        } else
        {
            fader.color = new Color(0f, 0f, 0f, 0f);
            for (int i = 1; i <= 80; i++ )
            {
                fader.color = new Color(0f, 0f, 0f, i / 80f);
                yield return new WaitForSeconds(0.025f);
            }
        }
    }
}
