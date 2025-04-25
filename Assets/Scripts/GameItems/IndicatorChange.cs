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

    [SerializeField] private Sprite[] Page;
    [SerializeField] private Canvas Tutorial;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private int _num = 0;
    private Image _page;
    private bool _first = false;
    [SerializeField] private GameObject Skip;
    [SerializeField] private GameObject Resume;
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
        _page = gameObject.GetComponent<Image>();
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

    public void On()
    {
        gameObject.SetActive(true);
        Tutorial.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(Skip);
    }

    public void Off()
    {
        gameObject.SetActive(false);
        Tutorial.gameObject.SetActive(false);
        _first = true;
        _page.sprite = Page[0];
        _num = 0;
        EventSystem.current.SetSelectedGameObject(Resume);
    }
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
