using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    Rigidbody rdb;
    Animator anim;
    public float velMove = 3;
    public Vector3 move;
    public GameObject arma;
    float velCorrer;
    float velocidade;
    public static bool dirigindoCarro = false;
    public static bool dirigindoHover = false;
    public bool podeDirigir = false;
    public GameObject carro;
    public Transform volante;
    public enum EstadoAnim
    {
        ComArma,
        SemArma,
        Dirigindo
    }
    public EstadoAnim estadoAtual;
    public EstadoAnim estadoAnterior;


    void Awake()
    {
        Rigidbody[] rdbs = GetComponentsInChildren<Rigidbody>();
        Joint[] joints = GetComponentsInChildren<Joint>();

        foreach (Joint joint in joints)
            Destroy(joint);

        for (int x = 1; x < rdbs.Length; x++)
            Destroy(rdbs[x]);

        rdb = rdbs[0];
        anim = GetComponent<Animator>();
        velCorrer = velMove * 1.5f;
        velocidade = velMove;
        anim.SetLayerWeight(1,1);
        estadoAtual = EstadoAnim.SemArma;
        TrocaEstado();
    }

    void Update()
    {
        move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (Input.GetKey(KeyCode.LeftShift))
            velocidade = velCorrer;
        else
            velocidade = velMove;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (comArma)
                estadoAtual = EstadoAnim.SemArma;
            else
                estadoAtual = EstadoAnim.ComArma;
        }
    }

    IEnumerator ComArma()
    {
        comArma = true;
        anim.SetLayerWeight(1,0);
        arma.SetActive(true);
        while (estadoAtual == EstadoAnim.ComArma)
        {
            yield return new WaitForFixedUpdate();
            movimentacao();
        }
        TrocaEstado();
    }

    IEnumerator SemArma()
    {
        comArma = false;
        anim.SetLayerWeight(1,1);
        arma.SetActive(false);
        while (estadoAtual == EstadoAnim.SemArma)
        {
            yield return new WaitForFixedUpdate();
            movimentacao();
        }
        TrocaEstado();
    }

    IEnumerator Dirigindo() 
    {
        switch (carro.tag)
        {
            case "Carro":
                dirigindoCarro = true;
                break;
            case "Hover":
                dirigindoHover = true;
                break;
        }
        //dirigindoCarro = true;
        rdb.isKinematic = true;
        var cols = transform.GetComponentsInChildren<Collider>();
        foreach (Collider col in cols)
        {
            col.enabled = false;
        }
        anim.SetBool("Dirigindo", true);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        while(estadoAtual == EstadoAnim.Dirigindo)
        {
            yield return new WaitForFixedUpdate();
            anim.SetFloat("MoverLado", -move.x);
            print(MovePlayer.dirigindoCarro);
        }

        switch (carro.tag)
        {
            case "Carro":
                dirigindoCarro = false;
                break;
            case "Hover":
                dirigindoHover = false;
                break;
        }
        //dirigindoCarro = false;
        carro = null;
        volante = null;
        rdb.isKinematic = false;
        foreach (Collider col in cols)
        {
            col.enabled = true;
        }
        anim.SetBool("Dirigindo", false);
        TrocaEstado();
    }

    void TrocaEstado()
    {
        StartCoroutine(estadoAtual.ToString());
    }

    public void TrocarEstado(EstadoAnim estado)
    {
        estadoAnterior = estadoAtual;
        estadoAtual = estado;
    }

    public void TrocarEstado()
    {
        estadoAtual = estadoAnterior;
    }

    void movimentacao()
    {
        transform.Translate(move * velocidade * Time.deltaTime);

        anim.SetFloat("Mover", move.z);
        anim.SetFloat("MoverLado", move.x);
    }


    public bool comArma = false;
    public float compensa = 15;

    private void OnAnimatorIK(int leyerIndex)
    {
        if (comArma)
            anim.SetBoneLocalRotation(HumanBodyBones.Chest, Quaternion.Euler(0, compensa, 0));
        else
            anim.SetBoneLocalRotation(HumanBodyBones.Chest, Quaternion.Euler(0, 0, 0));
        
        if (dirigindoCarro && volante != null)
        {
            anim.SetIKPosition(AvatarIKGoal.RightHand, volante.position + volante.right * .35F);
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, volante.position - volante.right * .35F);
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);

            anim.SetIKRotation (AvatarIKGoal.RightHand, volante.rotation * Quaternion.Euler(90, 90, 0));
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, volante.rotation * Quaternion.Euler(90, -90, 0));
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        }

        if (dirigindoHover && volante != null)
        {
            anim.SetIKPosition(AvatarIKGoal.RightHand, volante.position + new Vector3(.28f, -.05f, -.12f));
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, volante.position - new Vector3(.28f, .05f, .12f));
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);

            anim.SetIKRotation(AvatarIKGoal.RightHand, volante.rotation * Quaternion.Euler(0, 0, -90));
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, volante.rotation * Quaternion.Euler(0, 0, 90));
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        }
    }
}