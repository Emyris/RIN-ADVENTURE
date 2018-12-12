using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_move : MonoBehaviour {

    public float moveSpeed;
    private Rigidbody2D myRigidbody;

    public bool isWalking;

    public float walkTime;
    private float walkCounter;

    public float waitTime;
    private float waitCounter;

    private int WalkDirection;

    ///agregado freeze
    public bool canMove;
    private Dialogue_ theDM;

    /// Limitamos el espacio de camino del NPC

    public Collider2D walkZone;

    private Vector2 minWalkPoint;
    private Vector2 maxWalkPoint;
    private bool hasWalkZone;

    //Agregamos los estados

    Animator anim;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        waitCounter = waitTime;
        walkCounter = walkTime;

        ChooseDirection();

        //Tomamos la animacion
        anim = GetComponent<Animator>();

        //Los valores de limite de caminado
        if(walkZone != null)
        {
        minWalkPoint = walkZone.bounds.min;
        maxWalkPoint = walkZone.bounds.max;
            hasWalkZone = true;
        }

        //Agregado para freeze
        canMove = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        theDM = FindObjectOfType<Dialogue_>();

    }

    void Update()
    {
        //// agregado

        if (!theDM.dialogActive)
        {
            canMove = true;

        }


        if (!canMove)
        {
            myRigidbody.velocity = Vector2.zero;
            return;
        }

        ////////////

        if (isWalking)
        {
            walkCounter-=Time.deltaTime;

            switch (WalkDirection)
            {
                //Arriba
                case 0:
                    myRigidbody.velocity = new Vector2(0, moveSpeed);

                    //agregamos animacion
                    Vector2 mov = new Vector2(0, moveSpeed);
                    anim.SetFloat("MovX", mov.x);
                    anim.SetFloat("MovY", mov.y);
                    
                    // checamos si salimos de la zona y paramos
                    if (hasWalkZone && transform.position.y > maxWalkPoint.y)
                    {
                        isWalking = false;
                        waitCounter = waitTime;
                    }

                        break;
                //Derecha
                case 1:
                    myRigidbody.velocity = new Vector2(moveSpeed,0);
                   
                    //agregamos animacion
                    Vector2 mov1 = new Vector2(moveSpeed, 0);
                    anim.SetFloat("MovX", mov1.x);
                    anim.SetFloat("MovY", mov1.y);

                    // checamos si salimos de la zona y paramos
                    if (hasWalkZone && transform.position.x > maxWalkPoint.x)
                    {
                        isWalking = false;
                        waitCounter = waitTime;
                    }


                    break;
                //Abajo
                case 2:
                    myRigidbody.velocity = new Vector2(0,-moveSpeed);

                    //agregamos animacion
                    Vector2 mov2 = new Vector2(0, -moveSpeed);
                    anim.SetFloat("MovX", mov2.x);
                    anim.SetFloat("MovY", mov2.y);
                    
                    // checamos si salimos de la zona y paramos
                    if (hasWalkZone && transform.position.y < minWalkPoint.y)
                    {
                        isWalking = false;
                        waitCounter = waitTime;
                    }

                    break;
                //Izquierda
                case 3:
                    myRigidbody.velocity = new Vector2(-moveSpeed,0);

                    //agregamos animacion
                    Vector2 mov3 = new Vector2(-moveSpeed, 0);
                    anim.SetFloat("MovX", mov3.x);
                    anim.SetFloat("MovY", mov3.y);


                    // checamos si salimos de la zona y paramos
                    if (hasWalkZone && transform.position.x < minWalkPoint.x)
                    {
                        isWalking = false;
                        waitCounter = waitTime;
                    }

                    break;

            }

            if (walkCounter < 0)
            {
                isWalking = false;
                waitCounter = waitTime;
            }

        }
        else
        {
            waitCounter -= Time.deltaTime;

            myRigidbody.velocity = Vector2.zero;

            if (waitCounter < 0)
            {
                ChooseDirection();
            }

        }

    }

    public void ChooseDirection()
    {

        WalkDirection = Random.Range(0, 4);
        isWalking = true;
        walkCounter = walkTime;

    }
}
