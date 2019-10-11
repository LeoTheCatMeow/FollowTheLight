using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour, IInteractable
{
    public delegate void handler();
    public static event handler onClick;
    public Material offMat;
    public Material onMat;
    public AudioClip offSound;
    public AudioClip onSound;
    public Pillar[] connections;
    public bool active { get; private set; }
    MeshRenderer core;
    GameObject aura;

    void Start()
    {
        core = transform.GetChild(1).GetComponent<MeshRenderer>();
        aura = transform.GetChild(2).gameObject;
    }

    public void Interact()
    {
        SwitchState();
        onClick();
    }

    public void SwitchState(bool switchConnected = true)
    {
        if (!active)
        {
            active = true;
            core.material = onMat;
            aura.SetActive(true);
            PlayerControl.PlaySound(onSound);
        } else
        {
            active = false;
            core.material = offMat;
            aura.SetActive(false);
            PlayerControl.PlaySound(offSound);
        }

        if (switchConnected)
        {
            foreach (Pillar p in connections)
            {
                p.SwitchState(false);
            }
        }
    }
}
