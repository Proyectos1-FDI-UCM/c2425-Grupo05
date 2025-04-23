//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using System.Collections;
using UnityEngine.UI; // Necesario para trabajar con UI Image

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class ScreenFader : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private Image fadeImage;  // Imagen negra usada para hacer el efecto de fade
    [SerializeField] private float fadeDuration = 1f; // DUración del efecto (en segundos) 
    private static ScreenFader _instance;
    public static ScreenFader Instance { get; private set; }   
    // Singleton estático para que otros scripts accedan a este script fácilmente


    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
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
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    // Llama a esta corrutina para hacer un fade-out (pantalla a negro)
    public IEnumerator FadeOut()
    {
        yield return StartCoroutine(Fade(1f));
    }

    // Llama a esta corrutina para hacer un fade-in (pantalla desde negro a juego)
    public IEnumerator FadeIn()
    {
        yield return StartCoroutine(Fade(0f));
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // El termino alpha hace referencia a la opacidad del color
    //Ya que en unity los colores tienen 4 componentes: Los RGB y el Alpha
    private IEnumerator Fade(float finalOpacity)
    {
        float startAlpha = fadeImage.color.a;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, finalOpacity, timer / fadeDuration);
            SetImageAlpha(alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        SetImageAlpha(finalOpacity);
    }

    private void SetImageAlpha(float opacity)
    {
        Color c = fadeImage.color;
        c.a = opacity;
        fadeImage.color = c;
    }
}
    #endregion   

} // class TransitionEffect 
// namespace
