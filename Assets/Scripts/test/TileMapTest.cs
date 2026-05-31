using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileMapTest : MonoBehaviour
{
    public Tilemap map;
    public Transform target;
    public Vector3 dirct;
    public float speed = 5f;
    Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = target.GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            rb2d = target.AddComponent<Rigidbody2D>();
        }
        rb2d.gravityScale = 0; // 禁止重力影响
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation; // 禁止旋转
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            dirct = new Vector3(0, 1, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dirct = new Vector3(0, -1, 0);

        }
        else if (Input.GetKey(KeyCode.A))
        {
            dirct = new Vector3(-1, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dirct = new Vector3(1, 0, 0);
        }

        rb2d.velocity = dirct * speed;
        var pos = map.WorldToCell(target.position);
        map.GetTile(pos);


    }
}
