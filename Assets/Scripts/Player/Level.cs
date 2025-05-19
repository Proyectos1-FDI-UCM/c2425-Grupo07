//---------------------------------------------------------
// Muestra la información del nivel jugado además de que lo accede
// Liling Chen
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------


using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using
using TMPro;


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Level : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] Canvas CanvasInfo; //Canvas con la información del nivel
    [SerializeField] TextMeshProUGUI Money; //Texto que muestra la cantidad de dinero
    [SerializeField] int LevelNum; //Numero del nivel
    [SerializeField] TextMeshProUGUI TimeText; //Texto que muestra el tiempo
    [SerializeField] TextMeshProUGUI RankText; //Texto que muestra el rango
    [SerializeField] string LevelName; //Nombre del nivel al que se carga en SceneLoader
    [SerializeField] Canvas SelectionPlayer; //Canvas con la seleccion de jugador
    [SerializeField] GameObject RanksDeco; //Canvas con la seleccion de jugador

    [SerializeField] bool _isThisInfiniteLevel; //Booleano que indica si el nivel es infinito o no;

    [SerializeField] Sprite _unlocked; //Sprite del nivel infinito desbloqueado


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private GameManager _gameManager; //instancia del game manager
    private PlayerLevel _player; //script del PlayerLevel
    string _rankLetter; //letra que se ha sacado el jugador, en un inicio es un F
    private Level _thisLevel; //Obtención de la información de este nivel
    [SerializeField] private ChangePreview _changePreview; //referencia a ChangePreview

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// Asigna MoneyPref y RankPref según lo que se ha guardado en el GameManager,
    /// Por defecto no hay dinero y el Rango es "F".
    /// </summary>
    void Start()
    {
        if(_gameManager == null)
        {
            _gameManager = GameManager.Instance;
        }
        if(_changePreview == null)
        {
            _changePreview = FindObjectOfType<ChangePreview>();
        }
        _thisLevel = gameObject.GetComponent<Level>();
        string MoneyPref = "MoneyLevel: " + LevelNum; // Tengo que crear el string para que
                                                      // el playerPrefs lo detecte
        
        if (!_isThisInfiniteLevel) // Si el nivel es infinito, se pone "Infinity"
        {
            string RankPref = "RangeLevel: " + LevelNum; // Tengo que crear el string para que
                                                         // el playerPrefs lo detecte

                                                         // Si no hay dinero, se pone "--"
            Money.text = PlayerPrefs.GetString(MoneyPref, "--"); 
            // Si no hay rango, se pone "F"
            _rankLetter = PlayerPrefs.GetString(RankPref, "F");

            CalculateRank(_rankLetter);
        }
        else 
        {
            string BestTimeInSeconds = PlayerPrefs.GetString(MoneyPref, "No record yet");
            if (BestTimeInSeconds != "--") // Si hay tiempo, se pone el tiempo
            {
                int minutes = int.Parse(BestTimeInSeconds) / 60;
                int seconds = int.Parse(BestTimeInSeconds) % 60;
                if (minutes > 0)
                {
                    Money.text = minutes + " minutes, " + seconds + " seconds";
                }
                else
                {
                    Money.text = seconds + " seconds";
                }
            }
            else
            {
                Money.text = BestTimeInSeconds; // Si no hay tiempo, se pone "No record yet"
            }

            if (_gameManager.GetLevelRank(0) != "F")
            {
                GetComponent<SpriteRenderer>().sprite = _unlocked;
            }
        }
  
        
    }



    /// <summary>
    /// Verifica si el jugador se colisiona con el objeto para cargar el CanvasInfo con los datos 
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CanvasInfo.gameObject.SetActive(true);
        if (!_isThisInfiniteLevel)
        {
            RanksDeco.gameObject.SetActive(true);
        }
        else
        {
            RanksDeco.gameObject.SetActive(false);
        }
        _player = collision.GetComponent<PlayerLevel>();
        _player.SetLevelScript(_thisLevel);
    }
    /// <summary>
    /// Verifica si el jugador se sale de la colisión del objeto para hacer invisible el CanvasInfo con los datos
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        CanvasInfo.gameObject.SetActive(false);
        _player = null;
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
    /// Cuando se realiza la acción context, se activa el canvas para elegir el personaje y para el tiempo del juego 
    /// para evitar que el jugador se mueva en escena, además, se llama al GameManager para obtener 
    /// el dato de este script y guardarlo como referencia
    /// </summary>
    public void OnEnterLevel()
    {
        SelectionPlayer.gameObject.SetActive(true);
        _changePreview.SetImagePreview(_thisLevel);
        InputManager.Instance.EnableActionMap("UI");
        TextMeshProUGUI text = SelectionPlayer.GetComponentInChildren<TextMeshProUGUI>();
        string[] s = Regex.Split(LevelName, @"(?<!^)(?=[A-Z])");
        text.text = string.Join(" ", s);
        EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject); // Selecciona el primer botón del canvas que encuentre para el funcionamiento del mando

        Time.timeScale = 0f;
        _gameManager.SetLevelData(_thisLevel);
    }

    /// <summary>
    /// Devuelve el nombre del nivel
    /// </summary>
    /// <returns>Retorna un string al ser llamado</returns>
    public string GetLevelName() { return LevelName; }

    public TextMeshProUGUI GetMoney()
    {
        return Money;
    }

    public TextMeshProUGUI GetRankText()
    {
        return RankText;
    }

    public string GetRankLetter()
    {
        return RankText.text;
    }

    /// <summary>
    /// Creado por Guillermo
    /// Asigna un color de la imagen por cada rango obtenido
    /// </summary>
    /// <param name="rankObtained"></param>
    public void CalculateRank(string rankObtained)
    {
        _rankLetter = rankObtained;
        if (rankObtained == "S")
        {
            RankText.color = Color.green;
            RankText.fontSize = 200;
        }
        else if (rankObtained == "A")
        {
            RankText.color = Color.cyan;
        }
        else if (rankObtained == "B")
        {
            RankText.color = Color.yellow;
        }
        else if (rankObtained == "C")
        {
            RankText.color = new Color(1.0f, 0.64f, 0.0f);
        }
        else if (rankObtained == "D")
        {
            RankText.color = Color.red;
        }
        else if (rankObtained == "E")
        {
            RankText.color = new Color(0.5f, 0, 0);
        }
        else
        {
            RankText.color = Color.gray;
        }
        RankText.text = _rankLetter;
    }

    /// <summary>
    /// Creado por Guillermo
    /// Asigna el dinero del nivel y guarda el valor en el playerPref
    /// MoneyToSet es el dinero a asignar
    /// (Lo usa el GameManager para cambiar el dinero de la partida)
    /// </summary>
    /// <param name="_moneyToSet"></param>
    public void SetMoney(string _moneyToSet)
    {
        string PlayerPref = "MoneyLevel: " + LevelNum;
        PlayerPrefs.SetString(PlayerPref, _moneyToSet);
        if (_isThisInfiniteLevel && _moneyToSet!="--")
        {
            Money.text = secondsToMMSS(float.Parse(_moneyToSet));
        }
        else
        {
            Money.text = _moneyToSet;
        }
    }

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
    /// <summary>
    /// Creado por Guillermo
    /// Asigna un rango específico al nivel
    /// rankToSet es el rango a asignar
    /// (Lo usa el GameManager para reiniciar el rango)
    /// </summary>
    /// <param name="_rankToSet"></param>
    public void SetRank(string _rankToSet)
    {
        string PlayerPref = "RangeLevel: " + LevelNum;
        PlayerPrefs.SetString(PlayerPref, _rankToSet);
        CalculateRank(_rankToSet);
    }

    public bool ReturnInfinite()
    {
        return _isThisInfiniteLevel;
    }

    public int GetLevelNum()
    {
        return LevelNum;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion


} // class _level 
// namespace
