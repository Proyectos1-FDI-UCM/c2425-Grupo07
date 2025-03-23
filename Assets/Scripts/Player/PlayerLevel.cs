//---------------------------------------------------------
// Responsable de verificar si el jugador está con un bloque de nivel y que esté presionando la acción que se le hace referencia
// Liling Chen
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerLevel : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private InputActionReference Enter; //El input al que va a accionar las cosas 
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private Level _level; //Script del nivel al que va a entrar
    [SerializeField] private PauseMenuManager _menuManager;
    [SerializeField] private bool _enabledPause = false;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

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
        
    }

    /// <summary>
    /// Habilita la acción de entrada y llama al método OnEnterLevel 
    /// </summary>
    private void OnEnable()
    {
        _menuManager = GameObject.FindObjectOfType<PauseMenuManager>();
        Enter.action.Enable();
        Enter.action.performed += OnEnterLevel;
    }

    /// <summary>
    /// Deja de llamar al método OnEnterLevel y deshabilita la acción de entrada
    /// </summary>
    private void OnDisable()
    {
        Enter.action.performed -= OnEnterLevel;
        Enter.action.Disable();
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
    /// Cuando presiona el input reference, primero se mira si la booleana del menu de pausa esta activa, sino entonces se llama al método OnEnterlevel para entrar a la escena del juego,
    /// solo si _level tiene una referencia, de otra forma, entonces no se activaría
    /// </summary>
    /// <param name="context"></param>
    public void OnEnterLevel(InputAction.CallbackContext context)
    {
        _enabledPause = _menuManager.PauseActive();
        if (_level != null && !_enabledPause) 
        {
            _level.OnEnterLevel(context);
            Debug.Log("Elección del jugador abierto");
        }
    }

    /// <summary>
    /// Retorna el script del nivel interactuado para más tarde cambiar los datos desde el GameManager
    /// </summary>
    /// <returns>Retorna el nivel al que el jugador entra</returns>
    public Level GetLevel()
    {
        return _level;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)


    /// <summary>
    /// Verifica si el objeto con el que colisiona el jugador tiene el script _level, si lo tiene almacena la referencia
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Level level = collision.GetComponent<Level>();
        if (level != null)
        {
            _level = level;
        }
    }

    // Método llamado cuando el jugador sale de un trigger
    /// <summary>
    /// Verifica si el objeto con el que sale el jugador tiene el script _level, si lo tiene y esta escena se corresponde con la que tiene almacenada la referencia actual, esta se vacía
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        Level level = collision.GetComponent<Level>();
        if (level != null && _level == level)
        {
            _level = null;
        }
    }

    #endregion

} // class _playerLevel 
// namespace
