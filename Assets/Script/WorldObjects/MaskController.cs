using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MaskController : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    { }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sr.color = new(sr.color.r, sr.color.g, sr.color.b, 0.0f);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sr.color = new(sr.color.r, sr.color.g, sr.color.b, 1f);
            
        }
    }
}
