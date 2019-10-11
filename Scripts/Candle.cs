using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour, IInteractable
{
    public delegate void handler(Candle c);
    public static event handler onClick;
    public AudioClip pickUpSound;
    public AudioClip dropSound;
    Quaternion originalRot;

    void Start()
    {
        originalRot = transform.rotation;
    }

    public void Interact()
    {
        onClick(this);
        PlayerControl.PlaySound(pickUpSound);
    }

    public void Reset()
    {
        transform.rotation = originalRot;
        transform.localPosition = new Vector3(transform.localPosition.x, 0f, transform.localPosition.z);
        PlayerControl.PlaySound(dropSound);
    }
}
