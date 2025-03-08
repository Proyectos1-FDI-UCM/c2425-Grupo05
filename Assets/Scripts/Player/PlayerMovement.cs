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
    private bool isGrounded;


    private GameObject _child;
    private Collider2D jumpCollider;
    private Collider2D platformEnterCollider;
    private Collider2D platformExitCollider;
    ///se necesitan platformEnter y platformExit porque si se usa solo 1 collider para detectar si estás en la plataforma o no,
    ///este tiene que ser tan ancho como el jugador(o casi).
    ///Si este collider se usara también para detectar si entras en la plataforma, si te pegas al lateral de la plataforma que se mueve hacia tí, 
    ///el collider detectará que la estás pisando aunque solamente estés pegado a una pared, lo que lleva a que te quedes pegado a la pared de la plataforma.
    /// </summary>
    

    PlatformMovement platform;
    [SerializeField]
    private float jumpTimeCounter;
    [SerializeField]
    private float jumpTime;
    [SerializeField]
    private bool isJumping;
    Vector2 moveInput;
    private bool justJumped = false;
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
        //collider salto
        _child = transform.GetChild(0).gameObject; // Obtiene el primer hijo directamente
        jumpCollider = _child.GetComponent<Collider2D>(); // Obtiene su Collider2D
        //collider entrado en plataforma
        _child = transform.GetChild(1).gameObject;
        platformEnterCollider = _child.GetComponent<Collider2D>();
        //collider salido de plataforma
        _child = transform.GetChild(2).gameObject;
        platformExitCollider = _child.GetComponent<Collider2D>();

        justJumped = false;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (jumpCollider == null || platformEnterCollider ==null || platformExitCollider ==null) return;
        if (InputManager.Instance == null) return;

        //Obtener el vector de movimiento desde el InputManager.
        moveInput = InputManager.Instance.MovementVector;

        //Voltear el sprite
        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }

        


        
        
        if (isGrounded)
        {
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = Color.yellow;
        }

        //SALTO
        if (isGrounded && InputManager.Instance.JumpWasPressedThisFrame())
        {
            justJumped = true;
            
        }
        else if (InputManager.Instance.JumpWasReleasedThisFrame()&&isJumping)
        {
            isJumping = false;
        }

        


    }
    void FixedUpdate()
    {
        //Detecta si el hijo está colisionando con algo
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(jumpCollider.bounds.center, jumpCollider.bounds.size, 0);

        isGrounded = false;
        platform = null;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject && ((1 << hitCollider.gameObject.layer) & ground) != 0) //por qué se usa & y no &&?
            {
                isGrounded = true;

                platform = hitCollider.gameObject.GetComponent<PlatformMovement>();

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
        else if (platform != null )
        {
            rb.velocity = new Vector2(moveInput.x * speed + platform.getVel().x, platform.getVel().y); //el problema está aquí.
            //el problema (audio)

        }

        #endregion
        if (justJumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y+jumpForceInitial);
            
            isJumping = true;
            jumpTimeCounter = 0;
            justJumped = false;
            
        }

        if (isJumping && jumpTimeCounter < jumpTime)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpForce);
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
    */


    #endregion

} // class PlayerMovement 
// namespace
