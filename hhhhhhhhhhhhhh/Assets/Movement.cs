using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D)) rb.velocity = Vector2.right * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) rb.velocity = -Vector2.right * speed * Time.deltaTime;
    }
}
