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
    [SerializeField] LevelSelectMenu levelMenu;
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
        animator.SetBool("abierto", PlayerPrefs.GetInt("RoomsPassed", 1) >= 5);
        pressE.SetActive(false);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (touchingPlayer) 
        {
            if (InputManager.Instance.SelectIsPressed())
            {
                // Antes abría la sala. Movido al menú de selección
                // GameManager.Instance.SceneWillChange_Set(true);
                // GameManager.Instance.GoToLvl(doorLevel);

                // Se comprueba si el jugador está quieto
                if (LevelManager.Instance.GetPlayer().GetComponent<Rigidbody2D>().velocity != Vector2.zero) return;
                if (!levelMenu.isMenuOpen()) levelMenu.AbrirMenu();
            }

            if (levelMenu.isMenuOpen() && InputManager.Instance.ReturnIsPressed())
            {
                levelMenu.CerrarMenu();
            }
        }

        // Puerta 1 siempre abierta. Puerta 2 abierta si se han pasado 5 salas
        animator.SetBool("abierto", doorLevel == 1 ? true : PlayerPrefs.GetInt("RoomsPassed", 0) >= 5);

        pressE.SetActive(touchingPlayer); // La E está activa si el jugador está cerca
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

    #endregion

} // class LevelEntrance 
// namespace
