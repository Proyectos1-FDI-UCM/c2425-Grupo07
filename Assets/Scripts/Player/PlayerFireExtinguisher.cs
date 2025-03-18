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

    [SerializeField] private InputActionReference ExtinguisherActionReference;
    private FireExtinguisher extinguisher;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    private void Start()
    {
        extinguisher = FindObjectOfType<FireExtinguisher>();

        if (extinguisher == null)
        {
            Debug.LogError("No se encontró un objeto con el script FireExtinguisher en la escena.");
        }
    }

    private void Awake()
    {
        ExtinguisherActionReference.action.performed += ctx => OnExtinguisherUsed(ctx);
        ExtinguisherActionReference.action.canceled += ctx => OnExtinguisherStopped(ctx);
        ExtinguisherActionReference.action.Enable();
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    private void OnExtinguisherUsed(InputAction.CallbackContext ctx)
    {
        // Verificar si el extintor existe y si es hijo de PickingPos
        Transform pickingPosTransform = transform.Find("PickingPos");

        if (extinguisher != null && pickingPosTransform != null && extinguisher.transform.IsChildOf(pickingPosTransform))
        {
            extinguisher.OnUseExtinguisher(ctx);
        }
    }

    private void OnExtinguisherStopped(InputAction.CallbackContext ctx)
    {
        // Verificar si el extintor existe y si es hijo de PickingPos
        Transform pickingPosTransform = transform.Find("PickingPos");

        if (extinguisher != null && pickingPosTransform != null && extinguisher.transform.IsChildOf(pickingPosTransform))
        {
            extinguisher.OnUseExtinguisher(ctx);
        }
    }

    #endregion
}


