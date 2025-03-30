//---------------------------------------------------------
// Archivo que gestiona la activación del extintor por el jugador y deshabilita su movimiento mientras lo usa.
// Cheng Xiang Ye Xu
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------


using UnityEngine;

/// <summary>
/// Clase que gestiona el uso del extintor por parte del jugador.
/// Se encarga de detectar la entrada del jugador y habilitar o deshabilitar
/// el movimiento y la visión cuando el extintor está en uso.
/// </summary>
public class PlayerFireExtinguisher : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private FireExtinguisher extinguisher; // Referencia al extintor
    private PlayerVision _playerVision; // Referencia al script de visión del jugador
    private PlayerMovement _playerMovement; // Referencia al script de movimiento del jugador

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se ejecuta al iniciar la escena. Busca el extintor y asigna referencias.
    /// </summary>
    private void Start()
    {
        extinguisher = FindObjectOfType<FireExtinguisher>(); // Buscamos el extintor en la escena

        if (extinguisher == null)
        {
            Debug.LogWarning("No se encontró un objeto con el script FireExtinguisher en la escena.");
        }

        // Inicializamos las referencias del jugador
        _playerVision = GetComponent<PlayerVision>();
        _playerMovement = GetComponent<PlayerMovement>();
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// Comprueba si el jugador ha mantenido el botón de interactuar para que cada frame 
    /// accione el exintor cuando no lo pulse, lo apaga
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.InteractIsPressed())
        {
            OnExtinguisherUsed();
        }
        else if (InputManager.Instance.InteractWasReleasedThisFrame())
        {
            OnExtinguisherStopped();
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Se llama cuando el jugador usa el extintor.
    /// Deshabilita el movimiento y la visión mientras el extintor está en uso.
    /// </summary>
    /// <param name="ctx">Contexto de la acción del Input System.</param>
    private void OnExtinguisherUsed()
    {
        if (extinguisher != null && extinguisher.IsExtinguisherAssociatedWithValidParent()) // Verificamos si el extintor es hijo del jugador
        {
            _playerMovement.enabled = false;
            _playerVision.enabled = false;
            extinguisher.OnUseExtinguisher(InputManager.Instance.InteractIsPressed()); // Activamos el extintor
        }
    }

    /// <summary>
    /// Se llama cuando el jugador deja de usar el extintor.
    /// Reactiva el movimiento y la visión cuando el extintor deja de usarse.
    /// </summary>
    /// <param name="ctx">Contexto de la acción del Input System.</param>
    private void OnExtinguisherStopped()
    {
        if (extinguisher != null && extinguisher.IsExtinguisherAssociatedWithValidParent()) // Verificamos si el extintor es hijo del jugador
        {
            _playerMovement.enabled = true;
            _playerVision.enabled = true;
            extinguisher.OnUseExtinguisher(InputManager.Instance.InteractIsPressed()); // Desactivamos el extintor
        }
    }

    #endregion
} // class PlayerFireExtinguisher

