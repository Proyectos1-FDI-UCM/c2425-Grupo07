//---------------------------------------------------------
// Archivo que controla el uso del extintor, activando partículas y apagando el fuego al contacto.
// Cheng Xiang Ye Xu
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------


using UnityEngine;

/// <summary>
/// Clase que representa el funcionamiento de un extintor en el juego.
/// Permite al jugador activar el extintor para apagar fuegos y afectar elementos en la escena.
/// </summary>
public class FireExtinguisher : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [Header("Configuración del extintor")]
    [SerializeField] private ParticleSystem extinguisherParticles; // Sistema de partículas del extintor
    [SerializeField] private Collider2D extinguisherTrigger; // Área de acción del extintor

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private bool _isUsing = false; // Indica si el extintor está en uso

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se ejecuta en cada frame. Controla la activación de las partículas del extintor.
    /// </summary>
    private void Update()
    {
        if (_isUsing && !extinguisherParticles.isPlaying)
        {
            extinguisherParticles.Play(); // Inicia las partículas del extintor
        }
        else if (!_isUsing && extinguisherParticles.isPlaying)
        {
            extinguisherParticles.Stop(); // Detiene las partículas del extintor
        }
    }

    /// <summary>
    /// Se ejecuta cuando el extintor entra en contacto con otro collider.
    /// Si el objeto tocado es fuego, lo apaga y notifica al horno.
    /// </summary>
    /// <param name="other">Collider del objeto en contacto con el extintor.</param>
    private void OnTriggerStay2D(Collider2D other)
    {
        if (_isUsing && other.CompareTag("Fire"))
        {
            Debug.Log("¡Extinguiendo fuego!");
            other.gameObject.SetActive(false); // Apaga el fuego

            // Busca un horno en la escena y ejecuta su método de apagado
            OvenScript horno = FindObjectOfType<OvenScript>();
            if (horno != null)
            {
                Debug.Log("Horno encontrado, llamando a OnExtinguish()");
                horno.OnExtinguish();
            }
            else
            {
                Debug.LogError("No se encontró el script OvenScript en la escena.");
            }
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Se llama cuando el jugador usa el extintor.
    /// Activa o desactiva su uso dependiendo del contexto.
    /// </summary>
    public void OnUseExtinguisher(bool isPressed)
    {
        _isUsing = isPressed;
    }

    /// <summary>
    /// Verifica si el extintor está asociado a un jugador válido.
    /// </summary>
    /// <returns>True si el objeto padre es el jugador, de lo contrario False.</returns>
    public bool IsExtinguisherAssociatedWithValidParent()
    {
        Transform parentTransform = transform.parent;
        return parentTransform != null && parentTransform.CompareTag("Player");
    }

    #endregion
} // class FireExtinguisher
