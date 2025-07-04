//---------------------------------------------------------
// Gestiona todas las escenas en las que el jugador se pueda mover.
//No guarda info entre escenas.
// Adrián Erustes Martín, Antonio Bucero, Adrián de Miguel
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;

//using UnityEditorInternal;
using UnityEngine;

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
    /// <summary>
    /// Referencia del jugador
    /// </summary>
    [SerializeField] private GameObject player;

    /// <summary>
    /// Array con los gameObjects usados como spawnPos en esta escena (En escenas Level indica los inicios de cada sala, en escena Hub indica spawn inicial y puertas).
    /// </summary>
    [SerializeField] private GameObject[] PlayerSpawnPosScene;
    /// <summary>
    /// Array con las distintas zonas grises usadas en el inicio de las salas
    /// </summary>
    [SerializeField] private GrayZone[] grayZone;

    /// <summary>
    /// Array con el tiempo máximo para pasarse cada sala
    /// </summary>
    [SerializeField] private float[] RoomMaxTime;

    /// <summary>
    /// Tiempo restante de la sala
    /// </summary>
    [SerializeField] private float RoomTimeRemaining;

    /// <summary>
    /// El tiempo que tarda en poner una imagen translúcida del siguiente estado
    /// </summary>
    [SerializeField] private float ChangeTimeTrasluz = 3f;

    /// <summary>
    /// Número de salas por nivel
    /// </summary>
    [SerializeField] private int roomsAmount = 5;

    /// <summary>
    /// Array con las distintas posiciones de la cámara para cada sala
    /// </summary>
    [SerializeField] Vector3[] CameraPos;

    /// <summary>
    /// Variable utilizada para bloquear el tiempo del cambio de estado a modo de pruebas
    /// </summary>
    [SerializeField] private bool stateLocked;

    /// <summary>
    /// Variable utilizada para bloquear el tiempo de la sala a modo de pruebas
    /// </summary>
    [SerializeField] private bool timeLocked;

    /// <summary>
    /// Array de objetos de estado de las salas a los que se les asigna el estado correspondiente
    /// </summary>
    [SerializeField] private CambioEstado[] estados;

    /// <summary>
    /// NÚmero de sala en la que se encuentra el jugador. La primera room es la 0 y la última, la roomsAmount-1
    /// </summary>
    [SerializeField] private int roomNo = 0; 

    /// <summary>
    /// Verifica que este LevelManager se encuentra en la escena del Hub
    /// </summary>
    [SerializeField] bool isInHub = false;

    /// <summary>
    /// Color en hexadecimal que se asigna tanto a las plataformas como a las barras de estado según al estado y la sala que correspondan. Estado 2
    /// </summary>
    [SerializeField] private string colorState0 = "";

    /// <summary>
    /// Color en hexadecimal que se asigna tanto a las plataformas como a las barras de estado según al estado y la sala que correspondan. Estado 2
    /// </summary>
    [SerializeField] private string colorState2 = "";
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static LevelManager _instance;

    /// <summary>
    /// Estado actual de la sala (0-Estado 1, 2-Estado 2)
    /// </summary>
    private int State = 0;

    /// <summary>
    /// 
    /// </summary>
    private PlatformMovement[] _platformMovement;

    // Variables del contador de tiempo del estado

    /// <summary>
    /// Tiempo máximo para el cambio de estado
    /// </summary>
    private float StateMaxTime = 4f;

    /// <summary>
    /// Tiempo actual transcurrido en el estado activo
    /// </summary>
    private float StateTime = 0f;

    /// <summary>
    /// Referencia de la cámara
    /// </summary>
    private Camera Camera;

    /// <summary>
    /// Referencia de la barra de estados
    /// </summary>
    private StateBarController StateBarController;


    /// <summary>
    /// Posición de aparición inicial y reinicio del jugador
    /// </summary>
    private Vector3 playerSpawnPos;

    /// <summary>
    /// Número de muertes del jugador
    /// </summary>
    private int deaths = 0;

    /// <summary>
    /// Texto que muestra el número de muertes del jugador
    /// </summary>
    private TextMeshProUGUI deathCount;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    protected void Awake()
    {
        if (_instance == null)
        {
            // Somos la primera y única instancia
            _instance = this;
        }


    }

    private void Start()
    {
        AudioSource _enterLevel = GetComponent<AudioManager>()?.EnterLevel;
        if (_enterLevel != null && !_enterLevel.isPlaying && !isInHub)
        {
            _enterLevel.Play();
        }

        GameManager.Instance.SceneWillChange_Set(false);
        Camera = FindObjectOfType<Camera>();
        deaths = GameManager.Instance.AskDeaths();
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
            deathCount = GameObject.Find("CountText").GetComponent<TextMeshProUGUI>();
            deathCount.text = DeathCountString(deaths);

            Camera.transform.position = CameraPos[roomNo];
            playerSpawnPos = PlayerSpawnPosScene[roomNo].transform.position;
            for (int i = 0; i < estados.Length; i++, i++)
            {
                estados[i].ChangeColor(colorState0);
            }
            for (int i = 1; i < estados.Length; i++, i++)
            {
                estados[i].ChangeColor(colorState2);
            }
            StateBarController = FindObjectOfType<StateBarController>();
            StateBarController.ChangeColorState0(colorState0);
            StateBarController.ChangeColorState2(colorState2);
        }
        //Lleva al player al primer inicio de sala.
        player.GetComponent<PlayerMovement>().ResetPlayer(playerSpawnPos);


        RoomTimeRemaining = RoomMaxTime[roomNo];
        _platformMovement = FindObjectsByType<PlatformMovement>(FindObjectsSortMode.None);
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
                ResetPlayer();
                RoomTimeRemaining = RoomMaxTime[roomNo];
            }
        }
        if (!stateLocked)
        {
            StateTime += Time.deltaTime;

            if (StateTime > StateMaxTime-0.2f)
            {
                AudioSource _heart = GetComponent<AudioManager>()?.Heart;
                if (!isInHub && !_heart.isPlaying) //ñapa
                {
                    _heart.Play();
                }
            }
            if (StateTime > StateMaxTime)
            {

                

                ChangeState();
                for (int i = 0; i < estados.Length; i++)
                {
                    estados[i].CambiaEstado(State);
                }

                StateTime = 0f;
            }
            else if ((StateMaxTime / 2 - 0.1) < StateTime && StateTime < (StateMaxTime / 2 + 0.1) && !isInHub) //Que suene a la mitad tambien
            {
                AudioSource _heart = GetComponent<AudioManager>()?.Heart;
                if (!_heart.isPlaying)
                {
                    _heart.Play();
                }
            }
            else if (StateTime > ChangeTimeTrasluz && StateTime < StateMaxTime)
            {
                for (int i = 0; i < estados.Length; i++)
                {
                    estados[i].CambiaEstadoTrasLuz(State);
                }
            }
        }
        if(!isInHub)
        {
            AudioSource _clock = GetComponent<AudioManager>()?.Clock;
            _clock.pitch = (RoomMaxTime[roomNo] - RoomTimeRemaining) / (3 * RoomMaxTime[roomNo]) + 1;
            if (!grayZone[roomNo].IsTimeStopped() && !_clock.isPlaying)
            {

                _clock.Play();

            }
            else if (grayZone[roomNo].IsTimeStopped())
            {
                _clock.Stop();
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

    //Reincia tanto al jugador, como a las características de la sala que se reinician con el, realizando un efecto de sonido
    //Se añade valor al contador de muertes
    public void ResetPlayer()
    {
        AudioSource _glass = GetComponent<AudioManager>()?.Glass;
        if (!_glass.isPlaying)
        {
            _glass.Stop();

        }
        _glass.Play();

        player.GetComponent<PlayerMovement>().ResetPlayer(playerSpawnPos);

      
            for (int i = 0; i < _platformMovement.Length; i++)
            {
                _platformMovement[i].ResetPlatform();
            }
        
       
        RoomTimeRemaining = RoomMaxTime[roomNo];

        GameManager.Instance.PlayerDied();
        deaths = GameManager.Instance.AskDeaths();
        deathCount.text = DeathCountString(deaths);
    }

    //Cambia entre estado 1 (0) y estado 2 (2)
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
    }

    // Pasa a la siguiente sala
    // Si se llega al máximo de salas transporta al jugador a la escena Hub, desbloqueando el siguiente nivel
    public void NextRoom()
    {
        if (roomNo < roomsAmount - 1)
        {
            roomNo++;
            playerSpawnPos = PlayerSpawnPosScene[roomNo].transform.position;
            player.GetComponent<PlayerMovement>().ResetPlayer(playerSpawnPos);
            Camera.transform.position = CameraPos[roomNo];
            RoomTimeRemaining = RoomMaxTime[roomNo];

            AudioSource _sigSala = GetComponent<AudioManager>()?.SigSala;
            if (_sigSala != null && !_sigSala.isPlaying)
            {
                _sigSala.Play();
            }
        }
        else
        {
            AudioSource _exitLevel = GetComponent<AudioManager>()?.ExitLevel;
            if (_exitLevel != null && !_exitLevel.isPlaying)
            {
                _exitLevel.Play();
            }
            GameManager.Instance.LevelCompleted();
        }
    }

    //Métodos de conversion de variables a públicas------------------------------------
    public int GetState()
    {
        return State;
    }

    public int GetRoomNo()
    {
        return roomNo;
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

    //-----------------------------------------------------------------------------------
    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    // Exploit para avanzar de sala pulsando el botón N
    private void OnGUI()
    {
        Event e = Event.current;
        if (!isInHub && e.type == EventType.KeyDown && e.keyCode == KeyCode.N)
        {
            NextRoom();
        }
    }

    /// <summary>
    /// Método que facilita el texto de la cantidad de muertes
    /// </summary>
    private string DeathCountString(int deaths)
    {
        string deathString = "";
        deathString += $"{deaths}";
        return deathString;
    }



    #endregion
} // class LevelManager 
  // namespace