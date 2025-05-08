//---------------------------------------------------------
// Comprueba si el jugador cumple con las condiciones de compleción de la sala
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
/// Se encarga de comprobar que se ha cumplido con la condición de poder pasar a
/// la siguiente sala por el tipo de material del objeto o si ha sido reparado 
/// totalmente. Además, en la última sala, si no están todas las mesas de arriba
/// con un objeto a reparar no se podrá pasar a la siguiente.
/// </summary>
public class CheckScript : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] Animator DoorToOpen; // La puerta que se abrirá al completarlo
    [SerializeField] GameObject AllowNextScene; // Si permite avanzar a la siguiente escena
    [SerializeField] MaterialType GameObjectReceived; //Tipo de objeto que abrirá la puerta
    [SerializeField] bool IsFirstDoor; // Si es la primera puerta esta se abre sola
    [SerializeField] AudioClip[] DoorSounds; // 0 abre 1 cierra
    [SerializeField] bool EsObjetoAReparar; // Si es un objeto a reparar necesita dos condiciones
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    AudioSource _doorSource; // El AudioSource de la puerta que se reproduce cuando se abra / cierra
    bool _isClosed=true; // Comprueba si la puerta está cerrada para no volver a reproducir un sonido
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// Recoge el AudioSource de la puerta que va a abrir y si es la primera puerta se abre sola.
    /// </summary>
    void Start()
    {
        _doorSource = DoorToOpen.gameObject.GetComponent<AudioSource>();
        if (IsFirstDoor)
        {
            _isClosed = false;
            DoorToOpen.SetBool("OpenNow", true);
            if (AllowNextScene != null)
            {
                AllowNextScene.SetActive(true);
            }
            _doorSource.PlayOneShot(DoorSounds[0]);
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
    /// <summary>
    /// Cuando una mesa ha recibido o ha dejado de tener comprueba:
    /// Si ha recibido un objeto, si es así comprueba si coincide con el material deseado o si 
    /// es un objeto que ha sido reparado (la batidora)
    /// Si ha dejado de tener un objeto reparable.
    /// </summary>
    private void OnTransformChildrenChanged()
    {
        if (transform.childCount > 0 && _isClosed&&(transform.GetChild(0).GetComponent<Material>() != null &&
            transform.GetChild(0).GetComponent<Material>().MaterialTypeReturn() == GameObjectReceived ||
            transform.GetChild(0).GetComponent<Objects>() != null && transform.GetChild(0).GetComponent<Objects>().IsCompleted()))
        {
            DoorToOpen.SetBool("OpenNow", true);
            _doorSource.PlayOneShot(DoorSounds[0]);
            if (AllowNextScene != null)
            {
                AllowNextScene.SetActive(true);
            }
            _isClosed = false;
        }
        if (!_isClosed &&(GameObjectReceived == MaterialType.Otro && transform.childCount == 0 || FindObjectOfType<PlayerManager>().transform.childCount>1))
        {
            DoorToOpen.SetBool("OpenNow", false);
            _doorSource.PlayOneShot(DoorSounds[1]);
            if (AllowNextScene != null)
            {
                AllowNextScene.SetActive(false);
            }
            _isClosed = true;
        }
    }
    #endregion


    } // class CheckScript 
// namespace
