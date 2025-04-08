//---------------------------------------------------------
// Gestor de escena. Podemos crear uno diferente con un
// nombre significativo para cada escena, si es necesario
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    /// <summary>
    /// // _maxTime es el tiempo máximo que puede durar la partida
    /// </summary>
    [SerializeField] private float _maxTime = 180;

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

    /// <summary>
    /// _minB es la cantidad mínima de dinero para alcanzar el rango C
    /// </summary>
    [SerializeField] private int _minC = 300;

    /// <summary>
    /// _minB es la cantidad mínima de dinero para alcanzar el rango D
    /// </summary>
    [SerializeField] private int _minD = 250;
    /// <summary>
    /// _minB es la cantidad mínima de dinero para alcanzar el rango E
    /// </summary>
    [SerializeField] private int _minE = 200;
    /// <summary>
    /// _levelNameText es un texto que indica el nombre del nivel
    /// </summary>
    [SerializeField] private TextMeshProUGUI _levelNameText;

    /// <summary>
    /// _moneyText es un texto que indica la cantidad de dinero obtenida en la partida
    /// </summary>
    [SerializeField] private TextMeshProUGUI _moneyEndText;

    /// <summary>
    /// _moneyInPlay es un texto que indica la cantidad de dinero siendo obtenida durante la partida
    /// </summary>
    [SerializeField] private TextMeshProUGUI _moneyInPlay;
    /// <summary>
    /// _moneyGaining es un texto que indica la cantidad de dinero que se suma
    /// </summary>
    [SerializeField] private TextMeshProUGUI _moneyGaining;

    /// <summary>
    /// _levelRangeText es un texto que indica el rango obtenido en la partida
    /// </summary>
    [SerializeField] private TextMeshProUGUI _levelRangeText;

    /// <summary>
    /// _deliveredObjectsText es un texto que indica el número de pedidos entregados
    /// </summary>
    [SerializeField] private TextMeshProUGUI _deliveredObjectsText;

    /// <summary>
    /// _failedDeliveriesText es un texto que indica el número de pedidos fallidos
    /// </summary>
    [SerializeField] private TextMeshProUGUI _failedDeliveriesText;

    /// <summary>
    /// Hecho por Guillermo
    /// 
    /// </summary>
    [SerializeField] private Vector2[] _initialPos;
    /// <summary>
    /// Hecho por Guillermo
    /// 
    /// </summary>
    [SerializeField] private Vector2 _endPos;

    /// <summary>
    /// Hecho por Guillermo
    /// 
    /// </summary>
    [SerializeField] private GameObject _pileOfCash;
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
    /// _continue determina si el timer debe empezar (true) o no (false)
    /// </summary>
    private bool _continue = false;

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

    /// <summary>
    /// _levelRange es el rango obtenido en la partida
    /// </summary>
    private Range _levelRange;

    /// <summary>
    /// Referencia al script Receiver
    /// </summary>
    private Receiver _receiver;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour


    private void Start()

    {
        if (FindAnyObjectByType<Receiver>() != null)
        {
            _receiver = FindAnyObjectByType<Receiver>();
        }
       
        if (_gameManager == null)
        {
            _gameManager = GameManager.Instance;
        }
        isRack = _gameManager.ReturnBool();
        StartTimer();
        Money = 0;

        // Establecer el modo de juego en pausado
        //Time.timeScale = 0;

        if (InputManager.Instance != null)
        {
            InputManager.Instance.EnableActionMap("Player");
        }
        else
        {
            Debug.LogWarning("InputManager.Instance es null. ¿Está inicializado?");
        }

        //Hecho por Guillermo
        if (_pileOfCash != null)
        {
            for (int i = 0; i < _pileOfCash.transform.childCount; i++)
            {
                _initialPos[i] = _pileOfCash.transform.GetChild(i).position;
            }
        }

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
            _levelRange = CalculateRange(Money);
            if ((_gameManager.GetRange() == Range.F && (_levelRange == Range.S || _levelRange == Range.A || _levelRange == Range.B)) ||
                (_gameManager.GetRange() == Range.B && (_levelRange == Range.S || _levelRange == Range.A)) ||
                (_gameManager.GetRange() == Range.A && _levelRange == Range.S))
            {
                _gameManager.SetRange(_levelRange);
            }
            if (_gameManager.GetMoney() < Money)
            {
                _gameManager.SetMoney(Money);
            }
            TimeIsOverText();
            Panel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject); // Selecciona el primer botón del canvas que encuentre para el funcionamiento del mando
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

    public void SumMoney(int amount, Color doneColor)
    {
        Money += amount;
        if (Money < 0)
        {
            _moneyInPlay.color = Color.red;
            Money = 0;
        }
        else
        {
            _moneyInPlay.color = Color.green;
        }
        _moneyGaining.color = doneColor;
        if (amount>0)
        {
            _moneyGaining.text = "+" + amount;
        }
        else
        {
            _moneyGaining.text = "" + amount;
        }
        StartCoroutine(SumaDinero(amount));
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
        F, E, D, C, B, A, S
    }

    // StartTimer() inicializa _currentSecondsLeft al valor de _maxTime y pone _continue a true para que el timer empiece
    public void StartTimer()
    {
        _currentSecondsLeft = _maxTime;
        _continue = true;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados


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

    private void TimeIsOverText()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "NivelPrincipal")
        {
            _levelNameText.text = "Nivel principal";
        }
        else
        {
            _levelNameText.text = "Nivel infinito";
        }

        _moneyEndText.text = "Dinero recopilado: " + Money.ToString();

        if (_levelRange == Range.S)
        {
            _levelRangeText.text = "S";
        }
        else if (_levelRange == Range.A)
        {
            _levelRangeText.text = "A";
        }
        else if (_levelRange == Range.B)
        {
            _levelRangeText.text = "B";
        }
        else if (_levelRange == Range.C)
        {
            _levelRangeText.text = "C";
        }
        else if (_levelRange == Range.D)
        {
            _levelRangeText.text = "D";
        }
        else if (_levelRange == Range.E)
        {
            _levelRangeText.text = "E";
        }
        else
        {
            _levelRangeText.text = "F";
        }

        if (_receiver != null)
        {
            _deliveredObjectsText.text = "Pedidos entregados: " + _receiver.GetDeliveredObjectsNumber().ToString();
            _failedDeliveriesText.text = "Pedidos fallidos: " + _receiver.GetFailedDeliveriesNumber().ToString();
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
        else if (_money >= _minC)
        {
            _range = Range.C;
        }
        else if (_money >= _minD)
        {
            _range = Range.D;
        }
        else if (_money >= _minE)
        {
            _range = Range.E;
        }
        else
        {
            _range = Range.F;
        }
        return _range;
    }
    /// <summary>
    /// Hecho por Guillermo
    /// 
    /// </summary>
    private void ResetCashPos()
    {
        for (int i = 0; i<_pileOfCash.transform.childCount; i++)
        {
            Transform _currentChild = _pileOfCash.transform.GetChild(i);
            _currentChild.position = _initialPos[i];
            _currentChild.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            _currentChild.gameObject.SetActive(false);
        }
    }

    IEnumerator SumaDinero(int MoneyToSum)
    {
        float dineroRate = 0.002f; // Velocidad en la que incrementa el dinero
        ResetCashPos();
        if (MoneyToSum > 0)
        {
            _pileOfCash.SetActive(true);
            float delay = 0.02f;
            for (int i = 0; i < _pileOfCash.transform.childCount - 1; i++) // Hace que cada objeto de dinero crezca
            {
                _pileOfCash.transform.GetChild(i).gameObject.SetActive(true);
                for (int j = 0; j < 10; j++)
                {
                    float rateAumenta = (float)j *5;
                    float tiempoDisminuye = (float)j / 1000;
                    yield return new WaitForSeconds(tiempoDisminuye);
                    _pileOfCash.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = new Vector2(rateAumenta, rateAumenta);
                }
                yield return new WaitForSeconds(delay);
            }
            // Mueve los objetos y los desactiva mientras incrementa el contador de dinero
            for (int k = 0; k <= MoneyToSum; k++)
            {
                yield return new WaitForSeconds(dineroRate);
                float _dineroARepresentar = (Money-MoneyToSum) + k; //esto se hace con una cantidad de dinero ya actualizada
                _moneyInPlay.text = "" + _dineroARepresentar;
                if (k < _pileOfCash.transform.childCount - 1)
                {
                    while (_pileOfCash.transform.GetChild(k).GetComponent<RectTransform>().anchoredPosition != _endPos)
                    {
                        _pileOfCash.transform.GetChild(k).GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(_pileOfCash.transform.GetChild(k).GetComponent<RectTransform>().anchoredPosition,
                            _endPos, 3000 * Time.deltaTime);
                        yield return 0; // Espera durante un frame
                    }
                    _pileOfCash.transform.GetChild(k).GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                    _pileOfCash.transform.GetChild(k).gameObject.SetActive(false);
                }
            }
 
            _pileOfCash.SetActive(false);
        }
        else
        {
            int contador = 0;
            _pileOfCash.transform.GetChild(0).gameObject.SetActive(true);
            while (Money >= 0 && contador >= MoneyToSum)
            {
                yield return new WaitForSeconds(dineroRate);
                float _dineroARepresentar = (Money - MoneyToSum)+ contador; //esto se hace con una cantidad de dinero ya actualizada
                _moneyInPlay.text = "" + _dineroARepresentar;
                _moneyInPlay.color = Color.red;
                _pileOfCash.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(_pileOfCash.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition, _endPos, 3000 * Time.deltaTime);
                contador--;
            }
            _moneyInPlay.color = Color.green;
            _pileOfCash.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            _pileOfCash.transform.GetChild(0).gameObject.SetActive(false);
            _moneyInPlay.text = "" + Money;
        }
    }



    #endregion
} // class LevelManager 
  // namespace