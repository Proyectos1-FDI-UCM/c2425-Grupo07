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

    // CompletionImage es la barra de compleción del proceso de refinamiento
    [SerializeField] private Image CompletionImage;
    // BurningImage es la barra de quemado cuando se empieza a calentar de más un material
    [SerializeField] private Image BurningImage;
    // VelCompletion es la velocidad con la que avanza la barra de completion y de burning
    [SerializeField] private float VelCompletion;
    // FlashImage es la imagen que aparece para advertir al jugador que se va a quemar un objeto
    [SerializeField] private GameObject FlashImage;
    // FireIco es la imagen temporal cuando se quema un objeto
    [SerializeField] private GameObject FireIco;
    // IsBurnt es el booleano que comprueba si el objeto se ha quemado para reiniciar el proceso
    [SerializeField] private bool IsBurnt = false;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    //_timerCompletion es el contador del Tiempo que dura en terminar el proceso
    private float _timerCompletion = 0;
    //_timerBurn Tiempo que le tomará al horno para incendiarse y quemar el material procesado
    private float _timerBurn = 0;
    //_timerFlash Intervalo de tiempo en que la imagen se activa/desactiva
    private float _timerFlash = 0;
    //_isProcessing booleano que comprueba si se está procesando el material
    private bool _isProcessing = false;
    //_hasFinished booleano que comprueba si el proceso se ha terminado el proceso
    private bool _hasFinished = false;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// Actualiza siempre las barras de compleción del horno
    /// </summary>
    void Update()
    {
        CompletionBars();
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
    /// Maneja cada barra del proceso del horno
    /// Si se completa la de procesamiento, ha terminado el procesamiento del material e inicia la de quemado si no se retira
    /// Mientras no se retire el material activa y desactiva la imagen de flash hasta que se quema y activa el fuego. Quemando el material
    /// </summary>
    void CompletionBars()
    {
        if (_isProcessing && !IsBurnt && transform.childCount == 1)
        {
            _timerCompletion += Time.deltaTime;
            CompletionImage.fillAmount = (_timerCompletion / 100) * VelCompletion;
            if (CompletionImage.fillAmount >= 1)
            {
                _hasFinished = true;
                Debug.Log("Se ha procesado el material");
                _timerBurn += Time.deltaTime;
                BurningImage.fillAmount = (_timerBurn / 100) * VelCompletion / 2;
                _timerFlash += Time.deltaTime;
                if (_timerFlash < 0.5 / _timerBurn)
                {
                    FlashImage.SetActive(false);
                }
                else if (_timerFlash < 1 / _timerBurn)
                {
                    FlashImage.SetActive(true);
                }
                else
                {
                    _timerFlash = 0;
                }
            }
        }
        if (BurningImage.fillAmount >= 1)
        {
            BurntMaterial();
        }
    }

    /// <summary>
    /// Cuando se saca al material del horno termina todos los procesos anteriores y se cambia al material procesado
    /// </summary>
    void ProcessedMaterial()
    {
        _timerCompletion = _timerBurn = _timerFlash = 0;
        BurningImage.fillAmount = CompletionImage.fillAmount = 0;
        FlashImage.SetActive(false);
    }
    /// <summary>
    /// Si el objeto procesado se mantiene demasiado tiempo en el horno,
    /// se incendia esta estación de trabajo y el material se convierte en “ceniza”,
    /// el cual no tendrá ninguna utilidad y podrá ser descartado en la basura.
    /// </summary>
    void BurntMaterial()
    {
        IsBurnt = true;
        FireIco.SetActive(true);
        FlashImage.SetActive(false);
        _timerCompletion = 0;
    }

    /// <summary>
    /// Detecta si se ha colocado un material en el horno y puede iniciar o pausar el proceso
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        //Se tiene que especificar en "Material" que es la arena
        if (other.gameObject.GetComponent<Material>() != null)
        {
            Debug.Log("Hay un material puesto");
            _isProcessing = true;
        }
        if (other.gameObject.GetComponent<Material>() != null && _hasFinished)
        {
            Debug.Log("Se ha procesado el material");
            ProcessedMaterial();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Material>() != null)
        {
            Debug.Log("No hay un material puesto");
            _isProcessing = false;
        }
    }
    #endregion

} // class Horno 
// namespace
