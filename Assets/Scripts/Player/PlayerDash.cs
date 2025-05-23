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

    // /// <summary>
    // /// Prefab del efecto visual de humo que aparece al hacer dash
    // /// </summary>
    // [SerializeField] private GameObject dashSmokePrefab;

    /// <summary>
    /// Duración en segundos del dash
    /// </summary>
    [SerializeField] float DashDuration;

    /// <summary>
    /// Las partículas que genera el jugador al realizar un dash
    /// </summary>
    [SerializeField] private ParticleSystem dashParticles;


    [SerializeField] private AudioSource DashSFX;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Indica si el dash está actualmente en ejecución
    /// </summary>
    private bool _isDashing = false;
    private float timecounter = 0f;
    private Vector2 _dashVelocity;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Reestablece el contador del dash al iniciar el juego para asegurar el buen funcionamiento.
    /// </summary>
    void Start()
    {
        timecounter = 0f;
        dashParticles = GetComponentInChildren<ParticleSystem>();
    }
    /// <summary>
    /// Este update se encarga de activar el dash y de establecer la velocidad del mismo.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.DashWasPressedThisFrame())
        {
            RequestDash();
        }
        if (_isDashing)
        {
            if (timecounter < DashDuration)
            {
                timecounter += Time.deltaTime;
                // Aplico la velocidad del dash al la variable _dashVelocity que después tomara PlayerMovement para aplicar la velocidad al Rigidbody2D
                _dashVelocity = InputManager.Instance.LastMovementVector * DashSpeed * (1 - (timecounter / DashDuration));
            }
            else
            {
                _isDashing = false;
                timecounter = 0f;
                _dashVelocity = Vector2.zero;
            }
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Procesa la solicitud de dash del jugador.
    /// Solo se ejecuta si no hay un dash actualmente en curso.
    /// </summary>
    /// <param name="context">Contexto del input proporcionado por el sistema de input</param>

    /// <summary>
    /// Devuelve una boleana que indica si el dash está activo o no.
    /// </summary>
    public bool IsDashing()
    { return _isDashing; }

    /// <summary>
    /// Devuelve la velocidad del dash en función de la dirección del movimiento del jugador.
    /// </summary>
    public Vector2 GetDashVelocity()
    { return _dashVelocity; }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Activa el dash alterando la boleana _isDashing a true.
    /// </summary>
    private void RequestDash()
    {
        if (!_isDashing)
        {
            Debug.Log("DASH ACTIVADO");
            _isDashing = true;

            // Reproducir el sonido del dash si está asignado
            if (DashSFX != null)
            {
                DashSFX.Play();
            }

            // Instanciar el efecto de humo
            if (dashParticles != null)
            {
                // Obtener la dirección del dash
                Vector2 dashDir = InputManager.Instance.LastMovementVector;

                // Si no hay movimiento, no hacemos nada
                if (dashDir != Vector2.zero)
                {
                    // Calcular dirección opuesta
                    Vector2 oppositeDir = -dashDir.normalized;

                    // Obtener ángulo en grados
                    float angle = Mathf.Atan2(oppositeDir.y, oppositeDir.x) * Mathf.Rad2Deg;

                    // Rotar el sistema de partículas hacia la dirección opuesta al dash
                    Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

                    // Instanciar en posición del jugador, con rotación

                    dashParticles.transform.rotation = rotation;
                    dashParticles.Play();
                    

                }
            }
        }
    }



    #endregion
} // class Dash 
// namespace
