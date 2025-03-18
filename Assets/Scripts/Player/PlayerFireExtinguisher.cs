//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFireExtinguisher : MonoBehaviour
{
    [SerializeField] private InputActionReference ExtinguisherActionReference;  // Acción del extintor
    private FireExtinguisher extinguisher;  // Referencia al extintor

    private PlayerVision _playerVision;
    private PlayerMovement _playerMovement;

    private void Start()
    {
        extinguisher = FindObjectOfType<FireExtinguisher>();  // Buscamos el extintor en la escena

        if (extinguisher == null)
        {
            Debug.LogError("No se encontró un objeto con el script FireExtinguisher en la escena.");
        }

        // Inicializamos las referencias del jugador
        _playerVision = GetComponent<PlayerVision>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Awake()
    {
        if (ExtinguisherActionReference == null || ExtinguisherActionReference.action == null)
        {
            Debug.LogError("ExtinguisherActionReference no está asignado o no tiene una acción.");
            return;
        }

        // Nos suscribimos a los eventos de la acción
        ExtinguisherActionReference.action.performed += ctx => OnExtinguisherUsed(ctx);
        ExtinguisherActionReference.action.canceled += ctx => OnExtinguisherStopped(ctx);
        ExtinguisherActionReference.action.Enable();
    }

    // Llamado cuando el jugador usa el extintor
    private void OnExtinguisherUsed(InputAction.CallbackContext ctx)
    {
        if (extinguisher.IsExtinguisherAssociatedWithValidParent())  // Verificamos si el extintor es hijo del jugador
        {
            _playerMovement.enabled = false;
            _playerVision.enabled = false;
            extinguisher.OnUseExtinguisher(ctx);  // Usamos el extintor
        }
    }

    // Llamado cuando el jugador deja de usar el extintor
    private void OnExtinguisherStopped(InputAction.CallbackContext ctx)
    {
        if (extinguisher.IsExtinguisherAssociatedWithValidParent())  // Verificamos si el extintor es hijo del jugador
        {
            _playerMovement.enabled = true;
            _playerVision.enabled = true;
            extinguisher.OnUseExtinguisher(ctx);  // Detenemos el uso del extintor
        }
    }
}
