//---------------------------------------------------------
// Gestor de escena. Podemos crear uno diferente con un
// nombre significativo para cada escena, si es necesario
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using TMPro;

/// <summary>
/// Componente que se encarga de la gestión de un nivel concreto.
/// Este componente es un singleton, para que sea accesible para todos
/// los objetos de la escena, pero no tiene el comportamiento de
/// DontDestroyOnLoad, ya que solo vive en una escena.
///
/// Contiene toda la información propia de la escena y puede comunicarse
/// con el GameManager para transferir información importante para
/// la gestión global del juego (información que ha de pasar entre
/// escenas)
/// </summary>
public class LevelManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    //Booleana que depende del jugador, si es Rack, está activo sino, significa que es Albert
    [SerializeField] public bool isRack;

    //Cantidad de dinero/puntuación que el jugador consigue
    [SerializeField] private int Money;

    // ShowText es el texto para mostrar en partida
    [SerializeField] private TextMeshProUGUI ShowText;

    // Panel es el panel que se muestra cuando se acaba el tiempo con el mensaje de que se ha acabado el tiempo
    [SerializeField] private GameObject Panel;


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static LevelManager _instance;

    /// <summary>
    /// _gameManager es el que gestiona el estado global del juego, es este cago para obtener una dato
    /// </summary>
    private GameManager _gameManager;

    /// <summary>
    /// _minS es la cantidad mínima de dinero para alcanzar el rango S
    /// </summary>
    [SerializeField] private int _minS = 900;

    /// <summary>
    /// _minA es la cantidad mínima de dinero para alcanzar el rango A
    /// </summary>
    [SerializeField] private int _minA = 650;

    /// <summary>
    /// _minB es la cantidad mínima de dinero para alcanzar el rango B
    /// </summary>
    [SerializeField] private int _minB = 400;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// _continue determina si el timer debe empezar (true) o no (false)
    /// </summary>
    private bool _continue = false;

    /// <summary>
    /// // _maxTime es el tiempo máximo que puede durar la partida
    /// </summary>
    private float _maxTime = 180;

    // _currentSecondsLeft es el tiempo restante en segundos
    private float _currentSecondsLeft;

    /// <summary>
    /// _minutesShow son los minutos para mostrar en el timer del juego
    /// </summary>
    private int _minutesShow;

    /// <summary>
    /// _secondsShow son los segundos para mostrar en el timer del juego
    /// </summary>
    private int _secondsShow;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour


    private void Start()
    {
        StartTimer();
        _gameManager = GameManager.Instance;
        isRack = _gameManager.ReturnBool();
        _gameManager.SpawnPlayer();
        _gameManager.FirstFindPlayerComponents();
        Money = 0;
    }

    private void Update()
    {
        if (_continue)
        {
            _currentSecondsLeft -= Time.deltaTime;
        }
        if (_currentSecondsLeft < 0)
        {
            StopTimer();
            _currentSecondsLeft = 0;
            Panel.SetActive(true);
            CalculateRange(Money);
            Time.timeScale = 0;
        }
        ShowTime();
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static LevelManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    public void SumMoney(int amount)
    {
        Money += amount;
    }

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el LevelManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Rango del juego al completarse
    /// </summary>
    public enum Range
    {
        S,A,B,F
    }


    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    // StartTimer() inicializa _currentSecondsLeft al valor de _maxTime y pone _continue a true para que el timer empiece
    public void StartTimer()
    {
        _currentSecondsLeft = _maxTime;
        _continue = true;
    }

    // StopTimer() pone _continue a false para que el timer pare
    private void StopTimer()
    {
        _continue = false;
    }

    // ShowTime() muestra el tiempo restante en formato MM:SS
    private void ShowTime()
    {
        _minutesShow = (int)_currentSecondsLeft / 60;
        _secondsShow = (int)_currentSecondsLeft % 60;
        if (_minutesShow < 10 && _secondsShow > 9)
        {
            ShowText.text = "0" + _minutesShow + ":" + _secondsShow;
        }
        else if (_minutesShow < 10 && _secondsShow < 10)
        {
            ShowText.text = "0" + _minutesShow + ":" + "0" + _secondsShow;
        }
        else if (_minutesShow > 9 && _secondsShow > 9)
        {
            ShowText.text = _minutesShow + ":" + _secondsShow;
        }
        else if (_minutesShow > 9 && _secondsShow < 10)
        {
            ShowText.text = _minutesShow + ":" + "0" + _secondsShow;
        }
    }

    // CalculateRange(int _money) calcula el rango según la cantidad de dinero lograda durante la partida
    private Range CalculateRange(int _money)
    {
        Range _range = new Range();
        if (_money >= _minS)
        {
            _range = Range.S;
        }
        else if (_money >= _minA)
        {
            _range = Range.A;
        }
        else if (_money >= _minB)
        {
            _range = Range.B;
        }
        else
        {
            _range = Range.F;
        }
        return _range;
    }

    #endregion
} // class LevelManager 
// namespace