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
    Metal, MetalProcesado,
    Engranaje,
    Madera, MaderaProcesada,
    Otro,
}
/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
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
    [SerializeField] private float _materialProgress; // El progreso de procesado del material
    [SerializeField] private Image CompletionBar; //La barra de progreso del material
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    //firstUse sirve para determinar si el objeto tiene 0 de progreso para determinar si la barra debe ser visible o no en escena
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
        UpdateBar();
    }

    /// <summary>
    /// Devuelve el progreso de procesado que lleve el script del material y actualiza la variable de progreso de la herramienta a la que el jugador se haya acercado..
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

    public MaterialType MaterialType()
    {
        return matType;
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
