
//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
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
public class Menu : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    //[SerializeField] private GameObject ResumeButton;
    [SerializeField] private GameObject ObjectPauseMenu;
    [SerializeField] private GameObject resumeButton; // Botón Resume
    [SerializeField] private GameObject mainMenuButton; // Botón Main Menu
    [SerializeField] private GameObject levelSelectorButton; // Botón Level Selector
    [SerializeField] private GameObject restartButton; // Botón Level Selector

    //[SerializeField] private GameObject exitGameButton;
    [SerializeField] private bool Paused;  //Paused = !Paused Alterna el estado de pausa
    [SerializeField] private PlayerMovement playermovement; // para desactivar su script cuando se acciona el PauseMenu
    [SerializeField] private GameObject PauseMenuFirst;

    //[SerializeField] private GameObject ExitButton;
    //[SerializeField] private PlayerMovement playerscript; 

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
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
        UpdateRestartButtonState();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
       
        if (InputManager.Instance.PauseIsPressed())   
        {
            if (!Paused)
            {
                TooglePause();
            }
            else
            {
                ResumeGame();
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
   
    // Método para el botón Resume
    public void ResumeGame()
    {
        Paused = false;
        Time.timeScale = 1; // Reanuda el tiempo del juego
        ObjectPauseMenu.SetActive(Paused); // Oculta el menú
        EventSystem.current.SetSelectedGameObject(null);
        playermovement.enabled = true;
        print("Reanuda");
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        ObjectPauseMenu.SetActive(false);
        playermovement.enabled = true;
        _levelManager.ResetPlayer();
    }

    // Método para cambiar a la primera escena
    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Reanudar el tiempo antes de cambiar de escena
        SceneManager.LoadScene("MainMenu");
    }

    // Método para cambiar a la segunda escena
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
        ObjectPauseMenu.SetActive(Paused); // Muestra u oculta el menú
        EventSystem.current.SetSelectedGameObject(PauseMenuFirst); // El menú de pausa al ser iniciado, también es seleccionado al primer boton de este, para indicar al jugador desde donde empieza a navegar por él
        playermovement.enabled = false; // El personaje realmente se queda quieto
        print("Pausa");

    }

    private void UpdateRestartButtonState() //Método para eliminar el botón de reinicar nivel del hub por estetica
    {

        if (SceneManager.GetActiveScene().name == "Hub") //Si la escena se llama Hub
        {
            restartButton.SetActive(false); //desactiva botón
            Debug.Log("El botón de reinicio está desactivado en la escena 'Hub'");
        }
        else
        {
            restartButton.SetActive(true); // lo activa
            Debug.Log("El botón de reinicio está activado");
        }
    }
    #endregion


} // class Menu 
  // namespace