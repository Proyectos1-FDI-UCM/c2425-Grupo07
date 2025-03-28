//---------------------------------------------------------
// Sistema de Dash del Jugador
// Este script implementa la mecánica de impulso rápido (dash)
// permitiendo al jugador moverse rápidamente en la dirección
// que está mirando.
// Óliver García Aguado
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using

/// <summary>
/// Clase que gestiona la mecánica de dash del jugador.
/// Se encarga de:
/// - Detectar la activación del dash mediante input
/// - Aplicar el impulso en la dirección del jugador
/// - Controlar el tiempo de duración y recarga del dash
/// - Gestionar el estado del dash (activo/inactivo)
/// </summary>
public class PlayerDash : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Velocidad a la que se moverá el jugador durante el dash
    /// </summary>
    [SerializeField] float DashSpeed;

    /// <summary>
    /// Duración en segundos del dash
    /// </summary>
    [SerializeField] float DashDuration;

    /// <summary>
    /// Referencia a la acción de input que activará el dash
    /// </summary>
    [SerializeField] private InputActionReference DashActionReference;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Componente Rigidbody2D del jugador para controlar su movimiento
    /// </summary>
    private Rigidbody2D _rb;
    private PlayerMovement _pM;

    /// <summary>
    /// Indica si el dash está actualmente en ejecución
    /// </summary>
    private bool _isDashing = false;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Inicializa y habilita el sistema de input del jugador.
    /// </summary>
    private void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.actions.Enable();
            Debug.Log("Player Input habilitado");
        }
    }

    /// <summary>
    /// Obtiene la referencia al componente Rigidbody2D del jugador.
    /// </summary>
    void Start()
    {
       _rb = GetComponent<Rigidbody2D>();
        _pM = GetComponent<PlayerMovement>();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Procesa la solicitud de dash del jugador.
    /// Solo se ejecuta si no hay un dash actualmente en curso.
    /// </summary>
    /// <param name="context">Contexto del input proporcionado por el sistema de input</param>

    public bool IsDashing()
    { return _isDashing; }

    private void RequestDash(InputAction.CallbackContext context)
    {
        if (!_isDashing)
        {
            Debug.Log("DASH ACTIVADO");
            _isDashing = true;
            StartCoroutine(StartDash());
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Ejecuta la mecánica del dash.
    /// Aplica la velocidad en la dirección actual del jugador y
    /// espera la duración configurada antes de permitir otro dash.
    /// </summary>
    /// <returns>IEnumerator para la corrutina</returns>
    private IEnumerator StartDash()
    {
        _rb.velocity = _pM.GetLastMove() * DashSpeed * Time.deltaTime;
        yield return new WaitForSeconds(DashDuration);
        _isDashing = false;
        Debug.Log("DASH RECARGADO");
    }

    /// <summary>
    /// Suscribe el método RequestDash al evento de input cuando el componente se habilita.
    /// </summary>
    private void OnEnable()
    {
        DashActionReference.action.performed += RequestDash;
        DashActionReference.action.Enable();
    }

    /// <summary>
    /// Desuscribe el método RequestDash del evento de input cuando el componente se deshabilita.
    /// </summary>
    private void OnDisable()
    {
        DashActionReference.action.performed -= RequestDash;
        DashActionReference.action.Disable();
    }
    #endregion
} // class Dash 
// namespace
