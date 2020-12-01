using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Pistola : MonoBehaviour
{
    public Animator anim;
    public AudioSource som;
    public VisualEffect vfx;
    public VisualEffect vfxFaisca;


    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Atirar");
            if (!CameraMan.mirando)
                vfx.Play();

            som.pitch = Random.Range(.8f, 1.2f);
            som.Play();

            if (Physics.Raycast(transform.position, Camera.main.transform.forward, out RaycastHit hit, 100))
            {
                print(hit.transform.name);
                var faisca = Instantiate<VisualEffect>(vfxFaisca);
                faisca.transform.position = hit.point;
                faisca.Play();
            }
        }
    }
}
