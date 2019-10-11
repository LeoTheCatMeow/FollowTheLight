using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainText : MonoBehaviour
{
    public static bool active; 
    static Text instance;
    static List<(string s, float duration)> displayQueue = new List<(string, float)>();
  
    void OnEnable()
    {
        if (instance == null)
        {
            instance = GetComponent<Text>();
        } else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (instance == gameObject)
        {
            active = false;
            instance = null;
        }
    }

    public static void DisplayText()
    {
        if (active)
        {
            return;
        }
        active = true;
        instance.StartCoroutine(TextFadeInOut(instance));
    }

    public static void AddToDisplayQueue(string s, float duration = 3f)
    {
        displayQueue.Add((s, duration));
    }

    static IEnumerator TextFadeInOut(Text t)
    {
        while (displayQueue.Count > 0)
        {
            (string s, float duration) item = displayQueue[0];
            t.text = item.s;
            t.CrossFadeAlpha(0f, 0f, true);
            t.CrossFadeAlpha(1f, 1.5f, true);
            yield return new WaitForSeconds(item.duration + 1.5f);
            t.CrossFadeAlpha(0f, 1f, true);
            yield return new WaitForSeconds(1f);
            displayQueue.RemoveAt(0);
        }
        active = false;
    }
}
