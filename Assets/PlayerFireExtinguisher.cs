//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Maneja la entrada del jugador para activar o desactivar el extintor.
/// Funciona mientras el jugador mantenga presionada la tecla asignada.
/// </summary>
public class PlayerFireExtinguisher : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    // Referencia a la acción del extintor en el Input System
    [SerializeField] private InputActionReference ExtinguisherActionReference;

    // Referencia al script del extintor
    private FireExtinguisher extinguisher;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Busca el objeto con el componente FireExtinguisher y lo asigna.
    /// </summary>
    private void Start()
    {
        extinguisher = FindObjectOfType<FireExtinguisher>();

        if (extinguisher == null)
        {
            Debug.LogError("❌ No se encontró un objeto con el script FireExtinguisher en la escena.");
        }
    }

    /// <summary>
    /// Se llama cuando el script se activa. 
    /// Se suscribe a los eventos de entrada del extintor.
    /// </summary>
    private void OnEnable()
    {
        ExtinguisherActionReference.action.performed += OnExtinguisherUsed;
        ExtinguisherActionReference.action.canceled += OnExtinguisherStopped;
        ExtinguisherActionReference.action.Enable();
    }

    /// <summary>
    /// Se llama cuando el script se desactiva. 
    /// Se desuscribe de los eventos de entrada del extintor.
    /// </summary>
    private void OnDisable()
    {
        ExtinguisherActionReference.action.performed -= OnExtinguisherUsed;
        ExtinguisherActionReference.action.canceled -= OnExtinguisherStopped;
        ExtinguisherActionReference.action.Disable();
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Se llama cuando el jugador **mantiene presionada** la tecla del extintor.
    /// </summary>
    private void OnExtinguisherUsed(InputAction.CallbackContext context)
    {
        if (extinguisher != null)
        {
            extinguisher.OnUseExtinguisher(context);
        }
    }

    /// <summary>
    /// Se llama cuando el jugador **suelta** la tecla del extintor.
    /// </summary>
    private void OnExtinguisherStopped(InputAction.CallbackContext context)
    {
        if (extinguisher != null)
        {
            extinguisher.OnUseExtinguisher(context);
        }
    }

    #endregion
}


// class PlayerFireExtinguisher 
// namespace
