//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    [SerializeField] private GameObject exitGameButton;
    [SerializeField] private bool Paused;
    [SerializeField] PlayerMovement playermovement; 
    //[SerializeField] private GameObject ExitButton;
    //[SerializeField] private PlayerMovement playerscript; 

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private InputAction _stopAction;
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

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
        /*PauseMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(ResumeButton);
        playerscript = FindAnyObjectByType<PlayerMovement>();
        */
        //Se oculta el menú de pausa al inicar el juego 
        ObjectPauseMenu.SetActive(false);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TooglePause();
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

    public void OnPause(InputAction.CallbackContext context) //Lo de de callbackcontext se lo pregunte a Gepeto, luego lo cambiare o lo intentare entender 
    {
        if (context.performed)
        {
            TooglePause(); // Alterna el estado de pausa
        }
    }
    private void TooglePause()
    {
        
            Paused = !Paused; // Alterna el estado de pausa
            Time.timeScale = Paused ? 0 : 1; // Pausa o reanuda el juego
            ObjectPauseMenu.SetActive(Paused); // Muestra u oculta el menú
        
    }
    // Método para el botón Resume
    public void ResumeGame()
    {
        Paused = false;
        Time.timeScale = 1; // Reanuda el tiempo del juego
        ObjectPauseMenu.SetActive(false); // Oculta el menú
    }

    // Método para cambiar a la primera escena
    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Reanudar el tiempo antes de cambiar de escena
        SceneManager.LoadScene("Menu"); 
    }

    // Método para cambiar a la segunda escena
    public void GoToLevelSelector()
    {
        Time.timeScale = 1; // Reanudar el tiempo antes de cambiar de escena
        SceneManager.LoadScene("Hub"); 
    }

    public void ExitGame()
    {
        Time.timeScale = 1; // Reanuda el tiempo antes de salir
        Application.Quit(); // Cierra la aplicación
        Debug.Log("Juego cerrado."); // Esto sólo se verá en el editor de Unity
    }
    /*
    public void OnStop(InputAction.CallbackContext context)
    {
        if (context.performed) // Comprueba si la acción se ha realizado
        {
            ObjectPauseMenu.SetActive(!ObjectPauseMenu.activeSelf); // Alterna entre mostrar y ocultar el menú
            Pause = true;
        }
    }
    */
    /*public void OpenExit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        playerscript.enabled = true;
    }

    public void OpenReset()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
        playerscript.enabled = true;
      
    }

    public void OpenResume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        playerscript.enabled = true;
    }
    */
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    /* private void OnEnable()
     {
         // Obtener la referencia de la acción "Stop" desde el InputActionMap
         var playerInput = new PlayerInput();
         //stopAction = playerInput.UI.Stop;

         // Suscribirse al evento cuando se ejecuta la acción
         //stopAction.performed += OnStopAction;
         stopAction.Enable();
     }

     private void OnDisable()
     {
         //stopAction.performed -= OnStopAction;
         stopAction.Disable();
     }
    */
    #endregion


} // class Menu 
// namespace
