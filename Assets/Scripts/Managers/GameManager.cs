//---------------------------------------------------------
// Contiene el componente GameManager
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;


/// <summary>
/// Componente responsable de la gestión global del juego. Es un singleton
/// que orquesta el funcionamiento general de la aplicación,
/// sirviendo de comunicación entre las escenas.
///
/// El GameManager ha de sobrevivir entre escenas por lo que hace uso del
/// DontDestroyOnLoad. En caso de usarlo, cada escena debería tener su propio
/// GameManager para evitar problemas al usarlo. Además, se debería producir
/// un intercambio de información entre los GameManager de distintas escenas.
/// Generalmente, esta información debería estar en un LevelManager o similar.
/// </summary>
public class GameManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] GameObject Rack;
    [SerializeField] GameObject Albert;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static GameManager _instance;
    private GameObject _player; //GameObject del jugador
    private PlayerLevel _playerLevel; //Script que contiene los datos del nivel al que va a entrar el jugador
    private Level _level; //Para almacenar el nivel entrado
    private string _levelName; //Para almacenar el nombre del nivel
    private PlayerBool _playerBool; //Para almacenar el script del personaje elegido
    private bool _isRack = false; //Booleana del personaje, true si es Rack, false si es Albert
    [SerializeField] private GameObject _spawnPlayer;
    
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    /// <summary>
    /// Método llamado en un momento temprano de la inicialización.
    /// En el momento de la carga, si ya hay otra instancia creada,
    /// nos destruimos (al GameObject completo)
    /// </summary>
    protected void Awake()
    {
        if (_instance != null)
        {
            // No somos la primera instancia. Se supone que somos un
            // GameManager de una escena que acaba de cargarse, pero
            // ya había otro en DontDestroyOnLoad que se ha registrado
            // como la única instancia.
            // Si es necesario, transferimos la configuración que es
            // dependiente de la escena. Esto permitirá al GameManager
            // real mantener su estado interno pero acceder a los elementos
            // de la escena particulares o bien olvidar los de la escena
            // previa de la que venimos para que sean efectivamente liberados.
            Debug.Log("Ya existe una instancia de GameManager. Destruyendo esta instancia.");
            TransferSceneState();

            // Y ahora nos destruímos del todo. DestroyImmediate y no Destroy para evitar
            // que se inicialicen el resto de componentes del GameObject para luego ser
            // destruídos. Esto es importante dependiendo de si hay o no más managers
            // en el GameObject.
            DestroyImmediate(this.gameObject);
        }
        else
        {
            // Somos el primer GameManager.
            // Queremos sobrevivir a cambios de escena.
            Debug.Log("Inicializando GameManager.");
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        } // if-else somos instancia nueva o no.
    }

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
    }

    /// <summary>
    /// Cuando se habilita el objeto en la escena, es decir, que se activa, se llama al OnSceneLoaded cada vez que se cargue una nueva escena
    /// </summary>
    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Lo mismo que la anterior, solo que en vez de activar el método de OnSceneLoaded, este detecta cuando el objeto
    /// se desativa al salir de escena y evita que se siga llamando a OnSceneLoaded
    /// </summary>
    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el GameManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Método que cambia la escena actual por la indicada en el parámetro.
    /// </summary>
    /// <param name="index">Índice de la escena (en el build settings)
    /// que se cargará.</param>
    public void ChangeScene(int index)
    {
        // Antes y después de la carga fuerza la recolección de basura, por eficiencia,
        // dado que se espera que la carga tarde un tiempo, y dado que tenemos al
        // usuario esperando podemos aprovechar para hacer limpieza y ahorrarnos algún
        // tirón en otro momento.
        // De Unity Configuration Tips: Memory, Audio, and Textures
        // https://software.intel.com/en-us/blogs/2015/02/05/fix-memory-audio-texture-issues-in-unity
        //
        // "Since Unity's Auto Garbage Collection is usually only called when the heap is full
        // or there is not a large enough freeblock, consider calling (System.GC..Collect) before
        // and after loading a level (or put it on a timer) or otherwise cleanup at transition times."
        //
        // En realidad... todo esto es algo antiguo por lo que lo mismo ya está resuelto)
        System.GC.Collect();
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        System.GC.Collect();
    } // ChangeScene

    /// <summary>
    /// Obtiene los datos del nivel al ser llamado, de aquí se obtiene: el nivel nombre del nivel y con ello la
    /// refencia de los datos de ese nivel que más tarde será sustituidos por los nuevos al ser completados
    /// </summary>
    public void GetData() 
    {
        _playerLevel = _player.GetComponent<PlayerLevel>();
        if (_playerLevel == null)
        {
            Debug.LogError("PlayerLevel no encontrado en el jugador.");
            return;
        }
        Debug.Log("Player Level obtenido");
        
        _level = _playerLevel.GetLevel();
        
        _levelName = _level.GetLevelName();
       
    }

    /// <summary>
    /// Obtiene del scritp del _playerBool el personaje elegido y es guardado en el GameManager
    /// </summary>
    public void GetPlayer()
    {
        if (_playerBool != null)
        {
            _isRack = _playerBool.PlayerSelection();
        }
    }

    //Carga la escena dependiendo de _levelName
    public void ChangeToLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_levelName);
    }

    //Devuelve la booleana _isRack al ser llamado
    public bool ReturnBool()
    {
        return _isRack;
    }

    /// <summary>
    /// Busca los componenes de _player, _playerLevel, _playerBool y _levelName para almacenarlos en el GameManager.
    /// _player para el GameObject del jugador
    /// _playerLevel para obtener de aquí el nombre del nivel en el principio
    /// _playerBool para obtener la elección del personaje, este escript solo esta en la escena de MenuLevelSelection
    /// Si hay nombre guardado en el string de _levelName, se llama al siguiente método FindLevelByName
    /// </summary>
    public void FirstFindPlayerComponents()
    {
        _player = GameObject.FindWithTag("Player"); //Encuentra al jugador en cuando inicializa en escena junto con sus componentes
        if (_player == null)
        {
            Debug.LogError("Player no encontrado");
            return;
        }
        _playerLevel = _player.GetComponent<PlayerLevel>();

        _playerBool = FindObjectOfType<PlayerBool>();

        if (_levelName != null)
        {
            FindLevelByName(_levelName);
        }
        else Debug.Log("No hay nivel asignado");
    }

    /// <summary>
    /// Inicializa el personaje y dependiendo de la booleana de _isRack, se le asigna a playerPrefab el prefab del personaje
    /// </summary>
    public void SpawnPlayer()
    {
        GameObject playerPrefab = _isRack ? Rack : Albert;

        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, _spawnPlayer.transform.position, Quaternion.identity);
            Debug.Log("Player SPAWNS");
        }
        else Debug.Log("No hay prefab del player");
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        
    }
    /// <summary>
    /// Método privado que crea una array de componentes con el script de _level (allLevels) y busca en él el dato del nombre de nivel para asignarlo a _level
    /// </summary>
    /// <param name="levelName"></param>
    private void FindLevelByName(string levelName)
    {
        Level[] allLevels = FindObjectsOfType<Level>();
        if (allLevels.Length == 0) { Debug.Log("No hay nivel en escena, está en juego");
            return;
        }
        else _level = allLevels.FirstOrDefault(level => level.GetLevelName() == levelName);
    }

    /// <summary>
    /// Tiene dos funciones este método, uno es inicializar el personaje en la escena cuando se trata de MenuLevelSelection
    /// , por default es Albert porque depende de la booleana de _isRack, en este caso es a falso y también, llama al método
    /// de FirstFindPlayerComponents para obtener la referencia de los componenetes descritos en este método
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Escena cargada: " + scene.name);
        _spawnPlayer = GameObject.FindWithTag("Spawn");
        if (_spawnPlayer == null)
        {
            Debug.LogError("No hay spawn");
        }
        if (scene.name == "MenuLevelSelection")
        {
            SpawnPlayer();
            Debug.Log("MenuLevelSelection");
        }
        FirstFindPlayerComponents();
    }

    private void TransferSceneState()
    {
        // De momento no hay que transferir ningún estado
        // entre escenas
    }

    #endregion
} // class GameManager 
// namespace