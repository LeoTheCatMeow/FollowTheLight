using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Fader.FadeIn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void Load(int i)
    {
        AsyncOperation intoDarkness = SceneManager.LoadSceneAsync(i);
    }
}
