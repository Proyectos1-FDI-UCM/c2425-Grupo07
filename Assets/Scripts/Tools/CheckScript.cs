//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Guillermo Isaac Ramos Medina
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
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
    [SerializeField] Animator DoorToOpen;
    [SerializeField] GameObject AllowNextScene;
    [SerializeField] MaterialType GameObjectReceived;
    [SerializeField] bool IsFirstDoor;
    [SerializeField] AudioClip[] DoorSounds; // 0 abre 1 cierra
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    AudioSource _doorSource;
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
        _doorSource = DoorToOpen.gameObject.GetComponent<AudioSource>();
        if (IsFirstDoor)
        {
            DoorToOpen.SetTrigger("Open");
            if (AllowNextScene != null)
            {
                AllowNextScene.SetActive(true);
            }
            _doorSource.PlayOneShot(DoorSounds[0]);
        }
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
    private void OnTransformChildrenChanged()
    {
        if (transform.childCount > 0 && (transform.GetChild(0).GetComponent<Material>() != null &&
            transform.GetChild(0).GetComponent<Material>().MaterialTypeReturn() == GameObjectReceived ||
            transform.GetChild(0).GetComponent<Objects>() != null && transform.GetChild(0).GetComponent<Objects>().IsCompleted()))
        {
            DoorToOpen.SetTrigger("Open");
            _doorSource.PlayOneShot(DoorSounds[0]);
            if (AllowNextScene != null)
            {
                AllowNextScene.SetActive(true);
            }
        }
        if (GameObjectReceived == MaterialType.Otro && transform.childCount == 0)
        {
            DoorToOpen.SetTrigger("Close");
            _doorSource.PlayOneShot(DoorSounds[1]);
            if (AllowNextScene != null)
            {
                AllowNextScene.SetActive(false);
            }
        }
    }
    #endregion


    } // class CheckScript 
// namespace
