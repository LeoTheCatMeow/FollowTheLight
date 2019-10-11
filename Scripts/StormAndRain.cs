using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StormAndRain : MonoBehaviour
{
    public delegate void Trigger(string info);
    public static event Trigger trigger;

    private void OnEnable()
    {
        trigger += OnTrigger;
    }

    private void OnDisable()
    {
        trigger -= OnTrigger;
    }

    void Start()
    {
        MainText.AddToDisplayQueue("Lost within the rainstorm, I search for a Guiding Light");
        MainText.DisplayText();
        Fader.FadeIn();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnTrigger(string info)
    {
        
    }

}
