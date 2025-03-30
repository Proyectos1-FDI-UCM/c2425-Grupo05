//---------------------------------------------------------
// cambiador del estado
// Adrián de Miguel Cerezo
// I´m losing it
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.Tilemaps;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class CambioEstado : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    #endregion




    [SerializeField]
    private int State = 0;//el estado al que le corresponde en cuanto al valor del LevelManager
    [SerializeField]
    private bool Tilemap = false;//indica si se usarán tilemaps para el cambio de estado
    [SerializeField]
    private PlayerMovement Player;//referencia al jugador para controlar la muerte por estar dentro de un bloque

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    #endregion



    private Tilemap _spriteTile; //si es un tilemap, coge el sprite del tilemap
    private TilemapRenderer _renderer; //si es un tilemap, coge el TilemapRenderer
    private TilemapCollider2D _colliderTile;//si es un tilemap, coge su TilemapCollider2D
    private SpriteRenderer _spriteRenderer;//si no es un tilemap, coge su SpriteRenderer
    private Collider2D _collider;//si no es un tilemap, coge su Collider2D
    private LevelManager _levelManager;

    private float time = 0;//el tiempo usado en el método GradualChangeAlpha

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
        Player = FindObjectOfType<PlayerMovement>();
        _spriteTile = GetComponent<Tilemap>();
        _renderer = GetComponent<TilemapRenderer>();
        _colliderTile = GetComponent<TilemapCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _levelManager = FindObjectOfType<LevelManager>();
        if (_levelManager.EstadoActual() == State)
        {
            SetComponentsActive(true);
        }
        else
        {
            SetComponentsActive(false);
        }
        

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    ///<summary>
    ///Cambia el estado del bloque al otro. Si no está en el estado 1, entonces cambia a él, y viceversa.
    ///Los bloques que están en su estado 1 son los que aparecen visibles y con colisión.
    ///</summary>
    public void CambiaEstado(int StateL)
    {
        if (StateL == State)
        {
            SetComponentsActive(true);
        }
        else
        {
            SetComponentsActive(false);
        }
        ChangeAlpha(1f);
        time = 0f;
    }

    /// <summary>
    /// Cambia el estado de una forma diferente al método anterior, ya que este enseña una imagen traslúcida del siguiente estado
    /// antes de que cambie a ese estado, como forma de advertencia. Hay dos modos para si es un tilemap o no, ya que usan 
    /// componentes diferentes.
    /// </summary>
    
    public void CambiaEstadoTrasLuz(int StateL)
    {
        if (StateL!=State)
        {
            if (Tilemap)
            {
                if (_renderer != null)
                    _renderer.enabled = true;
                GradualChangeAlpha();
            }
            else
            {
                if (_spriteRenderer != null)
                    _spriteRenderer.enabled = true;
                GradualChangeAlpha();
            }

        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

    /// <summary>
    /// Este método sirve para activar y desactivar los componentes del sprite y el collider de los bloques de cada estado.
    /// Si están desactivados significa que no están en su estado, y si están activados sí lo están. Hay dos modos en cuanto
    /// a si es un tilemap o no porque usan componentes diferentes.
    /// </summary>
    /// <param name="isActive"></param>
    private void SetComponentsActive(bool isActive)
    {
        if (Tilemap)
        {
            if (_renderer != null)
                _renderer.enabled = isActive;
            if (_colliderTile != null)
                _colliderTile.enabled = isActive;
        }
        else
        {
            if (_spriteRenderer != null)
                _spriteRenderer.enabled = isActive;
            if (_collider != null)
                _collider.enabled = isActive;
        }
    }

    /// <summary>
    /// Este método cambia el alfa del sprite del bloque para que se vea con una opacidad determinada. Hay dos modos para si
    /// es un tilemap o no porque usan componentes distintos.
    /// </summary>
    /// <param name="alpha"></param>
    private void ChangeAlpha(float alpha)
    {
        if (Tilemap)
        {
            if (_spriteTile != null)
            {
                Color color = _spriteTile.color;
                color.a = alpha;
                _spriteTile.color = color;
            }
        }
        else
        {
            if (_spriteRenderer != null)
            {
                Color color = _spriteRenderer.color;
                color.a = alpha;
                _spriteRenderer.color = color;
            }
        }

    }

    /// <summary>
    /// Este método cambia el alfa del sprite del bloque correspondiente de forma gradual, para que la imagen traslúcida de los bloques
    /// del siguiente estado aparezcan poco a poco.
    /// </summary>
    
    private void GradualChangeAlpha()
    {
        if (Tilemap)
        {
            if (_spriteTile != null)
            {
                Color color = _spriteTile.color;
                time += Time.deltaTime;
                color.a = time;
                _spriteTile.color = color;
            }
        }
    }

} // class CambioEstado 
// namespace
