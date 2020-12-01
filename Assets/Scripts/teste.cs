using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teste : MonoBehaviour
{
    void Start()
    {
        print(GetComponentInChildren<Animation>() ? GetComponentInChildren<Animation>().gameObject.name : "sem animation");
        print(GetComponentInChildren<Animator>() ? GetComponentInChildren<Animation>().gameObject.name : "sem animator");
    }
}