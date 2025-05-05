//---------------------------------------------------------
// Se programa el desplazamiento de los créditos
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

    [SerializeField] private GameObject CreditsGroup;
    [SerializeField] private Animator Animator;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private float _normalSpeed = 1f;
    private float _fastSpeed = 3f;
    private bool _isActive = false;
    [SerializeField]private InputManager _inputManager;

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
        if(Animator == null)
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

    private void HandleInput()
    {
        if (_inputManager.PickDropWasPressedThisFrame())
        {
            ScrollQuick();
        }
        else if (_inputManager.EnterWasPressedThisFrame())
        {
            ReturnToTitle();
        }
    }

    private void ScrollQuick()
    {
        _isActive = !_isActive;
        SetSpeed(_isActive ? _fastSpeed : _normalSpeed);
    }

    public void SetSpeed(float speed)
    {
        Animator.SetFloat("ScrollSpeed", speed);
    }

    private void OnCreditsFinished()
    {
        ReturnToTitle();
    }

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
