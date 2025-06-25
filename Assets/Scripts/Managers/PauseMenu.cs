
//---------------------------------------------------------
// Define el commportamiento de el activar menú de pausa tanto como el de los botones dentro de el
// Óscar Silva Urbina
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEditor;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    
    [SerializeField] private GameObject ObjectPauseMenu; //Elemento del objeto del menu de pausa
    [SerializeField] private GameObject resumeButton; // Botón Resume
    [SerializeField] private GameObject mainMenuButton; // Botón Main Menu
    [SerializeField] private GameObject levelSelectorButton; // Botón Level Selector
    [SerializeField] private GameObject restartButton; // Botón Level Selector

    
    [SerializeField] private bool Paused;  //Paused = !Paused Alterna el estado de pausa
    [SerializeField] private PlayerMovement playermovement; // para desactivar su script cuando se acciona el PauseMenu
    [SerializeField] private GameObject PauseMenuFirst; // Objeto de el primer botón en ser preseleccionado al inicar el PauseMenu



    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    // Variable privada que contiene una instancia de LevelManager
    LevelManager _levelManager;
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
        //Se oculta el menú de pausa al inicar el juego 
        ObjectPauseMenu.SetActive(false);
        _levelManager = LevelManager.Instance;
        HubButtonsState();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
       //Si detecta la tecla/botón que activa el menu de pausa
        if (InputManager.Instance.PauseIsPressed())   
        {
            if (!Paused) //Si no estaba pausado se pausa
            {
                TooglePause();
            }
            else //Si lo estaba vuelve el juego a su estado normal
            {
                ResumeGame();
            }
        }
        else if (InputManager.Instance.ReturnIsPressed()) ResumeGame();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
   
    // Método para el botón Resume
    public void ResumeGame()
    {
        Paused = false;
        Time.timeScale = 1; // Reanuda el tiempo del juego
        ObjectPauseMenu.SetActive(Paused); // Oculta el menú
        EventSystem.current.SetSelectedGameObject(null);
        playermovement.enabled = true;
        
    }

    // Método para el botón Restart, resetear al player
    public void RestartScene()
    {
        Time.timeScale = 1;
        ObjectPauseMenu.SetActive(false);
        playermovement.enabled = true;
        _levelManager.ResetPlayer();
    }

    // Método del botón de Mainmenu, para cambiar a la primera escena
    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Reanudar el tiempo antes de cambiar de escena
        SceneManager.LoadScene("MainMenu");
    }

    // Método del botón levelselector, para cambiar a la segunda escena
    public void GoToLevelSelector()
    {
        Time.timeScale = 1; // Reanudar el tiempo antes de cambiar de escena
        SceneManager.LoadScene("Hub");
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    //Método que hace la pausa e inovca al menú
    private void TooglePause()
    {
        Paused = true;
        Time.timeScale = 0f; // Para el tiempo del juego
        Debug.Log("Pausa activada, TimeScale: " + Time.timeScale);
        ObjectPauseMenu.SetActive(Paused); // Muestra u oculta el menú
        EventSystem.current.SetSelectedGameObject(PauseMenuFirst); // El menú de pausa al ser iniciado, también es seleccionado al primer boton de este, para indicar al jugador desde donde empieza a navegar por él
        playermovement.enabled = false; // El personaje realmente se queda quieto
    }

    private void HubButtonsState() //Método para eliminar el botón de reinicar nivel y levelselector del hub por estetica
    {
        // Ciertos botones desactivados en Hub y Tutorial
        if (SceneManager.GetActiveScene().buildIndex == (int)GameManager.MyGameScenes.Hub)
        {
            restartButton.SetActive(false);
            levelSelectorButton.SetActive(false);
        }
        else if (SceneManager.GetActiveScene().buildIndex == (int)GameManager.MyGameScenes.Tutorial)
        {
            levelSelectorButton.SetActive(false);
        }
        else
        {
            restartButton.SetActive(true);
            levelSelectorButton.SetActive(true);
        }
    }
    #endregion


} // class Menu 
  // namespace