//---------------------------------------------------------
// ChangeScene es un componente de pruebas para cambiar entre escenas
// Antonio Bucero Coronel
// Oscar Silva Urbina
// I´m losing it
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------



using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


/// <summary>
/// Componente de pruebas que cambia a otra escena que se
/// configura desde el editor. Se usa principalmente para
/// comunicarse con el GameManager desde un botón y hacer el
/// cambio de escena
/// </summary>
public class ChangeScene : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Índice de la escena (en el build settings)
    /// que se cargará. 
    /// </summary>
    [SerializeField]
    private int sceneNumber;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            QuitGame();
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Cambia de escena haciendo uso del GameManager
    /// </summary>
    public void ChangeToScene()
    {
        GameManager.Instance.ChangeScene(sceneNumber);
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Detiene el juego en el editor
    #else
            Application.Quit(); // Cierra el juego en una build
    #endif
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    private void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Detiene el juego en el editor
        #else
            Application.Quit(); // Cierra el juego en una build
        #endif
    }

    #endregion   

} // class ChangeScene 
// namespace
