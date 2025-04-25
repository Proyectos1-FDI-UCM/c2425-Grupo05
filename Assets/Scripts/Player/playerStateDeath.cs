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

    Collider2D _statePlayerCollider;
    Tilemap _tilemapActual;
    LevelManager _levelManager;
    private int _roomNo = 0;

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
        _statePlayerCollider = GetComponent<Collider2D>();
        _levelManager = LevelManager.Instance;
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// En este update se encarga de detectar si estamos en el Hub para comprobar
    /// </summary>
    void Update()
    {
        // Habria que pasar todo esto a realizar la comprobacion solo cuando se cambia de estado (eventos?)
        // Devuelves el estado que esta activo

        if (!_levelManager.GetIsHub())
        {
            _roomNo = _levelManager.GetRoomNo() * 2;
            _tilemapActual = _levelManager.GetEstados()[(_levelManager.EstadoActual() == 0 ? _roomNo : _roomNo + 1)].GetComponent<Tilemap>();



            if (IsColliderInsideTilemap(_statePlayerCollider, _tilemapActual))
            {
                Debug.Log("El collider está dentro del Tilemap");
                _levelManager.ResetPlayer();
            }

            if (InputManager.Instance.RestartIsPressed())
            {
                _levelManager.ResetPlayer();
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

    #endregion   

} // class playerStateDeath 
// namespace

