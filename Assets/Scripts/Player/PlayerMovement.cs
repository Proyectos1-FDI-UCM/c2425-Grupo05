//---------------------------------------------------------
// Contiene lo referente al movimiento del jugador.
// Edición y o creación: Víctor, Óscar, Adrián Erustes, Amiel
// I'm Loosing It
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] private float speed = 7f;
    [SerializeField] private float deceleration = 0.5f;
    [SerializeField] private float jumpForceInitial = 2f;
    [SerializeField] private float jumpForce = 0.1f;
    [SerializeField] private float jumpTime;
    [SerializeField] private float bufferTime = 0.2f;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float tiempocoyotetime; // en segundos 
    public LayerMask ground;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private float physicsComparationDistance = 0.05f;

    //para comprobar colisiones
    private Rigidbody2D rb;
    private GameObject _child;
    private Collider2D jumpCollider;
    private Collider2D upLeftCollider;
    private Collider2D upRightCollider;
    private Collider2D upCenterCollider;

    //para efectuar salto
    PlatformMovement platform;
    [SerializeField]private bool isGrounded;
    private bool isAccelerating;

    //cuando pasa a true, salta y justo depués se pone a false para no saltar varias veces con un input.
    [SerializeField] private bool justJumped = false;
    [SerializeField] private bool coyotetime = false;

    [SerializeField] private float justSpringed = 0;

    //sirve para que aunque hayas dejado de pulsar el input antes de empezar el salto (se puede dar el caso debido a input buffer),
    //detecte que has dejado de pulsar el salto ergo deje de impulsarte para arriba.
    private bool jumpWasReleased = true;
    private float tiempocoyote = 0f;
    private float jumpTimeCounter;
    [SerializeField] private float jumpBufferCounter;

    //para corner correction
    private Vector3 lastPhisicsFrameVelocity;
    private bool cornerCorrectedLastFrame;


    //input del inputManager
    Vector2 moveInput;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    
    void Start()
    {
        
        rb = gameObject.GetComponent<Rigidbody2D>();

        #region//Coge referencias a los colliders correspondientes a los hijos

        _child = transform.GetChild(0).gameObject; // Obtiene el primer hijo directamente
        jumpCollider = _child.GetComponent<Collider2D>(); // Obtiene su Collider2D

        _child = transform.GetChild(1).gameObject; // Obtiene el segundo hijo directamente
        upLeftCollider = _child.GetComponent<Collider2D>(); // Obtiene su Collider2D

        _child = transform.GetChild(2).gameObject;
        upRightCollider = _child.GetComponent<Collider2D>();

        _child = transform.GetChild(3).gameObject;
        upCenterCollider = _child.GetComponent<Collider2D>();
        #endregion

        justJumped = false;

        cornerCorrectedLastFrame = false;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (jumpCollider == null) return;
        if (InputManager.Instance == null) return;

        //Obtener el vector de movimiento desde el InputManager.
        moveInput = InputManager.Instance.MovementVector;

        //Voltear el sprite
        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }

        if (InputManager.Instance.JumpWasPressedThisFrame())
        {
            jumpBufferCounter = bufferTime;
            jumpWasReleased = false;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (InputManager.Instance.JumpWasReleasedThisFrame())
        {
            jumpWasReleased = true;
        }

        //CoyoteTime
        if (!isGrounded && coyotetime)
        {
            tiempocoyote += Time.deltaTime;  // Incrementa el tiempo temporizador

            if (tiempocoyote > tiempocoyotetime)
            {
                coyotetime = false;  // Termina el Coyote Time cuando temporizador supera el límite
            }
        }
        //Detección inputs salto
        if ((coyotetime || isGrounded) && jumpBufferCounter > 0f)
        {

            justJumped = true;
            jumpBufferCounter = 0f;

        }
        else if (jumpWasReleased && isAccelerating)
        {
            isAccelerating = false;
        }
    }
    void FixedUpdate()
    {

        CollisionDetection();

        SetInitialVelocity();

        JumpCalculations();

        CornerCorrection();


        lastPhisicsFrameVelocity = rb.velocity;
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    public void Spring(float i) // Accion del jugador con el muelle
    {

        justSpringed = i; // Se le da verticalmente la fuerza recibida
        
    }

    public bool inGround() { return isGrounded; }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// se encarga de detectar si el jugador ha colisionado con una plataforma y con el suelo para futuras comprobaciones.
    /// </summary>

    private void CollisionDetection()
    {

        isGrounded = false;
        platform = null;

        //Detecta si el jumpCollider está colisionando con algo
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(jumpCollider.bounds.center, jumpCollider.bounds.size, 0);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject && ((1 << hitCollider.gameObject.layer) & ground) != 0)
            {
                isGrounded = true;
                coyotetime = true;
                tiempocoyote = 0f;

                if (hitCollider.gameObject.GetComponent<PlatformMovement>() != null && hitCollider.isTrigger)
                {
                    platform = hitCollider.gameObject.GetComponent<PlatformMovement>();
                }
            }
        }
    }

    /// <summary>
    /// Setea la velocidad inicial a la correspondiente, dependiendo del input del jugador, de la velocidad previa, y de si está en una plataforma o no.
    /// </summary>
    private void SetInitialVelocity()
    {
        if (platform == null)
        {
            // si la velocidad mia es mayor a la máxima y no estoy pulsando la tecla de la dir contraria o el 0:
            if ((moveInput.x < 0 && rb.velocity.x < moveInput.x * speed) || (moveInput.x > 0 && rb.velocity.x > moveInput.x * speed))
            {
                //va al fixedUpdate porque si va a más fps, se resta la deceleración más veces/segundo
                rb.velocity = new Vector2(rb.velocity.x - deceleration, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);
            }

        }
        else if (platform != null)
        {


            Debug.Log(platform.getVel());
            rb.velocity = new Vector2(moveInput.x * speed + platform.getVel().x, platform.getVel().y); 

        }
    }

    /// <summary>
    /// Aplica la corner correction.
    /// Siempre guarda la velocidad al inicio del frame de físicas anterior (antes de que si te chocas, te reste velocidad).
    /// Si te chocas conta una esquina(como te chocas, tu velocidad pasa a ser 0), te recoloca y te vuelve a aplicar la velocidad que tenías antes de chocar contra la esquina.
    /// </summary>
    private void CornerCorrection()
    {

        bool Left = false;
        bool Right = false;
        bool Center = false;





        #region Detecta tres colliders de corner correction y si hay colisión, Lo devuelve en Left, Right y Center
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(upLeftCollider.bounds.center, upLeftCollider.bounds.size, 0);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject && ((1 << hitCollider.gameObject.layer) & ground) != 0) //por qué se usa & y no &&?
            {
                Left = true;
            }
        }

        hitColliders = Physics2D.OverlapBoxAll(upRightCollider.bounds.center, upRightCollider.bounds.size, 0);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject && ((1 << hitCollider.gameObject.layer) & ground) != 0) //por qué se usa & y no &&?
            {
                Right = true;
            }
        }

        hitColliders = Physics2D.OverlapBoxAll(upCenterCollider.bounds.center, upCenterCollider.bounds.size, 0);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject && ((1 << hitCollider.gameObject.layer) & ground) != 0) //por qué se usa & y no &&?
            {
                Center = true;
            }
        }
        #endregion

        if (Left && !(Right || Center))
        {

            RaycastHit2D hit = Physics2D.Raycast(new Vector2(upLeftCollider.bounds.center.x + upLeftCollider.bounds.extents.x, upLeftCollider.bounds.center.y + upLeftCollider.bounds.extents.y + physicsComparationDistance), new Vector2(-1, 0), upLeftCollider.bounds.size.x, ground);
            // Debug.Log("Left ray casted");
            Debug.DrawLine(new Vector2(upLeftCollider.bounds.center.x + upLeftCollider.bounds.extents.x, upLeftCollider.bounds.center.y + upLeftCollider.bounds.extents.y + physicsComparationDistance), hit.point, Color.yellow);
            if (hit)
            {
                rb.velocity = new Vector2(0, lastPhisicsFrameVelocity.y);
                rb.transform.Translate(new Vector2(upLeftCollider.bounds.size.x - hit.distance + .0001f, 0));
            }
        }
        else if (Right && !(Left || Center))
        {

            RaycastHit2D hit = Physics2D.Raycast(new Vector2(upRightCollider.bounds.center.x - upRightCollider.bounds.extents.x, upRightCollider.bounds.center.y + upRightCollider.bounds.extents.y + physicsComparationDistance), new Vector2(1, 0), upRightCollider.bounds.size.x, ground);
            // Debug.Log("Right ray casted");
            Debug.DrawLine(new Vector2(upRightCollider.bounds.center.x - upRightCollider.bounds.extents.x, upRightCollider.bounds.center.y + upRightCollider.bounds.extents.y + physicsComparationDistance), hit.point, Color.yellow);
            if (hit)
            {
                rb.transform.Translate(new Vector2(-(upRightCollider.bounds.size.x - hit.distance + 0.0001f), 0));
                rb.velocity = new Vector2(0, lastPhisicsFrameVelocity.y);

            }

        }

        if (Center) { isAccelerating = false; }

    }

    /// <summary>
    /// Añade las velocidades necesarias para que el jugador salte.
    /// </summary>
    private void JumpCalculations()
    {
        if (justSpringed != 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, justSpringed);
            coyotetime = false;
            justJumped = false;
            isGrounded = false;
            justSpringed = 0;
        }
        else
        {
            //salto(inicial)
            //JustJumped sirve para que se ejecute el impulso de salto inicial una sola vez.
            if (justJumped)
            {
                //Debug.Log("salto");
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpForceInitial);
                isAccelerating = true;
                jumpTimeCounter = 0;
                justJumped = false;

                if (coyotetime)
                {
                    coyotetime = false;
                }

            }

            //salto(continuación)
            if (isAccelerating && jumpTimeCounter < jumpTime)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpForce);
                jumpTimeCounter += Time.fixedDeltaTime;//Time.fixedDeltaTime para el fixedUpdate
            }
            else
            {
                isAccelerating = false;
            }
        }
    }

    #endregion

} // class PlayerMovement 
// namespace
