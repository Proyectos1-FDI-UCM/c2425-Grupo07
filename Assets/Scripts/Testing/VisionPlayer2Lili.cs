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

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            ContentAnalizer();
            if (heldObject != null && lookedObject == null && actualMesa != null) Drop(); // hay objeto en la mano
            else if (heldObject == null && lookedObject != null) Pick(); // no hay objeto en la mano
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

    public void InsertMaterial()
    {
        if (lookedObject.GetComponent<Objets>() != null)
        {
            bool materialAdded = lookedObject.GetComponent<Objets>().AddMaterial(heldObject);
            if (materialAdded)
            {
                heldObject.SetActive(false);
                heldObject = null; // El material ha sido introducido, por lo que ya no está en la mano.

            }
            else
            {
                Debug.Log("No se pudo añadir el material al objeto.");
            }
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
