//---------------------------------------------------------
// Determina la seleccion del jugador con cual personaje va a jugar en el nivel
// Liling Chen
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Añadir aquí el resto de directivas using
using UnityEngine.UI;


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerBool : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private Canvas Canvas; //Canvas con la selección del jugador

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private bool _isRack; // true si el jugador eligió a Rack, false si eligió a Albert
    private GameManager _gameManager; //Referencia para el GameManager
    private bool _isSelectionActive; // Indica si el SelectionPlayer está activo

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
        if(_gameManager == null) _gameManager = GameManager.Instance;
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
    /// Devuelve una booleana que hace referencia a la selección del jugador
    /// </summary>
    /// <returns>True si es Rack, false si es Albert</returns>
    public bool PlayerSelection()
    {
        return _isRack;
    }
    /// <summary>
    /// Al llamar al método, pone la booleana a true además de encontar el CanvasInfo y desabilitarlo para después llamar a los métodos de Game Manager para obtener la booleana del personaje elegido y por último
    /// transladar a la escena elegida por el jugador
    /// </summary>
    public void SelectRack()
    {
        _isRack = true;
        Debug.Log("Jugador seleccionó a Rack.");
        _gameManager.GetPlayer();
        _gameManager.ChangeToLevel();
    }

    //Mismo método que de arriba pero con Albert
    public void SelectAlbert()
    {
        _isRack = false;
        Debug.Log("Jugador seleccionó a Albert.");
        _gameManager.GetPlayer();
        _gameManager.ChangeToLevel();
    }

    //Esconde el canvas para la seleccion del personaje en caso de querer cambiar de mapa 
    public void ExitCanva()
    {
        // Esto asegurará que se vuelva al mapa de acción del jugador
        InputManager.Instance.EnableActionMap("Player");
        Canvas.gameObject.SetActive(false); // Desactiva el panel de selección de jugador
        Time.timeScale = 1f; // Reanuda el tiempo si es necesario
    }

    public void ShowSelectionPlayer(string levelName)
    {
        Canvas.gameObject.SetActive(true);  // Activamos el canvas de selección
        Time.timeScale = 0f;  // Pausamos el juego
        InputManager.Instance.EnableActionMap("UI");  // Cambiamos a ActionMap de UI

        // Configurar el texto para mostrar el nombre del nivel de manera bonita
        TextMeshProUGUI text = Canvas.GetComponentInChildren<TextMeshProUGUI>();
        string[] s = Regex.Split(levelName, @"(?<!^)(?=[A-Z])");
        text.text = string.Join(" ", s);

        // Seleccionamos el primer botón para el uso de un control de mando
        EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject);
    }

    public void HideSelectionPlayer()
    {
        Canvas.gameObject.SetActive(false);  // Ocultamos el canvas de selección
        Time.timeScale = 1f;  // Reanudamos el juego
        InputManager.Instance.EnableActionMap("Player");  // Volvemos a cambiar el ActionMap a Player
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)


    #endregion

} // class _playerBool 
// namespace
