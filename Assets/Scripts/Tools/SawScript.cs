//---------------------------------------------------------
// En este script se programa el funcionamiento de la sierra
// Ferran Escribá Cufí
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using System.Collections;
using UnityEngine.UI;


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// 
/// Esta clase se encarga de convertir el material de madera en madera procesada.
/// También hace que la barra de compleción tenga animación cada vez que se incrementa el progreso.
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

    // Madera es el GameObject correspondiente a la madera
    [SerializeField] private GameObject Madera;

    // MaderaProcesada es el GameObject correspondiente a la madera procesada
    [SerializeField] private GameObject MaderaProcesada;


    // CarriesWood determina si hay madera en la sierra (true) o no (false)
    [SerializeField] private bool HasWood = false;

    //CompletionTime son las unidades de tiempo necesario para que el material se procese (segundos)
    [SerializeField] private int _completionTime = 6;

    //_animator es el animator que controla la animación de la sierra
    [SerializeField] private Animator _animator;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    //progress: es la unidad que indica el progreso de la acción, cuanto lleva soldado un objeto
    private float _progress;

    // _materialSource es el material que hay en la sierra
    private Material _materialSource;

    //isWorking: es la booleana que indica si la soldadora está trabajando o no;
    private bool _isWorking;

    private float _completionDelta;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    void Start()
    {
        _progress = 0;
        _isWorking = false;
        _completionDelta = 1f / _completionTime;
        _materialSource = null;
    }

    void Update()
    {
      if (_isWorking)
        {
            _progress += _completionDelta * Time.deltaTime;
            _materialSource.UpdateProgress(_progress);
            if (_progress >= 1)
            {
                ProcessWood();
                _progress = 0;
            }
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
    /// Devuelve la variable HasWood, que determina si hay madera en la sierra (true) o no (false)
    /// </summary>
    /// <returns>Devuelve la variable HasWood</returns>
    public bool GetHasWood()
    {
        return HasWood;
    }

    /// <summary>
    /// Cambia el tiempo máximo acorde a qué jugador interactua con la soldadora
    /// </summary>
    public void ChangeMaxTime(int time)
    {
        _completionTime = time;
    }
  
    /// <summary>
    /// Este método es el encarga analizar el objeto que se le pasa como parámetro y colocar el material si es apto, además se encarga de establecer todas las variables necesarias de la mesa de trabajo correspondiente.
    /// </summary>
    /// <param name="item"></param>
    public void Drop(GameObject item)
    {
        if (item.GetComponent<Material>() != null)
        {
            Material material = item.GetComponent<Material>();
            if (material.MaterialTypeReturn() == MaterialType.Madera)
            {
                item.GetComponentInParent<PlayerVision>().Drop(true);
                _materialSource = material;
                _materialSource.GetComponent<SpriteRenderer>().sortingOrder = 1;
                _progress = _materialSource.ReturnProgress();
                HasWood = true;
            }
            else Debug.Log("No se puede introducir este material en esta estacion de trabajo");
        }
        
    }

    /// <summary>
    /// Se encarga de actualizar las variables de la mesa de trabajo para cuando el jugador recoge el material procesado
    /// /// </summary>
    public void Pick()
    {
        _materialSource.GetComponent<SpriteRenderer>().sortingOrder = -1;
        _materialSource = null;
        HasWood = false;
    }

    public void TurnOnSaw()
    {
        if (HasWood)
        {
           _isWorking = true;
           _animator.SetBool("working", true);
        }
    }
    public void TurnOffSaw()
    {
        if (HasWood)
        {
            _isWorking = false;
            _animator.SetBool("working", false);
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
    /// Procesa la madera destruyendo el material de madera e instanciando el material de madera procesada poniéndolo como hijo de la sierra
    /// </summary>
    private void ProcessWood()
    {
        _materialSource.ProcessTheMaterial();
       _animator.SetBool("working", false);
       _materialSource = null;
    }

    #endregion   

} // class SawScript 
// namespace
