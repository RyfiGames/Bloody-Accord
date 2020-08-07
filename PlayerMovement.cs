using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    //    public Animator anim;
    public PlayerController player;
    public Rigidbody rb;
    public GameObject infoPanel;
    public GameObject menuPanel;
    public Transform pp;

    [Header("Cam Stuff")]
    public Camera pcam;
    public Transform[] camSpots;
    public int camSpotID;

    [Header("Variables")]
    public float speed;
    public float rotSpeed;
    public float jumpMag;
    public float jumpDist;
    public bool moveable = true;

    public bool moving;
    public float moveTime;
    public float bobSpeed;

    public bool paused;

    /** Timers **/
    int pTimer;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void RotateCamera()
    {
        camSpotID++;
        if (camSpotID >= camSpots.Length)
        {
            camSpotID = 0;
        }

        pcam.transform.parent = camSpots[camSpotID];
        pcam.transform.position = pcam.transform.parent.position;
        pcam.transform.rotation = pcam.transform.parent.rotation;
    }

    bool OnGround()
    {
        bool r = Physics.Raycast(transform.position, Vector3.down, jumpDist, 9);
        //print(r);
        return r;
    }

    public void OpenMenu(bool open)
    {
        //infoPanel.SetActive(!open);
        menuPanel.SetActive(open);
        moveable = !open;

        if (open)
        {
            player.RespondBuy(false);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            paused = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            paused = false;
        }
    }


    public void Move()
    {

        /*        if (Input.GetKey(KeyCode.L))
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
                if (Input.GetKey(KeyCode.E))
                {
                    Cursor.lockState = CursorLockMode.None;
                } */

        /*    if (Input.GetKey(KeyCode.P) && pTimer <= 0)
            {
                RotateCamera();
                pTimer = 15;
            } */

        if (Input.GetKey(KeyCode.Space) && OnGround())
        {
            rb.AddRelativeForce(Vector3.up * jumpMag);
        }

        if (Input.GetAxisRaw("Vertical") > 0)
        {
            rb.AddRelativeForce(Vector3.forward * speed);
            moving = true;
            //anim.SetBool("Walking", true);
        }
        else
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            rb.AddRelativeForce(Vector3.back * speed);
            moving = true;
            //anim.SetBool("Walking", true);
        }
        else
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            rb.AddRelativeForce(Vector3.right * speed);
            moving = true;
            //anim.SetBool("Walking", true);
        }
        else
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            rb.AddRelativeForce(Vector3.left * speed);
            moving = true;
            //anim.SetBool("Walking", true);
        }
        else
        {
            //anim.SetBool("Walking", false);
            moving = false;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + Input.GetAxis("Mouse X") * rotSpeed, transform.eulerAngles.z);
        Vector3 neckAngles = new Vector3(pcam.transform.eulerAngles.x + Input.GetAxis("Mouse Y") * -rotSpeed, pcam.transform.eulerAngles.y, pcam.transform.eulerAngles.z);

        if (neckAngles.x < 40f || neckAngles.x > 320f)
        {
            pcam.transform.eulerAngles = neckAngles;
        }


        if (Input.GetKey(KeyCode.Escape))
        {
            OpenMenu(true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            player.ClickAction("down");
        }
        if (Input.GetMouseButtonUp(0))
        {
            player.ClickAction("up");
        }
        if (Input.GetMouseButton(0))
        {
            player.ClickAction("full");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (moveable)
                Move();

            if (pTimer > 0)
            {
                pTimer--;
            }

            if (!moving)
            {
                moveTime = 0;
                pcam.transform.localPosition = new Vector3(pcam.transform.localPosition.x, 0.4f, pcam.transform.localPosition.z);
            }
            else if (System.Math.Abs(speed) > 0.001f)
            {
                moveTime++;
                pcam.transform.localPosition = new Vector3(pcam.transform.localPosition.x, 0.1f * Mathf.Sin(bobSpeed * moveTime) + 0.4f, pcam.transform.localPosition.z);
            }
        }
        else
        {

        }
    }
}
