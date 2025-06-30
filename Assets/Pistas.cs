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
public class Pistas : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private GameObject pista1;
    [SerializeField] private GameObject pista2;
    [SerializeField] private GameObject pista3;
    [SerializeField] private GameObject[] camino1;
    [SerializeField] private GameObject[] camino2;
    [SerializeField] private GameObject[] camino3;
    [SerializeField] private int contadorPista1;
    [SerializeField] private int contadorPista2;
    [SerializeField] private int contadorPista3;

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
        pista1.SetActive(false);
        pista2.SetActive(false);
        pista3.SetActive(false);
        for (int i = 0; i < camino1.Length; i++)
        {
            Debug.Log(camino1.Length);
            camino1[i].SetActive(false);
            camino2[i].SetActive(false);
            camino3[i].SetActive(false);
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (LevelManager.Instance.GetIsInGrayZone() == true)
        {
            if (LevelManager.Instance.GetDeathsInRoom() >= contadorPista1 && LevelManager.Instance.GetDeathsInRoom() < contadorPista2)
            {
                pista1.SetActive(true);
            }
            if (LevelManager.Instance.GetDeathsInRoom() >= contadorPista2 && LevelManager.Instance.GetDeathsInRoom() < contadorPista3)
            {
                pista1.SetActive(false);
                pista2.SetActive(true);
            }
            if (LevelManager.Instance.GetDeathsInRoom() >= contadorPista3)
            {
                Debug.Log(LevelManager.Instance.GetDeathsInRoom());
                pista2.SetActive(false);
                pista3.SetActive(true);
            }
            if (LevelManager.Instance.GetDeathsInRoom() == 0) 
            {
                pista1.SetActive(false);
                pista2.SetActive(false);
                pista3.SetActive(false);
            }
        } else
        {
            pista1.SetActive(false);
            pista2.SetActive(false);
            pista3.SetActive(false);
            camino1[LevelManager.Instance.GetRoomNo()].SetActive(false);
            camino2[LevelManager.Instance.GetRoomNo()].SetActive(false);
            camino3[LevelManager.Instance.GetRoomNo()].SetActive(false);

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

    public void Pista1()
    {
        camino1[LevelManager.Instance.GetRoomNo()].SetActive(true);
        camino2[LevelManager.Instance.GetRoomNo()].SetActive(false);
        camino3[LevelManager.Instance.GetRoomNo()].SetActive(false);
    }

    public void Pista2()
    {
        camino1[LevelManager.Instance.GetRoomNo()].SetActive(false);
        camino2[LevelManager.Instance.GetRoomNo()].SetActive(true);
        camino3[LevelManager.Instance.GetRoomNo()].SetActive(false);
    }

    public void Pista3()
    {
        camino1[LevelManager.Instance.GetRoomNo()].SetActive(false);
        camino2[LevelManager.Instance.GetRoomNo()].SetActive(false);
        camino3[LevelManager.Instance.GetRoomNo()].SetActive(true);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class Pistas 
// namespace
