using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTrigger : MonoBehaviour, IInteractable
{
    public string eventString;
    public AudioClip triggerSound;
    public string[] messages;
    public float[] durations;
    public delegate void handler(string info);
    public static event handler triggered;

    public void Interact()
    {
        if (enabled && !MainText.active)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                messages[i] = messages[i].Replace("|", "\n");
                MainText.AddToDisplayQueue(messages[i], i >= durations.Length? 3f : durations[i]);
            }
            MainText.DisplayText();
            if (eventString != "")
            {
                triggered(eventString);
            }
            if (triggerSound != null)
            {
                PlayerControl.PlaySound(triggerSound);
            }
            enabled = false;
        }
    }
}
