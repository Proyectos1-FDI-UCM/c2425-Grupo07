//---------------------------------------------------------
// Este script es el responsable de la habilidad de Dash del personaje
// Óliver García Aguado
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerDash : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] float DashSpeed; // Velocidad del dash
    [SerializeField] float DashDuration; // Tiempo que durará el dash
    [SerializeField] private InputActionReference DashActionReference; //El inputAction que va a realizar el Dash

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    // componentes de físicas y movimiento del jugador
    private Rigidbody2D _rb;
    private bool _isDashing = false; // Esta boleana se activa mientras que se está realizando un Dash


    #endregion
    // ---- ATRIBUTOS PÚBLICOS ----
    #region



    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Awake se llama para recoger el playerinput y detectar cuando está disponible.
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
    /// Start se llama para los componentes de físicas y movimiento
    /// </summary>
    void Start()
    {
       _rb = GetComponent<Rigidbody2D>();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    /// <summary>
    /// Cuando se pulsa el Input de dash pregunta si se puede usar y si es que sí, realiza el dash.
    /// El parámetro context es manejado por el input system de forma automática, se establece la fase del input a ser detectada desde la suscr
    /// </summary>
    /// <param name="context"></param>
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
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)


    /// <summary>
    /// Realiza el dash igualando la velocidad del rigidbody del juegador a una velocidad dada en la dirección que este este mirando y durante un tiempo establecido.
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartDash()
    {
        
        _rb.velocity = (Vector2)transform.up * DashSpeed;
        yield return new WaitForSeconds(DashDuration);
        _isDashing = false;
        Debug.Log("DASH RECARGADO");
    }

    // La suscripción del input al Dash
    private void OnEnable()
    {
        DashActionReference.action.performed += RequestDash;
        DashActionReference.action.Enable();

    }

    private void OnDisable()
    {
        DashActionReference.action.performed -= RequestDash;
        DashActionReference.action.Disable();
    }
    #endregion

} // class Dash 
// namespace
