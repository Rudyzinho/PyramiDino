using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoControle : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public Transform foot;

    public LayerMask floor;

    public float velocidade;
    float inputX;

    public float jumpForce;
    bool isGrounded;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(foot.position, 0.3f, floor);

        inputX = Input.GetAxis("Horizontal");

        rb2D.velocity = new Vector2(inputX * velocidade, rb2D.velocity.y);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb2D.velocity = Vector2.up * jumpForce;
        }
            
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
