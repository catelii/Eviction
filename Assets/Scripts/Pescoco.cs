using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pescoco : MonoBehaviour
{
    GameObject player;
    public float minCam = -8, maxCam = -4.5f;
    public float max = 35, min = 15;
    [Space (20)]
    public float distCam = -5;
    public float velCameraY = 50;
    public GameObject canoArma;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        transform.Translate(0, (Input.GetAxis("Mouse Y") * -1) * velCameraY * Time.fixedDeltaTime, 0);

        Vector3 pos = transform.localPosition;

        if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.mouseScrollDelta.y > 0)
            distCam += .5f;

        if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.mouseScrollDelta.y < 0)
            distCam -= .5f;

        if (pos.y < min)
            pos.y = min;

        if (pos.y > max)
            pos.y = max;

        if (distCam < maxCam)
            distCam = maxCam;

        if (distCam > minCam)
            distCam = minCam;

        if (CameraMan.mirando && player.GetComponent<MovePlayer>().comArma)
        {
            transform.position = canoArma.transform.position;
            transform.LookAt(transform.position + transform.forward);
        }
        else
        {
            transform.localPosition = new Vector3(0, pos.y, distCam);
            transform.LookAt(player.transform.position + Vector3.up);
        }
    }
}