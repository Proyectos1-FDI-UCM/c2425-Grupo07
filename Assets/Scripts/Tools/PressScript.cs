//---------------------------------------------------------
// Se programa el funcionamiento de la prensa cuando un objeto es puesto en este
// devuelve después de x tiempo el objeto en su estado original
// Liling Chen
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PressScript : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
   
    [SerializeField] private GameObject CurrentObject; // Objeto actual en la prensa
    [SerializeField] private float PressingTime = 0f; // Tiempo actual de prensado
    [SerializeField] private float VelCompletion; //Unidad de progreso que se añade al material por segundo
    [SerializeField] private Image ProgressBarFill; // Referencia a la barra de progreso
    [SerializeField] private Canvas BarCanvasGroup;// Referencia al Canvas de la barra de progreso
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints


    private bool _isPressing = false; // Indica si la prensa está activa.
    private bool _isComplete = false; //Indica si se ha completado el progreso

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
        if (transform.childCount > 0 && !_isComplete)
        {
            _isPressing = true;
            _isComplete = false;
            BarCanvasGroup.gameObject.SetActive(true);
        }
        else if(transform.childCount == 0)
        {
            _isPressing = false;
            BarCanvasGroup.gameObject.SetActive(false);
            PressingTime = 0f;
        }
        if (_isPressing)
        {
            PressInProcess();
        }
    }

    /// <summary>
    /// Detecta si un objeto entra en la zona de la prensa.
    /// Si el objeto tiene el componente "Objects" y la prensa no tiene hijos,
    /// lo asigna como el objeto actual.
    /// </summary>
    /// <param name="other">Colisión del objeto que entra en la prensa</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Objects>() != null && transform.childCount == 0)
        {
            Debug.Log("Objeto colocado en la prensa.");
            CurrentObject = other.gameObject; // Asignación del objeto actual.
        }
    }

    /// <summary>
    /// Detecta si un objeto sale de la zona de la prensa.
    /// Si el objeto tiene el componente "Objects" y la prensa no tiene hijos,
    /// reinicia el estado de la prensa.
    /// </summary>
    /// <param name="other">Colisión del objeto que entra en la prensa</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Objects>() != null && transform.childCount == 0)
        {
            Debug.Log("No hay objeto");
            ResetPress();
            _isComplete = false;
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
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Controla el proceso de la prensa, actualiza el tiempo de este
    /// y verifica si el proceso ha finalizado.
    /// </summary>
    private void PressInProcess()
    {
        if (CurrentObject != null)
        {
            PressingTime += (Time.deltaTime / 100) * VelCompletion;
            
            if (ProgressBarFill != null)
            {
                ProgressBarFill.fillAmount = PressingTime;
            }
            
            if (PressingTime >= 1)
            {
                _isComplete = true;
                ResetObject();
            }
        }
    }

    /// <summary>
    /// Devuelve el objeto a su estado original y reinicia la prensa.
    /// </summary>
    private void ResetObject()
    {
        if (CurrentObject != null)
        {
            CurrentObject.GetComponent<Objects>().ResetObject();

            ResetPress();
        }
    }

    /// <summary>
    /// Reinicia el estado de la prensa, eliminando el objeto actual
    /// y reinicia el tiempo de este.
    /// </summary>
    private void ResetPress()
    {
        CurrentObject = null;
        PressingTime = 0f;
        _isPressing = false;


        if (ProgressBarFill != null)
        {
            ProgressBarFill.fillAmount = 0f;
        }

        BarCanvasGroup.gameObject.SetActive(false);
    }
    #endregion

} // class PressScript 
// namespace
