//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.Tilemaps;


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class playerStateDeath : MonoBehaviour
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

    Collider2D stateCol;
    LevelManager lM;
    CambioEstado[] estados;
    Tilemap tilemapAct;

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
        stateCol = GetComponent<Collider2D>();
        lM = LevelManager.Instance;
        estados = lM.GetEstados();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // Devuelves el estado que está activo
        tilemapAct = lM.GetEstados()[lM.EstadoActual() == 0 ? 1 : 0].GetComponent<Tilemap>();
    
        if (IsColliderInsideTilemap(stateCol, tilemapAct))
        {
            Debug.Log("El collider está dentro del Tilemap");
            lM.ResetPlayer();
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

    bool IsColliderInsideTilemap(Collider2D col, Tilemap tilemap)
    {
        // Coordenadas del collider (sacadas de bounds) en cuanto al tilemap
        Vector3Int minTile = tilemap.WorldToCell(col.bounds.min);
        Vector3Int maxTile = tilemap.WorldToCell(col.bounds.max);

        // Recorrer las celdas
        for (int x = minTile.x; x <= maxTile.x; x++)
        {
            for (int y = minTile.y; y <= maxTile.y; y++)
            {
                // Si hay un tile en las coordenadas que especifican la celda del tilemap:
                if (tilemap.GetTile(new Vector3Int(x, y, 0)) != null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<CambioEstado>())
        {
            Debug.Log("Muerte por cambio de estado colision");
            lM.ResetPlayer();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<CambioEstado>())
        {
            Debug.Log("Muerte por cambio de estado trigger");
            lM.ResetPlayer();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<CambioEstado>())
        {
            Debug.Log("Muerte por cambio de estado trigger stay");
            lM.ResetPlayer();
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<CambioEstado>())
        {
            Debug.Log("Muerte por cambio de estado");
            lM.ResetPlayer();
        }
    }

    #endregion   

} // class playerStateDeath 
// namespace

