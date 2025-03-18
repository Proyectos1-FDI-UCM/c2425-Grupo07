//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Guillermo
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
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
    [SerializeField] GameObject siguienteBelt;
    [SerializeField] float beltVel;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    [SerializeField]private Vector3 _direction = Vector3.up;

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
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
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
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Herramienta" && transform.parent.tag != "Player")
        {
            siguienteBelt = other.gameObject;
            AvanzaConParent();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Herramienta" && transform.parent.tag != "Player")
        {
            //transform.SetParent(siguienteBelt.transform);
            CompruebaParent();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Herramienta" && transform.parent.tag != "Player")
        {
            //transform.SetParent(siguienteBelt.transform);
        }
    }
    void CompruebaParent()
    {
        if (siguienteBelt.transform.childCount == 0)
        {
            transform.SetParent(siguienteBelt.transform);
            transform.position = siguienteBelt.transform.position;
            if (siguienteBelt.transform.rotation.y != transform.rotation.y)
            {
                transform.rotation = siguienteBelt.transform.rotation;
                _direction = siguienteBelt.transform.up;
            }
        }
    }
    void AvanzaConParent()
    {
        transform.Translate(_direction * -1 * Time.deltaTime * beltVel, Space.World);
        if (true)
        {
            Vector2.MoveTowards(transform.position, siguienteBelt.transform.position, Time.deltaTime * beltVel);
            //transform.position = siguienteBelt.transform.position;
            transform.SetParent(siguienteBelt.transform);
            if (Vector3.Distance(transform.position, siguienteBelt.transform.position) < 0.1)
            {
                _direction = siguienteBelt.transform.up;
                //transform.rotation = siguienteBelt.transform.rotation;
                //_direction = siguienteBelt.transform.rotation * -_direction;
            }
        }
        if (transform.position.y <= -1 || transform.position.x <= -1)
        {
            transform.SetParent(siguienteBelt.transform);
        }

    }
    void OnTransformChildrenChanged()
    {
        AvanzaConParent();
    }
    #endregion

} // class ConveyorItems 
// namespace
