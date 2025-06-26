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
public class RayGun : MonoBehaviour
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

    LevelManager levelManager;
    [SerializeField] GameObject player;
    private Transform bulletSpawnPoint;


    private LineRenderer lineRenderer; // Linerenderer del raycast
    private RaycastHit2D hit; // Raycast para colisiones
    private RaycastHit2D visualHit; // Raycast para visualización
    

    [SerializeField] private float rotDelay = 1; // Retraso de rotación
    [SerializeField] private float preShootTime = 0.5f;
    [SerializeField] private float shootingDuration = 0.5f; // Duración del disparo
    [SerializeField] private float cooldownTime = 1f;
    private bool shooting = false; // Bool si está en estado de disparar
    private float shootTimer = 0f; // Timer de disparo
    private float maxCooldown = 0f; // Timer de cooldown

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

        // lineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Si no le aplicamos un material, no tiene textura (rosa)

        lineRenderer.startColor = Color.blue; // Color inicial
        lineRenderer.endColor = Color.blue; // Color final
        lineRenderer.startWidth = 0.05f; // Ancho inicial
        lineRenderer.endWidth = 0.1f; // Ancho final
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// En este update se encarga de detectar si estamos en el Hub para comprobar
    /// </summary>
    void Update()
    {
        hit = Physics2D.Raycast(bulletSpawnPoint.position, transform.up); // Incluye a Player para la colisión
        lineRenderer.SetPosition(0, bulletSpawnPoint.position);
        visualHit = Physics2D.Raycast(bulletSpawnPoint.position, transform.up, 100f, ~LayerMask.GetMask("Player")); // Excluye a Player para visualización
        lineRenderer.SetPosition(1, visualHit.point);

        if (levelManager.IsTimeStopped()) maxCooldown = 0; // Si el tiempo está parado, reestablecemos el cooldown

        if (!shooting) // Si no shooting calculamos la rotación
        {
            if (shootTimer >= maxCooldown) // Fuera de cooldown
            {
                lineRenderer.enabled = true;

                // Calcula la dirección y el ángulo hacia el jugador
                Vector2 direction = player.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x);
                Quaternion targetRotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg - 90, Vector3.forward);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1 / rotDelay);

                // Comprueba colisión con el jugador para pasar a shooting
                if (hit.collider && hit.collider.gameObject == player) { shootTimer = 0f; shooting = true; }
            }
            else // En cooldown
            {
                lineRenderer.enabled = false;

                shootTimer += Time.deltaTime;
            }
        }
        else
        {
            shootTimer += Time.deltaTime;
            lineRenderer.startColor = Color.red;

            if (shootTimer >= preShootTime && shootTimer < preShootTime + shootingDuration)
            {
                Shoot();
                lineRenderer.endColor = Color.red;
            }
            else if (shootTimer >= preShootTime + shootingDuration)
            {
                maxCooldown = shootTimer + cooldownTime;
                lineRenderer.startColor = Color.blue;
                lineRenderer.endColor = Color.blue;
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