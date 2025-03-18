//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
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
public class PlayerAnimation : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] private InputActionReference InteractActionReference;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private Animator _animator; //atributo animator para las animaciones
    private PlayerVision _playerVision;//
    private PlayerDash _playerDash;
    private PlayerMovement _playerMovement;
    private bool _picked;
    private bool _working;
    private Vector2 _movement; 
    private Vector2 _lastMove;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _animator = GetComponentInParent<Animator>();
        _playerVision = GetComponent<PlayerVision>();
        _playerDash = GetComponent<PlayerDash>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        InteractActionReference.action.canceled += SetBoolFalse;
        InteractActionReference.action.performed += SetBoolTrue;
        InteractActionReference.action.Enable();
    }

    private void OnDisable()
    {
        InteractActionReference.action.performed -= SetBoolTrue;
        InteractActionReference.action.canceled -= SetBoolFalse;
        InteractActionReference.action.Disable();
    }

    //void Awake()
    //{
    //    InteractActionReference.action.performed += ctx => SetBool(true);
    //    InteractActionReference.action.canceled += ctx => SetBool(false);
    //    InteractActionReference.action.Enable();
    //}

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    public void Animate()
    {
        _picked = _playerVision.IsBeingPicked();
        _movement = _playerMovement.GetMovement();
        _lastMove = _playerMovement.GetLastMove();

        _animator.SetFloat("WalkX", _movement.x);
        _animator.SetFloat("WalkY", _movement.y);
        _animator.SetFloat("MoveMagnitude", _movement.magnitude);
        _animator.SetFloat("LastMoveX", _lastMove.x);
        _animator.SetFloat("LastMoveY", _lastMove.y);
        _animator.SetBool("Picked", _picked);
        _animator.SetBool("CanUse", _working);

        //No Activo, cambia entre el estado de Idle y Walk
        if (_movement != Vector2.zero)
        {
            _animator.SetFloat("BlendNA", 1f); 
            _working = false;
        }
        else _animator.SetFloat("BlendNA", 0f);

        //dash no activo 
        if (_movement == Vector2.zero && _playerDash.IsDashing())
        {
            _animator.SetFloat("BlendNA", 2f);
        }

        //Activo, cambia entre IdleActivo y WalkActivo 
        if (_picked && _movement != Vector2.zero)
        {
            _animator.SetFloat("Blend", 1f);
        }
        else if (_picked && _movement == Vector2.zero)
        {
            _animator.SetFloat("Blend", 0f);
        }
        //dash activo 
        if (_movement == Vector2.zero && _picked && _playerDash.IsDashing())
        {
            _animator.SetFloat("Blend", 2f);
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    private void SetBoolTrue(InputAction.CallbackContext context)
    {
        if (_movement == Vector2.zero)
        {
            _working = true;
        }
    }

    private void SetBoolFalse(InputAction.CallbackContext context)
    {
        _working = false;
    }

    #endregion

} // class PlayerAnimation 
// namespace
