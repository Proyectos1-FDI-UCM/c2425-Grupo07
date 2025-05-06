//---------------------------------------------------------
// Este script se centra en el movimiento principal del jugador. El jugador puede moverse libremente sin dash y sin colisiones. Contiene:
// Movimiento Continuo Libre
// Rotación dependiendo de la dirección
// El input correspondiente es el acordado
// Guillermo
// Clank&Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
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
    private Vector2 _translateMovement; // El movimiento que realizará el personaje con su movimiento
    private Quaternion _targetRotation; // La rotación que seguirá el personaje 
    private Rigidbody2D _rigidBody; // La rotación que seguirá el personaje 
    /// <summary>
    /// Controlador de las acciones del Input. Es una instancia del asset de 
    /// InputAction que se puede configurar desde el editor y que está en
    /// la carpeta Settings
    /// </summary>
    private PlayerAnimation _playerAnimation;
    private PlayerDash _playerDash;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 


    /// <summary>
    /// Start asigna la velocidad actual con la máxima
    /// </summary>
    void Start()
    {
        CurrentVelocity = MaxVelocity;
        _playerAnimation = GetComponent<PlayerAnimation>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerDash = GetComponent<PlayerDash>();
    }

    /// <summary>
    /// FixedUpdate se encarga de mover siempre al personaje teniendo en cuenta antes las colisiones
    /// </summary>
    private void FixedUpdate()
    {
        OnMove();
       _playerAnimation.Animate();
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
        _translateMovement = InputManager.Instance.MovementVector * CurrentVelocity; // Indico el vector de movimiento en función de la dirección y la velocidad
        if (!_playerDash.IsDashing()) // Esta condicional busca ahorrar algo de recursos por el hecho de no tener que recoger
        //la velocidad de dash en todo momento. Si resulta que esto no es eficiente, lo cambio posteriormente.
        {
            _rigidBody.velocity = _translateMovement;
        }
        else 
        {
            _rigidBody.velocity = _translateMovement + _playerDash.GetDashVelocity();
        }
        //_rigidBody.AddForce(_translateMovement, ForceMode2D.Force); // Muevo al personaje en el espacio del mundo
        //transform.Translate(_translateMovement, Space.World); Anteriormente
    }

    #endregion

    // ---- METODOS PUBLICOS ----
    #region Métodos Públicos

    #endregion

} // class PlayerMovement 
// namespace
