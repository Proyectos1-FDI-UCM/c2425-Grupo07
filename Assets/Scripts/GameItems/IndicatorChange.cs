//---------------------------------------------------------
// Se programa el paso de imagenas para las instrucciones del juego
// Liling Chen
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class IndicatorChange : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] private Sprite[] Page; //Array de las páginas de indicaciones
    [SerializeField] private Canvas Tutorial; //Canvas que lleva los botones
    [SerializeField] private GameObject Skip; //Botón de cerrar la pestaña
    [SerializeField] private GameObject Resume; //Botton del resume del menu de pausa
    [SerializeField] private PauseMenuManager _pauseMenu; //Script del menu de pausa
    [SerializeField] private GameObject TutorialObject; //Game object del que tendrá los cambios de las´páginas
    [SerializeField] private Button TutorialButton; //Botón del turorial para activar la pestaña
    

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private int _num = 0; //Contador del número de página en el que está el jugador
    private Image _page;//Imagen del Game Object a cambiar
    private bool _first;// si es la primera vez en jugar
    private bool _active;//Si está activo o no las indicaciones
    private GameManager _gameManager; //Instancia del Game Manager
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
        _page = TutorialObject.GetComponent<Image>();
        if (FindAnyObjectByType<PauseMenuManager>() != null)
        {
            _pauseMenu = FindAnyObjectByType<PauseMenuManager>();
        }
        if (_gameManager == null)
        {
            _gameManager = GameManager.Instance;
        }
    }

    private void Update()
    {
        if (!_first) 
        {
            _first = _gameManager.ReturnFirst();
        }
        if (_first && !_active)
        {
            TutorialButton.image.enabled = true;
        }
    }


    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    public void Pass() //flecha para avanzar
    {
        if (!_first && _num < Page.Length - 1)
        {
            _num++;
            ChangePage();
        }
        else 
        {
            if (_num < Page.Length - 2)
            {
                _num++;
                ChangePage();
            }
        }
    }

    public void Return() //flecha para retroceder
    {
        if (_num > 0)
        {
            _num--;
            ChangePage();
        }
    }

    public void ChangePage() //Cambia la imagen del Game Object al del siguiente o anterior de la array
    {
        _page.sprite = Page[_num];
    }

    public void On() //Para activar la indicación junto con sus componentes
    {
        TutorialObject.gameObject.SetActive(true);
        Tutorial.gameObject.SetActive(true);
        _active = true;
        EventSystem.current.SetSelectedGameObject(Skip);
    }

    public void Off() //Para activar desactivar la indicación junto con sus componentes y dependiendo si es la primera vez o no, se cierra junto con el menu de pausa
    {
        if (!_first)
        {
            _first = true;
            _active = false;
            TutorialObject.gameObject.SetActive(false);
            Tutorial.gameObject.SetActive(false);
            _page.sprite = Page[0];
            _num = 0;
            _pauseMenu.HandleInput();
        }
        else
        {
            TutorialObject.gameObject.SetActive(false);
            Tutorial.gameObject.SetActive(false);
            _page.sprite = Page[0];
            _num = 0;
            _active = false;
            EventSystem.current.SetSelectedGameObject(Resume);
        }
    }

    public void SetFirst(bool game) //Obtiene la booleana First del game manager
    {
        _first = game;
    }

    public bool ReturnActive() { return _active; } 
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class IndicatorChange 
// namespace
