using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntoDarkness : MonoBehaviour
{
    //references
    public GameObject cam;

    //0
    public AudioClip preludeSound;
    public AudioClip lightSound;
    public AudioClip aVoiceFromAbove;

    //1
    public GameObject lightGroup1;
    public GameObject ironPoles;
    public AudioClip keyEvent1Sound;

    //2
    public GameObject lightGroup2;
    public GameObject candles;
    public AudioClip keyEvent2Sound;
    public AudioClip keyEvent2FinishSound;
    public AudioClip lightDisappearSound;

    //3
    public GameObject lightGroup3;
    public GameObject pillars;
    public AudioClip keyEvent3Sound;
    public AudioClip keyEvent3FinishSound;

    //4
    public GameObject lightGroup4;
    public AudioClip whisperSound;
    public AudioClip keyEvent4Sound;
    public RectTransform footprints;

    //story variables
    int S1Counter = 0;
    int S2Counter = 0;
    bool S2PuzzleStart;
    bool S2PuzzleSuccess;
    Candle activeCandle;
    int S3Counter = 0;
    bool S3PuzzleStart;
    bool S3PuzzleSuccess;
    
    void OnEnable()
    {
        MessageTrigger.triggered += MessageListener;
        Candle.onClick += CandleOnClick;
        Pillar.onClick += PillarOnClick;
    }

    void OnDisable()
    {
        MessageTrigger.triggered -= MessageListener;
        Candle.onClick -= CandleOnClick;
        Pillar.onClick -= PillarOnClick;
    }

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        StartCoroutine(Prelude());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    void MessageListener(string info)
    {
        if (info == "S1")
        {
            S1Counter++;
            if (S1Counter == 3)
            {
                StartCoroutine(S1KeyEvent());
            }
        } else if (info == "S2")
        {
            S2Counter++;
            if (S2Counter == 3)
            {
                StartCoroutine(S2KeyEvent());
            }
        } else if (info == "S3")
        {
            S3Counter++;
            if (S3Counter == 3)
            {
                StartCoroutine(S3KeyEvent());
            }
        }
    }

    void CandleOnClick(Candle c)
    {
        if (!S2PuzzleStart || S2PuzzleSuccess)
        {
            return;
        }
        if (activeCandle == null)
        {
            activeCandle = c;
            Vector3 candlePos = cam.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
            c.transform.position = candlePos + cam.transform.forward;
            c.transform.SetParent(cam.transform);
        } else
        {
            activeCandle.transform.SetParent(candles.transform);
            activeCandle.Reset();
            activeCandle = null;

            int i = 0;
            foreach (Transform t in candles.transform)
            {
                if (Vector3.Distance(t.position, candles.transform.position) <= 3f)
                {
                    i++;
                }
            }
            if (i >= 9)
            {
                S2PuzzleSuccess = true;
            }
        }
    }

    void PillarOnClick()
    {
        if (!S3PuzzleStart || S3PuzzleSuccess)
        {
            return;
        }

        bool success = true;
        foreach (Transform t in pillars.transform)
        {
            if (!t.GetComponent<Pillar>().active)
            {
                success = false;
                return;
            }
        }
        if (success)
        {
            S3PuzzleSuccess = true;
        }
    }

    IEnumerator Prelude()
    {
        PlayerControl.Constrain();
        yield return new WaitForSeconds(1f);
        MainText.AddToDisplayQueue("In Darkness, I search for a path . . .");
        MainText.DisplayText();
        PlayerControl.PlaySound(preludeSound);
        yield return new WaitWhile(() => MainText.active);
        yield return new WaitForSeconds(1f);
        Fader.FadeIn();
        lightGroup1.SetActive(true);
        PlayerControl.Release();
    }

    IEnumerator S1KeyEvent()
    {
        yield return new WaitWhile(() => MainText.active);
        yield return new WaitForSeconds(3f);
        PlayerControl.PlaySound(keyEvent1Sound);
        MainText.AddToDisplayQueue("Little did I know, my innocent childhood was surrounded by danger.", 4f);
        MainText.AddToDisplayQueue("The Communist regime persecuted us. They wanted to tear my family apart.", 4f);
        MainText.AddToDisplayQueue("My parents . . . they protected me. They hid everything from me.", 3f);
        MainText.AddToDisplayQueue("Alone in this silent cage,\n unaware of the cruelty of the outside world, I wonder where my future lies.", 4f);
        MainText.AddToDisplayQueue("And then I heard a voice . . .");
        MainText.DisplayText();
        yield return new WaitWhile(() => MainText.active);
        yield return new WaitForSeconds(2f);
        MainText.AddToDisplayQueue("<i><color=#ffff00ff>A Voice From Above</color></i>\nDon't worry now, let me show you a path.");
        MainText.DisplayText();
        yield return new WaitWhile(() => MainText.active);
        yield return new WaitForSeconds(1f);
        lightGroup2.SetActive(true);
        PlayerControl.PlaySound(lightSound);
        yield return new WaitForSeconds(2f);
        MainText.AddToDisplayQueue("But these poles . . . I can't get through.");
        MainText.AddToDisplayQueue("<i><color=#ffff00ff>A Voice From Above</color></i>\nAre you willing to <b>believe</b>?");
        MainText.DisplayText();
        yield return new WaitWhile(() => MainText.active);
        CapsuleCollider[] colliders = ironPoles.GetComponentsInChildren<CapsuleCollider>();
        foreach (CapsuleCollider c in colliders)
        {
            c.enabled = false;
        }
        yield return new WaitWhile(() => Vector3.Distance(cam.transform.position, lightGroup2.transform.position) > 25f);
        MainText.AddToDisplayQueue("Here am I, Texas, US, a breath of fresh air.");
        MainText.DisplayText();
    }

    IEnumerator S2KeyEvent()
    {
        lightGroup1.SetActive(false);
        yield return new WaitWhile(() => MainText.active);
        yield return new WaitForSeconds(3f);
        MainText.AddToDisplayQueue("Each of the candles here is a good memory.");
        MainText.AddToDisplayQueue("Gather the candles.");
        MainText.DisplayText();
        PlayerControl.PlaySound(keyEvent2Sound);
        S2PuzzleStart = true; 
        yield return new WaitUntil(() => S2PuzzleSuccess);
        PlayerControl.PlaySound(keyEvent2FinishSound);
        MainText.AddToDisplayQueue("<i><color=#add8e6ff>Mom</color></i>\nI have great news! We are coming to New York! Come stay with us.", 4f);
        MainText.AddToDisplayQueue("Wait, why can’t you come here?");
        MainText.AddToDisplayQueue("I don’t want to go.");
        MainText.DisplayText();
        yield return new WaitWhile(() => MainText.active);
        yield return new WaitForSeconds(3f);
        lightGroup3.SetActive(true);
        PlayerControl.PlaySound(lightSound);
        MainText.AddToDisplayQueue("I don’t know what is ahead of me. I don’t like change.");
        MainText.AddToDisplayQueue("Dear God . . . Is this the right path?");
        MainText.DisplayText();
        yield return new WaitWhile(() => MainText.active);
        yield return new WaitWhile(() => Vector3.Distance(cam.transform.position, lightGroup3.transform.position) > 55f);
        PlayerControl.Constrain();
        lightGroup2.SetActive(false);
        PlayerControl.PlaySound(lightDisappearSound);
        MainText.AddToDisplayQueue("<i><color=#add8e6ff>Mom</color></i>\nThe immigration office rejected us!");
        MainText.AddToDisplayQueue("<i><color=#add8e6ff>Mom</color></i>\nThe officer, he didn’t even listen to us! He didn’t even let our lawyer talk!", 4f);
        MainText.AddToDisplayQueue("<i><color=#add8e6ff>Mom</color></i>\nHow can those fake ones pass. Yet we, the real persecuted, cannot!");
        MainText.AddToDisplayQueue("<i><color=#add8e6ff>Mom</color></i>\nLife is so not fair.");
        MainText.DisplayText();
        yield return new WaitWhile(() => MainText.active);
        lightGroup3.SetActive(false);
        PlayerControl.PlaySound(lightDisappearSound);
        MainText.AddToDisplayQueue("Oh no.");
        MainText.AddToDisplayQueue("<i><color=#ffff00ff>A Voice From Above</color></i>\nWhy be worried? Don’t you have faith in me?");
        MainText.DisplayText();
        yield return new WaitWhile(() => MainText.active);
        PlayerControl.Release();
        yield return new WaitForSeconds(3f);
        lightGroup3.SetActive(true);
        PlayerControl.PlaySound(aVoiceFromAbove);
        yield return new WaitWhile(() => Vector3.Distance(cam.transform.position, lightGroup3.transform.position) > 30f);
        PlayerControl.Constrain();
        MainText.AddToDisplayQueue("<i><color=#add8e6ff>Mom</color></i>\nOh my god can you believe this?");
        MainText.AddToDisplayQueue("<i><color=#add8e6ff>Mom</color></i>\nOur letter to the immigration office worked!");
        MainText.AddToDisplayQueue("<i><color=#add8e6ff>Mom</color></i>\nThey say that they will review our case again as a special case!", 4f);
        MainText.AddToDisplayQueue("<i><color=#add8e6ff>Mom</color></i>\nWe passed! Yes! We can legally stay here now!");
        MainText.DisplayText();
        PlayerControl.PlaySound(keyEvent2Sound);
        yield return new WaitWhile(() => MainText.active);
        PlayerControl.Release();
    }

    IEnumerator S3KeyEvent()
    {
        yield return new WaitWhile(() => MainText.active);
        yield return new WaitForSeconds(3f);
        int randomInt = Random.Range(1, 4);
        int randomPillar;
        for (int i = 0; i <= randomInt; i++)
        {
            do
            {
                randomPillar = Random.Range(0, pillars.transform.childCount);
            } while (pillars.transform.GetChild(randomPillar).GetComponent<Pillar>().active);
            pillars.transform.GetChild(randomPillar).GetComponent<Pillar>().SwitchState(false);
        }
        MainText.AddToDisplayQueue("Life is full of challenges.");
        MainText.AddToDisplayQueue("Make all pillars <b>glow</b>.");
        MainText.DisplayText();
        PlayerControl.PlaySound(keyEvent3Sound);
        yield return new WaitWhile(() => MainText.active);
        S3PuzzleStart = true;
        yield return new WaitUntil(() => S3PuzzleSuccess);
        PlayerControl.PlaySound(keyEvent3FinishSound);
        yield return new WaitForSeconds(5f);
        MainText.AddToDisplayQueue("I wander in this world of darkness, lost.", 4f);
        MainText.AddToDisplayQueue("But through these lights, I come to realize God's great plan for me.", 4f);
        MainText.AddToDisplayQueue("A light for innocence.");
        MainText.AddToDisplayQueue("A light for spirituality.");
        MainText.AddToDisplayQueue("A light for knowledge.");
        MainText.AddToDisplayQueue("What light awaits me?");
        MainText.DisplayText();
        yield return new WaitWhile(() => MainText.active);
        yield return new WaitForSeconds(3f);
        lightGroup4.SetActive(true);
        PlayerControl.PlaySound(aVoiceFromAbove);
        StartCoroutine(Finale());
    }

    IEnumerator Finale()
    {
        yield return new WaitWhile(() => Vector3.Distance(cam.transform.position, lightGroup4.transform.position) > 25f);
        MainText.AddToDisplayQueue("<i><color=#ff0000ff>A Voice From Elsewhere</color></i>\nYou are an idiot! Be yourself! Do what you want!");
        MainText.AddToDisplayQueue("<i><color=#ff0000ff>Another Voice</color></i>\nGod is a joke man. Grow up. What you believe in is so out-of-date.");
        MainText.AddToDisplayQueue("<i><color=#ff0000ff>Yet Another Voice</color></i>\nFuck off. I don’t care. I just want my paychecks.");
        MainText.DisplayText();
        PlayerControl.PlaySound(whisperSound);
        yield return new WaitWhile(() => MainText.active);
        yield return new WaitForSeconds(2f);
        yield return new WaitWhile(() => Vector3.Distance(cam.transform.position, lightGroup4.transform.position) > 3f);
        PlayerControl.Constrain();
        StartCoroutine(Footprints());
        PlayerControl.PlaySound(keyEvent4Sound);
        MainText.AddToDisplayQueue("Listen, there are so many paths to take in this world.");
        MainText.AddToDisplayQueue("Wealth, fame, happiness, knowledge, love . . . so many to live for.", 4f);
        MainText.AddToDisplayQueue("I don’t know what is right. Nor can I see my future path.");
        MainText.AddToDisplayQueue("So I seek guidance, looking for a guiding light that stands above matter and time.", 4f);
        MainText.AddToDisplayQueue("And after all this journey, I am sure...");
        MainText.AddToDisplayQueue("He is worthy to believe in.");
        MainText.DisplayText();
        yield return new WaitWhile(() => MainText.active);
        MainText.AddToDisplayQueue("Therefore,", 2f);
        MainText.DisplayText();
        yield return new WaitForSeconds(3f);
        Fader.FadeOut();
        yield return new WaitWhile(() => MainText.active);
        MainText.AddToDisplayQueue("I shall follow the light.");
        MainText.DisplayText();
        yield return new WaitWhile(() => MainText.active);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }

    IEnumerator Footprints()
    {
        foreach (RectTransform t in footprints)
        {
            t.GetComponent<Animator>().Play("Show");
            yield return new WaitForSeconds(1f);
        }
    }
}
