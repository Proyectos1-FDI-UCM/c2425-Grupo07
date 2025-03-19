//---------------------------------------------------------
// Este scipt se encarga de mover al objeto que tiene el script en dirección de la cinta mecánica
// Guillermo
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// 
/// 
/// </summary>
public class ConveyorItems : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] GameObject NextBelt;
    [SerializeField] float BeltVel;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private Vector3 _direction = Vector3.up;
    private float _timerDeletion;
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

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Cinta" && transform.parent.tag != "Player")
        {
            NextBelt = other.gameObject;
            transform.Translate(_direction * -1 * Time.deltaTime * BeltVel , Space.World);
           
            AvanzaConParent();
        }
        if (other.gameObject.tag == "Basura" && NextBelt != null)
        {
            _timerDeletion += Time.deltaTime;
            if (_timerDeletion > 0.5f)
            {
                Destroy(gameObject);
            }
        }
        if (other.gameObject == null)
        {
            transform.position = NextBelt.transform.position;
        }
    }
    void AvanzaConParent()
    {
        if (Vector3.Distance(transform.position, NextBelt.transform.position) < 0.1 && _direction != NextBelt.transform.up)
        {
            _direction = NextBelt.transform.up;
            transform.position = NextBelt.transform.position;
            transform.SetParent(NextBelt.transform);
        }
        else if (Vector3.Distance(transform.position, NextBelt.transform.position) < 0.8)
        {
            transform.SetParent(NextBelt.transform);
        }
    }
    void OnTransformParentChanged()
    {
        NextBelt = null;
    }
    #endregion

} // class ConveyorItems 
// namespace
