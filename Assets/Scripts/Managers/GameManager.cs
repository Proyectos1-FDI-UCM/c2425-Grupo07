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
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static GameManager _instance;
    [SerializeField] private GameObject _player; //GameObject del jugador en la escena
    [SerializeField] private PlayerLevel _playerLevel; //Script que contiene los datos del nivel al que va a entrar el jugador
    [SerializeField] private Level _level; //Para almacenar el nivel entrado
    [SerializeField] private string _levelName; //Para almacenar el nombre del nivel
    [SerializeField] private PlayerBool _playerBool; //Para almacenar el script del personaje elegido
    [SerializeField] private bool _isRack = false; //Booleana del personaje, true si es Rack, false si es Albert
    private LevelManager.Range _levelRange1; //Mejor rango obtenido del nivel principal
    private LevelManager.Range _levelRange2; //Mejor rango obtenido del nivel infinito
    [SerializeField] private Text _moneyText1; //Texto que muestra el dinero recopilado en el nivel prinicpal
    [SerializeField] private Text _moneyText2; //Texto que muestra el dinero recopilado en el nivel infinito
    [SerializeField] private Image _rankImage1; //Imagen que indica el mejor rango obtenido en el nivel prinicpal
    [SerializeField] private Image _rankImage2; //Imagen que indica el mejor rango obtenido en el nivel inifito
    [SerializeField] private int _moneyNumber1 = 0; //Entero que indica el dinero recopilado en el nivel principal
    [SerializeField] private int _moneyNumber2 = 0; //Entero que indica el dinero recopilado en el nivel infinito

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
        if (index == 3)
        {
            UpdateStats();
        }
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
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 1)
        {
            _levelRange1 = _range;
        }
        else
        {
            _levelRange2 = _range;
        }
    }

    /// <summary>
    /// GetRange devuelve el rango del nivel
    /// </summary>
    /// <returns></returns>
    public LevelManager.Range GetRange()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 1)
        {
            return _levelRange1;
        }
        else
        {
            return _levelRange2;
        }
    }

    /// <summary>
    /// SetMoney modifica _money según el dinero obtenido en la partida
    /// </summary>
    /// <param name="_money">Cantidad de dinero obtenido en la partida</param>
    public void SetMoney(int _money)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 1)
        {
            _moneyNumber1 = _money;
        }
        else
        {
            _moneyNumber2 = _money;
        }
    }

    /// <summary>
    /// GetMoney devuelve la cantidad de dinero obtenida en el nivel
    /// </summary>
    /// <returns></returns>
    public int GetMoney()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 1)
        {
            return _moneyNumber1;
        }
        else
        {
            return _moneyNumber2;
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

    private void UpdateStats()
    {
        if (_rankImage1 != null)
        {
            if (_levelRange1 == LevelManager.Range.S)
            {
                _rankImage1.color = Color.green;
            }
            else if (_levelRange1 == LevelManager.Range.A)
            {
                _rankImage1.color = Color.cyan;
            }
            else if (_levelRange1 == LevelManager.Range.B)
            {
                _rankImage1.color = Color.yellow;
            }
            else if (_levelRange1 == LevelManager.Range.F)
            {
                _rankImage1.color = Color.red;
            }
        }

        if (_rankImage2 != null)
        {
            if (_levelRange2 == LevelManager.Range.S)
            {
                _rankImage2.color = Color.green;
            }
            else if (_levelRange2 == LevelManager.Range.A)
            {
                _rankImage2.color = Color.cyan;
            }
            else if (_levelRange2 == LevelManager.Range.B)
            {
                _rankImage2.color = Color.yellow;
            }
            else if (_levelRange2 == LevelManager.Range.F)
            {
                _rankImage2.color = Color.red;
            }
        }

        if (_moneyText1 != null)
        {
            _moneyText1.text = _moneyNumber1.ToString();
        }

        if (_moneyText2 != null)
        {
            _moneyText2.text = _moneyNumber2.ToString();
        }
    }

    #endregion
} // class GameManager 
  // namespace