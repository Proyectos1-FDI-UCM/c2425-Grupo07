//---------------------------------------------------------
// Cambia la preview de los paneles de los niveles dependiendo de qué nivel se trate
// Liling Chen
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.UI;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class ChangePreview : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private Image Preview; //Imagen a cambiar
    [SerializeField] private Sprite[] ImagePreview; //Array para enseñar la imabgen del nivel
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    [SerializeField] private string[] _allLevelNames; //array con los nombres de los niveles
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
        SearchNames();
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
    /// Obtiene todos los niveles y se guarda su string en una array
    /// </summary>
    public void SearchNames()
    {
        Level[] allLevels = FindObjectsOfType<Level>();
        _allLevelNames = new string[allLevels.Length];

        for (int i = 0; i < allLevels.Length; i++)
        {
            _allLevelNames[i] = allLevels[i].GetLevelName();
        }
    }

    /// <summary>
    /// Se busca la imagen correspondiente del nivel y lo cambia en el panel, es llamado desde el script de Level
    /// </summary>
    /// <param name="level"></param>
    public void SetImagePreview(Level level)
    {
        int i = 0;
        while (_allLevelNames[i] != level.GetLevelName() && i < _allLevelNames.Length)
        {
            i++;
        }

        Preview.sprite = ImagePreview[i];
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class ChangePreview 
// namespace
