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


    /// <summary>
    ///Las componentes (x,y) del vector indican posición a la que se viaja (pos del siguiente waypoint).
    ///La componente z indica tiempo que se tarda en llegar al siguiente waypoint.
    ///La componente w indica la velocidad hasta llegar al siguiente waypoint.
    /// </summary>

    /*Se puede elegir si mover las plataformas estableciendo un tiempo o una velocidad entre waypoints.
         - La velocidad(w) se calcula automáticamente siempre y cuando el tiempo(z) != 0. 
        - Si el tiempo(z) == 0, este se calcula automáticamente a partir de la velocidad introducida.
    */
    [SerializeField] private Vector4[] waypoints;


    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Indica el momento en el que empezaste a moverte hacia el waypoint n ó acabaste de moverte hacia el waypoint n-1 (es lo mismo).
    /// </summary>
    private float _lastWaypointTime;

    /// <summary>
    /// Indica el waypoint hacia el que me estoy moviendo.
    /// </summary>
    private int _currentWaypoint = 0;

    /// <summary>
    /// Array con las velocidades para optimización (se calcula solo una vez)
    /// </summary>
    private Vector2[] speeds;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    private void Awake()
    {
        //autocompleta las velocidades y/o los tiempos de los waypoints;
        speeds = new Vector2[waypoints.Length];
        AutocompleteWaypoint(waypoints.Length - 1, 0);

        for (int i = 1; i < waypoints.Length; i++)
        {
            AutocompleteWaypoint(i - 1, i);
        }

        ResetPlatform();


    }
    private void FixedUpdate()
    {
        //lo pongo en el fixedUpdate porque el movimiento de la plataforma tiene que ir en sincronía con el movimiento del jugador para que este se pueda quedar encima de la plataforma.
        //Esto lo conseguimos dándole al jugador la misma velocidad que la de la plataforma durante esa parte del trayecto.
        //Si dejan de moverse en sincronía, la detección de colisión con la plataforma para tener la misma velocidad se rompe.
        if (waypoints[_currentWaypoint].w == 0)
        {
            transform.localPosition = new Vector2(waypoints[_currentWaypoint].x, waypoints[_currentWaypoint].y);
        }
        else
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, waypoints[_currentWaypoint], Time.fixedDeltaTime * waypoints[_currentWaypoint].w);
        }
        if (Time.time > _lastWaypointTime + waypoints[_currentWaypoint].z)
        {
            if (_currentWaypoint + 1 < waypoints.Length) _currentWaypoint++; //voy a hacer una encuesta sobre este if
            else _currentWaypoint = 0;
            _lastWaypointTime = Time.time;
        }
    }

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <returns>Velocidad de la plataforma
    /// </returns>
    public Vector2 getVel()
    {
        return speeds[_currentWaypoint];
    }

    /// <summary>
    /// reinicia la plataforma a su posición inicial.
    /// </summary>
    public void ResetPlatform()
    {
        _currentWaypoint = 0;

        //Va al waypoint inicial
        transform.localPosition = waypoints[0];
        /*
          Hace que el momento en el que hayas acabado de moverte hacia el waypoint inicial sea el actual => 
          => en el siguiente FixedUpdate empezará a moverse al siguiente waypoint (el número 1) 
        */
        _lastWaypointTime = Time.fixedTime;
        //Suma 1 a n para que se empiece a mover hacia el waypoint 1;
        if (waypoints.Length > 1) _currentWaypoint++;
    }
    #endregion



    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados


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
