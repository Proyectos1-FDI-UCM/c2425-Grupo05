using UnityEngine;

public class SlowMotionController : MonoBehaviour
{
    // FACTORES DE TIEMPO
    [Header("FACTORES DE TIEMPO")]
    [SerializeField] private float slowDownFactor = 0.2f;
    [SerializeField] private float speedUpFactor = 3f;

    // CARGA Y DESCARGA
    [Header("CARGA Y DESCARGA")]
    [SerializeField] private float abilityCharge = 1f; // Carga actual, del 0 al 1, de la habilidad
    [SerializeField] private float minChargeToUse = 1f; // 50%
    [SerializeField] private float timeToCompletelyCharge = 5f;
    [SerializeField] private float timeToCompletelyDischarge = 3f;

    // COOLDOWN
    [Header("COOLDOWN")]
    [SerializeField] private float maxCooldown = 1f; // Cooldown tras un solo uso (la barra aparecerá si no es 0 o 1)
    [SerializeField] private float cooldownTimer = 0f; // Timer de cooldown tras un solo uso (la barra de la UI solo aparecerá si no es 0 o 1)

    // TIMER DE USO
    [Header("TIMER DE USO")]
    [SerializeField] private float useTimer = 0f; // Se usa Time.unscaledDeltaTime para calcular tiempo real
    [SerializeField] private float maxUseDuration = 3f;


    // Booleanos
    private bool isAtLevelStart = false; // Si está en la zona neutral
    private bool isUsingTimeControl = false; // Se está usando la habilidad
    private int lastUsed = 0; // -1: lento, 1: rapido, 0: ninguno


    // Referencia al LevelManager para comprobar la zona neutral
    private LevelManager levelManager;

    void Start()
    {
        levelManager = LevelManager.Instance;
    }

    void Update()
    {

        if (levelManager.GetIsHub()) return;
        isAtLevelStart = levelManager.IsTimeStopped();

        // Detectar botones
        bool slowDown = InputManager.Instance.TimeIzqIsDown();
        bool speedUp = InputManager.Instance.TimeDerIsDown();

        // Si ambos están pulsados, solo se mantiene el último usado
        if (slowDown && speedUp)
        {
            slowDown = lastUsed == -1;
            speedUp = lastUsed == 1;
        }

        // Se puede usar si estás en la zona neutral o carga>0.5 y cooldown<=0
        bool canUseTime = isAtLevelStart || (abilityCharge >= minChargeToUse && cooldownTimer <= 0f);

        // SI SE PUEDE USAR Y SE ESTÁN PULSANDO BOTONES || SI SE ESTÁ USANDO
        if ((canUseTime || isUsingTimeControl) && (slowDown || speedUp))
        {
            // Si no se está usando, usar
            if (!isUsingTimeControl)
            {
                isUsingTimeControl = true;
                useTimer = 0f;
                lastUsed = slowDown ? -1 : 1;
            }

            // Se está usando, aumenta el timer
            useTimer += Time.unscaledDeltaTime;

            // Si no estamos en zona neutral, modificamos la carga
            if (!isAtLevelStart)
            {
                // Descarga proporcional
                abilityCharge -= Time.unscaledDeltaTime / timeToCompletelyDischarge;
                abilityCharge = Mathf.Clamp01(abilityCharge); // Limitamos la carga entre 0 y 1


                // Si se agota el tiempo de uso o la carga, termina
                if (useTimer >= maxUseDuration || abilityCharge <= 0f)
                {
                    cooldownTimer = maxCooldown;
                    isUsingTimeControl = false;
                    lastUsed = 0;
                }
            }
        }
        // !canUseTime o no se pulsan los botones, recargamos cooldown y carga
        else
        {
            // Reseteamos el cooldownTimer
            if (isUsingTimeControl)
            {
                cooldownTimer = maxCooldown;
                isUsingTimeControl = false;
                lastUsed = 0;
            }
            // Recarga proporcional
            if (!isAtLevelStart && abilityCharge < 1f)
            {
                abilityCharge += Time.unscaledDeltaTime / timeToCompletelyCharge;
                if (abilityCharge > 0.9f) abilityCharge = 1f;
            }
            abilityCharge = Mathf.Clamp01(abilityCharge);

            // Cooldown tras uso
            if (cooldownTimer > 0f) cooldownTimer -= Time.unscaledDeltaTime;
        }

        // Modificar timeScale
        if (isUsingTimeControl) Time.timeScale = lastUsed == -1 ? slowDownFactor : speedUpFactor;
        else Time.timeScale = 1f;
    }

    // Métodos públicos para la UI
    public float GetTimeChargePercent() => abilityCharge;
    public bool CanUseTimeControl() => isAtLevelStart || (abilityCharge > minChargeToUse && cooldownTimer <= 0f);
    public bool IsUsingTimeControl() => isUsingTimeControl;
}