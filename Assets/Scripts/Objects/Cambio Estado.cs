//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
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
    private float ChangeTime = 3.5f;// el tiempo que tarda en cambiar de estado
    [SerializeField]
    private float ChangeTimeTrasluz = 3f;//el tiempo que tarda en poner una imagen translúcida del siguiente estado
    [SerializeField]
    private bool State1 = false;//indica si se encuentra en el estado que empieza ya presente
    [SerializeField]
    private bool Trasluz = false;//indica si va a ser la imagen translúcida que aparece antes de cambiar de estado
    [SerializeField]
    private bool OnRoom = false;//indica si está en la sala de estos prefabs y, por lo tanto, si deberían funcionar
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


    private float time = 0f;
    private SpriteRenderer _sprite;
    private Collider2D _collider;
    private Rigidbody2D _body;

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
        _sprite = GetComponent<SpriteRenderer>();
        _body = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        if (!State1)
        {
            SetComponentsActive(State1);
        }
        else if (Trasluz)
        {
            SetComponentsActive(false);
        }



    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        time += Time.deltaTime;
        if (OnRoom)
        {
            if (time > ChangeTime)
            {
               
                    SetComponentsActive(State1);
                
               
                if (State1) State1 = false;
                else State1 = true;
                time = 0f;
            }
            else if (time > ChangeTimeTrasluz && time < ChangeTime)
            {
                if (Trasluz)
                {
                    SetComponentsActive(State1);

                }
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

    #endregion


    private void SetComponentsActive(bool isActive) //Activa el objeto cuando pasa a un estado en el que se debe activar
    {
        if (_sprite != null)
            _sprite.enabled = isActive;
        if (!Trasluz)
        {
            if (_collider != null)
                _collider.enabled = isActive;

            if (_body != null)
                _body.isKinematic = !isActive;
        }

    }

} // class CambioEstado 
// namespace
