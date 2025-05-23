//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Alicia Sarahi Sánchez Varela
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// La clase PlayerAnimtor se encarga principalmente de definir los parametros usados en el Animator del editor que hacen posibles 
/// las animaciones de Idle, Caminar, Dash y de Trabajar/usar estaciones de trabajo.
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

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private Animator _animator; //atributo PressAnimator para las animaciones.
    private PlayerVision _playerVision;// sirve para llaamar al playerVision scrpit.
    private PlayerDash _playerDash; // sirve para llmar al playerDash scrpit.
    private PlayerMovement _playerMovement;// sirve para llamar al playerMovemnt script. 
    private bool _picked = false; // indica si un objecto está siendo reocgido o no.
    private bool _working;// indica si el jugador está realizando una acción o no.
    private Vector2 _movement; // guarda el movimiento del jugador.
    private Vector2 _lastMove; //guarda la última posición del jugador.

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// cada atributo inicializa su script correspondiente.
    /// </summary>
    void Start()
    {
        _animator = GetComponentInParent<Animator>();
        _playerVision = GetComponent<PlayerVision>();
        _playerDash = GetComponent<PlayerDash>();
        _playerMovement = GetComponent<PlayerMovement>();
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// Comprueba si el jugador ha pulsado el botón de interactuar para 
    /// reproducir la animación acorde
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.InteractWasPressedThisFrame())
        {
            SetBoolTrue();
        }
        else if (InputManager.Instance.InteractWasReleasedThisFrame())
        {
            SetBoolFalse();
        }
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
    /// La función animate se encarga de llevar acabo todo el proceso de animación, se han dado el valor correspondiente a las 
    /// variable para que su escritura sea más fácil, se definen los parámetros para ser usados en el PressAnimator del editor y se definen
    /// las condiciones para cada estado del personaje.
    /// </summary>
    public void Animate()
    {
        if (_playerVision != null)
        {
            _picked = _playerVision.IsBeingPicked();

            _animator.SetBool("Picked", _picked);

            _movement = InputManager.Instance.MovementVector;
            _lastMove = InputManager.Instance.LastMovementVector;
            _animator.SetFloat("WalkX", _movement.x);
            _animator.SetFloat("WalkY", _movement.y);
            _animator.SetFloat("MoveMagnitude", _movement.magnitude);
            _animator.SetFloat("LastMoveX", _lastMove.x);
            _animator.SetFloat("LastMoveY", _lastMove.y);
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
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Se activa la booleana de working para enviarla a las funciones de Input y así el correcto funcionamiento del inputAction.
    /// </summary>
    private void SetBoolTrue()
    {
        if (_movement == Vector2.zero)
        {
            Debug.Log("Trabajando");
            _working = true;
        }
    }

    /// <summary>
    /// Se desactiva la booleana de working para enviarla a las funciones de Input y así el correcto funcionamiento del inputAction.
    /// </summary>
    private void SetBoolFalse()
    {
        Debug.Log("No Trabajando");
        _working = false;
    }

    #endregion

} // class PlayerAnimation 
// namespace
