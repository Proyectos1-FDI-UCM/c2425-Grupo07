//---------------------------------------------------------
// Este es el script responsable de la identidad de cada material y de su progreso de procesado. 
// Óliver García Aguado
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using
/// <summary>
/// Este enum clasifica los prefabs para luego usarse en determinadas máquinas
/// </summary>
public enum MaterialType
{
    Arena, Cristal,
    MetalRoca, MetalMineral, MetalProcesado,
    Engranaje,
    Madera, MaderaProcesada,
    Otro,
}
/// <summary>
/// Esta clase es la responsable de el funcionamiento interno de los materiales, cada prefab de material viene establecido
/// con su propio enum dependiendo del material.
/// La clase contiene métodos públicos para almacenar y retornar el progreso de procesamiento del material.
/// Al actualizar el progreso se modifica la barra de progreso del material (bar.fillammount) en base al progreso que tenga en el momento de la actualización
/// En aspectos visuales, la actualización de la barra es inmediata (no se realiza ninguna animación) por lo que para obtener una animación fluida/interpolada 
/// se tendrá que obtener una referencia de la barra de progreso del material mediante el método publico ReturnProgressBar()
/// para después ser alterada a conveniencia.
/// </summary>
public class Material : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    // Hace que los métodos puedan acceder al tipo de Material 
    [SerializeField] private MaterialType matType; //Este enum sirve para que las herramientas sepan diferenciar entre los distintos materiales
    [SerializeField] private Image CompletionBar; //La barra de progreso del material
    [SerializeField] private Sprite[] MatState; //Sprite Procesado, 0 procesado, 1 quemado, 2 procesado final (para el metal)
    //[SerializeField] private Sprite BurntSprite; //Sprite Quemado

    /// <summary>
    /// Sonido que se reproducirá cuando un material se procese de la manera intencionada (no se quema)
    /// </summary>
    [SerializeField] private AudioClip goodCompletionSound;

    /// <summary>
    /// Sonido que se reproducirá cuando un material se queme
    /// </summary>
    [SerializeField] private AudioClip badCompletionSound;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private float _materialProgress; // El progreso de procesado del material

    //_UsedOnce sirve para determinar si el objeto tiene 0 de progreso para determinar si la barra debe ser visible o no en escena
    private bool _UsedOnce = false;

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
        UpdateBar();
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
    /// Actualiza el progreso de procesado del material, se almacenan y recogen valores entre 0 y 1 siendo 0 el objeto intacto y 1 el objeto ya procesado 
    /// </summary>
    /// <param name="progress"></param>
    public void UpdateProgress(float progress)
    {
        _UsedOnce = true;
        _materialProgress = progress;
        Debug.Log($"Progreso actualizado: {_materialProgress}");
        UpdateBar();
    }

    /// <summary>
    /// Devuelve el progreso de procesado que lleve el script del material y actualiza la variable de progreso de la herramienta a
    /// la que el jugador se haya acercado..
    /// </summary>
    public float ReturnProgress()
    {
        Debug.Log("Progreso aplicado");
        return _materialProgress;
    }
    /// <summary>
    ///Devuelve una referencia de la barra de progreso del material, esta referencia es utilizada para que la sierra pueda realizar la animación de procesado
    /// </summary>
    /// <returns></returns>
    public Image ReturnProgressBar()
    {
        return CompletionBar;
    }
    /// <summary>
    /// Devuelve el tipo de material, "otro" engloba elementos que no son materiales
    /// </summary>
    /// <returns></returns>
    public MaterialType MaterialTypeReturn()
    {
        return matType;
    }
    /// <summary>
    /// Procesa el material cambiando su sprite (el metal tendrá otro estado extra)
    /// Y su enum
    /// </summary>
    public void ProcessTheMaterial()
    {
        GetComponent<SpriteRenderer>().sprite = MatState[0];
        GetComponent<AudioSource>().PlayOneShot(goodCompletionSound);
        switch (matType)
        {
            case MaterialType.Arena:
                matType = MaterialType.Cristal;
                break;
            case MaterialType.MetalRoca:
                matType = MaterialType.MetalMineral;
                break;
            case MaterialType.MetalMineral:
                matType = MaterialType.MetalProcesado;
                GetComponent<SpriteRenderer>().sprite = MatState[2];
                break;
            case MaterialType.Madera:
                matType = MaterialType.MaderaProcesada;
                break;
        }
        ProcessHasEnded();
    }
    /// <summary>
    /// Funciona igual que si se procesa el material, los materiales que se queman tendrán un estado extra
    /// </summary>
    public void BurnTheMaterial()
    {
        GetComponent<SpriteRenderer>().sprite= MatState[1];
        GetComponent<AudioSource>().PlayOneShot(badCompletionSound);
        switch (matType)
        {
            case MaterialType.Arena:
                matType = MaterialType.Otro;
                break;
            case MaterialType.MetalMineral:
                matType = MaterialType.Otro;
                break;
        }
        ProcessHasEnded();
    }
    /// <summary>
    /// Al terminar el procesamiento de un material se pone en verde la barra para indicar que se vuelve a procesar
    /// El progreso se reiniciará y se desactivará su barra de progreso.
    /// </summary>
    public void ProcessHasEnded()
    {
        CompletionBar.color = Color.green;
        UpdateProgress(0);
        CompletionBar.gameObject.GetComponentInParent<Canvas>().enabled = false;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Se encarga de actualizar como de avanzada debe estar la barra del material, toma como argumento el progreso del material (valores entre 0 y 1) 
    /// </summary>
    private void UpdateBar()
    {
        if (CompletionBar != null) //No todos los materiales tienen una barra, esta condicional evita errores de tipo NullReferenceException
        {
            if (!_UsedOnce) CompletionBar.gameObject.GetComponentInParent<Canvas>().enabled = false;
            else
            {
                CompletionBar.gameObject.GetComponentInParent<Canvas>().enabled = true;
                CompletionBar.fillAmount = _materialProgress;
            }
        }
    }

    #endregion

} // class Material 
// namespace
