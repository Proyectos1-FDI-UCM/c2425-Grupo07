//---------------------------------------------------------
// El horno deberá procesar un material que se ha insertado cuando vaya pasando un tiempo. Si pasa demasiado tiempo, el material se quema y sale fuego.
// Guillermo Isaac Rmaos Medina
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Horno : MonoBehaviour
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

    // CompletionImage es la barra de compleción del proceso de refinamiento
    [SerializeField] private Image CompletionImage;
    [SerializeField] private Image QueamadoImage;
    [SerializeField] private float VelComplecion;
    [SerializeField] private GameObject FlashImage;
    // _nClicks es el número de clicks necesario para completar el proceso de refinamiento
    public int MaxClicks = 6;

    // _currentClicks es el número de clicks necesario para completar el proceso de refinamiento
    public int CurrentClicks = 0;

    // _carriesWood determina si el jugador porta madera (true) o no (false)
    public bool CarriesWood = true;

    // _isOnRange determina si el jugador está en el rango de interacción de la sierra (true) o no (false)
    public bool IsOnRange;

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

    // _maxDistanceSquared es el cuadrado de la distancia máxima que puede haber entre el jugador y la
    // sierra y que se siga considerando que el jugador está dentro del rango de interacción de la sierra
    private float _maxDistanceSquared = 2.5f;

    //
    public float timerComplecion = 0;
    public float timerQuemado = 0;
    public float timerFlash = 0;
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
        UpdateCompletionBar(MaxClicks, CurrentClicks, _pastClicks);
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
        if ((PlayerPosition.position - gameObject.transform.position).sqrMagnitude < _maxDistanceSquared)
        {
            timerComplecion += Time.deltaTime;
            IsOnRange = true;
        }

        else
        {
            IsOnRange = false;
        }
        CompletionImage.fillAmount = (timerComplecion / 100)*VelComplecion;
        if (CompletionImage.fillAmount >= 1)
        {
            timerQuemado += Time.deltaTime;
            QueamadoImage.fillAmount = (timerQuemado / 100) * VelComplecion/2;
            timerFlash += Time.deltaTime;
            if (timerFlash < 0.5/timerQuemado)
            {
                FlashImage.SetActive(true);
            }
            else if (timerFlash < 1/timerQuemado)
            {
                FlashImage.SetActive(false);
            }
            else
            {
                timerFlash = 0;
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
    public void Click()
    {
        CurrentClicks++;
        UpdateCompletionBar(MaxClicks, CurrentClicks, _pastClicks);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    private void UpdateCompletionBar(float _maxCompletion, float _currentCompletion, float _pastCompletion)
    {
        float _targetCompletion = _currentCompletion / _maxCompletion;
        _pastCompletion = _pastCompletion / _maxCompletion;
    }
    #endregion

} // class Horno 
// namespace
