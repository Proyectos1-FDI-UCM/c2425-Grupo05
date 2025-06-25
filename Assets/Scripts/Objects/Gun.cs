//---------------------------------------------------------
// Objeto de Gun
// Amiel Ramos Juez
// I'm Losing It
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.Tilemaps;


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Gun : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    [SerializeField] private float fireRate = 4f;
    [SerializeField] private float rotDelay = 1; // Retraso de rotación
    private float shootTimer = 0f; // Timer de disparo

    LevelManager levelManager;
    [SerializeField] GameObject player;
    private Transform bulletSpawnPoint;
    [SerializeField] private Bullet bulletPrefab;


    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    ///Se inicializan las variables a sus valores correspondientes
    /// </summary>
    void Start()
    {
        // Si no estamos en shooter mode, nos destruimos
        if (!GameManager.Instance.GetShooterMode()) Destroy(gameObject);
        levelManager = LevelManager.Instance;
        bulletSpawnPoint = transform.GetChild(0).transform;
        if (!player) player = levelManager.GetPlayer();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// En este update se encarga de detectar si estamos en el Hub para comprobar
    /// </summary>
    void Update()
    {
        Vector2 direction = player.transform.position - transform.position; // Vector desdse la referencia hasta el objeto
        float angle = Mathf.Atan2(direction.y, direction.x); // Atan2 calcula el ángulo entre el eje x y el vector dirección

        Quaternion targetRotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg - 90, Vector3.forward); // Convierte el ángulo en radianes a grados y lo aplica a la rotación del objeto

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1/rotDelay); // Lerp avanza un valor hasta otro con una velocidad

        if (!levelManager.IsTimeStopped()) shootTimer += Time.deltaTime;

        if (shootTimer >= fireRate)
        {
            Shoot();
            shootTimer = 0f;
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

    public void Shoot()
    {
        // Instancia la bala (la velocidad se gestiona en la propia bala)
        Bullet bala = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        LevelManager.Instance.AddBala(bala);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class Gun
// namespace