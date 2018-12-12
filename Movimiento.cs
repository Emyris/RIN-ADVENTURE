using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Movimiento : MonoBehaviour {

    public float speed = 4f;

    Animator anim;
    Rigidbody2D rb2d;
    Vector2 mov;

    Aura aura;

    ///Agregado a Freeze
    public bool canMove;
    //private Rigidbody2D myRigidbody; //<- no lo necesite
    /// ////
    /// AGREGAMOS PARA ATTCAR
    CircleCollider2D attackCollider;

    public GameObject initialMap;

    // AGREGAMOS EL SLASH
    public GameObject SlashPrefab;
    bool movPrevent;

    /// ////
    //sonidos
    private SFXManager sfxMAn;

    void Awake()
    {
        Assert.IsNotNull(initialMap);
        Assert.IsNotNull(SlashPrefab);
    }

    void Start () {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        //agregamos el sonido
        sfxMAn = FindObjectOfType<SFXManager>();

       // myRigidbody = GetComponent<Rigidbody2D>();


        //Agregado para freeze player
        canMove = true;
        //

        //Agregado para ATTACK player
        attackCollider = transform.GetChild(0).GetComponent<CircleCollider2D>();
        attackCollider.enabled = false;
       
        //

       Camera.main.GetComponent<Main_Camera>().SetBound(initialMap);

        aura = transform.GetChild(1).GetComponent<Aura>();
    }

    void Update()
    {
         Movement();

        SwordAttack();

        PreventMovement();

        SlashAttack();


    }

    void FixedUpdate()
    {   //nos movemos en el fixed por las fisicas
        rb2d.MovePosition(rb2d.position + mov * speed * Time.deltaTime);  
    }
    void Movement()
    {
        /// Agregado por Freeze
        if (!canMove)
        {
            //myRigidbody.velocity = Vector2.zero; // <- no lo necesite
            anim.SetBool("Walking", false);
            speed = 0;
            return;
        }

        ////
        if (canMove)
        {
            speed = 5F;
            mov = new Vector2(

            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
            
            );

            if (mov != Vector2.zero)
            {
                anim.SetFloat("movX", mov.x);
                anim.SetFloat("movY", mov.y);

                anim.SetBool("Walking", true);
            }
            else
            {
                anim.SetBool("Walking", false);
            }

        }

    }

    void SwordAttack()
    {
        // DESTROY  FUNCTION ======================================//

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        bool attaking = stateInfo.IsName("Player_Attack");

        // ATTACK  FUNCTION ======================================//

        if (Input.GetKeyDown("m") && !attaking)
        {
            sfxMAn.PlayerAttack.Play();
            anim.SetTrigger("Attacking");
            
        }

        if (mov != Vector2.zero) attackCollider.offset = new Vector2(mov.x / 2, mov.y / 2);

        if (attaking)
        {
            float playbackTime = stateInfo.normalizedTime;
            if (playbackTime > 0.33 && playbackTime < 0.66) attackCollider.enabled = true;
            else attackCollider.enabled = false;

        }

    }


    void SlashAttack()
    {
        // Buscamos el estado actual mirando la información del animador
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        bool loading = stateInfo.IsName("Player_Slash");

        // Ataque a distancia
        if (Input.GetKeyDown("n"))
        {
            sfxMAn.PlayerPowerCharging.Play();
            anim.SetTrigger("Loading");
            aura.AuraStart();

        }
        else if (Input.GetKeyUp("n"))
        {
            anim.SetTrigger("Attacking");

            if (aura.IsLoaded()) {
                sfxMAn.PlayerPowerRelease.Play();
                // Para que se mueva desde el principio tenemos que asignar un
                // valor inicial al movX o movY en el edtitor distinto a cero
                float angle = Mathf.Atan2(
                anim.GetFloat("movY"),
                anim.GetFloat("movX")
            ) * Mathf.Rad2Deg;

            GameObject slashObj = Instantiate(
                SlashPrefab, transform.position,
                Quaternion.AngleAxis(angle, Vector3.forward)
            );

            Slash slash = slashObj.GetComponent<Slash>();
            slash.mov.x = anim.GetFloat("movX");
            slash.mov.y = anim.GetFloat("movY");

            }
            aura.AuraStop();
            // esperar momentos y reactivar movimiento
            StartCoroutine(EnableMovementAfter(0.4f));

          }

            // Prevenimos el movimiento mientras cargamos
           if (loading)
                {
                movPrevent = true;
                }
    }

    void PreventMovement()
    {
        if (movPrevent)
        {
            mov = Vector2.zero;
        }
    }

    IEnumerator EnableMovementAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        movPrevent = false;
    }


}
