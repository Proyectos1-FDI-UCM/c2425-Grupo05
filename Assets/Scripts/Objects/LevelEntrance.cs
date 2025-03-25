//---------------------------------------------------------
// Script que usan las puertas para ir del hub a un nivel
// La plataformas se mueven cíclicamente siguiendo los waypoints representados como posiciones en el array.
// Adrián Erustes Martín
// I'm loosing it
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class LevelEntrance : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] int doorLevel;
    [SerializeField] SpriteRenderer sprite;
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
        ChangeDoorColor(doorLevel <=GameManager.Instance.MaxLevel()+1);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //solo puedes entrar en la puerta 2 si ya te has pasado el nivel 1 =>
        if (doorLevel <= GameManager.Instance.MaxLevel()+1) 
        {
            //el +1 representa las escenas del hub y el menú -1 porque las puertas empiezan desde el nivel 1 y las escenas desde la escena 0.
            GameManager.Instance.ChangeScene(doorLevel + 1);

        }
    }
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

    private void ChangeDoorColor(bool open)
    {
        if (open)
        {
            sprite.color = Color.green;
        }
        else
        {
            sprite.color = Color.red;
        }
        
    }

    #endregion

} // class LevelEntrance 
// namespace
