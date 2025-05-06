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
using TMPro;


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
    [SerializeField] private TextMeshProUGUI[] _moneyTextLevel;

    /// <summary>
    /// Texto que indica el mejor rango obtenido en el nivel
    /// </summary>
    [SerializeField] private TextMeshProUGUI[] _rankTextLevel;

    /// <summary>
    /// Mejor rango obtenido del nivel
    /// </summary>
    private LevelManager.Range[] _levelsRange;

    /// <summary>
    /// Entero que indica el dinero recopilado en el nivel
    /// </summary>
    [SerializeField] private int[] _moneyNumberlevel;


    /// <summary>
    /// Tiempo del nivel infinito aguantado
    /// </summary>
    [SerializeField] private Text _timeTextInfinito;

    /// <summary>
    /// Array de referencias a Level
    /// </summary>
    [SerializeField] private Level[] _levels;

    /// <summary>
    /// Comprueba si se han actualizado las stats de MenuLevelSelection
    /// </summary>
    [SerializeField] private bool _statsUpdated;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static GameManager _instance;
    [SerializeField]private bool _firstTime = false;
    private PauseMenuManager _pauseMenu;
    private IndicatorChange _indicatorChange;
    private Button _tutorial;
    private bool IsDev = false;
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
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MenuLevelSelection")
        {
            _statsUpdated = false;
        }
        else if (!_statsUpdated)
        {
            UpdateStats();
            _statsUpdated = true;
        }
        if (InputManager.Instance.DevModeIsPressed() && InputManager.Instance.InteractIsPressed() && !IsDev)
        {
            Time.timeScale = 2.0f;
            Debug.Log("DEVMODE");
        }
        else if (InputManager.Instance.DevModeIsPressed() && InputManager.Instance.InteractIsPressed())
        {
            Time.timeScale = 1.0f;
            Debug.Log("NO DEVMODE");
        }
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "NivelPrincipal" && !_firstTime)
        {
            if (FindAnyObjectByType<PauseMenuManager>() != null)
            {
                _pauseMenu = FindAnyObjectByType<PauseMenuManager>();
                _pauseMenu.HandleInput();
            }
            if (FindAnyObjectByType<IndicatorChange>() != null)
            {
                _indicatorChange = FindAnyObjectByType<IndicatorChange>();

                _indicatorChange.SetFirst(_firstTime);
                _tutorial = _indicatorChange.GetComponent<Button>();
                _tutorial.interactable = true;
                _tutorial.onClick.Invoke();
                _firstTime = true;
            }
        }
        //else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "NivelPrincipal" && !_firstTime && _tutorial == null)
        //{
        //    if (FindAnyObjectByType<IndicatorChange>() != null)
        //    {
        //        _indicatorChange = FindAnyObjectByType<IndicatorChange>();

        //        _tutorial = _indicatorChange.GetComponent<Button>();
        //        _tutorial.interactable = false;
        //        _tutorial.image.enabled = false;
        //    }
        //}
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
    /// Desactiva o activa el componente PlayerVision del jugador.
    /// true: desactiva el componente PlayerVision del jugador.
    /// false: activa el componente PlayerVision del jugador.
    /// </summary>

    public void DeactivatePlayer(bool _stateToDeactivate)
    {
        if (_player != null)
        {
            PlayerVision playerVision = _player.GetComponent<PlayerVision>();
            if (playerVision != null)
            {
                playerVision.enabled = !_stateToDeactivate;
            }
            else
            {
                Debug.LogError("No se ha encontrado el componente PlayerVision en el jugador.");
            }
        }
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
    /// Obtiene los datos del nivel al ser llamado, de aquí se obtiene: 
    /// el nivel nombre del nivel y con ello la
    /// refencia de los datos de ese nivel que más tarde será sustituidos 
    /// por los nuevos al ser completados
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
    /// Comprueba si el nivel es el infinito, en caso de serlo devuelve true
    /// </summary>
    /// <returns></returns>
    public bool isInfiniteMode()
    {
        return _levelName == "NivelInfinito";
    }

    public void SetLevelData(Level _levelToAssign)
    {
        _level = _levelToAssign;
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

    public void SetPlayer(GameObject _playerToSet)
    {
        _player = _playerToSet;
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
    /// <param name="_levelToAssign">El nivel para asignar el rango</param>
    public void SetRange(LevelManager.Range _range, int _levelToAssign)
    {
        _levelsRange[_levelToAssign] = _range;
        PlayerPrefs.SetString("RangeLevel: " + _levelToAssign, _range.ToString());
    }

    /// <summary>
    /// GetRange devuelve el rango del nivel
    /// </summary>
    /// <param name="_levelToAssign">El nivel para asignar el rango</param>
    /// <returns></returns>
    public LevelManager.Range GetRange(int _levelToAssign)
    {
        return _levelsRange[_levelToAssign];
    }

    /// <summary>
    /// SetMoney modifica _money según el dinero obtenido en la partida
    /// </summary>
    /// <param name="_money">Cantidad de dinero obtenido en la partida</param>
    /// <param name="_levelToAssign">El nivel para asignar el rango</param>
    public void SetMoney(int _money, int _levelToAssign)
    {
        _moneyNumberlevel[_levelToAssign] = _money;
        PlayerPrefs.SetString("MoneyLevel: " + _levelToAssign, _money.ToString());
    }

    /// <summary>
    /// SetMoney modifica el tiempo dinero según el tiempo obtenido en la partida
    /// Utiliza la variable _moneyNumberlevel para almacenar el tiempo ya que no conozco
    /// el funcionamiento de PlayerPrefs para implementar el almacenamiento de tiempo
    /// </summary>
    /// <param name="_money">Cantidad de dinero obtenido en la partida</param>
    /// <param name="_levelToAssign">El nivel para asignar el rango</param>
    public void SetTime(int seconds, int _levelToAssign)
    {
        _moneyNumberlevel[_levelToAssign] = seconds;
        PlayerPrefs.SetString("MoneyLevel: " + _levelToAssign, seconds.ToString());
    }
    /// <summary>
    /// GetMoney devuelve la cantidad de dinero obtenida en el nivel
    /// </summary>
    /// <param name="_levelToAssign">El nivel para asignar el rango</param>
    /// <returns></returns>
    public int GetMoney(int _levelToAssign)
    {
        return _moneyNumberlevel[_levelToAssign];
    }

    /// <summary>
    /// GetTime devuelve la cantidad de dinero obtenida en el nivel (en el nivel infinito es el tiempo)
    /// </summary>
    /// <param name="_levelToAssign">El nivel para asignar el rango</param>
    /// <returns></returns>
    public int GetTime(int _levelToAssign)
    {
        return _moneyNumberlevel[_levelToAssign];
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Método privado que crea una array de componentes con el script de _level 
    /// (allLevels) y busca en él el dato del nombre de nivel para asignarlo a _level
    /// </summary>
    /// <param name="_levelName"></param>
    private void FindLevelByName(string _levelName)
    {
        Level[] allLevels = FindObjectsOfType<Level>();
        _levels = allLevels;
        if (allLevels.Length == 0)
        {
            Debug.Log("No hay nivel en escena, está en juego");
            return;
        }
        else _level = allLevels.FirstOrDefault(level => level.GetLevelName() == _levelName);
    }


    public void UpdateStats()
{
    _levels = FindObjectsOfType<Level>();
    _moneyTextLevel = new TextMeshProUGUI[_levels.Length];
    _moneyNumberlevel = new int[_levels.Length];
    _rankTextLevel = new TextMeshProUGUI[_levels.Length];
    _levelsRange = new LevelManager.Range[_levels.Length];
    
    if (_levels != null)
    {
        for (int i = 0; i < _levels.Length; i++)
        {
            if (PlayerPrefs.HasKey("MoneyLevel: " + i))
            {
                string savedValue = PlayerPrefs.GetString("MoneyLevel: " + i);
                if (int.TryParse(savedValue, out int value))
                {
                    _moneyNumberlevel[i] = value;
                }
            }
            
            if (PlayerPrefs.HasKey("RangeLevel: " + i))
            {
                string savedRank = PlayerPrefs.GetString("RangeLevel: " + i);
                if (Enum.TryParse(savedRank, out LevelManager.Range rank))
                {
                    _levelsRange[i] = rank;
                }
            }
            
            _moneyTextLevel[i] = _levels[i].GetMoney();
            _rankTextLevel[i] = _levels[i].GetRankText();
            _rankTextLevel[i].text = _levels[i].GetRankLetter();
            
            if (_moneyNumberlevel[i] > 0)
            {
                if (_levels[i].GetLevelName() == "NivelInfinito")
                {
                    _levels[i].SetMoney(_moneyNumberlevel[i].ToString());
                }
                else
                {
                    _levels[i].SetMoney(_moneyNumberlevel[i].ToString());
                }
            }
        }
    }
}

    public void ResetProgress()
    {
        for (int i = 0; i < _levels.Length; i++)
        {
            if (!_levels[i].ReturnInfinite())
            {
                PlayerPrefs.DeleteKey("RangeLevel: " + i);
                PlayerPrefs.DeleteKey("MoneyLevel: " + i);
                _levels[i].SetMoney("--");
                _levels[i].SetRank("F");
            }
        }
    }

    public bool ReturnFirst()
    {
        return _firstTime;
    }

    /// <summary>
    /// Devuelve la letra correspondiente al rango del nivel principal
    /// </summary>
    /// <returns>La letra correspondiente al rango del nivel principal</returns>
    public string GetMainLevelRank()
    {
        int i = 0;
        string _mainLevelRank = "";
        if (_levels != null)
        {
            while (i < _levels.Length && _levels[i].GetLevelName() != "NivelPrincipal")
            {
                i++;
            }

            if (_levels[i].GetLevelName() == "NivelPrincipal")
            {
                _mainLevelRank = _levels[i].GetRankLetter();
            }
        }
        return _mainLevelRank;
    }
    #endregion
} // class GameManager 
  // namespace