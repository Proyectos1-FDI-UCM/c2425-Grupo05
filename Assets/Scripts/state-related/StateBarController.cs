//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class StateBarController : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private Scrollbar timeScrollBar;
    [SerializeField] private Image filler;
    [SerializeField] private Image statebarBackground;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private Color _cState0 = new Color();
    private Color _cState2 = new Color();
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
        LevelManager.Instance.getStateTime();
        timeScrollBar.size = 0;
        //filler.color = new Color(170, 100, 57, 1);
        //statebarBackground.color = new Color(40, 83, 108, 1);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

        timeScrollBar.size = LevelManager.Instance.getStateTime() / LevelManager.Instance.getStateMaxTime();

        if (LevelManager.Instance.GetState() == 0)
        {
            filler.color = _cState2;
            statebarBackground.color = _cState0;

            
        }
        else
        {
            filler.color = _cState0;
            statebarBackground.color = _cState2;


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
    public void ChangeColorState0(string hexColor)
    {
        Color newColor;
        if (ColorUtility.TryParseHtmlString(hexColor, out newColor))
        {
            _cState0=newColor;
        }
        else
        {
            Debug.LogWarning("Invalid hex color string.");
        }
    }
    public void ChangeColorState2(string hexColor)
    {
        Color newColor;
        if (ColorUtility.TryParseHtmlString(hexColor, out newColor))
        {

            _cState2 = newColor;
            //Debug.Log(_cState2);
        }
        else
        {
            Debug.LogWarning("Invalid hex color string.");
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

} // class StateBarController 
// namespace
