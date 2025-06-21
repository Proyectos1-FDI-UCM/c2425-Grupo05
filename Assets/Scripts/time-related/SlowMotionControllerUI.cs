//---------------------------------------------------------
// Conecta la carga de la habilidad de tiempo con su UI
// Amiel Ramos Juez
// I'm Losing It
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------


using UnityEngine;
using UnityEngine.UI;

public class TimeChargeUI : MonoBehaviour
{
    [SerializeField] private Image chargeCircle;
    [SerializeField] private Image cooldownImage;
    private SlowMotionController slowMotionController;

    void Start()
    {        
        slowMotionController = LevelManager.Instance.GetPlayer().GetComponent<SlowMotionController>();
    }

    void Update()
    {
        chargeCircle.fillAmount = slowMotionController.GetTimeChargePercent();
        chargeCircle.color = slowMotionController.IsUsingTimeControl() ? new Color(0.9f, 0.6f, 0.6f, 1) : Color.white;
        chargeCircle.color = new Color(chargeCircle.color.r, chargeCircle.color.g, chargeCircle.color.b, slowMotionController.CooldownFinished() ? 1 : 0.5f);

        cooldownImage.fillAmount = slowMotionController.GetCooldownPercent();
    }
}