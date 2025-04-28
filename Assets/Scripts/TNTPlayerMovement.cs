using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TNTPlayerMovement : MonoBehaviour
{
    [Header("Stats")]
    public float velocidade = 5f;
    public float forcapulo = 10f;
    [SerializeField] private float dashingVelocidade = 18f;
    [SerializeField] private float dashingTempo = 0.2f;
    private Vector2 dashingDirecao;
    public float segundos = 6f;

    [Space]
    [Header("Components")]
    private Rigidbody2D rb;
    public Transform verificadordechao;
    public LayerMask camadadochao;
    [SerializeField] private LayerMask camadaParede;
    private Animator animator;
    private TrailRenderer trailRenderer;
    public AudioSource dashSound;

    [Space]
    [Header("Booleans")]
    private bool estaNOchao;
    private bool estaDashing;
    private bool podeDash = true;
    private bool podeMover = true;
    private bool viradoParaDireita = true;
    private bool estaNaParede = false;

    [Space]
    [Header("wallmovement")]
    public float wallslidSpeed = 2f;
    private bool iswallSlidin;

    [Header("wallJump")]
    private bool isWallJumping;
    private float WallJumpingDirection;
    private float WallJumpingTime = 0.3f;
    private float WallJumpingCouter;
    private float WallJumpingDuration = 0.4f;
    private Vector2 WallJumpingPower = new Vector2(5f, 12f);
    

    private GameObject paredeAtual;
    private GameObject ultimaParedeUsada;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    void Update()
    {

        if (!estaDashing)
        {
            estaNOchao = Physics2D.OverlapCircle(verificadordechao.position, 0.2f, camadadochao);
        }
        

        if (estaNOchao)
        {
            ultimaParedeUsada = null;
        }

        if (!estaDashing)
        {
            Mover();
        }

        animator.SetBool("isWallSliding", (estaNaParede && !estaNOchao));
        animator.SetBool("isDashing", estaDashing);


        Pular();
        Virar();
        Dash();
        processwallSlid();
        AgarrarEMoverNaParede();
        WallJump();

        if (estaDashing)
        {
            rb.velocity = dashingDirecao.normalized * dashingVelocidade;
            return;
        }
    }

    private void FixedUpdate()
    {
        if(!isWallJumping && !estaDashing)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(horizontal * velocidade, rb.velocity.y);
        }
    }

    public void Mover()
    {
        if (podeMover && !isWallJumping)
        {
            float entradaMovimento = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(entradaMovimento * velocidade, rb.velocity.y);

            bool isRunning = Mathf.Abs(entradaMovimento) > 0.1f;
            animator.SetBool("isRunning", isRunning);
        }
    }

    public void Pular()
    {
        if (estaNOchao && (Input.GetKeyDown(KeyCode.Space) || (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)))
        {
            rb.velocity = new Vector2(rb.velocity.x, forcapulo);
            rb.gravityScale = 3;
        }

        if ((Input.GetKeyUp(KeyCode.Space) || (Gamepad.current != null && Gamepad.current.buttonSouth.wasReleasedThisFrame)) && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (estaNOchao || estaDashing)
        {
            animator.SetBool("isJumping", false);
        }
        else
        {
            animator.SetBool("isJumping", true);
        }
    }

    void Virar()
    {
        float entradaMovimento = Input.GetAxis("Horizontal");
        if (entradaMovimento > 0 && !viradoParaDireita || entradaMovimento < 0 && viradoParaDireita)
        {
            viradoParaDireita = !viradoParaDireita;
            Vector3 escalaLocal = transform.localScale;
            escalaLocal.x *= -1;
            transform.localScale = escalaLocal;
        }
    }

    public void Dash()
    {
        if ((Input.GetKeyDown(KeyCode.K) || (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)) && podeDash)
        {
            estaNOchao = false;
            estaDashing = true;
            podeDash = false;
            podeMover = false;
            trailRenderer.emitting = true;
            dashSound.Play();

            float dashX = Input.GetAxisRaw("Horizontal");
            float dashY = Input.GetAxisRaw("Vertical");

            if (dashX == 0 && dashY == 0)
            {
                dashX = transform.localScale.x > 0 ? 1 : -1;
            }

            dashingDirecao = new Vector2(dashX, dashY).normalized;

            StartCoroutine(PararDash());

            if (estaNaParede)
            {
                estaDashing = false;
            }
        }

        if (estaNOchao)
        {
            podeDash = true;
        }
    }

    private IEnumerator PararDash()
    {
        yield return new WaitForSeconds(dashingTempo);
        trailRenderer.emitting = false;
        estaDashing = false;
        podeMover = true;
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    IEnumerator DashNOchao()
    {
        estaNOchao = false;
        yield return new WaitForSeconds(dashingTempo);
        if (estaNOchao)
        {
            podeDash = true;
        }
    }


    public void AgarrarEMoverNaParede()
    {
        bool segurandoShift = (Input.GetKey(KeyCode.LeftShift) || (Gamepad.current != null && Gamepad.current.buttonWest.isPressed));

        if (segurandoShift && estaNaParede && !estaNOchao)
        {
            float entradaVertical = Input.GetAxisRaw("Vertical");
            animator.SetBool("isWallSliding", true);

            if (Mathf.Abs(entradaVertical) > 0.1f)
            {
                rb.velocity = new Vector2(0, entradaVertical * (velocidade * 0.7f));
            }
            else
            {
                rb.velocity = new Vector2(0, 0);
            }
            rb.gravityScale = 0;
            iswallSlidin = false;
            isWallJumping = false;
        }
        else if (estaNaParede && !estaNOchao)
        {
            rb.gravityScale = 3;
            isWallJumping = false;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((camadaParede.value & (1 << collision.gameObject.layer)) > 0)
        {
            estaNaParede = true;
            paredeAtual = collision.gameObject;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((camadaParede.value & (1 << collision.gameObject.layer)) > 0)
        {
            paredeAtual = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((camadaParede.value & (1 << collision.gameObject.layer)) > 0)
        {
            estaNaParede = false;
            rb.gravityScale = 3;
            
            if (paredeAtual == collision.gameObject)
            {
                paredeAtual = null;
            }
        }
    }

    private void processwallSlid()
    {
        bool escalando = (Input.GetKey(KeyCode.LeftShift) || (Gamepad.current != null && Gamepad.current.buttonWest.isPressed)) && estaNaParede && !estaNOchao;

        if (estaNOchao || escalando)
        {
            iswallSlidin = false;
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");

        bool pressionandoNaDirecao = false;

        if (estaNaParede)
        {
            if (viradoParaDireita && horizontal > 0.1f)
            {
                pressionandoNaDirecao = true;
            }
            else if (!viradoParaDireita && horizontal < -0.1f)
            {
                pressionandoNaDirecao = true;
            }
        }

        if (estaNaParede && pressionandoNaDirecao)
        {
            iswallSlidin = true;
            rb.velocity = new Vector2(0, Mathf.Max(rb.velocity.y, -wallslidSpeed));
        }
        else
        {
            iswallSlidin = false;

            if (estaNaParede && !estaNOchao)
            {
                rb.gravityScale = 3;
            }
        }

    }

    public void WallJump()
    {
        if (iswallSlidin)
        {
            WallJumpingDirection = -transform.localScale.x;
            WallJumpingCouter = WallJumpingTime;
        }
        else
        {
            WallJumpingCouter -= Time.deltaTime;
        }
        

        bool paredeJaUsada = (paredeAtual != null && paredeAtual == ultimaParedeUsada);


        if ((Input.GetKeyDown(KeyCode.Space) || (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)) && WallJumpingCouter > 0f && !paredeJaUsada)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(WallJumpingDirection * WallJumpingPower.x, WallJumpingPower.y);
            WallJumpingCouter = 0f;
            

            ultimaParedeUsada = paredeAtual;

            if (transform.localScale.x != WallJumpingDirection)
            {
                viradoParaDireita = !viradoParaDireita;
                Vector3 escalaLocal = transform.localScale;
                escalaLocal.x *= -1;
                transform.localScale = escalaLocal;
            }

            Invoke(nameof(stopwalljump), WallJumpingDuration);

            
        }
    }

    private void stopwalljump()
    {
        isWallJumping = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("power up"))
        {
            other.gameObject.SetActive(false);
            podeDash = true;
            StartCoroutine(ReativarDepois(other.gameObject, segundos));
        }
    }

    private IEnumerator ReativarDepois(GameObject objetoParaReativar, float segundos)
    {
        yield return new WaitForSeconds(segundos);

        if (objetoParaReativar != null)
        {
            objetoParaReativar.SetActive(true);
        }
    }

}