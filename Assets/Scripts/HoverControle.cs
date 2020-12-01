using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverControle : MonoBehaviour
{
    public bool hoverLigado;
    public float velMove = 5;
    public float velRot = 10;
    public Vector3 move;


    void Update()
    {
        if (hoverLigado)
            move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        else
            move = Vector3.zero;
    }

    void FixedUpdate()
    {
        transform.Translate(move.z * Vector3.forward * velMove * Time.fixedDeltaTime);
        transform.Rotate(move.x * Vector3.up * velRot * Time.fixedDeltaTime);
    }
}