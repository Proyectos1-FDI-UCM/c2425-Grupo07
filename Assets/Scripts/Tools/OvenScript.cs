//---------------------------------------------------------
// El horno deberá procesar un material que se ha insertado cuando vaya pasando un tiempo.
// Si pasa demasiado tiempo, el material se quema y sale fuego. Contiene:
// Contador de procesamiento de la arena que inicia si se coloca el material concreto
// Contador de quemado que empieza cuando se procesa un material
// Método que actualiza el progreso del material
// Guillermo Isaac Ramos Medina
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Collections;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// 
/// Este script es el que procesa la arena, la convierte en cristal y la roca de metal(Rmetal) lo convierte en un metal refinado(metalR). 
/// Se tiene que inserta un objeto de arena/Rmetal soltándolo sobre el horno para que inicie el contador de progreso del material
/// Después de “_n” segundos, se tendrá preparado un material procesado de “cristal” / "metalR". 
/// Si el objeto procesado se queda un tiempo en el horno, se incendia esta estación de trabajo, 
/// Y el material se convierte en “Cristal roto”/"metal quemado", el cual no tendrá ninguna utilidad y podrá ser descartado en la basura 
/// (después de quitar el fuego).
/// </summary>
public class OvenScript : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    // VelCompletion es la unidad de progreso que se añade al material por segundo, si VelCompletion == CompletionTime
    // entonces el material tardaría 1 segundo en procesarse 
    [SerializeField] private float VelCompletion;
    // FlashImage es la imagen que aparece para advertir al jugador que se va a quemar un objeto
    [SerializeField] private GameObject FlashImage;
    // FireIco es la imagen temporal cuando se quema un objeto
    [SerializeField] private GameObject FireIco;
    // IsBurnt es el booleano que comprueba si el objeto se ha quemado para reiniciar el proceso
    [SerializeField] public bool IsBurnt = false;

    //PressAnimator es el animator que controla la animación del horno
    [SerializeField] private Animator _animator;

    /// <summary>
    /// Componente encargado de repoducir el sonido de alerta, se una uno independiente para tener el control y conocimiento del sonido de alerta
    /// </summary>
    [SerializeField] private AudioSource AlertAudioSource;

    //Particulas del horno en función
    [SerializeField] private GameObject Smoke;

    //Particulas del horno cuando se esta quemando el objeto
    [SerializeField] private ParticleSystem SmokeBurn;

    /// <summary>
    /// GameObject que contiene el canvas con todas las indicaciones del horno
    /// </summary>
    [SerializeField] private GameObject IndicationsCanvas;

    /// <summary>
    /// GameObject con la indicación del material que procesa el horno
    /// </summary>
    [SerializeField] private GameObject MaterialIndication;

    /// <summary>
    /// Bolenana para hacer visible las indicaciones visuales del horno
    /// </summary>
    [SerializeField] private bool ShowIndications;

    /// <summary>
    /// Bolenana para activar las indicaciones dinamicas, estas hacen que las indicaciones desaparezcan una vez el jugador cumpla el proposito de estas. 
    ///</summary>
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

    //_timerBurn Tiempo que le tomará al horno para incendiarse y quemar el material procesado
    private float _timerBurn = 0;
    //_timerFlash Intervalo de tiempo en que la imagen se activa/desactiva
    private float _timerFlash = 0;

    private float _progress = 0;
    //_isProcessing booleano que comprueba si se está procesando el material
    private bool _isProcessing = false;
    //_hasFinished booleano que comprueba si el proceso se ha terminado el proceso
    private bool _hasFinished = false;
    //Recoge el script de material para acceder a su progreso
    private Material _matScr;

    /// <summary>
    /// Componente encargado de reproducir el sonido del horno mientras que está procesando.
    /// </summary>
    private AudioSource _furnaceAudioSource;

    /// <summary>
    /// Boleana que determina si es la primera vez que el jugador ha coloca un objeto en el yunque
    /// </summary>
    private bool _firstDrop = true;


    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    void Start()
    {
     _furnaceAudioSource = GetComponent<AudioSource>(); 
      if (!ShowIndications)
        {
            IndicationsCanvas.SetActive(false); // Esconde las indicaciones de la herramienta
            _firstDrop=false;
            // Si no se van a ver las indicaciones, no son necesarias las boleanas para desativarlos. 
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// Actualiza siempre las barras de compleción del horno
    /// </summary>
    void Update()
    {
        Processing();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    // Devuelve si el material se ha quemado para saber si se puede interactuar con él
    public bool ReturnBurnt()
    {  return IsBurnt; }
    // Cambia la velocidad del horno acorde a qué jugador interactua él
    public void ChangeVelocity(int vel)
    { VelCompletion = vel; }
    // Devuelve si se está procesando el horno para cambiarle la velocidad o no
    public bool ReturnInProgress()
    { return _isProcessing; }

    /// <summary>
    /// Este método es el encarga analizar el objeto que se le pasa como parámetro y colocar el material si es apto, además se encarga de establecer todas las variables necesarias de la mesa de trabajo correspondiente.
    /// </summary>
    /// <param name="item"></param>
    public void Drop(GameObject item)
    {
        if (item.GetComponent<Material>() != null)
        {
            Material material = item.GetComponent<Material>();
            if (material.MaterialTypeReturn() == MaterialType.Arena || material.MaterialTypeReturn() == MaterialType.MetalRoca)
            {
                item.GetComponentInParent<PlayerVision>().Drop(true);
                _matScr = material;
                _matScr.GetComponent<SpriteRenderer>().sortingOrder = 1;
                _progress = _matScr.ReturnProgress();
                _isProcessing = true;
                _animator.SetBool("working", true);
                if (_furnaceAudioSource != null) {
                _furnaceAudioSource.Play();
                }
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
    /// Se encarga de actualizar las variables de la mesa de trabajo para cuando el jugador recoge el material procesado
    /// /// </summary>
    public void Pick()
    {
        _matScr.GetComponent<SpriteRenderer>().sortingOrder = -1;
        if (_furnaceAudioSource != null) {
        _furnaceAudioSource.Stop();
        }
        if (_hasFinished)
        {
            _matScr.ProcessHasEnded();
        }
        _isProcessing = false;
        _hasFinished = false;
        Smoke.gameObject.SetActive(false);
        SmokeBurn.gameObject.SetActive(false);
        FlashImage.SetActive(false);
        _matScr = null;
        _animator.SetBool("working", false);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    ///Este método se ejecutará cuando el extintor lo active
    ///Enviará el mensaje indicado al gameManager y comprobará si está en el tutorial para ponerlos
    /// </summary>
    public void OnExtinguish()
    {
        if (IsBurnt)
        {
            IsBurnt = false;
            FireIco.SetActive(false);
            SmokeBurn.gameObject.SetActive(false);
            _hasFinished = false;  // Permite reiniciar el proceso
            Debug.Log("¡Horno apagado y listo para usar de nuevo!");

            GameManager.Instance.SetTutorialString("Nice! Now pick that material and throw it in the <color=\"green\">green bin<color=\"white\">, then go back to processing!");
        }
    }



    /// <summary>
    /// Maneja cada barra del proceso del horno
    /// Si se completa la de procesamiento, ha terminado el procesamiento del material e inicia la de quemado con si no se retira a tiempo
    /// Mientras no se retire el material activa y desactiva la imagen de flash hasta que se quema y activa el fuego. Quemando el material
    /// </summary>
    void Processing()
    {
        if (_isProcessing && !IsBurnt && _matScr != null && (_matScr.MaterialTypeReturn() == MaterialType.Arena || 
            _matScr.MaterialTypeReturn() == MaterialType.MetalRoca) && transform.childCount == 1)
        {
            _progress += (Time.deltaTime / 100) * VelCompletion;
            _matScr.UpdateProgress(_progress);
            Smoke.gameObject.SetActive(true);
            if (_progress >= 1)
            {
                ProcessedMaterial();
            }

        }
        else if (_hasFinished && !IsBurnt && transform.childCount == 1 && _matScr != null && (_matScr.MaterialTypeReturn() == MaterialType.Cristal ||
            _matScr.MaterialTypeReturn() == MaterialType.MetalMineral))
        {
            _matScr.UpdateProgress(_progress);
            _matScr.ReturnProgressBar().color = Color.red;
            _timerBurn += Time.deltaTime;
            _progress = (_timerBurn / 100) * VelCompletion / 1.5f;
            _timerFlash += Time.deltaTime;

            SmokeBurn.gameObject.SetActive(true);

            if (_timerFlash < 0.5f / _timerBurn)
            {
                FlashImage.SetActive(false);
            }
            else if (_timerFlash < 1f / _timerBurn)
            {
                FlashImage.SetActive(true);
                if (AlertAudioSource != null && !AlertAudioSource.isPlaying) // reproduzco el sonido solo si ha terminado de sonar el anterior para evitar metralleta de sonidos.
                {
                    AlertAudioSource.Play();
                }

            }
            else
            {
                _timerFlash = 0;
            }
            if (_progress >= 1)
            {
                BurntMaterial();
            }
        }
    }

    /// <summary>
    /// Cuando se saca al material del horno termina todos los procesos anteriores y se cambia al material procesado
    /// </summary>
    void ProcessedMaterial()
    {
        Debug.Log("Se ha procesado el material");
        _progress = _timerBurn = _timerFlash = 0;
        _matScr.ProcessTheMaterial();
        _hasFinished = true;
        FlashImage.SetActive(false);

    }
    /// <summary>
    /// Si el objeto procesado se mantiene demasiado tiempo en el horno,
    /// se incendia esta estación de trabajo y el material se convierte en "ceniza",
    /// el cual no tendrá ninguna utilidad y podrá ser descartado en la basura.
    /// Y enviará el mensaje indicado al gameManager y comprobará si está en el tutorial para ponerlos
    /// </summary>
    void BurntMaterial()
    {
        IsBurnt = true;
        FireIco.SetActive(true);
        Smoke.gameObject.SetActive(false);
        FlashImage.SetActive(false);
        _progress = 0;
        _isProcessing = false;
        //Se cambia el material a ceniza
        _matScr.BurnTheMaterial();
        _animator.SetBool("working", false);
        if (_furnaceAudioSource != null)
        {
            _furnaceAudioSource.Stop();
        }
        GameManager.Instance.SetTutorialString("Oh well... the oven is on fire, pick the Fire Extinguisher and interact near the oven with J key <sprite name=\"Jkey\"> or square button <sprite name=\"Square_Button\">.");
    }
    #endregion

} // class Horno 
// namespace
