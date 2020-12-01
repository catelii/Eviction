using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodasControle : MonoBehaviour
{
    public WheelCollider rodaColisor;
    public Transform rodaObjeto;

    void Awake()
    {
        rodaColisor = GetComponent<WheelCollider>();
        rodaObjeto = transform.GetChild(0).transform;
    }

    void FixedUpdate()
    {
        Vector3 pos;
        Quaternion rot;
        rodaColisor.GetWorldPose(out pos, out rot);
        rodaObjeto.position = pos;
        rodaObjeto.rotation = rot;
    }
}