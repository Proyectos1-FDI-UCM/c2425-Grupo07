//---------------------------------------------------------
// Funcionamiento de la soldadora
// Alicia Sanchez
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class WelderScript : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    //MetalProcesado: material de metal procesado
    [SerializeField] private GameObject _metalProcesado;

    //Rapidez de trabajo: las unidades de tiempo en segundos que avanza el procesamiento del material
    [SerializeField] private float _workSpeed;

    //CompletionTime son las unidades de tiempo necesario para que el material se procese (segundos)
    [SerializeField] private int _completionTime = 6;

    //materialSource es una variable para indicar el material inicial
    [SerializeField] private Material _materialSource;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    //isWorking: es la booleana que indica si la soldadora está trabajando o no;
    private bool _isWorking;

    //progress: es la unidad que indica el progreso de la acción, cuanto lleva soldado un objeto
    private float _progress;

    //hasMetal: es una boleana que indica si la soldadora tiene el material de metal
    private bool hasMetal;

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
        _progress = 0;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_isWorking)
        {
            _progress += (Time.deltaTime * _workSpeed) / _completionTime;
            _materialSource.UpdateProgress(_progress);
        }
        if (_progress >= 1)
        {
            _progress = 0;
            Destroy(_materialSource.gameObject);
            GameObject metalProcesado = Instantiate(_metalProcesado, this.gameObject.transform.position, gameObject.transform.rotation);
            metalProcesado.transform.SetParent(this.transform);
            hasMetal = false;
            _isWorking = false;
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

    /// <summary>
    /// Cambia el tiempo máximo acorde a qué jugador interactua con la soldadora
    /// </summary>
    public void ChangeMaxTime(int time)
    {
        _completionTime = time;
    }
    /// <summary>
    /// Se llama a este metodo para hacer funcionar la soldadora si el jugador le da a la j teniendo a la soldadora como actualmesa
    /// </summary>
    public void TurnOnWelder() 
    {

        if (hasMetal && transform.childCount == 1)
        {
            Debug.Log("encendiendo soldadora");
            _isWorking = true;
        }
    }
    /// <summary>
    /// Se llama a este metodo para hacer parar la soldadora si el jugador le da a la j teniendo a la soldadora como actualmesa
    /// </summary>
    public void TurnOffWelder()
    {
        if(hasMetal && transform.childCount == 1)
        {
            Debug.Log("apagando soldadora");
            _isWorking = false;
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Cuando colisiona con la soldadora se activa el trigger
    /// </summary>
    /// <param name="collision"></param>

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Material>() != null && collision.gameObject.GetComponent<Material>().MaterialType() == MaterialType.Metal)
        {
            _materialSource = collision.GetComponent<Material>();
            _progress = _materialSource.ReturnProgress();
            hasMetal = true;
        }
    }

    /// <summary>
    /// Cuando se aleja/ sale de la soldadora se activa el trigger
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Material>() != null)
        {
            _materialSource = null;
            hasMetal = false;
        }
    }

    #endregion   

} // class Soldadora 
// namespace
