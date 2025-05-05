//---------------------------------------------------------
// Script que usan las puertas para ir del hub a un nivel
// Adrián Erustes Martín
// I'm loosing it
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class LevelEntrance : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Nivel al que lleva esta puerta
    /// </summary>
    [SerializeField] int doorLevel; 

    //spriteRenderer de la puerta
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] GameObject pressE;
    #endregion


    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Indica si estoy tocando al jugador
    /// </summary>
    bool touchingPlayer = false;
    Animator animator;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();
        //sprite.color = new Color(0.77254f, 0.52941f, 0.18039f, 1f);
        animator.SetBool("abierto", doorLevel <= GameManager.Instance.MaxLevel() + 1);
        pressE.SetActive(false);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (touchingPlayer) // aquí también hay que poner && tecla "" detectada, pero no sé cómo hacerlo con el nuevo input system.
        {
            //solo puedes entrar en la puerta 2 si ya te has pasado el nivel 1 =>
            if (doorLevel <= GameManager.Instance.MaxLevel() + 1 && InputManager.Instance.EnterIsPressed())
            {
                GameManager.Instance.GoToLvl(doorLevel);
            }
        }
        animator.SetBool("abierto", doorLevel <= GameManager.Instance.MaxLevel() + 1);
        ShowE(doorLevel <= GameManager.Instance.MaxLevel() + 1,touchingPlayer);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        touchingPlayer = true;
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        touchingPlayer = false;
        
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

    private void ShowE(bool open, bool touchingPlayer)
    {
        if (open && !touchingPlayer)
        {

            //Debug.Log("Puerta abierta y no tocando al jugador");
            
           
            pressE.SetActive(false);
        }
        else if (open && touchingPlayer)
        {
            //Debug.Log("Puerta abierta y tocando al jugador");
            
            pressE.SetActive(true);
        }
        else if (!open && !touchingPlayer)
        {

            //Debug.Log("Puerta cerrada y no tocando al jugador");
            
            
            pressE.SetActive(false);
        }
        else if (!open && touchingPlayer)
        {
            //Debug.Log("Puerta cerrada y tocando al jugador");
            
            pressE.SetActive(false);
        }

    }

    #endregion

} // class LevelEntrance 
// namespace
