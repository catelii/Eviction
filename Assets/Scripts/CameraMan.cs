using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    MovePlayer movePlayer;
    GameObject player;
    //GameObject alvo;
    public float velCameraX = 100;
    public static bool mirando = false;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        transform.position =  player.transform.position;
        transform.Rotate(0, Input.GetAxis("Mouse X") * velCameraX * Time.deltaTime, 0);
        if (MovePlayer.dirigindoCarro || MovePlayer.dirigindoHover)
            return;
        player.transform.rotation = transform.rotation;
    }

    void Update()
    {
        if (MovePlayer.dirigindoCarro || MovePlayer.dirigindoHover)
            return;

        if (Input.GetButtonDown("Fire2"))
            mirando = true;

        if (Input.GetButtonUp("Fire2"))
            mirando = false;
    }
}