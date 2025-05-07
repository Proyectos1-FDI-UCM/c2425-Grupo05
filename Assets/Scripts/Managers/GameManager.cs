//---------------------------------------------------------
// Contiene el componente GameManager
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Componente responsable de la gestión global del juego. Es un singleton
/// que orquesta el funcionamiento general de la aplicación,
/// sirviendo de comunicación entre las escenas.
///
/// El GameManager ha de sobrevivir entre escenas por lo que hace uso del
/// DontDestroyOnLoad. En caso de usarlo, cada escena debería tener su propio
/// GameManager para evitar problemas al usarlo. Además, se debería producir
/// un intercambio de información entre los GameManager de distintas escenas.
/// Generalmente, esta información debería estar en un LevelManager o similar.
/// </summary>
public class GameManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static GameManager _instance;

    /// <summary>
    /// Máximo nivel alcanzado (está serializada para testing)
    /// </summary>
    [SerializeField]private int maxCurrentLvl = 0;

    /// <summary>
    /// Último nivel en el que has estado
    /// </summary>
    private int currentLvl = 0; 

    /// <summary>
    /// Cantidad de muertes
    /// </summary>
    private int deaths = 0;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    protected void Awake()
    {
        if (_instance != null)
        {
            // No somos la primera instancia. Se supone que somos un
            // GameManager de una escena que acaba de cargarse, pero
            // ya había otro en DontDestroyOnLoad que se ha registrado
            // como la única instancia.
            // Si es necesario, transferimos la configuración que es
            // dependiente de la escena. Esto permitirá al GameManager
            // real mantener su estado interno pero acceder a los elementos
            // de la escena particulares o bien olvidar los de la escena
            // previa de la que venimos para que sean efectivamente liberados.
            TransferSceneState();

            // Y ahora nos destruímos del todo. DestroyImmediate y no Destroy para evitar
            // que se inicialicen el resto de componentes del GameObject para luego ser
            // destruídos. Esto es importante dependiendo de si hay o no más managers
            // en el GameObject.
            DestroyImmediate(this.gameObject);
        }
        else
        {
            // Somos el primer GameManager.
            // Queremos sobrevivir a cambios de escena.
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        } // if-else somos instancia nueva o no.
    }

    /// <summary>
    /// Método llamado cuando se destruye el componente.
    /// </summary>
    protected void OnDestroy()
    {
        if (this == _instance)
        {
            // Éramos la instancia de verdad, no un clon.
            _instance = null;
        } // if somos la instancia principal
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static GameManager Instance
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
    /// cierre para evitar usar el GameManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Devuelve el máx level alcanzado actualmente
    /// </summary>
    /// <returns></returns>
    public int MaxLevel() { return maxCurrentLvl; }

    
    /// <summary>
    /// Devuelve el nivel actual
    /// </summary>
    /// <returns></returns>
    public int GetLvl() {  return currentLvl; }


    /// <summary>
    /// Va a la escena del nivel indicado.
    /// </summary>
    /// <param name="level"></param>
    public void GoToLvl(int level)
    {
        // Tenemos las siguientes escenas: 0-Main Menu, 1-Controles, 2-Hub. Por ello sumamos +2, para ignorar dichas escenas y nos lleve al nivel correspondiente
        currentLvl = level;

        GameManager.Instance.ChangeScene(level + 2); 
    }


    /// <summary>
    /// Se llama cuando se completa un nivel para volver al hub y actualizar el nivel máximo. Este se utiliza para desbloquear el siguiente nivel
    /// </summary>
    public void LevelCompleted()
    {
        if (currentLvl >= maxCurrentLvl) { maxCurrentLvl = currentLvl; }
        ChangeScene(2);//el hub es la escena 2.
    }
    /// <summary>
    /// Se utiliza para ir a la escena HUB. Ya sea desde el menu de pausa o desde la escena de controles
    /// </summary>
    public void GoToHub()
    {
        ChangeScene(2);//Hub es la escena 2.
    }


    /// <summary>
    /// Método que cambia la escena actual por la indicada en el parámetro.
    /// </summary>
    /// <param name="index">Índice de la escena (en el build settings)
    /// que se cargará.</param>
    public void ChangeScene(int index)
    {
        // Antes y después de la carga fuerza la recolección de basura, por eficiencia,
        // dado que se espera que la carga tarde un tiempo, y dado que tenemos al
        // usuario esperando podemos aprovechar para hacer limpieza y ahorrarnos algún
        // tirón en otro momento.
        // De Unity Configuration Tips: Memory, Audio, and Textures
        // https://software.intel.com/en-us/blogs/2015/02/05/fix-memory-audio-texture-issues-in-unity
        //
        // "Since Unity's Auto Garbage Collection is usually only called when the heap is full
        // or there is not a large enough freeblock, consider calling (System.GC..Collect) before
        // and after loading a level (or put it on a timer) or otherwise cleanup at transition times."
        //
        // En realidad... todo esto es algo antiguo por lo que lo mismo ya está resuelto)
        System.GC.Collect();
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        System.GC.Collect();
    } // ChangeScene



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

    private void TransferSceneState()
    {
        // De momento no hay que transferir ningún estado
        // entre escenas
    }

    /// <summary>
    /// Se llama cuando el jugador muere para incrementar el contador de muertes.
    /// </summary>
    public void PlayerDied()
    {
        if (deaths < 999){
            deaths++;
        }
    }
    /// <summary>
    /// Devuelve la cantidad de veces que ha muerto el jugador
    /// </summary>
    public int AskDeaths()
    {
        return deaths;
    }

    #endregion
} // class GameManager 
  // namespace