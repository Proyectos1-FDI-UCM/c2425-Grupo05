using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class LevelSelectMenu : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectMenu;
    [SerializeField] private Button[] roomButtons; // Asigna en el inspector los 5 botones
    [SerializeField] private int nivel = 1; // 1 o 2, según el menú
    [SerializeField] private Image qFlecha;

    private int selectedIndex = 0;

    private bool menuIsOpen = false;
    private Vector3 originalScale;
    private PlayerMovement playerMovement;

    void Start()
    {
        // Inicializa los botones según si están desbloqueados
        for (int i = 0; i < roomButtons.Length; i++)
        {
            int sala = i + 1;
            bool desbloqueada = LevelManager.Instance.SalaDesbloqueada(nivel, sala);
            roomButtons[i].interactable = desbloqueada;
            roomButtons[i].GetComponentInChildren<Text>().text = nivel + "-" + sala;
            originalScale = levelSelectMenu.transform.localScale;
        }

        playerMovement = LevelManager.Instance.GetPlayer().GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Detectar qué botón está seleccionado
        for (int i = 0; i < roomButtons.Length; i++)
        {
            if (EventSystem.current.currentSelectedGameObject == roomButtons[i].gameObject)
            {
                selectedIndex = i;
                break;
            }
        }
    }

    public void CargarSala()
    {
        int sala = selectedIndex + 1;
        if (LevelManager.Instance.SalaDesbloqueada(nivel, sala))
        {
            Debug.Log("Cargando sala " + sala);
            
            GameManager.Instance.SceneWillChange_Set(true);
            GameManager.Instance.GoToLvlSala(nivel, sala);
        }
        else
        {
            // REALIZAR ANIMACIÓN BLOQUEADO!!
            Debug.Log("Sala " + sala + " está bloqueada.");
        }
    }

    public bool isMenuOpen() { return menuIsOpen; }

    public void AbrirMenu()
    {
        levelSelectMenu.SetActive(true); // Activar objeto antes de la animación
        EventSystem.current.SetSelectedGameObject(roomButtons[0].gameObject); // Seleccionar el primer botón por defecto
        StartCoroutine(AnimacionMenu(1)); // Abrir menú
        playerMovement.enabled = false; // Bloquear el movimiento del jugador
    }

    public void CerrarMenu()
    {
        EventSystem.current.SetSelectedGameObject(null); // Deseleccionar botones
        StartCoroutine(AnimacionMenu(-1)); // Cerrar menú
        playerMovement.enabled = true; // Desbloquear el movimiento del jugador
        qFlecha.enabled = false; // Desactivar qFlecha
    }

    private IEnumerator AnimacionMenu(int direccion) // 1 abrir, -1 cerrar
    {
        // Si ya está abierto y se intenta abrir o viceversa
        if (menuIsOpen && direccion == 1 || !menuIsOpen && direccion == -1) { Debug.Log("YIELD BREAK"); yield break; }
        menuIsOpen = (direccion == 1);
        
        if (direccion == 1) // Si se abre, empieza en 0
            levelSelectMenu.transform.localScale = new Vector3(0f, originalScale.y, originalScale.z);


        float targetX = (direccion == 1) ? originalScale.x : 0f;
        float startX = (direccion == 1) ? 0f : originalScale.x;
                
        float duration = 0.25f;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(t / duration);
            float scaleX = Mathf.Lerp(startX, targetX, progress);

            levelSelectMenu.transform.localScale = new Vector3(scaleX, originalScale.y, originalScale.z);
            yield return null;
        }
        
        // Última escala para asegurarse
        levelSelectMenu.transform.localScale = new Vector3(targetX, originalScale.y, originalScale.z);

        // Aparecer qFlecha
        if (direccion == 1) { qFlecha.GetComponent<Image>().enabled = true; Debug.Log("qFlecha activada"); }

        if (direccion == -1) levelSelectMenu.SetActive(false); // Cuando termina de cerrarse, desactivarlo
    }
}