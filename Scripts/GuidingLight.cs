using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidingLight : MonoBehaviour
{
    public GameObject triggerEffect;
    public GameObject staticEffect;
    public Light particleLight;

    bool isTriggered;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && !isTriggered)
        {
            isTriggered = true;
            ParticleSystem.MainModule main = staticEffect.GetComponent<ParticleSystem>().main;
            main.loop = false;
            staticEffect.GetComponent<AudioSource>().Stop();
            triggerEffect.SetActive(true);
            StartCoroutine(ChangeLightIntensity());
        }
    }

    IEnumerator ChangeLightIntensity()
    {
        for (int i = 0; i < 30; i++)
        {
            particleLight.intensity += 0.06f;
            yield return new WaitForSeconds(0.1f);
        }

        for (int i = 0; i < 30; i++)
        {
            particleLight.intensity -= 0.08f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
