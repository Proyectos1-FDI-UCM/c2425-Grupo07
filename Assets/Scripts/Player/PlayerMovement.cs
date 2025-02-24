//---------------------------------------------------------
// Este script se centra en el movimiento principal del jugador. El jugador puede moverse libremente sin dash y sin colisiones. Contiene:
//Movimiento Continuo Libre
//Rotación dependiendo de la dirección
//El input correspondiente es el acordado
// Guillermo
// Clank&Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] int Velocity; // La velocidad máxima con la que se mueve el personaje en cada dirección.
    [SerializeField] int RotationSpeed; // La velocidad máxima con la que rota el personaje en cada dirección.
    [SerializeField] Vector2 MovementVector;


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    InputActionSettings MoveControls;
    Vector2 _translateMovement;
    Quaternion _targetRotation;
    private PlayerDash dash;

    /// <summary>
    /// Controlador de las acciones del Input. Es una instancia del asset de 
    /// InputAction que se puede configurar desde el editor y que está en
    /// la carpeta Settings
    /// </summary>
    private InputActionSettings _theController;
    

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    void Awake()
    {
        // Creamos el controlador del input y activamos los controles del jugador
        MoveControls = new InputActionSettings();
        MoveControls.Player.Enable();
        // Cacheamos la acción de movimiento
        InputAction movement = MoveControls.Player.Move;
        // Para el movimiento, actualizamos el vector de movimiento usando, si se deja de pulsar no se mueve el jugador
        movement.performed += ctx => MovementVector = ctx.ReadValue<Vector2>();
        movement.canceled += ctx => MovementVector = new Vector2(0,0);

    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //OnMove();
    }
    private void FixedUpdate()
    {
        OnMove();
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
    private void OnMove()//InputAction.CallbackContext context)
    {
        _translateMovement = MovementVector * Velocity * Time.deltaTime; // Indico el vector de movimiento en función del tiempo y la velocidad
        transform.Translate(_translateMovement, Space.World); // Muevo al personaje en el espacio del mundo
        
        if (_translateMovement != Vector2.zero) // Quiero que cambie de dirección solo cuando se mueve el personaje
        {
            _targetRotation = Quaternion.LookRotation(transform.forward, _translateMovement); // El Quaternion apunta a la dirección
            _targetRotation.Normalize();
            transform.rotation = _targetRotation; // El jugador gira a la dirección
        }
    }
    #endregion

} // class PlayerMovement 
// namespace
