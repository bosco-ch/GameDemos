using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class movement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D playerBody;
    public float curretSpeed;
    private Vector2 vector;
    public SpriteRenderer spriteRenderer;
    public Animator anim;
    private void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
        vector = transform.position;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }
    private void FixedUpdate()
    {
        float intputX = Input.GetAxisRaw("Horizontal");
        float intputY = Input.GetAxisRaw("Vertical");
        Vector2 velocity =
            new Vector2(intputX, intputY) * curretSpeed;
        playerBody.velocity = new Vector2(intputX, intputY) * curretSpeed;
        anim.SetFloat("speed", playerBody.velocity.sqrMagnitude);
        if (intputX != 0)
        {
            GetComponent<SpriteRenderer>().flipX = intputX < 0;
        }
    }
}
