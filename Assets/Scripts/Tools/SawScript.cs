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

    // CompletionBarReference es la barra de compleción del material, se usa para la animación de su barra
    [SerializeField] private Image CompletionBarReference;

    // Madera es el GameObject correspondiente a la madera
    [SerializeField] private GameObject Madera;

    // MaderaProcesada es el GameObject correspondiente a la madera procesada
    [SerializeField] private GameObject MaderaProcesada;

    // MaxClicks es el número de clicks necesario para completar el proceso de refinamiento
    [SerializeField] private int MaxClicks = 6;

    // CurrentClicks es el número de clicks necesario para completar el proceso de refinamiento
    [SerializeField] private int CurrentClicks = 0;

    // CarriesWood determina si hay madera en la sierra (true) o no (false)
    [SerializeField] private bool HasWood = false;

    // Unpickable determina si el material que tiene la sierra como hijo se puede pickear (false) o no (true)
    [SerializeField] private bool Unpickable = false;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    // _pastClicks es el número anterior de clicks, se usa para la animación de la barra de compleción
    private int _pastClicks = 0;

    // _materialSource es el material que hay en la sierra
    private Material _materialSource;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        _pastClicks = CurrentClicks;

        if (transform.childCount == 1 && GetComponentInChildren<Material>().MaterialTypeReturn() == MaterialType.Madera)
        {
            HasWood = true;
        }

        else
        {
            HasWood = false;
        }

        if (CurrentClicks >= MaxClicks)
        {
            Unpickable = true;
            Invoke("ProcessWood", 0.35f);
            CurrentClicks = 0;
            _pastClicks = CurrentClicks;
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
    /// Click() suma 1 al número de clicks necesario para completar el proceso de refinamiento
    /// cada vez que se hace click sobre la sierra y actualiza la barra de compleción
    /// </summary>
    public void Click()
    {
        CurrentClicks++;
        _materialSource.UpdateProgress(CurrentClicks);
        if (CompletionBarReference != null && CurrentClicks <= MaxClicks)
        {
            UpdateCompletionBar(MaxClicks, CurrentClicks, _pastClicks);
        }
    }

    /// <summary>
    /// Devuelve el número de clicks necesario para completar el proceso de refinamiento
    /// </summary>
    public int GetMaxClicks()
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
    public int GetCurrentClicks()
    {
        return CurrentClicks;
    }

    // Devuelve la variable HasWood, que determina si hay madera en la sierra (true) o no (false)
    public bool GetHasWood()
    {
        return HasWood;
    }

    /// <summary>
    /// Devuelve la variable Unpickable, que determina si se puede coger el material de la sierra (false) o no (true)
    /// </summary>
    public bool GetUnpickable()
    {
        return Unpickable;
    }

    /// <summary>
    /// Actualiza la referencia del material directamente desde PlayerVision
    /// </summary>
    public void UpdateMaterialReference(Material material)
    {
        if (material != null && material.GetComponent<Material>().MaterialTypeReturn() == MaterialType.Madera)
        {
            _materialSource = material;
            CompletionBarReference = _materialSource.ReturnProgressBar();
            CurrentClicks = (int)_materialSource.ReturnProgress();
            _pastClicks = CurrentClicks;
        }
        else
        {
            _materialSource = null;
            CompletionBarReference = null;
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    // Procesa la madera destruyendo el material de madera e instanciando el material de madera procesada poniéndolo como hijo de la sierra
    private void ProcessWood()
    {
        Destroy(transform.GetChild(0).gameObject);
        GameObject child = Instantiate(MaderaProcesada, gameObject.transform.position, gameObject.transform.rotation);
        child.transform.SetParent(this.transform);
        Unpickable = false;
    }

    // Actualiza la barra de compleción de la sierra
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

    // Cuando un objeto entra en el área de colisión de la sierra, mira si es madera y en ese caso se lo
    // asigna a _materialSource y asigna la referencia de la barra de compleción y del progreso actual
   /* private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Material>() != null && collision.gameObject.GetComponent<Material>().MaterialType() == MaterialType.Madera)
        {
            _materialSource = collision.GetComponent<Material>();
            CompletionBarReference = _materialSource.ReturnProgressBar();
            CurrentClicks = (int)_materialSource.ReturnProgress();
            _pastClicks = CurrentClicks;
        }
    }

    // Cuando un objeto sale del área de colisión de la sierra, mira si es un material
    // y en ese caso se quita la referencia del material y de la barra de compleción
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Material>() != null)
        {
            _materialSource = null;
            CompletionBarReference = null;
        }
    } */

    #endregion   

} // class SawScript 
// namespace
