//---------------------------------------------------------
// Este script se centra en el movimiento principal del jugador. El jugador puede moverse libremente sin dash y sin colisiones. Contiene:
// Movimiento Continuo Libre
// Rotación dependiendo de la dirección
// El input correspondiente es el acordado
// Guillermo
// Clank&Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// 
/// Este script se encarga de recoger un input de jugador dedicado al movimiento y mueve al personaje en las 
/// direcciones introducidas por el jugador. Además se gira al jugador acourde al input del jugador 
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
    [SerializeField] float CurrentVelocity; // La velocidad actual con la que se mueve el personaje en cada dirección.
    [SerializeField] float MaxVelocity;// La velocidad máxima con la que se mueve el personaje en cada dirección.
    [SerializeField] int RotationSpeed; // La velocidad máxima con la que rota el personaje en cada dirección.
    [SerializeField] Vector2 MovementVector;// El vector de movimiento que sigue el personaje
    [SerializeField] public Vector2 LastMovementVector; // La última posición que siguió el personaje


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
    private InputActionSettings _moveControls; // El input en el que se rige el movimiento
    private Vector2 _translateMovement; // El movimiento que realizará el personaje con su movimiento
    private Quaternion _targetRotation; // La rotación que seguirá el personaje 

    /// <summary>
    /// Controlador de las acciones del Input. Es una instancia del asset de 
    /// InputAction que se puede configurar desde el editor y que está en
    /// la carpeta Settings
    /// </summary>
    private InputActionSettings _theController;

    //atributo aniamtior para las animaciones
    private Animator _animator;

    ////guarda el valor de la ultima posicion del jugador para el idle
    //private Vector2 _lastMoveDir; 

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    // Creamos el controlador del input y activamos los controles del jugador
    void Awake()
    {
        _moveControls = new InputActionSettings();
        _moveControls.Player.Enable();
        // Cacheamos la acción de movimiento
        InputAction movement = _moveControls.Player.Move;
        // Para el movimiento, actualizamos el vector de movimiento usando, si se deja de pulsar no se mueve el jugador
        movement.performed += ctx => MovementVector = ctx.ReadValue<Vector2>();
        movement.canceled += ctx => MovementVector = new Vector2(0, 0);

        _animator = GetComponentInParent<Animator>();
    }
    /// <summary>
    /// Start asigna la velocidad actual con la máxima
    /// </summary>
    void Start()
    {
        CurrentVelocity = MaxVelocity;
    }

    /// <summary>
    /// FixedUpdate se encarga de mover siempre al personaje teniendo en cuenta antes las colisiones
    /// </summary>
    private void FixedUpdate()
    {
        OnMove();
        Animate();
    }
    #endregion


    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    ///<summary>
    ///Movimiento libre con una velocidad máxima que sólo podrá ser 
    ///superada momentáneamente con el dash.
    ///La rotación del personaje y su vista irán determinados a la dirección en la que este se mueva
    ///</summary>
    private void OnMove()
    {
        _translateMovement = MovementVector * CurrentVelocity * Time.deltaTime; // Indico el vector de movimiento en función del tiempo y la velocidad
        transform.Translate(_translateMovement, Space.World); // Muevo al personaje en el espacio del mundo

        if (_translateMovement != Vector2.zero) // Quiero que cambie de dirección solo cuando se mueve el personaje
        {
            LastMovementVector = _translateMovement;
            //_targetRotation = Quaternion.LookRotation(transform.forward, _translateMovement); // El Quaternion apunta a la dirección
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, RotationSpeed * Time.deltaTime); // El jugador gira a la dirección
        }
    }

    private void Animate()
    {
        _animator.SetFloat("WalkX", MovementVector.x);
        _animator.SetFloat("WalkY", MovementVector.y);
        _animator.SetFloat("MoveMagnitude", MovementVector.magnitude);
        _animator.SetFloat("LastMoveX", LastMovementVector.x);
        _animator.SetFloat("LastMoveY", LastMovementVector.y);
    }

    public Vector2 GetLastMove()
    {
        return LastMovementVector;
    }

    #endregion

} // class PlayerMovement 
// namespace
