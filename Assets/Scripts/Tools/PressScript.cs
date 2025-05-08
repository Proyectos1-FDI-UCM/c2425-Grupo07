//---------------------------------------------------------
// Se programa el funcionamiento de la prensa cuando un objeto con material es puesto en este
// devuelve después de x tiempo el objeto en su estado original, osea, vacío
// Liling Chen
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
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
   
    [SerializeField] private Objects CurrentObject; // Objeto actual en la prensa
    [SerializeField] private float PressingTime = 0f; // Tiempo actual de prensado
    [SerializeField] private float VelCompletion; //Unidad de progreso que se añade al material por segundo
    [SerializeField] private Image ProgressBarFill; // Referencia a la barra de progreso
    [SerializeField] private Canvas BarCanvasGroup;// Referencia al Canvas de la barra de progreso
    [SerializeField] private Animator PressAnimator; // Referencia al Animator de la prensa
    [SerializeField] private ParticleSystem Smoke; //Referencia a la partícula después de terminar el proceso de la prensa

    //Hecho por Guillermo
    [SerializeField] private bool OnTutorial; // Comprueba si está en el tutorial para activar una flecha
    [SerializeField] private GameObject ArrowIndicator; //Objecto que se activará al terminar de procesar el objeto
    /// <summary>
    /// Sonido que se reproduce cuando se termina de reiniciar un objeto.
    /// </summary>
    [SerializeField] private AudioClip ResetSFX;
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
    
    /// <summary>
    /// Componente que reproduce el sonido de la prensa mientras se está reiniciando un objeto
    /// </summary>
    private AudioSource _pressAudioSource;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    void Start()
    {
        _pressAudioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_isPressing)
        {
            PressInProcess();
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
    /// Método que asigna los atributos necesarios para que funcione la prensa, este llamará al script del playerVision
    /// para que inserte el objecto como child a la prensa, este tambien pondrá _isPressing en true, _isComplete en false
    /// y activa la barra de progreso
    /// </summary>
    /// <param name="item"></param>
    public void Drop(GameObject item)
    {
        if (item.GetComponent<Objects>() != null)
        {
            Objects objects = item.GetComponent<Objects>();
            if (!objects.ThereIsMaterial())
            {
                if (_pressAudioSource != null)
                {
                    _pressAudioSource.Play();
                }
                item.GetComponentInParent<PlayerVision>().Drop(true);
                CurrentObject = objects;
                CurrentObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
                _isPressing = true;
                PressAnimator.SetBool("working", true);
                
                BarCanvasGroup.gameObject.SetActive(true);
            }
            else Debug.Log("No se puede introducir este material en esta estacion de trabajo");
        }
    }
    /// <summary>
    /// Al llamar a este método, se pone la booleana de la prensa en falso, resetea el tiempo de la prensa y desactiva la 
    /// barra de progreso
    /// </summary>
    public void Pick()
    {
        if (_pressAudioSource != null)
        {
            _pressAudioSource.Stop();
        }
        CurrentObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
        BarCanvasGroup.gameObject.SetActive(false);
        PressingTime = 0f;
        PressAnimator.SetBool("working", false);
        
        ResetPress();
    } 
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
        if (_isPressing)
        {
            
            PressingTime += (Time.deltaTime / 100) * VelCompletion;
            
            if (ProgressBarFill != null)
            {
                ProgressBarFill.fillAmount = PressingTime;
                
            }
            
            if (PressingTime >= 1)
            {
                if (OnTutorial)
                {
                    ArrowIndicator.SetActive(true);
                }
                if (_pressAudioSource != null)
                {
                    _pressAudioSource.Stop();
                }
                if (Smoke != null) Instantiate(Smoke, transform.position, Quaternion.identity);
                ResetObject();
                _isPressing = false;
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
            PressAnimator.SetBool("working", false);
        }
        if (_pressAudioSource != null)
        {
            _pressAudioSource.PlayOneShot(ResetSFX);
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

        if (ProgressBarFill != null)
        {
            ProgressBarFill.fillAmount = 0f;
        }

        BarCanvasGroup.gameObject.SetActive(false);
    }
    #endregion

} // class PressScript 
// namespace
