//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Globalization;
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
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] float distanciaparedizquierda = 7.5f;
    [SerializeField] float playerwidth = 1f;
    [SerializeField] float distanciaparedderecha = 23.5f;
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
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;
    private Rigidbody2D rb;
    [SerializeField]private bool isGrounded;
    private GameObject child;
    private Collider2D childCollider;
    PlatformMovement platform;
    [SerializeField]
    private float jumpTimeCounter;

    [SerializeField]
    private float jumpTime;

    [SerializeField]
    private bool isJumping;//indica si ya se ha dado inicio el salto

    //solo activa en el frame en el que ha saltado.Se desactiva en cuánto se añade la velocidad que da inicio al salto
    [SerializeField]
    private bool _justJumped; 

    Vector2 moveInput;

    SpriteRenderer _spriteRenderer;//debugear
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        child = transform.GetChild(0).gameObject; // Obtiene el primer hijo directamente
        childCollider = child.GetComponent<Collider2D>(); // Obtiene su Collider2D
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (childCollider == null) return;
        if (InputManager.Instance == null) return;

        //Obtener el vector de movimiento desde el InputManager.
        moveInput = InputManager.Instance.MovementVector;
        //moveInput.x != 0 ? moveInput.x * speed : rb.velocity.x --> si se está pulsando input se mueve, si no, arrastra la velocidad
        // rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);
        // rb.velocity = new Vector2(moveInput.x != 0 ? moveInput.x * speed : rb.velocity.x * deacceleration, rb.velocity.y);
        

        //Voltear el sprite
        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }



        if (isGrounded && InputManager.Instance.JumpWasPressedThisFrame())
        {
            _justJumped = true;
        }

        if (InputManager.Instance.JumpWasReleasedThisFrame() && isJumping)
        {
            isJumping = false;
        }

        //debugear

        Debug.Log(isJumping);
        if (isJumping) 
        { 
            _spriteRenderer.color= new Color(_spriteRenderer.color.r,spriteRenderer.color.g,0); 
        }
        else {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, spriteRenderer.color.g, 255);
            
        }
    }
    void FixedUpdate()
    {

        //Detecta si el hijo está colisionando con algo
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(childCollider.bounds.center, childCollider.bounds.size, 0);
        isGrounded = false;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject && ((1 << hitCollider.gameObject.layer) & ground) != 0)
            {
                isGrounded = true;
            }
        }



        #region seteo y cambio de velocidad.x, velocidad.y a las correspondientes de base(no contando salto).
        if (platform == null)
        {
            // si la velocidad mia es mayor a la máxima y no estoy pulsando la tecla de la dir contraria o el 0:
            if (rb.velocity.x > (moveInput.x * speed) && (rb.velocity.x * moveInput.x) > 0)
            {
                //va al fixedUpdate porque si va a más fps, se resta la deceleración más veces/segundo
                rb.velocity = new Vector2(rb.velocity.x - deceleration, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);
            }

        }
        else
        {
            rb.velocity = new Vector2(moveInput.x * speed + platform.getVel().x, platform.getVel().y);
        }
        #endregion

        //SALTO
        if (isGrounded && !isJumping && _justJumped)
        {
            isJumping = true;
            _justJumped = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpForceInitial);
            platform = null;
            jumpTimeCounter = 0;
        }

        if (isJumping && jumpTimeCounter < jumpTime)
        {
            rb.velocity=new Vector2(rb.velocity.x, rb.velocity.y + jumpForce);
            jumpTimeCounter += Time.fixedDeltaTime;//Time.FixedDeltatime para el FixedUpdate 
        }
        else
        {
            isJumping = false;
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra)
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    void OnCollisionEnter2D(Collision2D collision)
    {
        platform = collision.gameObject.GetComponent<PlatformMovement>();
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        platform = null;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    //como hipoteticamente podría hacer que el jugador pueda volver a saltar del suelo con un collider como hijo
    // public Vector 2 MovementVector {get; private set;)
    /*
     private void OnMove(InputAction.CallbackContext context)
    {
        MovementVector = context.ReadValue<Vector2>();
    }

    /*
    void Update()
    {
      dashing = GetComponent<PlayerDash>().dash();
    
    MoveDirection = (Vector3)InputManager.Instance.MovementVector;

    if ((cD.GetCollisions()[0] && MoveDirection.y > 0) || cD.GetCollisions()[1] && MoveDirection.y < 0) MoveDirection.y = 0;

    MoveDirection = MoveDirection.Normalized

    if (MoveDirection != Vector3.zero)
    {
        LastDirection = MoveDirection;
    }
    if (!dashing) 
    rb.velocity = MoveDirection * MoveSpeed * Time.fixedDeltaTime; 



    }
    */


    #endregion

} // class PlayerMovement 
// namespace
