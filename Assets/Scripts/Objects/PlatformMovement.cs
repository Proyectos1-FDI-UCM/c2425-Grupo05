//---------------------------------------------------------
// Script que usan las plataformas para moverse(está en las plataformas)
// La plataformas se mueven cíclicamente siguiendo los waypoints representados como posiciones en el array.
// Adrián Erustes Martín
// I'm loosing it
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>


public class PlatformMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----



    //Las componentes x,y del vector indican posición a la que se viaja.
    //La componente z indica tiempo que se tarda en llegar a (x,y).
    //La componente w indica la velocidad hasta llegar a (x,y).

    /*
     Se puede elegir si mover las plataformas estableciendo un tiempo o una velocidad entre waypoints.
      
      - La velocidad(w) se calcula automáticamente siempre y cuando el tiempo(z) != 0. 
      - Si el tiempo(z) == 0, este se calcula automáticamente a partir de la velocidad introducida.

    */
    [SerializeField] private Vector4[] waypoints;


    // ---- ATRIBUTOS PRIVADOS ----

    //Indica el momento en el que empezaste a moverte hacia el waypoint n / acabaste de moverte hacia el waypoint n-1.
    private float _lastWaypointTime;

    //Indica el waypoint hacia el que me estoy moviendo la plataforma.
    private int _n = 0;
    private Vector2[] speeds;
    //Pos ini de la plataforma


    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    private void Awake()
    {
        //autocompleta las velocidades y/o los tiempos de los waypoints;
        speeds = new Vector2[waypoints.Length];
        AutocompleteWaypoint(waypoints.Length - 1, 0);
        
        for (int i = 1; i < waypoints.Length; i++)
        {
            AutocompleteWaypoint(i-1,i);
        }

        ResetPlatform();


    }
    private void FixedUpdate()
    {
        if (waypoints[_n].w == 0)
        {
            transform.position = new Vector2(waypoints[_n].x, waypoints[_n].y);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[_n], Time.fixedDeltaTime * waypoints[_n].w);
        }
        if (Time.fixedTime > _lastWaypointTime + waypoints[_n].z)
        {
            if (_n + 1 < waypoints.Length) _n++;
            else _n = 0;
            _lastWaypointTime = Time.fixedTime;
        }

    }

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    public Vector2 getVel()
    {
        return speeds[_n];
    }
    //reinicia la plataforma a su posición inicial.
    public void ResetPlatform()
    {
        _n = 0;

        //Va al waypoint inicial
        transform.position = waypoints[0];
        /*
          Hace que el momento en el que hayas acabado de moverte hacia el waypoint inicial sea el actual => 
          => en el siguiente FixedUpdate empezará a moverse al siguiente waypoint (el número 1) 
        */
        _lastWaypointTime = Time.fixedTime;
        //Suma 1 a n para que se empiece a mover hacia el waypoint 1;
        if (waypoints.Length > 1) _n++;
    }
    #endregion



    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    /// <summary>
    /// Autocompleta los valores de vel o tiempo de la plataforma y agrega la vel(i,j) al array speeds
    /// </summary>
    /// <param name="posPrev"></param>
    /// <param name="pos"></param>
    private void AutocompleteWaypoint(int posPrev, int pos)
    {
        if (waypoints[pos].z == 0)
        {
            if (waypoints[pos].w != 0)
            {
                waypoints[pos].z = Vector2.Distance(waypoints[posPrev], waypoints[pos]) / waypoints[pos].w; // tiempo = espacio / velocidad
            }
        }
        else
        {

            waypoints[pos].w = Vector2.Distance(waypoints[posPrev], waypoints[pos]) / waypoints[pos].z; // velocidad = espacio / tiempo


            if (waypoints[pos].w != 0) Debug.Log("Cuidado: Tiempo(z) y Velocidad(w) de la plataforma móvil no deben ser manipuladas a la vez");

        }
        speeds[pos].x = (waypoints[pos].x - waypoints[posPrev].x) / waypoints[pos].z;
        speeds[pos].y = (waypoints[pos].y - waypoints[posPrev].y) / waypoints[pos].z;


    }
    #endregion
} // class PlatformMovement 
// namespace
