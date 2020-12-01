using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverAutomatico : MonoBehaviour
{
    public bool playerPerto = false;
    public MovePlayer player;
    public float alturaLigado = .3f;
    public float alturaDesligado = .000001f;
    public float velVertical;
    public GameObject motorista;
    public Transform volante;
    public HoverControle hover;
    public Vector3 posEntrada;


    void Awake()
    {
        player = FindObjectOfType<MovePlayer>();
        hover = transform.GetComponent<HoverControle>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = true;
            player.podeDirigir = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = false;
            player.podeDirigir = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && player.podeDirigir && !MovePlayer.dirigindoHover && playerPerto && !MovePlayer.dirigindoCarro)
        {
            player.carro = gameObject;
            player.volante = volante;
            hover.hoverLigado = true;
            //somMotor.Play();
            player.TrocarEstado(MovePlayer.EstadoAnim.Dirigindo);
            posEntrada = player.transform.position;
            player.transform.SetParent(motorista.transform);
            player.transform.localPosition = Vector3.zero;
            player.transform.localRotation = Quaternion.identity;
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && !player.podeDirigir && MovePlayer.dirigindoHover && hover.hoverLigado)
        {
            hover.hoverLigado = false;
            //somMotor.Stop();
            player.TrocarEstado();
            player.transform.SetParent(null);
            //player.transform.position = player.transform.position + Vector3.left * 1;
            player.transform.position = posEntrada;
            posEntrada = Vector3.zero;
            //player.carro = null;
            //player.volante = null;
            return;
        }
    }
}