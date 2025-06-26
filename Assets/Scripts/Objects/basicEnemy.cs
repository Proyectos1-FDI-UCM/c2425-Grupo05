//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UIElements;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class basicEnemy : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] private float enemySpeed = 0.5f;
    [SerializeField] bool initMoveRight = true;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    LevelManager levelManager;
    Animator animator;
    private Rigidbody2D rb;
    private Vector3 orPos;
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
        if (!GameManager.Instance.GetShooterMode()) Destroy(gameObject);
        levelManager = LevelManager.Instance;
        animator = GetComponent<Animator>();
        levelManager.AddEnemy(this.gameObject);

        orPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().flipX = initMoveRight ? false : true;
        enemySpeed = Mathf.Abs(enemySpeed) * (initMoveRight ? 1 : -1);
    }

    void Update()
    {
        animator.SetBool("enemyWalking", !levelManager.IsTimeStopped());
        if (!levelManager.IsTimeStopped())
        {
            rb.velocity = new Vector2(enemySpeed, rb.velocity.y);
        }
        else rb.velocity = Vector2.zero;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            // Debug.Log("Cambio de dirección");
            enemySpeed *= -1;
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        }
        else if (other.CompareTag("Player"))
        {
            if (levelManager.GetPlayer().transform.position.y - 0.75f > transform.position.y)
            {
                // Debug.Log("Enemy kill");
                // Destroy(gameObject);
                gameObject.SetActive(false);

                Rigidbody2D playerRb = levelManager.GetPlayer().GetComponent<Rigidbody2D>();
                playerRb.velocity = new Vector2(playerRb.velocity.x, 0f);
                playerRb.AddForce(Vector2.up * 80f);
            }
            else
            {
                // Debug.Log("Player lateral");
                levelManager.ResetPlayer();
            }
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

    public void ResetEnemy()
    {
        gameObject.SetActive(true);
        transform.position = orPos;
        GetComponent<SpriteRenderer>().flipX = initMoveRight ? false : true;

        rb.velocity = Vector2.zero;
        enemySpeed = Mathf.Abs(enemySpeed) * (initMoveRight ? 1 : -1);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class basicEnemy 
// namespace
// namespace
