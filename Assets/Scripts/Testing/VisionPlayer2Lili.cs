//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using JetBrains.Annotations;
using System.Diagnostics.Contracts;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
/// 
public class VisionPlayer2Lili : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] GameObject actualMesa;
    [SerializeField] GameObject lookedObject;
    [SerializeField] GameObject heldObject;
    [SerializeField] Transform PickingPos;
    [SerializeField] Color mesaTint;
    //las dejo serializadas de momento para hacer debug

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (actualMesa != null) actualMesa.GetComponent<SpriteRenderer>().color = Color.white;
        actualMesa = collision.gameObject;
        actualMesa.GetComponent<SpriteRenderer>().color = mesaTint;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject != actualMesa) collision.GetComponent<SpriteRenderer>().color = Color.white;
        else actualMesa = null;
    }

    /// <summary>
    /// ++(Drop) Si hay un objeto en la mano (heldObject) y hay una mesa interactiva (actualMesa),
    /// se intenta soltar el elemento:
    ///     Si el objeto en la mano es un material y la mesa actual es una mesa de trabajo, no se permite
    ///     soltar el material y muestra un mensaje por Debug. De lo contrario, suelta el objeto que tenga
    ///     en heldObject
    /// ++(InsertMaterial) Si hay un objeto en la mano (heldObject) y hay objeto en donde mira (lookedObject)
    /// se llama a InsertMaterial
    /// </summary>
    /// <param name="context"></param>
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            ContentAnalizer();
            if (heldObject != null && lookedObject == null && actualMesa != null)
            {
                if (heldObject.GetComponent<Material>() && actualMesa.tag == "CraftingTable")
                { Debug.Log("No se puede dropear el material en la mesa de trabajo"); }
                else Drop(); // hay objeto en la mano
            }
            else if (heldObject == null && lookedObject != null)
            {
                if (actualMesa.GetComponent<OvenScript>() != null && actualMesa.GetComponent<OvenScript>().ReturnBurnt())
                { Debug.Log("No se puede recoger el material"); }
                else
                {
                    Pick(); // no hay objeto en la mano
                }
            }
            else if (heldObject != null && lookedObject != null) InsertMaterial();
        }
    }
    public void Pick()
    {
        heldObject = lookedObject;
        heldObject.transform.position = PickingPos.position;
        heldObject.transform.SetParent(PickingPos);
        heldObject.transform.rotation = transform.rotation;
        lookedObject = null;
    }
    public void Drop()
    {
        heldObject.transform.position = actualMesa.transform.position;
        heldObject.transform.rotation = Quaternion.identity;
        heldObject.transform.SetParent(actualMesa.transform);
        heldObject = null;
    }

    /// <summary>
    /// Método que inserta el material al objeto llamando al AddMaterial del script de Objets, siempre y cuando si el lookedObject
    /// está en una mesa con el tag de "CraftingTable", también mira si el objeto añadido es otro objeto, si es así no se realizará
    /// el AddMaterial y si el objeto en el que se le añade el material está lleno, se notifica de dicho detalle.
    /// </summary>
    public void InsertMaterial()
    {
        if(heldObject.GetComponent<Objets>() && lookedObject.GetComponent<Objets>())
        {
            Debug.Log("No puedes insertar un objeto dentro de otro objeto");
        }
        else if (lookedObject.GetComponent<Objets>())
        {
            if (actualMesa != null && actualMesa.tag == "CraftingTable")
            {
                Objets objetoScript = lookedObject.GetComponent<Objets>();
                bool materialAdded = objetoScript.AddMaterial(heldObject);
                if (materialAdded)
                {
                    heldObject.SetActive(false);
                    heldObject = null; // El material ha sido introducido, por lo que ya no está en la mano.
                }
                else Debug.Log("No se pudo añadir el material al objeto");
            }
            else Debug.Log("Solo insertar materiales en objetos que estén en la mesa de trabajo");
        }   
    }

    public void ContentAnalizer()
    {
        if (actualMesa != null)
        {
            if (actualMesa.transform.childCount > 0)
            {
                lookedObject = actualMesa.transform.GetChild(0).gameObject;
            }
            else lookedObject = null;
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



    #endregion


}// class PlayerVision 
 // namespace
