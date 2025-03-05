//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Globalization;
using System.Runtime.Serialization.Formatters;
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
    private GameObject child;
    private Collider2D childCollider;    
    [SerializeField]
    private float jumpTimeCounter;
    [SerializeField]
    private float jumpTime;
    [SerializeField]
    private bool isJumping;
 
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
        rb = GetComponent<Rigidbody2D>();
        child = transform.GetChild(0).gameObject; // Obtiene el primer hijo directamente
        childCollider = child.GetComponent<Collider2D>(); // Obtiene su Collider2D
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (childCollider == null) return;
        if (InputManager.Instance == null) return;

        //Obtener el vector de movimiento desde el InputManager.
        Vector2 moveInput = InputManager.Instance.MovementVector;
        rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);

        //Voltear el sprite
        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }
        //Detecta si el hijo está colisionando con algo
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(childCollider.bounds.center, childCollider.bounds.size , 0);

        isGrounded = false;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject && ((1 << hitCollider.gameObject.layer) & ground) != 0)
            {
            isGrounded = true;
            }
        }

        //SALTO
        if (isGrounded && InputManager.Instance.JumpWasPressedThisFrame())
        {
            isJumping = true;
            rb.velocity += new Vector2(rb.velocity.x, 0); 
            rb.AddForce(Vector2.up * jumpForceInitial, ForceMode2D.Impulse);
            jumpTimeCounter = 0;
        }
        if (InputManager.Instance.JumpWasReleasedThisFrame())
        {
            isJumping = false;
        }

    }
    void FixedUpdate()
    {
        if (isJumping && jumpTimeCounter < jumpTime)
        {
            rb.velocity += new Vector2(rb.velocity.x, 0); 
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlatformMovement platform = collision.gameObject.GetComponent<PlatformMovement>();
        if (platform != null)
        {
            transform.SetParent(platform.gameObject.transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        PlatformMovement platform = collision.gameObject.GetComponent<PlatformMovement>();
        if (platform != null)
        {
            transform.SetParent(null);
            // rb.velocity += new Vector2(platform.getVel().x, platform.getVel().y);
            // Debug.Log(platform.getVel());
        }
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
    */


    #endregion

} // class PlayerMovement 
// namespace
