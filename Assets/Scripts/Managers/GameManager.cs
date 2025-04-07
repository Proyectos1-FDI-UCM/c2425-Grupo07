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
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;


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

    /// <summary>
    /// GameObject del jugador en la escena
    /// </summary>
    [SerializeField] private GameObject _player;

    /// <summary>
    /// Script que contiene los datos del nivel al que va a entrar el jugador
    /// </summary>
    [SerializeField] private PlayerLevel _playerLevel;

    /// <summary>
    /// Para almacenar el nivel entrado
    /// </summary>
    [SerializeField] private Level _level;

    /// <summary>
    /// Para almacenar el nombre del nivel
    /// </summary>
    [SerializeField] private string _levelName;

    /// <summary>
    /// Para almacenar el script del personaje elegido
    /// </summary>
    [SerializeField] private PlayerBool _playerBool;

    /// <summary>
    /// Booleana del personaje, true si es Rack, false si es Albert
    /// </summary>
    [SerializeField] private bool _isRack = false;

    /// <summary>
    /// Texto que muestra el dinero recopilado en el nivel prinicpal
    /// </summary>
    [SerializeField] private Text _moneyTextPrincipal;

    /// <summary>
    /// Texto que muestra el dinero recopilado en el nivel infinito
    /// </summary>
    [SerializeField] private Text _moneyTextInfinito;

    /// <summary>
    /// Imagen que indica el mejor rango obtenido en el nivel prinicpal
    /// </summary>
    [SerializeField] private Image _rankImagePrincipal;

    /// <summary>
    /// Imagen que indica el mejor rango obtenido en el nivel inifito
    /// </summary>
    [SerializeField] private Image _rankImageInfinito;

    /// <summary>
    /// Entero que indica el dinero recopilado en el nivel principal
    /// </summary>
    [SerializeField] private int _moneyNumberPrincipal = 0;

    /// <summary>
    /// Entero que indica el dinero recopilado en el nivel infinito
    /// </summary>
    [SerializeField] private int _moneyNumberInfinito = 0;

    /// <summary>
    /// Tiempo del nivel principal
    /// </summary>
    [SerializeField] private Text _timeTextPrincipal;

    /// <summary>
    /// Tiempo del nivel infinito
    /// </summary>
    [SerializeField] private Text _timeTextInfinito;

    /// <summary>
    /// Array de referencias a Level
    /// </summary>
    [SerializeField] private Level[] _levels;

    /// <summary>
    /// Comprueba si se han actualizado las stats de MenuLevelSelection
    /// </summary>
    [SerializeField] private bool _statsUpdated = false;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static GameManager _instance;
    
    /// <summary>
    /// Mejor rango obtenido del nivel principal
    /// </summary>
    private LevelManager.Range _levelRangePrincipal;

    /// <summary>
    /// Mejor rango obtenido del nivel infinito
    /// </summary>
    private LevelManager.Range _levelRangeInfinito;

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

    protected void Update()
    {
        /*if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MenuLevelSelection" &&
            FindAnyObjectByType<Level>() == null && _rankImage1 == null && _rankImage2 == null && _moneyText1 == null && _moneyText2 == null)
        {
            _levels[0] = FindObjectsOfType<Level>()[0];
            _levels[1] = FindObjectsOfType<Level>()[1];

            _rankImage1 = _levels[0].GetRank();
            _rankImage2 = _levels[1].GetRank();

            _moneyText1 = _levels[0].GetMoney();
            _moneyText2 = _levels[1].GetMoney();
        }
        else
        {
            Debug.Log("No se han podido cargar las referencias");
        }*/

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MenuLevelSelection")
        {
            _statsUpdated = false;
        }
        else if (!_statsUpdated)
        {
            UpdateStats();
            _statsUpdated = true;
        }

        /*if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MenuLevelSelection" && !_statsUpdated)
        {
            UpdateStats();
            _statsUpdated = true;
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MenuLevelSelection")
        {
            _statsUpdated = false;
        }*/
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
        // and after loading a LevelName (or put it on a timer) or otherwise cleanup at transition times."
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

    public void SetLevelData(Level level)
    {
        _level = level;
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

    public void SetPlayer(GameObject player)
    {
        _player = player;
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
        if (_player == null)
        {
            Debug.Log("Player no encontrado");
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
    /// SetRange modifica _levelRange según el rango que ha obtenido en la partida
    /// </summary>
    /// <param name="_range">Rango obtenido en la partida</param>
    public void SetRange(LevelManager.Range _range)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "NivelPrincipal")
        {
            _levelRangePrincipal = _range;
        }
        else
        {
            _levelRangeInfinito = _range;
        }
    }

    /// <summary>
    /// GetRange devuelve el rango del nivel
    /// </summary>
    /// <returns></returns>
    public LevelManager.Range GetRange()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "NivelPrincipal")
        {
            return _levelRangePrincipal;
        }
        else
        {
            return _levelRangeInfinito;
        }
    }

    /// <summary>
    /// SetMoney modifica _money según el dinero obtenido en la partida
    /// </summary>
    /// <param name="_money">Cantidad de dinero obtenido en la partida</param>
    public void SetMoney(int _money)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "NivelPrincipal")
        {
            _moneyNumberPrincipal = _money;
        }
        else
        {
            _moneyNumberInfinito = _money;
        }
    }

    /// <summary>
    /// GetMoney devuelve la cantidad de dinero obtenida en el nivel
    /// </summary>
    /// <returns></returns>
    public int GetMoney()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "NivelPrincipal")
        {
            return _moneyNumberPrincipal;
        }
        else
        {
            return _moneyNumberInfinito;
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
    }
    /// <summary>
    /// Método privado que crea una array de componentes con el script de _level (allLevels) y busca en él el dato del nombre de nivel para asignarlo a _level
    /// </summary>
    /// <param name="levelName"></param>
    private void FindLevelByName(string levelName)
    {
        Level[] allLevels = FindObjectsOfType<Level>();
        if (allLevels.Length == 0)
        {
            Debug.Log("No hay nivel en escena, está en juego");
            return;
        }
        else _level = allLevels.FirstOrDefault(level => level.GetLevelName() == levelName);
    }

    private void TransferSceneState()
    {
        // De momento no hay que transferir ningún estado
        // entre escenas
    }

    public void UpdateStats()
    {
        _levels = FindObjectsOfType<Level>();

        if (_levels != null)
        {
            if (_levels[0].name == "MainLevel")
            {
                _levels[0] = FindObjectsOfType<Level>()[1];
                _levels[1] = FindObjectsOfType<Level>()[0];
            }
            else
            {
                _levels[0] = FindObjectsOfType<Level>()[0];
                _levels[1] = FindObjectsOfType<Level>()[1];
            }
        }

        _rankImagePrincipal = _levels[1].GetRank();
        _rankImageInfinito = _levels[0].GetRank();

        _moneyTextPrincipal = _levels[1].GetMoney();
        _moneyTextInfinito = _levels[0].GetMoney();

        if (_rankImagePrincipal != null && _levelName == "NivelPrincipal")
        {
            if (_levelRangePrincipal == LevelManager.Range.S)
            {
                _rankImagePrincipal.color = Color.green;
            }
            else if (_levelRangePrincipal == LevelManager.Range.A)
            {
                _rankImagePrincipal.color = Color.cyan;
            }
            else if (_levelRangePrincipal == LevelManager.Range.B)
            {
                _rankImagePrincipal.color = Color.yellow;
            }
            else if (_levelRangePrincipal == LevelManager.Range.C)
            {
                _rankImagePrincipal.color = Color.yellow;
            }
            else if (_levelRangePrincipal == LevelManager.Range.D)
            {
                _rankImagePrincipal.color = Color.yellow;
            }
            else if (_levelRangePrincipal == LevelManager.Range.E)
            {
                _rankImagePrincipal.color = Color.yellow;
            }
            else
            {
                _rankImagePrincipal.color = Color.red;
            }
        }

        else if (_rankImageInfinito != null && _levelName == "NivelInfinito")
        {
            if (_levelRangeInfinito == LevelManager.Range.S)
            {
                _rankImageInfinito.color = Color.green;
            }
            else if (_levelRangeInfinito == LevelManager.Range.A)
            {
                _rankImageInfinito.color = Color.cyan;
            }
            else if (_levelRangeInfinito == LevelManager.Range.B)
            {
                _rankImageInfinito.color = Color.yellow;
            }
            else if (_levelRangeInfinito == LevelManager.Range.F)
            {
                _rankImageInfinito.color = Color.red;
            }
        }

        if (_moneyTextPrincipal != null && _levelName == "NivelPrincipal")
        {
            _moneyTextPrincipal.text = _moneyNumberPrincipal.ToString();
        }

        else if (_moneyTextInfinito != null && _levelName == "NivelInfinito")
        {
            _moneyTextInfinito.text = _moneyNumberInfinito.ToString();
        }
    }

    #endregion
} // class GameManager 
  // namespace