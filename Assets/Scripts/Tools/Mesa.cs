//---------------------------------------------------------
// El Script permite diferenciar entre los distintos sitios en los que se pueden dejar los materiales
// Guillermo Isaac Ramos Medina
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Creo un enum con tres posibilidades, si es Conveyor el material se moverá. 
/// Si es Table el material solamente se podrá colocar si no hay un objeto ya colocado (incluye las basuras). 
/// Si es Tool solo se podrán colocar materiales concretos si no se está procesando ya uno.
/// </summary>
public class Mesa : MonoBehaviour
{
    /// <summary>
    /// Este enum clasifica los prefabs para luego se coloquen determinados objetos
    /// </summary>
    public enum TableType
    {
        Conveyor, Table, Tool
    }
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private TableType tabType; //Este enum sirve para que el jugador sepa diferenciar entre las distintas mesas
    Color ItemTint = Color.grey; // El color con el cual se tintará el item que esté siendo selecionada/vista por el jugador (_lookedObject)

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private GameObject _lookedObject; // El item que esté mirando el jugador se tintará de un color específico
    private bool _isLooked; // El item que esté mirando el jugador se tintará de un color específico

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
    /// Devuelve el tipo de mesa para que el playerVision las identifique
    /// </summary>
    /// <returns></returns>
    public TableType TableTypeReturn()
    {
        return tabType;
    }
    public void TintObject(GameObject collision)
    {
        _lookedObject = collision.gameObject;
        if (_lookedObject != null)
        {
            _lookedObject.GetComponent<SpriteRenderer>().color = ItemTint;
        }
    }
    public void UnTintObject(GameObject collision)
    {
        if (_lookedObject != null)
        {
            _lookedObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        _lookedObject = null;
    }
    public void IsBeingLooked(bool isLooked)
    {
        _isLooked = isLooked;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    private void OnTransformChildrenChanged()
    {
        if (_lookedObject != null)
        {
            UnTintObject(_lookedObject);
        }
        else if (_isLooked && transform.childCount > 0) 
        {
            _lookedObject = transform.GetChild(0).gameObject;
            TintObject(_lookedObject);
        }
    }
    #endregion   

} // class Mesa 
// namespace
