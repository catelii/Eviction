using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaAutomatica : MonoBehaviour
{
    public GameObject porta;
    MovePlayer player;
    public bool playerPerto;
    public GameObject motorista;
    public Transform volante;
    CarroControle carro;
    public AudioSource somPorta;
    public AudioClip abrirPorta;
    public AudioClip fecharPorta;
    public AudioSource somMotor;


    void Awake()
    {
        porta = transform.Find("door_fl").gameObject;
        somPorta = porta.transform.GetComponent<AudioSource>();
        player = FindObjectOfType<MovePlayer>();
        carro = transform.GetComponent<CarroControle>();
        volante = transform.Find("Direcao").transform.GetChild(0).transform;
        somMotor = transform.Find("Motor").transform.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && player.podeDirigir && !MovePlayer.dirigindoCarro && playerPerto && !MovePlayer.dirigindoHover)
        {
            player.carro = gameObject;
            player.volante = volante;
            carro.carroLigado = true;
            somMotor.Play();
            player.TrocarEstado(MovePlayer.EstadoAnim.Dirigindo);
            player.transform.SetParent(motorista.transform);
            player.transform.localPosition = Vector3.zero;
            player.transform.localRotation = Quaternion.identity;
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && !player.podeDirigir && MovePlayer.dirigindoCarro && carro.carroLigado)
        {
            carro.carroLigado = false;
            somMotor.Stop();
            player.TrocarEstado();
            player.transform.SetParent(null);
            player.transform.position = player.transform.position + Vector3.left * 2;
            //player.carro = null;
            //player.volante = null;
            return;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = true;
            StartCoroutine(AbrirPorta());
            player.podeDirigir = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = false;
            StartCoroutine(FecharPorta());
            player.podeDirigir = false;
        }
    }

    IEnumerator AbrirPorta()
    {
        somPorta.PlayOneShot(abrirPorta);
        float ang = 0;
        while (ang <= 45)
        {
            yield return new WaitForFixedUpdate();
            ang += Time.fixedDeltaTime * 180;
            porta.transform.localRotation = Quaternion.Euler(0, ang, 0);
        }
        porta.transform.localRotation = Quaternion.Euler(0, 45, 0);
    }

    IEnumerator FecharPorta()
    {
        float ang = 45;
        while (ang >= 0)
        {
            carro.freando = true;
            yield return new WaitForFixedUpdate();
            ang -= Time.fixedDeltaTime * 60;
            porta.transform.localRotation = Quaternion.Euler(0, ang, 0);
        }
        carro.freando = false;
        somPorta.PlayOneShot(fecharPorta);
        porta.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}