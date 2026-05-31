using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Disappear : MonoBehaviour
{
    // public Transform target;
    public Transform transform;
    public ParticleSystem particleSystem;

    Material material;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
        // particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Die();
        }
    }
    private void Die()
    {
        particleSystem.Play();
        material.DOFloat(-1, "_Strength", 1.5f);
    }
}
