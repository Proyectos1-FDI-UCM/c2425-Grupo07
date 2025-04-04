//---------------------------------------------------------
// Este scipt se encarga de mover al objeto que tiene el script en dirección de la cinta mecánica
// Guillermo Isaac Ramos Medina
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// 
/// Los objetos con este script serán transportados con una velocidad lineal por la cinta mecánica hasta alcanzar el final
/// de la misma donde se encontrará la basura. 
/// Si un material u objeto transportado por la cinta mecánica alcanza la basura, 
/// Este será desechado de igual forma que si el jugador descarta el objeto que lleve en sus manos al interactuar con ella.
/// El jugador podrá recoger y soltar elementos no estáticos (exceptuando el extintor) sobre la cinta mecánica.

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
    [SerializeField] GameObject NextBelt; // Detecta cuál sería la siguiente tile de cinta mecánica para moverse hacia ella
    [SerializeField] float BeltVel; // La velocidad apropiada a la cinta
    [SerializeField] bool enCinta; // La distancia hasta que se cambia de cinta
    [SerializeField] float BeltDistance; // La distancia hasta que se cambia de cinta
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private Vector3 _direction = Vector3.up; // Para que siempre siga la dirección hacia arriba del material
                                             // 90º Derecha, 0º Abajo, -90º Izquierda, 180º Arriba, en el Eje Z de la cinta
    private float _timerDeletion; // Tiempo que tarda en borrarse el material cuando toca el trigger de la basura
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    void Update()
    {
        if (enCinta)
        {
            transform.Translate(_direction * -1 * Time.deltaTime * BeltVel, Space.World);
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

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    // Detecta si el item está en una cinta fuera de las manos del jugador para que empieze el movimiento
    // Inicia un timer al tocar la basura para que si llega a un tiempo concreto se borra el item
    // Si no hay un objeto al final se mueve directamente a la siguiente cinta
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Mesa>() != null && 
            other.GetComponent<Mesa>().TableTypeReturn() == Mesa.TableType.Conveyor && transform.parent.GetComponent<PlayerVision>() == null)
        {
            NextBelt = other.gameObject;
            enCinta = true;
            AvanzaConParent();
        }
        else if (transform.parent.GetComponent<PlayerVision>() != null)
        {
            enCinta = false;
        }
        if (other.GetComponent<BinScript>() != null && NextBelt != null)
        {
            BeltVel = 0.1f;
            transform.SetParent(other.gameObject.transform);
        }
        if (other.gameObject == null)
        {
            transform.position = NextBelt.transform.position;
        }
    }
    // Avanza si llega al centro de la cinta y cambia la dirección si es distinta a la del objeto
    void AvanzaConParent()
    {
        if (Vector3.Distance(transform.position, NextBelt.transform.position) < 0.1 && _direction != NextBelt.transform.up)
        {
            _direction = NextBelt.transform.up;
            transform.position = NextBelt.transform.position;
            transform.SetParent(NextBelt.transform);
        }
        else if (Vector3.Distance(transform.position, NextBelt.transform.position) < BeltDistance)
        {
            transform.SetParent(NextBelt.transform);
        }
    }
    // Si se recoge el item la siguiente cinta será nula
    void OnTransformParentChanged()
    {
        NextBelt = null;
    }
    #endregion

} // class ConveyorItems 
// namespace
