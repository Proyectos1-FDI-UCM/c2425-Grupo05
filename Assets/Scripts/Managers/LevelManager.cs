//---------------------------------------------------------
// Gestiona todas las escenas en las que el jugador se pueda mover.
//No guarda info entre escenas.
// Adrián Erustes Martín, Antonio Bucero, 
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Componente que se encarga de la gestión de un nivel concreto.
/// Este componente es un singleton, para que sea accesible para todos
/// los objetos de la escena, pero no tiene el comportamiento de
/// DontDestroyOnLoad, ya que solo vive en una escena.
///
/// Contiene toda la información propia de la escena y puede comunicarse
/// con el GameManager para transferir información importante para
/// la gestión global del juego (información que ha de pasar entre
/// escenas)
/// </summary>
public class LevelManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private GameObject player;

    /// <summary>
    /// Array con los gameObjects usados como spawnPos en esta escena (En escenas Level indica los inicios de cada sala, en escena Hub indica spawn inicial y puertas).
    /// </summary>
    [SerializeField] private GameObject[] PlayerSpawnPosScene;
    [SerializeField] private GrayZone[] grayZone;

    // Variables del contador de tiempo de la sala
    [SerializeField] private float[] RoomMaxTime;

    [SerializeField] private float ChangeTimeTrasluz = 3f;//el tiempo que tarda en poner una imagen translúcida del siguiente estado

    [SerializeField] private int roomsAmount = 5;
    [SerializeField] Vector3[] CameraPos;
    // Variables de cambios de estado (0-Estado Neutral, 1-Estado 1, 2-Estado 2
    public int State = 0;
    [SerializeField] private bool stateLocked;
    [SerializeField] private bool timeLocked;
    [SerializeField] private CambioEstado[] estados;

    [SerializeField] private int roomNo = 0; //la primera room es la 0 y la última, la roomsAmount-1

    // indica si este manager es el del hub
    [SerializeField] bool isInHub = false;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static LevelManager _instance;
    private PlatformMovement[] _platformMovement;

    [SerializeField] private float RoomTimeRemaining;
    // Variables del contador de tiempo del estado
    private float StateMaxTime = 4f;
    private float StateTime = 0f;
    private Camera Camera;
    

    /// <summary>
    /// Pos de respawn e inicio por el momento
    /// </summary>
    private Vector3 playerSpawnPos;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    protected void Awake()
    {
        if (_instance == null)
        {
            // Somos la primera y única instancia
            _instance = this;
            Init();
        }
    }

    private void Start()
    {
        Camera = FindObjectOfType<Camera>();

        //Setea el tiempo a 0
        StateTime = 0f;
        
        //setea la pos inicial a la pos del objeto que indica el primer inicio de sala, o la última puerta en la que has entrado, en caso del hub.
        if (isInHub)
        {
            // la posición inicial es la de la última puerta visitada
            playerSpawnPos = PlayerSpawnPosScene[GameManager.Instance.GetLvl()].transform.position;
        }
        else
        {
            Camera.transform.position = CameraPos[roomNo];
            playerSpawnPos = PlayerSpawnPosScene[roomNo].transform.position;
        }
        //Lleva al player al primer inicio de sala.
        player.transform.position = playerSpawnPos;


        RoomTimeRemaining = RoomMaxTime[roomNo];
        _platformMovement = FindObjectsByType<PlatformMovement>(FindObjectsSortMode.None);
      

        //Debug.Log(estados.Length);
        
    }


    private void Update()
    {

        if (!timeLocked && !grayZone[roomNo].IsTimeStopped())
        {
            if (RoomTimeRemaining > 0)
            {
                RoomTimeRemaining -= Time.deltaTime;
            }
            else
            {
                //Debug.Log("TiempoFinalizado");
                ResetPlayer();
                RoomTimeRemaining = RoomMaxTime[roomNo];
            }

        }
        if (!stateLocked)
        {
            StateTime += Time.deltaTime;

            if (StateTime > StateMaxTime)
            {
                ChangeState();
                for (int i = 0; i < estados.Length; i++)
                {
                    estados[i].CambiaEstado(State);
                }
                
                StateTime = 0f;
            }

            else if (StateTime > ChangeTimeTrasluz && StateTime < StateMaxTime)
            {
                for (int i = 0; i < estados.Length; i++)
                {
                    estados[i].CambiaEstadoTrasLuz(State);
                }
            }
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static LevelManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el LevelManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }
    public int GetRoomNo()
    {
        return roomNo;
    }

    public void ResetPlayer()
    {
        AudioSource _glass = GetComponent<AudioManager>()?.Glass;
        if (!_glass.isPlaying)
        {
            _glass.Play();
        }


        player.transform.position = playerSpawnPos;

        for (int i = 0; i < _platformMovement.Length; i++)
        {
            _platformMovement[i].ResetPlatform();
        }
        /*StateTime = 0;
        State = 0;*/
        RoomTimeRemaining = RoomMaxTime[roomNo];


    }
    public void ChangeState()
    {
        if (State == 0)
        {
            State = 2;
        }
        else if (State == 2)
        {
            State = 0;
        }
        //if (state == 2)
        //{
        //    State = 0;
        //}
    }

    public void NextRoom()
    {
        if (roomNo < roomsAmount-1)
        {
            roomNo++;
            player.transform.position = PlayerSpawnPosScene[roomNo].transform.position;
            playerSpawnPos = PlayerSpawnPosScene[roomNo].transform.position;
            Camera.transform.position = CameraPos[roomNo];
            RoomTimeRemaining = RoomMaxTime[roomNo];
        }
        else
        {
            GameManager.Instance.LevelCompleted();
        }
    }

    public CambioEstado[] GetEstados()
    {
        return estados;
    }

    public int EstadoActual()
    {
        return State;
    }
    public bool GetIsHub()
    {
        return isInHub;
    }

    public float getMaxTime()
    {
        return RoomMaxTime[roomNo];
    }

    public float getStateMaxTime()
    {
        return StateMaxTime;
    }  

    public float getStateTime()
    {
        return StateTime;
    }  
    public float getRoomTimeRemaining()
    {
        return RoomTimeRemaining;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        // De momento no hay nada que inicializar
    }

    // Botón N para avanzar sala
    private void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.N)
        {
            NextRoom();
        }
    }

    #endregion
} // class LevelManager 
  // namespace