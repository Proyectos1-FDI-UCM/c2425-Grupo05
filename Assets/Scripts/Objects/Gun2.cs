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
public class Gun2 : MonoBehaviour
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

    [SerializeField] private float rotDelay = 1; // Retraso de rotación
    [SerializeField] private float preShootTime = 0.5f;
    [SerializeField] private float shootingDuration = 0.5f; // Duración del disparo
    [SerializeField] private float cooldownTime = 1f;
    private float shootTimer = 0f; // Timer de disparo
    private float maxCooldown = 0f; // Timer de cooldown

    LevelManager levelManager;
    [SerializeField] GameObject player;
    private Transform bulletSpawnPoint;

    // LineRenderer para visualizar el rayo
    private LineRenderer _lineRenderer;
    private RaycastHit2D hit;
    private RaycastHit2D visualHit;

    // Añade un temporizador para el tiempo de disparo fijo (0.5s)
    
    private bool shooting = false;

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
        levelManager = LevelManager.Instance;
        bulletSpawnPoint = transform.GetChild(0).transform;
        if (!player) player = levelManager.GetPlayer();

        // Configuración del LineRenderer
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.startColor = Color.blue; // Color inicial
        _lineRenderer.endColor = Color.blue; // Color final
        _lineRenderer.startWidth = 0.05f; // Ancho inicial
        _lineRenderer.endWidth = 0.05f; // Ancho final
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// En este update se encarga de detectar si estamos en el Hub para comprobar
    /// </summary>
    void Update()
    {
        hit = Physics2D.Raycast(bulletSpawnPoint.position, transform.up); // Incluye a Player para la colisión
        _lineRenderer.SetPosition(0, bulletSpawnPoint.position);
        visualHit = Physics2D.Raycast(bulletSpawnPoint.position, transform.up, 100f, ~LayerMask.GetMask("Player")); // Excluye a Player para visualización
        _lineRenderer.SetPosition(1, visualHit.point);

        if (!shooting) // Si no shooting calculamos la rotación
        {
            if (shootTimer > maxCooldown) // Fuera de cooldown
            {
                // Calcula la dirección y el ángulo hacia el jugador
                Vector2 direction = player.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x);
                Quaternion targetRotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg - 90, Vector3.forward);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1 / rotDelay);

                _lineRenderer.enabled = true;
                if (hit.collider && hit.collider.gameObject == player) { shootTimer = 0f; shooting = true; }
            }
            else // En cooldown
            {
                shootTimer += Time.deltaTime;
                _lineRenderer.enabled = false;
            }
        }
        else
        {
            shootTimer += Time.deltaTime;
            _lineRenderer.startColor = Color.red;

            if (shootTimer >= preShootTime && shootTimer < preShootTime + shootingDuration)
            {
                Shoot();
                _lineRenderer.endColor = Color.red;
            }
            else if (shootTimer >= preShootTime + shootingDuration)
            {
                maxCooldown = shootTimer + cooldownTime;
                _lineRenderer.startColor = Color.blue;
                _lineRenderer.endColor = Color.blue;
                shooting = false;
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


    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    private void Shoot()
    {
        Debug.Log("Shooting raygun!");
        
        if (!hit.collider) return; // Si no hay colisión, no hacemos nada

        if (hit.collider.gameObject == player)
        {
            // Resetear al jugador
            Debug.Log("Player hit by raygun!");
            levelManager.ResetPlayer();
        }
        else if (hit.collider.GetComponent<CambioEstado>())
        {
            Tilemap tilemap = hit.collider.GetComponent<Tilemap>();

            Vector3Int cell = tilemap.WorldToCell(hit.point);
            tilemap.SetTile(cell, null);
            Debug.Log("Tile destruido por el rayo de la gun");
        }
    }
    #endregion

} // class Gun
// namespace