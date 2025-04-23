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
public class CameraZoom : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private Camera cam;
    [SerializeField] private float zoomSize = 1f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float cameraOffset = 1.5f;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private float defaultSize;
    private Vector3 defaultPosition;
    private float timeToEnterLevel = 3f;
    private bool isZooming; 

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
        // Guardamos el tamaño y posición inicial
        defaultSize = cam.orthographicSize;
        defaultPosition = cam.transform.position;
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

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    private void OnTriggerEnter2D(Collider2D coll)
    {
        PlayerMovement playerMovement = coll.gameObject.GetComponent<PlayerMovement>();
        Debug.Log("Trigger");

        if (playerMovement != null) 
        {
            Debug.Log("ZOOOOOM");
            StartCoroutine(ZoomIn(gameObject.transform, playerMovement));
        }
    }
        private void OnTriggerExit2D(Collider2D coll)
    {
        PlayerMovement playerMovement = coll.gameObject.GetComponent<PlayerMovement>();

        if (playerMovement != null) 
        {
            Debug.Log("ZOOOOOM");
            
            StartCoroutine(ZoomOut(playerMovement));
        }
    }

    System.Collections.IEnumerator ZoomIn(Transform target, PlayerMovement playerMovement)
    {
        isZooming = true;
        CameraHubFollow cameraHubFollow = cam.GetComponent<CameraHubFollow>();
        if (cameraHubFollow != null) cameraHubFollow.StartZoom();

        //playerMovement.enabled = false;
        while (Mathf.Abs(cam.orthographicSize - zoomSize) > 0.1f && isZooming)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomSize, Time.deltaTime * zoomSpeed);
            cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(target.position.x, target.position.y+cameraOffset, cam.transform.position.z), Time.deltaTime * zoomSpeed);
            Debug.Log("Dentro");
            yield return null;
        }
        cam.orthographicSize = zoomSize;
        //playerMovement.enabled = true;

    }

    System.Collections.IEnumerator ZoomOut(PlayerMovement playerMovement)
    {
        isZooming = false;
        CameraHubFollow cameraHubFollow = cam.GetComponent<CameraHubFollow>();
        if (cameraHubFollow != null) cameraHubFollow.StopZoom();

        //playerMovement.enabled = false;
        while (Mathf.Abs(cam.orthographicSize - defaultSize) >0.1f && !isZooming)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, defaultSize, Time.deltaTime * zoomSpeed);
            cam.transform.position = Vector3.Lerp(cam.transform.position, defaultPosition, Time.deltaTime * zoomSpeed);
            yield return null;
        }
        //playerMovement.enabled = true;
    }

    #endregion   

} // class CameraZoom 
// namespace
