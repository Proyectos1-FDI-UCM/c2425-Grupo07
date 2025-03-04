//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using JetBrains.Annotations;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
/// 
public class PlayerVision : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private InputActionReference PickDropActionReference;
    [SerializeField] private InputActionReference InteractActionReference;
    [SerializeField] GameObject actualMesa;
    [SerializeField] GameObject lookedObject;
    [SerializeField] GameObject heldObject;
    [SerializeField] Transform PickingPos;
    [SerializeField] LayerMask detectedTilesLayer;
    [SerializeField] float centerOffset;
    [SerializeField] float circleRadius;
    [SerializeField] Color mesaTint = Color.yellow;
    [SerializeField] Color gizmosColor = Color.green;
    //las dejo serializadas de momento para hacer debug

    [SerializeField] float detectionRate;
    [SerializeField] float detectionTime;
    [SerializeField] private bool _onMesasRange = false;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints


    private GameObject player;

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
        player = GetComponentInParent<PlayerDash>().gameObject;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_onMesasRange)
        {
            detectionTime += Time.deltaTime;
            if (detectionTime > detectionRate)
            {
                CalculateNearest();
                detectionTime = 0;
            }
        }
    }
    private void CalculateNearest()
    {
        bool atLeastOneDetected = false;
        GameObject lastMesa = actualMesa;
        float nearestDistance = Mathf.Infinity;
        Collider2D[] colisiones = Physics2D.OverlapCircleAll((Vector2)(player.transform.position + transform.up * centerOffset), circleRadius, detectedTilesLayer);
        foreach (Collider2D collider in colisiones)
        {
            float colliderDistance = Vector3.Distance(collider.transform.position, player.transform.position);
            if (colliderDistance < nearestDistance)
            {
                atLeastOneDetected = true;
                actualMesa = collider.gameObject;
                nearestDistance = colliderDistance;
            }
        }
        if (lastMesa != actualMesa && lastMesa != null) 
        {
            lastMesa.GetComponent<SpriteRenderer>().color = Color.white;
        }
        if (actualMesa != null)
        {
            actualMesa.GetComponent<SpriteRenderer>().color = mesaTint;
            if (!atLeastOneDetected)
            {
                actualMesa.GetComponent<SpriteRenderer>().color = Color.white;
                actualMesa = null;
            }

        }
    }
    private void OnDrawGizmos()
    {
        if (player == null) return;

        // Draw the search radius circle
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireSphere((Vector2)(player.transform.position + transform.up * centerOffset), circleRadius);

        // Highlight the nearest object
        if (actualMesa != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(player.transform.position, actualMesa.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "RangoDeMesas") _onMesasRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "RangoDeMesas")
        {
            _onMesasRange = false;
            if (actualMesa != null) actualMesa.GetComponent<SpriteRenderer>().color = Color.white;
        }
       
    }

    private void OnEnable()
    {
        PickDropActionReference.action.performed += PickDrop;
        PickDropActionReference.action.Enable();
        //InteractActionReference.action.performed += Interact;
        //InteractActionReference.action.canceled += Interact;
        InteractActionReference.action.Enable();

    }

    private void OnDisable()
    {
        PickDropActionReference.action.performed -= PickDrop;
        PickDropActionReference.action.Disable();
        //InteractActionReference.action.performed -= Interact;
        //InteractActionReference.action.canceled -= Interact;
        InteractActionReference.action.Disable();
    }

    public void PickDrop(InputAction.CallbackContext context)
    {
        ContentAnalizer();
        if (heldObject != null && lookedObject == null && actualMesa != null)
        {
            if (heldObject.GetComponent<Material>().matType != MaterialType.Arena && actualMesa.GetComponent<OvenScript>() != null||
                heldObject.GetComponent<Material>() && actualMesa.tag == "CraftingTable")
            { Debug.Log("No se puede dropear el material"); }
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

    //public void Interact(InputAction.CallbackContext context)
    //{
    //    if
    //}
    public void ContentAnalizer()
    {
        if (actualMesa != null)
        {
            if (actualMesa.transform.childCount >= 1 && actualMesa.GetComponentInChildren<Material>() != null)
            {
                lookedObject = actualMesa.GetComponentInChildren<Material>().gameObject;
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

    // Devuelve actualMesa
    public GameObject GetActualMesa()
    {
        return actualMesa;
    }

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
