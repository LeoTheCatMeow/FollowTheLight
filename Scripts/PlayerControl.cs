using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerControl : MonoBehaviour
{
    //public var
    public AudioClip walkSound;
    public float walkSpeed;

    //internal var
    Rigidbody rb;
    static Transform cam;
    static AudioSource mainAu;
    static AudioSource walkAu;
    float d; //total distance travelled 
    float h; //original cam height
    Vector2 lastPos; //for calculating d
    bool isMoving; //for footstep sound
    Vector2 mouseDelta; //for camera rotation 
    float mouseXSensitivity;
    float mouseYSensitivity;

    //state
    static bool disabled;

    void Awake()
    {
        //initialization
        rb = GetComponent<Rigidbody>();
        cam = transform.GetChild(0);
        d = 0f;
        h = cam.localPosition.y;
        lastPos = new Vector2(transform.position.x, transform.position.z);
        mainAu = GetComponent<AudioSource>();
        walkAu = GetComponents<AudioSource>()[1];
        walkAu.clip = walkSound;

        //general game settings 
        mouseXSensitivity = 19200f / Display.main.renderingWidth;
        mouseYSensitivity = 5400f / Display.main.renderingHeight;
        Cursor.visible = false;
    }

    void Update()
    {
        if (disabled)
        {
            if (isMoving)
            {
                walkAu.Pause();
                isMoving = false;
            }
            return;
        }

        //footstep sound
        walkAu.pitch = 1f;
        if (Input.GetMouseButton(1)) { walkAu.pitch = 2f; }
        float m = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
        if (m > 0.1f && !isMoving)
        {
            walkAu.Play();
            isMoving = true;
        }
        else if (m < 0.1f && isMoving)
        {
            walkAu.Pause();
            isMoving = false;
        }

        //detect interaction with environment 
        RaycastHit hit;
        Ray sight = cam.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        if (Physics.Raycast(sight, out hit))
        {
            if (Input.GetMouseButtonDown(0) && hit.distance < 4f)
            {
                IInteractable i = hit.collider.GetComponent<IInteractable>() ??
                                  hit.collider.GetComponentInParent<IInteractable>() ??
                                  hit.collider.GetComponentInChildren<IInteractable>();
                if (i != null)
                {
                    i.Interact();
                }
            }
        }
    }

    void FixedUpdate()
    {   
        if (disabled)
        {
            goto cam;
        }

        //basic movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float v = walkSpeed;
        if (Input.GetMouseButton(1)) { v *= 2f;}
        rb.velocity = (Vector3.Normalize(new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z)) * z + cam.transform.right * x) * v + new Vector3(0f, rb.velocity.y, 0f);

        //head bob
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.z);
        d += Vector2.Distance(lastPos, currentPos);
        lastPos = currentPos;
        float y = h + Mathf.Abs(Mathf.Sin(d * 1.5f) * 0.15f);
        cam.localPosition = new Vector3(0f, y, 0f);

        //camera rotation
        cam:
        mouseDelta += new Vector2(Input.GetAxis("Mouse X") * mouseXSensitivity, Input.GetAxis("Mouse Y") * mouseYSensitivity);
        cam.rotation = Quaternion.Euler(Mathf.LerpAngle(cam.rotation.eulerAngles.x, -mouseDelta.y, 0.1f), Mathf.LerpAngle(cam.rotation.eulerAngles.y, mouseDelta.x, 0.1f), 0f);
    }

    public static void Constrain()
    {
        disabled = true;
    }

    public static void Release()
    {
        disabled = false;
    }

    public static void PlaySound(AudioClip clip)
    {
        mainAu.PlayOneShot(clip);
    }

    void OnDestroy()
    {
        disabled = false;
    }
}
