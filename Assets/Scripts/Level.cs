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
    [SerializeField] Image Rank; //Imagen del rango del CanvasInfo
    [SerializeField] Text Money; //Texto que muestra la cantidad de dinero
    [SerializeField] Text TimeText; //Texto que muestra el tiempo
    [SerializeField] string LevelName; //Nombre del nivel al que se carga en SceneLoader
    [SerializeField] Canvas SelectionPlayer; //Canvas con la seleccion de jugador


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private GameManager _gameManager;
    private PlayerLevel _player;
    private Level _thisLevel;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        if(_gameManager == null)
        {
            _gameManager = GameManager.Instance;
        }
        _thisLevel = gameObject.GetComponent<Level>();
    }



    /// <summary>
    /// Verifica si el jugador se colisiona con el objeto para cargar el CanvasInfo con los datos 
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CanvasInfo.gameObject.SetActive(true);
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
        Text text = SelectionPlayer.GetComponentInChildren<Text>();
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
