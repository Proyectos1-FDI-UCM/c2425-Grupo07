//---------------------------------------------------------
// En este script se programa el funcionamiento de la sierra
// Ferran
// Clank&Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.UI;


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class SawScript : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    // PlayerPosition es la posición del jugador
    [SerializeField] private Transform PlayerPosition;

    // CompletionBarReference es la barra de compleción del material, se usa para la animación de su barra
    [SerializeField] private Image CompletionBarReference;

    // _madera es el GameObject correspondiente a la madera
    [SerializeField] private GameObject _madera;

    // _maderaProcesada es el GameObject correspondiente a la madera procesada
    [SerializeField] private GameObject _maderaProcesada;

    // _nClicks es el número de clicks necesario para completar el proceso de refinamiento
    public int MaxClicks = 6;

    // _currentClicks es el número de clicks necesario para completar el proceso de refinamiento
    public int CurrentClicks = 0;

    // _carriesWood determina si hay madera en la sierra (true) o no (false)
    public bool HasWood = false;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private int _pastClicks = 0;

    private Material _materialSource;

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
        _pastClicks = CurrentClicks;
        PlayerPosition = GameObject.FindWithTag("Player").GetComponent<Transform>();
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        _pastClicks = CurrentClicks;

        if (CurrentClicks >= MaxClicks)
        {
            GameObject child = Instantiate(_maderaProcesada, transform.GetChild(0).gameObject.transform.position, transform.GetChild(0).gameObject.transform.rotation);
            child.transform.SetParent(this.transform);
            Destroy(transform.GetChild(0).gameObject);
            HasWood = false;
            _pastClicks = 0;
            CurrentClicks = 0;
            UpdateCompletionBar(MaxClicks, CurrentClicks, _pastClicks);
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

    // Click() suma 1 al número de clicks necesario para completar el
    // proceso de refinamiento cada vez que se hace click sobre la sierra
    public void Click()
    {
        if (transform.childCount == 1)
        {
            CurrentClicks++;
            UpdateCompletionBar(MaxClicks, CurrentClicks, _pastClicks);
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    // Actualiza la barra de compleción de la sierra
    private void UpdateCompletionBar(float _maxCompletion, float _currentCompletion, float _pastCompletion)
    {
        float _targetCompletion = _currentCompletion / _maxCompletion;
        _pastCompletion = _pastCompletion / _maxCompletion;
        CompletionBarReference.fillAmount = _currentCompletion / _maxCompletion;
        StartCoroutine(CompletionBarAnimation(_targetCompletion, _pastCompletion));
    }

    // Hace la animación de rellenar la barra de compleción de la sierra
    private IEnumerator CompletionBarAnimation(float _targetCompletion, float _pastCompletion)
    {
        float _transitionTime = 0.25f, _timePassed = 0f;
        while (_timePassed < _transitionTime)
        {
            _timePassed += Time.deltaTime;
            CompletionBarReference.fillAmount = Mathf.Lerp(_pastCompletion, _targetCompletion, _timePassed / _transitionTime);
            yield return null;
        }
        CompletionBarReference.fillAmount = _targetCompletion;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Material>() != null && collision.gameObject.GetComponent<Material>().matType == MaterialType.Madera)
        {
            HasWood = true;
            _materialSource = collision.GetComponent<Material>();
            CompletionBarReference = _materialSource.ReturnProgressBar();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Material>() != null)
        {
            HasWood = false;
            _materialSource = null;
            CompletionBarReference = null; 
        }
    }

    #endregion   

} // class SawScript 
// namespace
