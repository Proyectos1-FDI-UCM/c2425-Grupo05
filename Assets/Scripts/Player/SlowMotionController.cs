using UnityEngine;

public class SlowMotionController : MonoBehaviour
{
    // FACTORES DE TIEMPO
    [Header("FACTORES DE TIEMPO")]
    [SerializeField] private float slowDownFactor = 0.3f;
    [SerializeField] private float speedUpFactor = 3f;

    // CARGA Y DESCARGA
    [Header("CARGA Y DESCARGA")]
    [SerializeField] private float abilityCharge = 1f; // Carga actual, del 0 al 1, de la habilidad
    [SerializeField] private float minChargeToUse = 1f;
    [SerializeField] private float timeToCompletelyCharge = 7f;
    [SerializeField] private float timeToCompletelyDischarge = 2f;

    // COOLDOWN
    [Header("COOLDOWN")]
    [SerializeField] private float maxCooldown = 5f; // Cooldown tras un solo uso (la barra aparecerá si no es 0 o 1)
    [SerializeField] private float cooldownTimer = 0f; // Timer de cooldown tras un solo uso (la barra de la UI solo aparecerá si no es 0 o 1)

    // TIMER DE USO (se ha descartado su uso por ser innecesario)
    // [Header("TIMER DE USO")]
    // [SerializeField] private float useTimer = 0f; // Se usa Time.unscaledDeltaTime para calcular tiempo real
    // [SerializeField] private float maxUseDuration = 3f;


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

        if (levelManager.GetIsHub() || levelManager.GetIsTutorial()) return;
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

        // Se puede usar si estás en la zona neutral o carga > minCharge y cooldown <= 0
        bool canUseTime = isAtLevelStart || (abilityCharge >= minChargeToUse && cooldownTimer <= 0f);
        if (isAtLevelStart) { cooldownTimer = 0; abilityCharge = 1f; }// En zona neutral, carga al máximo

        // SI SE PUEDE USAR Y SE ESTÁN PULSANDO BOTONES || SI SE ESTÁ USANDO
        if ((canUseTime || isUsingTimeControl) && (slowDown || speedUp))
        {
            // Si no se está usando, usar
            if (!isUsingTimeControl)
            {
                isUsingTimeControl = true;
                lastUsed = slowDown ? -1 : 1;
                // useTimer = 0f;
            }

            // Se está usando, aumenta el timer
            // useTimer += Time.unscaledDeltaTime;

            // Si no estamos en zona neutral, modificamos la carga
            if (!isAtLevelStart)
            {
                // Descarga proporcional
                abilityCharge -= Time.unscaledDeltaTime / timeToCompletelyDischarge;
                abilityCharge = Mathf.Clamp01(abilityCharge); // Limitamos la carga entre 0 y 1

                // Si se agota la carga (o el tiempo de uso), termina
                if (abilityCharge <= 0f /*|| useTimer >= maxUseDuration*/)
                {
                    cooldownTimer = maxCooldown; // Comienza el contador de cooldown
                    isUsingTimeControl = false;
                    lastUsed = 0;
                }
            }
        }
        // !canUseTime o no se pulsan los botones, recargamos cooldown y carga
        else
        {
            // Tras soltar, reseteamos el cooldownTimer
            if (isUsingTimeControl)
            {
                if (!isAtLevelStart) cooldownTimer = maxCooldown;
                isUsingTimeControl = false;
                lastUsed = 0;
            }
            // Recarga proporcional (si es menor que 1 o falta justo el tiempo para llegar a 1)
            abilityCharge += Time.unscaledDeltaTime / timeToCompletelyCharge;
            abilityCharge = Mathf.Clamp01(abilityCharge);

            // Cooldown tras uso
            cooldownTimer -= Time.unscaledDeltaTime;
            cooldownTimer = Mathf.Clamp(cooldownTimer, 0f, maxCooldown);
        }

        // Modificar timeScale
        if (isUsingTimeControl)
            Time.timeScale = lastUsed == -1 ? slowDownFactor : speedUpFactor;
        else Time.timeScale = 1f;

        // FixedDeltaTime arregla los tirones (movimiento no fluido) al cambiar el timeScale
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // FixedDeltaTime por defecto es 0.02f
    }

    // Métodos públicos para la UI
    public float GetTimeChargePercent() { return abilityCharge; }
    public bool CanUseTimeControl() { return isAtLevelStart || (abilityCharge >= minChargeToUse && cooldownTimer <= 0f); }
    public bool IsUsingTimeControl() { return isUsingTimeControl; }
    public bool CooldownFinished() { return cooldownTimer <= 0 || isAtLevelStart; }
    public float GetCooldownPercent() {
        if (cooldownTimer <= 0 || cooldownTimer >= maxCooldown || isAtLevelStart) return 0f;
        else return 1f - (cooldownTimer / maxCooldown); }
}