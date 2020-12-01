using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarroControle : MonoBehaviour
{
    public WheelCollider DD;
    public WheelCollider DE;
    public WheelCollider TD;
    public WheelCollider TE;
    public Vector3 mov;
    public bool freando = false;
    public float torque = 1000;
    public float freioDeMao = 2000;
    public float curva = 30;
    public float curvaVolante = 90;
    public Rigidbody rdb;
    [Space (20)]
    public bool carroLigado = false;
    [Space (20)]
    public bool tracaoDianteira = true;
    public bool tracaoTraseira = false;
    PortaAutomatica auto;

    void Awake()
    {
        DD = transform.Find("DD").transform.GetComponent<WheelCollider>();
        DE = transform.Find("DE").transform.GetComponent<WheelCollider>();
        TD = transform.Find("TD").transform.GetComponent<WheelCollider>();
        TE = transform.Find("TE").transform.GetComponent<WheelCollider>();
        rdb = GetComponent<Rigidbody>();
        rdb.centerOfMass += new Vector3(0, -.5f, 0);
        auto = GetComponent<PortaAutomatica>();
    }

    void Update()
    {
        if (MovePlayer.dirigindoCarro && carroLigado)
        {
            mov = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            auto.volante.localRotation = Quaternion.Euler(0, mov.x * curvaVolante, 0);
        }
        if (!MovePlayer.dirigindoCarro)
            freando = true;

        if (Input.GetKeyDown(KeyCode.Space))
            freando = true;
        
        if (Input.GetKeyUp(KeyCode.Space))
            freando = false;
    }

    void FixedUpdate()
    {
        if (freando)
        {
            if (tracaoDianteira)
            {
                DD.brakeTorque = freioDeMao;
                DE.brakeTorque = freioDeMao;
            }

            if (tracaoTraseira)
            {
                TD.brakeTorque = freioDeMao;
                TE.brakeTorque = freioDeMao;
            }
        }
        else
        {
            if (tracaoDianteira)
            {
                DD.brakeTorque = 0;
                DE.brakeTorque = 0;
            }

            if (tracaoTraseira)
            {
                TD.brakeTorque = 0;
                TE.brakeTorque = 0;
            }
        }

        if (!carroLigado)
            return;

        if (tracaoDianteira)
        {
            DD.motorTorque = torque * mov.z;
            DE.motorTorque = torque * mov.z;
        }

        if (tracaoTraseira)
        {
            TD.motorTorque = torque * mov.z;
            TE.motorTorque = torque * mov.z;
        }

        float rpm = 1 + Mathf.Abs(mov.z) / 10 + (TD.rpm + TD.rpm + DD.rpm + DE.rpm) / 4000;
        auto.somMotor.pitch = Mathf.Clamp(rpm, 1f, 2f);

        DD.steerAngle = curva * mov.x;
        DE.steerAngle = curva * mov.x;
    }
}