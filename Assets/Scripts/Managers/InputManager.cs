//---------------------------------------------------------
// Contiene el componente de InputManager
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Manager para la gestión del Input. Se encarga de centralizar la gestión
/// de los controles del juego. Es un singleton que sobrevive entre
/// escenas.
/// La configuración de qué controles realizan qué acciones se hace a través
/// del asset llamado InputActionSettings que está en la carpeta Settings.
/// 
/// A modo de ejemplo, este InputManager tiene métodos para consultar
/// el estado de dos acciones:
/// - Move: Permite acceder a un Vector2D llamado MovementVector que representa
/// el estado de la acción Move (que se puede realizar con el joystick izquierdo
/// del gamepad, con los cursores...)
/// - Fire: Se proporcionan 3 métodos (FireIsPressed, FireWasPressedThisFrame
/// y FireWasReleasedThisFrame) para conocer el estado de la acción Fire (que se
/// puede realizar con la tecla Space, con el botón Sur del gamepad...)
///
/// Dependiendo de los botones que se quieran añadir, será necesario ampliar este
/// InputManager. Para ello:
/// - Revisar lo que se hace en Init para crear nuevas acciones
/// - Añadir nuevos métodos para acceder al estado que estemos interesados
///  
/// </summary>
public class InputManager : MonoBehaviour
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

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static InputManager _instance;

    /// <summary>
    /// Controlador de las acciones del Input. Es una instancia del asset de 
    /// InputAction que se puede configurar desde el editor y que está en
    /// la carpeta Settings
    /// </summary>
    private InputActionSettings _theController;
    
    /// <summary>
    /// Acción para Dash, Interactuarm Recoger/Dejar, entrar a un nivel y abrir el menú de pasa. 
    /// Si hubieran más botones tendremos que crear más
    /// </summary>
    private InputAction _dash, _interact, _pickOrDrop, _enterLevel, _openPauseMenu;


    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    /// <summary>
    /// Método llamado en un momento temprano de la inicialización.
    /// 
    /// En el momento de la carga, si ya hay otra instancia creada,
    /// nos destruimos (al GameObject completo)
    /// </summary>
    protected void Awake()
    {
        if (_instance != null)
        {
            // No somos la primera instancia. Se supone que somos un
            // InputManager de una escena que acaba de cargarse, pero
            // ya había otro en DontDestroyOnLoad que se ha registrado
            // como la única instancia.
            // Nos destruímos. DestroyImmediate y no Destroy para evitar
            // que se inicialicen el resto de componentes del GameObject para luego ser
            // destruídos. Esto es importante dependiendo de si hay o no más managers
            // en el GameObject.
            //DestroyImmediate(this.gameObject);
        }
        else
        {
            // Somos el primer InputManager.
            // NO Queremos sobrevivir a cambios de escena.
            _instance = this;
            //DontDestroyOnLoad(this.gameObject);
            Init();
        }

    } // Awake

    /// <summary>
    /// Método llamado cuando se destruye el componente.
    /// </summary>
    protected void OnDestroy()
    {
        if (this == _instance)
        {
            // Éramos la instancia de verdad, no un clon.
            _instance = null;
        } // if somos la instancia principal
    } // OnDestroy

    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static InputManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    } // Instance

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el GameManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>True si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Propiedad para acceder al vector de movimiento.
    /// Según está configurado el InputActionController,
    /// es un vector normalizado 
    /// </summary>
    public Vector2 MovementVector { get; private set; }
    public Vector2 LastMovementVector { get; private set; }

    /// <summary>
    /// Método para saber si el botón de interactuar (Interact) está pulsado
    /// Devolverá true en todos los frames en los que se mantenga pulsado
    /// <returns>True, si el botón está pulsado</returns>
    /// </summary>
    public bool InteractIsPressed()
    {
        return _interact.IsPressed();
    }

    /// <summary>
    /// Método para saber si el botón de impulso (Dash) se ha pulsado en este frame
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool DashWasPressedThisFrame()
    {
        return _dash.WasPressedThisFrame();
    }
    /// <summary>
    /// Método para saber si el botón de disparo (Fire) se ha pulsado en este frame
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool InteractWasPressedThisFrame()
    {
        return _interact.WasPressedThisFrame();
    }
    /// <summary>
    /// Método para saber si el botón de recoger/dejar (pickOrDrop) está pulsado
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </summary>
    public bool PickDropWasPressedThisFrame()
    {
        return _pickOrDrop.WasPressedThisFrame();
    }
    /// <summary>
    /// Método para saber si el botón de pausa (Pause) se ha pulsado en este frame
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool PauseWasPressedThisFrame()
    {
        return _openPauseMenu.WasPressedThisFrame();
    }
    /// <summary>
    /// Método para saber si el botón de entrar a un nivel (Enter) se ha pulsado en este frame
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool EnterWasPressedThisFrame()
    {
        return _enterLevel.WasPressedThisFrame();
    }
    /// <summary>
    /// Método para saber si el botón de interactuar (Interact) ha dejado de pulsarse
    /// durante este frame
    /// <returns>Devuelve true, si el botón se ha dejado de pulsar en
    /// este frame; y false, en otro caso.
    /// </returns>
    /// </summary>
    public bool InteractWasReleasedThisFrame()
    {
        return _interact.WasReleasedThisFrame();
    }

    public void EnableActionMap(string TypeActionMap)
    {
        if (TypeActionMap == "Player")
        {
            _theController.Player.Enable();
            _theController.UI.Disable();
        }
        else
        {
            _theController.Player.Disable();
            _theController.UI.Enable();
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        // Creamos el controlador del input y activamos los controles del jugador
        _theController = new InputActionSettings();
        _theController.Player.Enable();

        // Cacheamos la acción de movimiento
        InputAction movement = _theController.Player.Move;
        // Para el movimiento, actualizamos el vector de movimiento usando
        // el método OnMove
        movement.performed += OnMove;
        movement.canceled += OnMove;

        // Para el dash solo cacheamos la acción de impulsarse.
        // El estado lo consultaremos a través de los métodos públicos que 
        // tenemos (DashWasPressedThisFrame)
        _dash = _theController.Player.Dash;
        // Para el enter solo cacheamos la acción de entrar a un nivel.
        // El estado lo consultaremos a través de los métodos públicos que 
        // tenemos (EnterWasPressedThisFrame)
        _enterLevel = _theController.Player.EnterLevel;
        // Para interact solo cacheamos la acción de interactuar.
        // El estado lo consultaremos a través de los métodos públicos que 
        // tenemos (InteractIsPressed, InteractWasPressedThisFrame y InteractWasReleasedThisFrame)
        _interact = _theController.Player.Interact;
        // Para el pickOrDrop solo cacheamos la acción de recoger y dejar.
        // El estado lo consultaremos a través de los métodos públicos que 
        // tenemos (PickDropWasPressedThisFrame)
        _pickOrDrop = _theController.Player.PickOrDrop;
        // Para el pause solo cacheamos la acción de pausar el juego.
        // El estado lo consultaremos a través de los métodos públicos que 
        // tenemos (PauseWasPressedThisFrame)
        _openPauseMenu = _theController.Player.OpenPauseMenu;
    }

    /// <summary>
    /// Método que es llamado por el controlador de input cuando se producen
    /// eventos de movimiento (relacionados con la acción Move)
    /// </summary>
    /// <param name="context">Información sobre el evento de movimiento</param>
    private void OnMove(InputAction.CallbackContext context)
    {
        MovementVector = context.ReadValue<Vector2>();
        if (MovementVector != Vector2.zero)
        {
            LastMovementVector = MovementVector;
        }
    }

    #endregion
} // class InputManager 
// namespace