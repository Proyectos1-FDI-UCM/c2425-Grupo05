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
    private bool State1 = false;//indica si se encuentra en el estado que empieza ya presente
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


   
    private Tilemap _spriteTile;
    private TilemapRenderer _renderer;
    private TilemapCollider2D _colliderTile;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private Rigidbody2D _body;
    private float time = 0;

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
        _body = GetComponent<Rigidbody2D>();
        _colliderTile = GetComponent<TilemapCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
       
        SetComponentsActive(State1);

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


    public void CambiaEstado()
    {
        SetComponentsActive(!State1);
        ChangeAlpha(1f);
        if (State1) State1 = false;
        else State1 = true;
    }
    public void CambiaEstadoTrasLuz(float alphaTime)
    {
        if (!State1)
        {
            if (Tilemap)
            {
                if (_renderer != null)
                    _renderer.enabled = true;
                GradualChangeAlpha(alphaTime);    
            }
            else
            {
                if (_spriteRenderer != null)
                    _spriteRenderer.enabled = true;
                GradualChangeAlpha(alphaTime);
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


    private void SetComponentsActive(bool isActive) //Activa el objeto cuando pasa a un estado en el que se debe activar
    {
        if (Tilemap)
        {
            if (_renderer != null)
                _renderer.enabled = isActive;
            if(_colliderTile != null)
                _colliderTile.enabled = isActive;
        }
        else
        {
            if(_spriteRenderer != null)
                _spriteRenderer.enabled = isActive;
            if (_collider != null)
                _collider.enabled = isActive;
        }
    }

    private void ChangeAlpha(float alpha)//Cambia el alfa del sprite
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
    private void GradualChangeAlpha(float alphaTime)
    {
        if (Tilemap)
        {
            if (_spriteTile != null)
            {
                Color color = _spriteTile.color;


                while (time < alphaTime)
                {
                    time += Time.deltaTime;
                    color.a = time;

                }
                _spriteTile.color = color;
                time = 0;
            }
        }
    }

} // class CambioEstado 
// namespace
