//---------------------------------------------------------
// Se programa el aumento de la velocidad de desplazamiento de los créditos y también la posibilidad de omitirlos
// Liling Chen
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class CreditsScroll : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] private GameObject CreditsGroup; //Game Object que contiene todos los creditos
    [SerializeField] private Animator Animator;//Animator del Game Object
    [SerializeField] private float FastSpeed = 3f; //Velocidad de la animación cuando se acerera

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private float _normalSpeed = 1f; //Velocidad de la animación normal
    private bool _isActive = false; //Si esta activado o no el fast scroll
    private InputManager _inputManager; //Referencia al input manager

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
        if (Animator == null)
        {
            Animator = GetComponent<Animator>();
        }
        if(_inputManager == null)
        {
            _inputManager = InputManager.Instance;
        }
        SetSpeed(_normalSpeed);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        HandleInput();
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
    /// Dependiendo del input, si es enter, se va a la escena de título, si es la barra de espacio se aumenta 
    /// la velocidad, si se le pulsa otra vez se podrá volver a la velocidad normal
    /// </summary>
    private void HandleInput()
    {
        if (_inputManager.PickDropWasPressedThisFrame())
        {
            ScrollQuick();
        }
        else if (_inputManager.PauseWasPressedThisFrame())
        {
            ReturnToTitle();
        }
    }

    /// <summary>
    /// Llamado desde el HandleInput, activa y desactiva la velocidad de la animación
    /// </summary>
    private void ScrollQuick()
    {
        _isActive = !_isActive;
        SetSpeed(_isActive ? FastSpeed : _normalSpeed);
    }

    /// <summary>
    /// Determina la velocidad de la animación
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed)
    {
        Animator.SetFloat("ScrollSpeed", speed);
    }

    /// <summary>
    /// Carga la escena de título
    /// </summary>
    private void ReturnToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class CreditsScrow 
// namespace
