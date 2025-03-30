//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
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
    private Canvas _canvas; //Canvas con la selección del jugador
    private GameManager _gameManager; //Referencia para el GameManager
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

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
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
        _canvas = GameObject.Find("SelectionPlayer").GetComponent<Canvas>();
        _gameManager.GetPlayer();
        _gameManager.ChangeToLevel();
    }

    //Mismo método que de arriba pero con Albert
    public void SelectAlbert()
    {
        _isRack = false;
        Debug.Log("Jugador seleccionó a Albert.");
        _canvas = GameObject.Find("SelectionPlayer").GetComponent<Canvas>();
        _gameManager.GetPlayer();
        _gameManager.ChangeToLevel();
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
