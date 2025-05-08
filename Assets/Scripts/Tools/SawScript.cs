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
/// Esta clase se encarga de convertir el material de madera en madera procesada (procesar la madera).
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

    /// <summary>
    /// Madera es el GameObject correspondiente a la madera
    /// </summary>
    [SerializeField] private GameObject Madera;

    /// <summary>
    /// MaderaProcesada es el GameObject correspondiente a la madera procesada
    /// </summary>
    [SerializeField] private GameObject MaderaProcesada;

    /// <summary>
    /// HasWood determina si hay madera en la sierra (true) o no (false)
    /// </summary>
    [SerializeField] private bool HasWood = false;

    /// <summary>
    /// CompletionTime son las unidades de tiempo necesario para que el material se procese (segundos)
    /// </summary>
    [SerializeField] private int CompletionTime = 6;

    /// <summary>
    /// Animator es el animator que controla la animación de la sierra
    /// </summary>
    [SerializeField] private Animator Animator;

    /// <summary>
    /// SawSFX es el componente de audio responsable del sonido de la sierra cuando está en funcionamiento
    /// </summary>
    [SerializeField] private AudioSource SawSFX;

    /// <summary>
    /// Partículas de destello
    /// </summary>
    [SerializeField] private GameObject Sparks;

    /// <summary>
    /// Partículas de migas de madera
    /// </summary>
    [SerializeField] private ParticleSystem WoodPiece;

    /// <summary>
    /// GameObject que contiene el canvas con todas las indicaciones de la sierra
    /// </summary>
    [SerializeField] private GameObject IndicationsCanvas;

    /// <summary>
    /// GameObject con la indicación del material que procesa la sierra
    /// </summary>
    [SerializeField] private GameObject MaterialIndication;

    /// <summary>
    /// GameObject con la indicación del las teclas que se deben pulsar para utilizar la sierra
    /// </summary>
    [SerializeField] private GameObject ButtonsIndication;

    /// <summary>
    /// Booleana para hacer visible las indicaciones visuales de la sierra
    /// </summary>
    [SerializeField] private bool ShowIndications;

    /// <summary>
    /// Booleana para activar las indicaciones dinámicas, estas hacen que las indicaciones desaparezcan una vez el jugador cumpla el propósito de estas. 
    /// </summary>
    [SerializeField] private bool DynamicIndications;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    /// <summary>
    /// _progress es la unidad que indica el progreso de la acción, cuanto lleva soldado un objeto
    /// </summary>
    private float _progress;

    /// <summary>
    /// _materialSource es el material que hay en la sierra
    /// </summary>
    private Material _materialSource;

    /// <summary>
    /// _isWorking es la booleana que indica si la sierra está trabajando o no
    /// </summary>
    private bool _isWorking;

    /// <summary>
    /// _completionDelta es la constante por la cual se multiplica el tiempo transcurrido para el incremento del progreso
    /// </summary>
    private float _completionDelta;

    /// <summary>
    /// _firstDrop determina si es la primera vez que el jugador ha colocado un objeto en la sierra
    /// </summary>
    private bool _firstDrop = true;

    /// <summary>
    /// _firstInteraction determina si es la primera vez que el jugador interactúa con la sierra
    /// </summary>
    private bool _firstInteraction = true;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods are called the first time.
    /// Pone el progreso a 0, _isWorking a false, _materialSource a null, asigna la referencia a SawSFX,
    /// y muestra las indicaciones si corresponde.
    /// </summary>
    void Start()
    {
        _progress = 0;
        _isWorking = false;
        _materialSource = null;
        SawSFX = GetComponent<AudioSource>();
        if (!ShowIndications)
        {
            IndicationsCanvas.SetActive(false); // Esconde las indicaciones de la herramienta
            _firstInteraction=false;
            _firstDrop = false; 
            // Si no se van a ver las indicaciones, no son necesarias las booleanas para desativarlas.
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// Procesa la madera si _isWorking es true
    /// </summary>
    void Update()
    {
      if (_isWorking)
        {
            _completionDelta = 1f / CompletionTime;
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
        CompletionTime = time;
    }
  
    /// <summary>
    /// Este método es el encarga analizar el objeto que se le pasa como parámetro y colocar el material si es apto,
    /// además se encarga de establecer todas las variables necesarias de la mesa de trabajo correspondiente.
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
                if (_firstDrop && DynamicIndications)
                {
                    MaterialIndication.SetActive(false);
                    _firstDrop = false;
                }
            }
            else Debug.Log("No se puede introducir este material en esta estacion de trabajo");
        }
        
    }

    /// <summary>
    /// Se encarga de actualizar las variables de la sierra para cuando el jugador recoge el material de la sierra
    /// </summary>
    public void Pick()
    {
        _materialSource.GetComponent<SpriteRenderer>().sortingOrder = -1;
        _materialSource = null;
        HasWood = false;
        _isWorking = false;
        Animator.SetBool("working", false);
        if (SawSFX != null) SawSFX.Stop();
    }

    /// <summary>
    /// Procesa la madera (poniendo _isWorking a true), activa los efectos visuales y sonoros, y desactiva las indicaciones si están activas
    /// </summary>
    public void TurnOnSaw()
    {
        if (HasWood)
        {
            _isWorking = true;
            Sparks.gameObject.SetActive(true);
            WoodPiece.gameObject.SetActive(true);
            Animator.SetBool("working", true);
            if (SawSFX.clip != null)
            {
                SawSFX.Play();
            }
            if (_firstInteraction && DynamicIndications)
            {
                ButtonsIndication.SetActive(false);
                _firstInteraction = false;
            }
        }
    }

    /// <summary>
    /// Deja de procesar la madera (poniendo _isWorking a false) y desactiva los efectos visuales y sonoros
    /// </summary>
    public void TurnOffSaw()
    {
        if (HasWood)
        {
            _isWorking = false;
            Sparks.gameObject.SetActive(false);
            WoodPiece.gameObject.SetActive(false);
            Animator.SetBool("working", false);
            if (SawSFX != null) SawSFX.Stop();
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
    /// Cuando se haya procesado por completo la madera, cambia su sprite al de madera procesada y cambia su enum al de madera procesada.
    /// También detiene los efectos visuales y sonoros.
    /// </summary>
    private void ProcessWood()
    {
        _materialSource.ProcessTheMaterial();
        Sparks.gameObject.SetActive(false);
        WoodPiece.gameObject.SetActive(false);
        _isWorking = false;
       HasWood = false;
       Animator.SetBool("working", false);
       if (SawSFX != null) SawSFX.Stop();
    }

    #endregion   

} // class SawScript 
// namespace
