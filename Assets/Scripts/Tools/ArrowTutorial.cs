//---------------------------------------------------------
// Indicador que se activa por fases en una de las salas del tutorial
// Guillermo Isaac Ramos Medina
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// Siempre que un objeto tenga este script significará que está en el
/// tutorial o en una zona para enseñar una mecánica. En el ArrowIndicator
/// se almacenarán las flechas que se quieran activar o desactivar
/// y se podrá acceder desde otro componente para activar/desactivar flechas
/// </summary>
public class ArrowTutorial : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] GameObject[] ArrowIndicator;//Se puede poner más de una flecha
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
   
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    /// <summary>
    /// Activa una determinada flecha
    /// </summary>
    /// <param name="arrowPlace">la flecha deseada por activar</param>
    public void ActiveArrow(int arrowPlace)
    {
        ArrowIndicator[arrowPlace].SetActive(true);
    }
    /// <summary>
    /// Desactiva una determinada flecha
    /// </summary>
    /// <param name="arrowPlace">la flecha deseada por desactivar</param>
    public void DeactivateArrow(int arrowPlace)
    {
        ArrowIndicator[arrowPlace].SetActive(false);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class ArrowTutorial 
// namespace
