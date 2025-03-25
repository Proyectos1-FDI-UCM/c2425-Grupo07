//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Alicia Sarahi Sanchez Varela
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerWelder : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private WelderScript WelderScript; //Objeto para llamar al script de Welder
    [SerializeField] private InputActionReference InteractActionReference; //Input que permite realizar la accion

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private PlayerVision _playerVision;  // sirve para llamar luego al script de PlayerVision
    private PlayerMovement _playerMovement; //sirve para llamar luego al script de PlayerMovement


    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se encarga de encontrar la Soldadora y llamar a los scrpits de los atributos correspondientes.
    /// </summary>
    void Start()
    {
        if (GameObject.FindWithTag("Soldadora") != null)
        {
            WelderScript = GameObject.FindWithTag("Soldadora").GetComponent<WelderScript>();
        }
        _playerVision = GetComponent<PlayerVision>();
        _playerMovement = GetComponent<PlayerMovement>();
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

    /// <summary>
    /// Indica el tipo de estado que el input va a realizar.
    /// </summary>
    void Awake()
    {
            InteractActionReference.action.performed += ctx => TurningWelder();
            InteractActionReference.action.canceled += ctx => StopingWelder();
            InteractActionReference.action.Enable();
    }

    /// <summary>
    /// Indica que se puede utilizar la soldadora, y llama a la función para activar la soldadora.
    /// </summary>
    private void TurningWelder() 
    { 
        if (_playerVision.GetActualMesa() != null && _playerVision.GetActualMesa().CompareTag("Soldadora"))
        {
            _playerMovement.enabled = false;
            _playerVision.enabled = false;
            WelderScript.TurnOnWelder();
        }
    }

    /// <summary>
    /// indica que el jugador está en movimiento y llama a la función correspondiente para detener la soldadora.
    /// </summary>
    private void StopingWelder() 
    {
        if (_playerVision.GetActualMesa() != null && _playerVision.GetActualMesa().CompareTag("Soldadora"))
        {
            _playerMovement.enabled = true;
            _playerVision.enabled = true;
            WelderScript.TurnOffWelder();
        }
             
    }



    #endregion

} // class PlayerWelder 
// namespace
