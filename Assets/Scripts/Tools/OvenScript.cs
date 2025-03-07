//---------------------------------------------------------
// El horno deberá procesar un material que se ha insertado cuando vaya pasando un tiempo. Si pasa demasiado tiempo, el material se quema y sale fuego. Contiene:
// Contador de procesamiento de la arena que inicia si se coloca el material concreto
// Contador de quemado que empieza cuando se procesa un material
// Método que actualiza el progreso del material
// Guillermo 
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
/// Este script es el que procesa únicamente la arena y la convierte en cristal. 
/// Se tiene que inserta un objeto de arena soltándolo sobre el horno para que inicie el contador de progreso del material
/// Después de “n” segundos, se tendrá preparado un material procesado de “cristal”. 
/// Si el objeto procesado se queda un tiempo en el horno, se incendia esta estación de trabajo, 
/// Y el material se convierte en “Cristal roto”, el cual no tendrá ninguna utilidad y podrá ser descartado en la basura (después de quitar el fuego).
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

    // VelCompletion es la unidad de progreso que se añade al material por segundo, si VelCompletion == CompletionTime entonces el material tardaría 1 segundo en procesarse 
    [SerializeField] private float VelCompletion;
    // FlashImage es la imagen que aparece para advertir al jugador que se va a quemar un objeto
    [SerializeField] private GameObject FlashImage;
    // FireIco es la imagen temporal cuando se quema un objeto
    [SerializeField] private GameObject FireIco;
    // IsBurnt es el booleano que comprueba si el objeto se ha quemado para reiniciar el proceso
    [SerializeField] private bool IsBurnt = false;
    // StatesMat se especializa en almacenar los prefabs que aparecerán al procesar el material, 0 procesado, 1 quemado
    [SerializeField] private GameObject[] StatesMat;
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
    // Devuelve si el material se ha quemado
    public bool ReturnBurnt()
    {  return IsBurnt; }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Maneja cada barra del proceso del horno
    /// Si se completa la de procesamiento, ha terminado el procesamiento del material e inicia la de quemado con si no se retira a tiempo
    /// Mientras no se retire el material activa y desactiva la imagen de flash hasta que se quema y activa el fuego. Quemando el material
    /// </summary>
    void Processing()
    {
        if (_isProcessing && !IsBurnt && _matScr != null && _matScr.gameObject.GetComponent<Material>().matType == MaterialType.Arena && transform.childCount == 1)
        {
            _progress += (Time.deltaTime / 100) * VelCompletion;
            _matScr.UpdateProgress(_progress);
            if (_progress >= 1)
            {
                ProcessedMaterial();
            }
        }
        if (_hasFinished && !IsBurnt && transform.childCount == 1 && _matScr.gameObject.GetComponent<Material>().matType == MaterialType.Cristal)
        {
            _matScr.UpdateProgress(_progress);
            _timerBurn += Time.deltaTime;
            _progress = (_timerBurn / 100) * VelCompletion / 1.5f;
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
            if (_progress >= 1)
            {
                BurntMaterial();
            }
        }
        else
        {
            FlashImage.SetActive(false);
        }
    }

    /// <summary>
    /// Cuando se saca al material del horno termina todos los procesos anteriores y se cambia al material procesado
    /// </summary>
    void ProcessedMaterial()
    {
        Debug.Log("Se ha procesado el material");
        _progress = _timerBurn = _timerFlash = 0;
        _hasFinished = true;
        FlashImage.SetActive(false);

        Destroy(transform.GetChild(0).gameObject);
        GameObject child = Instantiate(StatesMat[0], transform.position, transform.rotation);
        child.transform.SetParent(this.transform);
        _matScr = child.GetComponent<Material>();
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
        _progress = 0;
        _isProcessing = false;
        //Se cambia el material a ceniza
        Destroy(transform.GetChild(0).gameObject);
        GameObject child = Instantiate(StatesMat[1], transform.position, transform.rotation);
        child.transform.SetParent(this.transform);
    }

    /// <summary>
    /// Detecta si se ha colocado un material en el horno y puede iniciar o pausar el proceso
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        //Se tiene que especificar en "Material" que es la arena
        if (other.gameObject.GetComponent<Material>() != null && other.gameObject.GetComponent<Material>().matType == MaterialType.Arena && transform.childCount == 0)
        {
            Debug.Log("Hay un material puesto");
            _matScr = other.gameObject.GetComponent<Material>();
            _progress = _matScr.ReturnProgress();
            _isProcessing = true;
        }
    }
    //Si se saca antes de tiempo pausa el proceso
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Material>() != null && (other.gameObject.GetComponent<Material>().matType == MaterialType.Cristal 
                                                                || other.gameObject.GetComponent<Material>().matType == MaterialType.Arena) && transform.childCount == 0)
        {
            Debug.Log("No hay un material puesto");
            FlashImage.SetActive(false);
            _matScr = null;
            _isProcessing = false;
        }
    }
    #endregion

} // class Horno 
// namespace
