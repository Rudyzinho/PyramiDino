using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Vari�veis
    public float gravity; //gravidade
    public Vector2 velocity; //velocidade
    public float maxXVelocity = 100;
    public float maxAcceleration = 10;
    public float acceleration = 10;
    public float distance = 0;
    public float jumpVelocity = 20; //velocidade do pulo
    public float groundHeight = 10; //distancia do ch�o
    public bool isGrounded = false; //boleano que armazena se o personagem est� no ch�o

    public bool isHoldingJump = false; //segurar o pulo
    public float maxHoldJumpTime = 0.4f; //tempo m�ximo para segurar o pulo
    public float maxMaxHoldJumpTime = 0.4f;
    public float holdJumpTimer = 0.0f; //tempo que ele est� segurando o pulo
   

    public float jumpGroundThreshold = 1; //margem de erro do jogador coseguir pular mesmo n�o estando no ch�o

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = transform.position;
        float groundDistance = Mathf.Abs(pos.y - groundHeight); //calcula a distancia do ch�o em rela��o a posi��o y

        if (isGrounded || groundDistance <= jumpGroundThreshold)  //se o personagem estiver no ch�o ou na margem de erro ele executa as func�es dentro da condicional
        {
            if(Input.GetKeyDown(KeyCode.Space)) //se a tecla "Space" � acionada o personagem deixa de estar no ch�o e pula / segura pulo, o tempo atual do pulo reseta
            {
                isGrounded = false;
                velocity.y = jumpVelocity;
                isHoldingJump = true;
                holdJumpTimer = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) //quando solta a tecla o pulo se torna falso e ele cai
        {
            isHoldingJump = false;
        }
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;

        if(!isGrounded) //Se passar do tempo no pulo ele desce
        {

            if (isHoldingJump)
            {
                holdJumpTimer += Time.fixedDeltaTime;
                if (holdJumpTimer >= maxHoldJumpTime)
                {
                    isHoldingJump = false;
                }
            }


            pos.y += velocity.y * Time.fixedDeltaTime;
            if(!isHoldingJump) //a gravidade cobra pai kk
            {
                velocity.y += gravity * Time.fixedDeltaTime;
            }

            Vector2 rayOrigin = new Vector2(pos.x + 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            if (hit2D.collider != null)
            {
                Ground ground = hit2D.collider.GetComponent<Ground>();
                if(ground !=  null)
                {
                    groundHeight = ground.groundHeight;
                    pos.y = groundHeight;
                    velocity.y = 0;
                    isGrounded = true;
                }

            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);

            //vai cair n�o pai deixa disso
           // if (pos.y <= groundHeight)
           // {
           //     pos.y = groundHeight;
            //    isGrounded = true;
           // }
        }

        distance += velocity.x * Time.fixedDeltaTime;


        if (isGrounded)
        {
            float velocityRatio = velocity.x / maxXVelocity;
            acceleration = maxAcceleration * (1 - velocityRatio);
            maxHoldJumpTime = maxMaxHoldJumpTime * velocityRatio;

            velocity.x += acceleration * Time.fixedDeltaTime;
            if (velocity.x >= maxXVelocity)
            { 
                velocity.x = maxXVelocity;
            }

            Vector2 rayOrigin = new Vector2(pos.x - 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            if (hit2D.collider == null)
            {
                isGrounded = false;

            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);
        }




        transform.position = pos;
        
    }
}
