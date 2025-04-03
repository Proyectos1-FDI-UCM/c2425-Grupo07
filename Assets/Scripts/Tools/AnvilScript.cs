//---------------------------------------------------------
// Funcionamiento de la soldadora
// Alicia Sanchez
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class AnvilScript : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

     // CompletionBarReference es la barra de compleción del material, se usa para la animación de su barra
    [SerializeField] private Image CompletionBarReference;

    //MetalProcesado: material de metal procesado
    [SerializeField] private GameObject _metalProcesado;

    //materialSource es una variable para indicar el material inicial
    [SerializeField] private Material _materialSource;

    // MaxClicks es el número de clicks necesario para completar el proceso de refinamiento
    [SerializeField] private float MaxClicks = 6f;

    // CurrentClicks es el número de clicks necesario para completar el proceso de refinamiento
    [SerializeField] private float CurrentClicks = 0f;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

  

    //hasMetal: es una boleana que indica si la soldadora tiene el material de metal
    private bool hasMetal;

    private bool _hasFinished;

    private float _pastClicks;

    private float _progress;


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
        CurrentClicks = 0;
        _progress = 0;
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
    /// Devuelve el número de clicks necesario para completar el proceso de refinamiento
    /// </summary>
    public float GetMaxClicks()
    {
        return MaxClicks;
    }

    /// <summary>
    /// Cambia el número de clicks máximo acorde a qué jugador interactua con ella
    /// </summary>
    public void ChangeMaxClicks(int clicks)
    {
        MaxClicks = clicks;
    }

    /// <summary>
    /// Devuelve el número de clicks que tiene actualmente el material
    /// </summary>
    public float GetCurrentClicks()
    {
        return CurrentClicks;
    }

       /// <summary>
    /// Click() suma 1 al número de clicks necesario para completar el proceso de refinamiento
    /// cada vez que se hace click sobre la sierra y actualiza la barra de compleción
    /// </summary>
    public void Click()
    {
        if(hasMetal)
        {
            
            if (CompletionBarReference != null && _progress < 1) // Es 1 ya que el progreso va de 0 a 1 (No es un magic number... creo...)
            {
                CurrentClicks++;
                _progress = CurrentClicks / MaxClicks;
                _materialSource.UpdateProgress(_progress);
                UpdateCompletionBar(MaxClicks, CurrentClicks, _pastClicks);
                _pastClicks = CurrentClicks;
            }
            else if (CompletionBarReference != null)
            {
                _materialSource.ProcessTheMaterial();
                CompletionBarReference.GetComponentInParent<Canvas>().enabled = false;
                hasMetal = false;
                _hasFinished = true;
                CurrentClicks = 0;
                _pastClicks = 0;
            }
        }
    }
        // Actualiza la barra de compleción del yunque
    private void UpdateCompletionBar(float _maxCompletion, float _currentCompletion, float _pastCompletion)
    {
        if (CompletionBarReference != null)
        {
            float _targetCompletion = _currentCompletion / _maxCompletion;
            _pastCompletion = _pastCompletion / _maxCompletion;
            CompletionBarReference.fillAmount = _currentCompletion / _maxCompletion;
            StartCoroutine(CompletionBarAnimation(_targetCompletion, _pastCompletion));
        }
    }

    // Hace la animación de rellenar la barra de compleción del yunque
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
    /// <summary>
    /// Devuelve una boleana que indica si la soldadora tiene metal dentro o no.
    /// </summary>
    /// <returns></returns>
    public bool GetHasMetal()
    {
        return hasMetal;
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
            if(material.MaterialTypeReturn() == MaterialType.MetalMineral)
            {
                item.GetComponentInParent<PlayerVision>().Drop(true);
                _materialSource = material;
                CompletionBarReference = _materialSource.ReturnProgressBar();
                _progress = _materialSource.ReturnProgress();
                CurrentClicks = (int)(_progress * MaxClicks);
                _pastClicks = CurrentClicks;
                hasMetal = true;
            }
            else Debug.Log($"No se puede introducir {material.MaterialTypeReturn()} en esta estacion de trabajo por que solo acepta Metal Mineral");
        }
    }
     /// <summary>
    /// Se encarga de actualizar las variables de la mesa de trabajo para cuando el jugador recoge el material procesado
    /// /// </summary>
    public void Pick()
    {
        if (_hasFinished)
        {
            _hasFinished = false;
            _materialSource = null;
            hasMetal = false;
        }
    }


    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class Soldadora 
// namespace
