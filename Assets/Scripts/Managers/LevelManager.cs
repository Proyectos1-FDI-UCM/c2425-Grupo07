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
    /// _levelNameText es un texto que indica el número del nivel
    /// </summary>
    [SerializeField] private int _levelNum;

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
    /// Recoge la posición inicial de la pila de dinero (7 objetos en total) que volverán a la posición inicial al ser invocados
    /// </summary>
    [SerializeField] private Vector2[] _initialPos;
    /// <summary>
    /// Hecho por Guillermo
    /// Recoge la posición final en la que pila de dinero se moverá
    /// </summary>
    [SerializeField] private Vector2 _endPos;

    /// <summary>
    /// /// Botón para reiniciar el nivel, se preselecciona al acabar un nivel
    /// </summary>
    [SerializeField] private GameObject RestartButton;

    /// <summary>
    /// Hecho por Guillermo
    /// Recoge el padre de los objetos de la pila de dinero que irá apareciendo de una en una hasta llegar al contador de dinero
    /// </summary>
    [SerializeField] private GameObject PileOfCash;
    /// <summary>
    /// Hecho por Guillermo
    /// Recoge el padre de los objetos de la pila de dinero que irá apareciendo de una en una hasta llegar al contador de dinero
    /// </summary>
    [SerializeField] private int DineroVel = 3000;

    /// <summary>
    /// Texto de avisos para el nivel infinito, el texto avisa del tiempo que alcanza el jugador
    /// y de la reducción de tiempo que se aplica al tiempo ganado con los pedidos
    /// </summary>
    [SerializeField] private TMP_Text AlertsText;

    [SerializeField] private bool InfiniteMode;

    [SerializeField] private int InfModeFirstWave = 4;
    [SerializeField] private int InfModeSecondWave = 8;
    [SerializeField] private int InfModeThirdWave = 15;
    [SerializeField] private int InfModeRemainingWave = 25;
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
    /// Variable que almacena cuanto tiempo lleva el jugador en el nivel Infinito desde el inicio
    /// </summary>
    private float elapsedTime;
    

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

    //Estas boleanas permiten que los metodos de alerta no se llamen cada frame en el que el "if" se cumple
    private bool hasShownFirstWaveAlert = false;
    private bool hasShownSecondWaveAlert = false;
    private bool hasShownThirdWaveAlert = false;
    private bool hasShownRemainingWaveAlert = false;

    /// <summary>
    /// Un flag que indica si el jugador ha conseguido un nuevo mejor tiempo o no, se resetea a false después de cada partida.
    /// </summary>
    public bool newBestFlag = false;
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
        elapsedTime = 0;

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
        if (PileOfCash != null)
        {
            for (int i = 0; i < PileOfCash.transform.childCount; i++)
            {
                _initialPos[i] = PileOfCash.transform.GetChild(i).GetComponent<SizeAnimation>().ReturnUIPos();
            }
        }
        newBestFlag = false;
    }
    /// <summary>
    /// Cuando se pueda continuar disminuye el timer
    /// Cuando termina el timer calcula el rango a partir del dinero y lo 
    /// manda al gameManager
    /// </summary>
    private void Update()
    {
        if (_continue)
        {
            _currentSecondsLeft -= Time.deltaTime;
            elapsedTime += Time.deltaTime;
            #region 
            float minutes = elapsedTime/60;
            /*
            if (InfiniteMode)
            {
               
            if (minutes < InfModeFirstWave)
            {
                if (!hasShownFirstWaveAlert)
                {
                    ShowAlerts(@$"{InfModeFirstWave} MINUTES!
                            20% LESS TIME ON DELIVERS
                            ");
                    hasShownFirstWaveAlert = true;
                }
            }
            else if (minutes < InfModeSecondWave)
            {
                if (!hasShownSecondWaveAlert)
                {
                    ShowAlerts(@$"{InfModeSecondWave} MINUTES!
                            30% LESS TIME ON DELIVERS
                            ");
                    hasShownSecondWaveAlert = true;
                }
            }
            else if (minutes < InfModeThirdWave)
            {
                if (!hasShownThirdWaveAlert)
                {
                    ShowAlerts(@$"{InfModeThirdWave} MINUTES!
                            50% LESS TIME ON DELIVERS
                            ");
                    hasShownThirdWaveAlert = true;
                }
            }
            else if (minutes < InfModeRemainingWave)
            {
                if (!hasShownRemainingWaveAlert)
                {
                    ShowAlerts(@$"{InfModeRemainingWave} MINUTES!
                            75% LESS TIME ON DELIVERS
                            ");
                    hasShownRemainingWaveAlert = true;
                }
            }
            }
            */
            #endregion
            
            
        }
        if (_currentSecondsLeft < 0)
        {
            StopTimer();
            PlayerPrefs.SetInt("IsFirstTime", 1); // 1 false
            _currentSecondsLeft = 0;
            if (!InfiniteMode) // Si es el nivel normal, se calcula el rango, el dinero y se almacena en el GameManager
            {
                _levelRange = CalculateRange(Money);
                if ((_gameManager.GetRange(_levelNum) == Range.F && (_levelRange == Range.S || _levelRange == Range.A || _levelRange == Range.B)) ||
                    (_gameManager.GetRange(_levelNum) == Range.B && (_levelRange == Range.S || _levelRange == Range.A)) ||
                    (_gameManager.GetRange(_levelNum) == Range.A && _levelRange == Range.S))
                {
                    _gameManager.SetRange(_levelRange, _levelNum);
                }
                if (_gameManager.GetMoney(_levelNum) < Money)
                {
                    _gameManager.SetMoney(Money, _levelNum);
                    _gameManager.SetRange(_levelRange, _levelNum);
                    newBestFlag = true;
                }
            }
            else // Si el nivel es infinito, se calcula el new best utilizando las mismas variables que con el dinero
            {
                Debug.Log($"BEFORE Best Time: {_gameManager.GetTime(_levelNum)}");
                int tiempotruncado = (int)elapsedTime;
                if (_gameManager.GetTime(_levelNum) + 0.05f < tiempotruncado ) // Se añade un pequeño margen para evitar errores de redondeo
                {
                    _gameManager.SetTime(tiempotruncado, _levelNum);
                    newBestFlag = true;
                }
                Debug.Log($"Current Time: {tiempotruncado}, Stored Best Time: {_gameManager.GetTime(_levelNum)}");
            }
            
            TimeIsOverText();
            Panel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(RestartButton); // Selecciona el primer botón del canvas que encuentre para el funcionamiento del mando
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

    public void SumMoney(int ammount, Color doneColor)
    {
        Money += ammount;
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
        if (!InfiniteMode)
        {
            if (ammount > 0)
            {
                _moneyGaining.text = "+" + ammount;
            }
            else
            {
                _moneyGaining.text = "" + ammount; // No tiene signo menos porque el propio número lo pone  
            }
            StartCoroutine(SumaCantidad(ammount));
        }
    }

    /// <summary>
    /// Este metodo suma dinero para los cheats, no utiliza los colores y está más simplificado que el método original
    /// </summary>
    /// <param name="ammount"></param>
    public void SumMoneyCheats(int ammount)
    {
        Money += ammount;
        if (Money < 0)
        {
            _moneyInPlay.color = Color.red;
            Money = 0;
        }
        else
        {
            _moneyInPlay.color = Color.green;
        }
            StartCoroutine(SumaCantidad(ammount));
    }

    /// <summary>
    /// Método que añadirá tiempo al nivel infinito cuando se entregue un pedido
    /// La suma del tiempo varía dependiendo de cuando tiempo lleve el jugador en el nivel infinito
    /// para ir aumentando la dificultado
    /// </summary>
    /// <param name="ammount"></param>
    public void SumTime(float ammount)
    {
        float minutes = elapsedTime/60;
        float timeFactor;
        if (minutes < InfModeFirstWave)
        {
            timeFactor = 1f;
        }
        else if (minutes < InfModeSecondWave)
        {
            timeFactor = 0.8f;
        }
        else if (minutes < InfModeThirdWave)
        {
            timeFactor = 0.7f;
        }
        else if (minutes < InfModeRemainingWave)
        {
            timeFactor = 0.5f;
        }
        else // A partir de estos minutos, el juego es extremo, casi injugable
        {
            timeFactor = 0.35f;
        }
        _moneyGaining.text = "+" + ammount * timeFactor;
        StartCoroutine(SumaCantidad((int)(ammount * timeFactor)));
        //_currentSecondsLeft += ammount * timeFactor;
    }

    /// <summary>
    /// Método simple que se encarga de variar la cantidad de tiempo restante en el nivel
    /// Se usa para los cheats y no tiene ninguna animación.
    /// </summary>
    /// <param name="ammount"></param>
    public void SumTimeCheats(float ammount)
    {
        _currentSecondsLeft += ammount;
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

    // GetCurrentSecondsLeft() devuelve el valor de _currentSecondsLeft
    public float GetCurrentSecondsLeft()
    {
        return _currentSecondsLeft;
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
        // Cambiar los nombres de las escenas conllevaba a varios conflictos, la mejor solucion que tuvimos
        // para ajustar el texto de la escena a ingles es de esta manera.
        switch (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
        {
            case "NivelPrincipal":
            _levelNameText.text = "Main Level";
            break;

            case "NivelInfinito":
            _levelNameText.text = "Infinite Level";
            break;
        }


        if (!InfiniteMode)
        {
            _moneyEndText.text = "Gained money: " + Money.ToString() + " $";
            if (newBestFlag)
            {
                _moneyEndText.text += " (NEW BEST!!!)";
                newBestFlag = false; // Reseteamos el flag para que no se muestre siempre
            }
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
        }
        else
        {
            _moneyEndText.text = "Elapsed time: " + secondsToMMSS(elapsedTime);
            if (newBestFlag)
            {
                _moneyEndText.text += " (NEW BEST!!!)";
                newBestFlag = false; // Reseteamos el flag para que no se muestre siempre
            }

        }

       

        if (_receiver != null)
        {
            _deliveredObjectsText.text = "Deliveries sent: " + _receiver.GetDeliveredObjectsNumber().ToString();
            _failedDeliveriesText.text = "Failed tasks: " + _receiver.GetFailedDeliveriesNumber().ToString();
        }
    }

    /// <summary>
    /// Método que convierte los segundos a minutos y segundos para mostrarlo en el texto del final de la partida
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    private string secondsToMMSS(float seconds)
    {
        int minutes = (int)seconds / 60;
        int secondsLeft = (int)seconds % 60;
        string result;
        if (minutes > 0)
        {
            result = minutes + " minutes, " + secondsLeft + " seconds";
        }
        else
        {
            result = secondsLeft + " seconds";
        }
        return result;
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
    /// Reseta la posición de cada uno de los billestes de _pileOfCash, incluyendo su tamaño para que crezca al invocarse el método
    /// </summary>
    private void ResetCashPos()
    {
        for (int i = 0; i<PileOfCash.transform.childCount; i++)
        {
            Transform _currentChild = PileOfCash.transform.GetChild(i);
            _currentChild.GetComponent<SizeAnimation>().PosUI(_initialPos[i]);
            _currentChild.GetComponent<SizeAnimation>().SizeUI(0);
            _currentChild.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// Hecho por Guillermo
    /// Resetea la posición del dinero para que incremente o disminuya la cantidad MoneyToSum, sumando o restando el dinero con un efecto que suma o resta de uno en uno
    /// Si el dinero a sumar es mayor que 0 realiza la animación del dinero, si no, convierte el texto a rojo y hace un efecto que no puede bajar más.
    /// Si el dinero a sumar es mayor que 0, incrementa el tamaño de cada billete por orden de la jerarquía del padre
    ///     Rate aumenta es la velocidad que aumenta, veces incrementado es lo mucho que se aumentará, tiempo
    /// Si el dinero a sumar es menor que 0, inicializa un contador que disminuye hasta MoneySum, coloreando el texto en rojo mientras decrementa
    ///     Al terminar el bucle vuelve a verde y desactiva los billetes.
    /// </summary>
    /// <param name="AmountToSum"></param>
    /// <returns></returns>
    IEnumerator SumaCantidad(int AmountToSum)
    {
        ResetCashPos();
        if (AmountToSum > 0)
        {
            PileOfCash.SetActive(true);
            float _delay = 0.02f;
            int _childCounter = 0;
            float _rateAumenta = 1;
            PileOfCash.transform.GetChild(_childCounter).gameObject.SetActive(true);
            while (_childCounter < PileOfCash.transform.childCount - 1)
            // Hace que cada objeto de dinero crezca
            {
                _rateAumenta += 10; // Crece de 5 en 5 para que sea más rápido el proceso
                yield return 0; // Espera durante un frame
                PileOfCash.transform.GetChild(_childCounter).GetComponent<SizeAnimation>().SizeUI(_rateAumenta);
                if (PileOfCash.transform.GetChild(_childCounter).GetComponent<SizeAnimation>().ReturnUISize() >= 50)
                {
                    _childCounter++;
                    PileOfCash.transform.GetChild(_childCounter).gameObject.SetActive(true);
                    yield return new WaitForSeconds(_delay);
                    _rateAumenta = 1;
                }
            }

            int _amountCounter = 0; // Inicio el contador de dinero que irá subiendo cada vez que un billete llegue al final, si no hay billete sube más rápido
            float _amountToRepresent = 0;

            while (_amountCounter <= AmountToSum)// Mueve los billetes y los desactiva al llegar a endPos mientras se incrementa el contador de dinero
            {
                yield return 0; // Espera durante un frame
                if (!InfiniteMode)
                {
                    _amountToRepresent = (Money - AmountToSum) + _amountCounter; //esto se hace con una cantidad de dinero ya actualizada
                    if (_amountToRepresent <= Money)
                    {
                        _moneyInPlay.text = "" + _amountToRepresent;
                    }
                }
                if (_amountCounter < PileOfCash.transform.childCount - 1 && PileOfCash.transform.GetChild(_amountCounter).GetComponent<SizeAnimation>().ReturnUIPos() != _endPos)
                // Mientras que moneyCounter sea menor que los billetes a mover y no hallan llegado al final
                {
                    PileOfCash.transform.GetChild(_amountCounter).GetComponent<SizeAnimation>().PosUI(
                    Vector2.MoveTowards(PileOfCash.transform.GetChild(_amountCounter).GetComponent<SizeAnimation>().ReturnUIPos(), _endPos, DineroVel * Time.deltaTime));
                    yield return 0; // Espera durante un frame
                }
                else if (_amountCounter < PileOfCash.transform.childCount - 1)
                //Va billete a billete desactivándolos y reseteando su tamaño
                {
                    PileOfCash.transform.GetChild(_amountCounter).GetComponent<SizeAnimation>().PosUI(new Vector2(0, 0));
                    PileOfCash.transform.GetChild(_amountCounter).gameObject.SetActive(false);
                    _amountCounter++;
                    if (InfiniteMode)
                    {
                        _currentSecondsLeft++;
                    }
                }
                else
                {
                    _amountCounter++;
                    if (InfiniteMode)
                    {
                        _currentSecondsLeft++;      
                    }
                }
            }
            PileOfCash.SetActive(false);
        }
        else
        {
            int _moneyCounter = 0;
            PileOfCash.transform.GetChild(0).gameObject.SetActive(true);
            while (Money >= 0 && _moneyCounter >= AmountToSum)
            {
                yield return 0; // Espera durante un frame
                float _dineroARepresentar = (Money - AmountToSum) + _moneyCounter; //esto se hace con una cantidad de dinero ya actualizada
                _moneyInPlay.text = "" + _dineroARepresentar;
                _moneyInPlay.color = Color.red;
                //Solamente mueve el contador de dinero a su posición
                PileOfCash.transform.GetChild(0).GetComponent<SizeAnimation>().PosUI(
                Vector2.MoveTowards(PileOfCash.transform.GetChild(0).GetComponent<SizeAnimation>().ReturnUIPos(), _endPos, DineroVel * Time.deltaTime));
                _moneyCounter--;
            }
            _moneyInPlay.color = Color.green;
            PileOfCash.transform.GetChild(0).GetComponent<SizeAnimation>().SizeUI(0);
            PileOfCash.transform.GetChild(0).gameObject.SetActive(false);
            _moneyInPlay.text = "" + Money;
        }
    }

    private void ShowAlerts(string text)
    {
        
        AlertsText.text = text;
        AlertsText.alpha = 1;
        StartCoroutine(AlertFadeOut(3));
        
    }

    private IEnumerator AlertFadeOut(float fadeDuration)
    {
        yield return new WaitForSeconds(2f);

        float elapsedTime = 0f;
        
        while (elapsedTime < fadeDuration)
        {

            float newAlpha = Mathf.Lerp(1, 0f, elapsedTime / fadeDuration);

            AlertsText.color = new Color(AlertsText.color.r, AlertsText.color.g, AlertsText.color.b, newAlpha);

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        
        AlertsText.alpha = 0;    
    }
    

    #endregion
} // class LevelManager 
  // namespace