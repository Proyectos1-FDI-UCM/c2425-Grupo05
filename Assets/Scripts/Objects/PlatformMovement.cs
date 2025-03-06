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
    private float lastWaypointTime;

    //Indica el waypoint hacia el que me estoy moviendo la plataforma.
    private int n = 0;
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

        if (waypoints[0].z == 0 && waypoints[0].w == 0)
        {
        }
        else if (waypoints[0].z != 0 && waypoints[0].w == 0)
        {
            waypoints[0].w = Vector2.Distance(waypoints[waypoints.Length - 1], waypoints[0]) / waypoints[0].z; // velocidad = espacio / tiempo
            if (waypoints[0].w != 0)
            {
                Debug.Log("Cuidado: Tiempo(z) y Velocidad(w) de la plataforma móvil no deben ser manipuladas al mismo tiempo");
            }
        }
        else if (waypoints[0].w != 0 && waypoints[0].z == 0)
        {
            waypoints[0].z = Vector2.Distance(waypoints[waypoints.Length - 1], waypoints[0]) / waypoints[0].w; // tiempo = espacio / velocidad
        }

        for (int i = 1; i < waypoints.Length; i++)
        {
            if (waypoints[i].z == 0 && waypoints[i].w == 0)
            {
            }
            else if (waypoints[i].z != 0 && waypoints[i].w == 0)
            {
                waypoints[i].w = Vector2.Distance(waypoints[i - 1], waypoints[i]) / waypoints[i].z;
                waypoints[i].w = Vector2.Distance(waypoints[i - 1], waypoints[i]) / waypoints[i].z; // velocidad = espacio / tiempo
                if (waypoints[i].w != 0)
                {
                    Debug.Log("Cuidado: Tiempo(z) y Velocidad(w) de la plataforma móvil no deben ser manipuladas al mismo tiempo");
                }


            }
            else if (waypoints[i].w != 0 && waypoints[i].z == 0)
            {
                waypoints[i].z = Vector2.Distance(waypoints[i - 1], waypoints[i]) / waypoints[i].w;
                waypoints[i].z = Vector2.Distance(waypoints[i - 1], waypoints[i]) / waypoints[i].w; // tiempo = espacio / velocidad
            }

            //precalcular velocidades (i,j)
            speeds[0].x = (waypoints[0].x - waypoints[waypoints.Length - 1].x) / waypoints[0].z;
            speeds[0].y = (waypoints[0].y - waypoints[waypoints.Length - 1].y) / waypoints[0].z;
            speeds[i].x = (waypoints[i].x - waypoints[i - 1].x) / waypoints[i].z;
            speeds[i].y = (waypoints[i].y - waypoints[i - 1].y) / waypoints[i].z;
        }


        ResetPlatform();


    }
    private void FixedUpdate()
    {
        if (waypoints[n].w == 0)
        {
            transform.position = new Vector2(waypoints[n].x, waypoints[n].y);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[n], Time.fixedDeltaTime * waypoints[n].w);
        }
        if (Time.fixedTime > lastWaypointTime + waypoints[n].z)
        {
            if (n + 1 < waypoints.Length) n++;
            else n = 0;
            lastWaypointTime = Time.fixedTime;
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

    public Vector2 getVel()
    {
        return speeds[n];
    }
    //reinicia la plataforma a su posición inicial.
    public void ResetPlatform()
    {
        n = 0;

        //Va al waypoint inicial
        transform.position = waypoints[0];
        /*
          Hace que el momento en el que hayas acabado de moverte hacia el waypoint inicial sea el actual => 
          => en el siguiente FixedUpdate empezará a moverse al siguiente waypoint (el número 1) 
        */
        lastWaypointTime = Time.fixedTime;
        //Suma 1 a n para que se empiece a mover hacia el waypoint 1;
        if (waypoints.Length > 1) n++;
    }

    // ---- MÉTODOS PRIVADOS ----

} // class PlatformMovement 
// namespace
