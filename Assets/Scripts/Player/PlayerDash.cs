//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
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
    private PlayerMovement _pM;


    #endregion
    // ---- ATRIBUTOS PÚBLICOS ----
    #region

    public bool _isDashing = false; // Bool que se activará para que el dash se pueda usar de nuevo

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
    /// Awake se llama para los componentes de físicas y movimiento
    /// </summary>
    void Start()
    {
       _rb = GetComponent<Rigidbody2D>();
       _pM = GetComponent<PlayerMovement>();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    //Cuando se pulsa el Input de dash pregunta si se puede usar y si es que sí, realiza el dash
    public void RequestDash(InputAction.CallbackContext context)
    {
        if (!_isDashing && context.phase == InputActionPhase.Started)
        {
            Debug.Log("DASH ACTIVADO");
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
    // Realiza el dash empujando por física al jugador una dirección con una velocidad, esperando un tiempo para volverlo a realizar
    private IEnumerator StartDash()
    {
        _isDashing = true;
        _rb.velocity = (Vector2)transform.up * DashSpeed;
        yield return new WaitForSeconds(DashDuration);
        Debug.Log("DASH RECARGADO");
        _isDashing = false;
    }
    #endregion

} // class Dash 
// namespace
