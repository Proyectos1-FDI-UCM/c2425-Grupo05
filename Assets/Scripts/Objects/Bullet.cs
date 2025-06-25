//---------------------------------------------------------
// Objeto de bala
// Amiel Ramos Juez
// I'm Losing It
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.Tilemaps;


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Bullet : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] float bulletSpeed = 10f;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    Collider2D bulletCollider;
    Rigidbody2D rb;

    Tilemap _tilemapActual;
    Tilemap[] walls;
    LevelManager levelManager;

    struct ResultadoColision
    {
        public bool col;
        public Vector2Int pos;
    }

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
        bulletCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        levelManager = LevelManager.Instance;
        rb.velocity = transform.up * bulletSpeed;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// En este update se encarga de detectar si estamos en el Hub para comprobar
    /// </summary>
    void Update()
    {
        int _roomNo = levelManager.GetRoomNo() * 2;
        _tilemapActual = levelManager.GetEstados()[(levelManager.EstadoActual() == 0 ? _roomNo : _roomNo + 1)].GetComponent<Tilemap>();

        var colResult = IsColliderInsideTilemap(bulletCollider, _tilemapActual);
        if (colResult.col)
        {
            _tilemapActual.SetTile(new Vector3Int(colResult.pos.x, colResult.pos.y, 0), null);
            Destroy(gameObject);
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

    public void SetWalls(Tilemap[] t)
    {
        walls = t;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    ResultadoColision IsColliderInsideTilemap(Collider2D col, Tilemap tilemap)
    {
        ResultadoColision res = new ResultadoColision();

        // Coordenadas del collider (sacadas de bounds) en cuanto al tilemap
        Vector3Int minTile = tilemap.WorldToCell(col.bounds.min); // Área en posición de tilemap abajo izquierda
        Vector3Int maxTile = tilemap.WorldToCell(col.bounds.max); // Área en posición de tilemap arriba derecha

        // Recorrer las celdas
        for (int x = minTile.x; x <= maxTile.x; x++)
        {
            for (int y = minTile.y; y <= maxTile.y; y++)
            {
                // Si hay un tile en las coordenadas que especifican la celda del tilemap:
                if (tilemap.GetTile(new Vector3Int(x, y, 0)) != null)
                {
                    res.col = true;
                    res.pos.x = x;
                    res.pos.y = y;
                    return res;
                }
            }
        }
        res.col = false;
        return res;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Tilemap>())
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<GrayZone>())
        {
            Destroy(gameObject);
        }
    }

    #endregion

} // class Bala
// namespace