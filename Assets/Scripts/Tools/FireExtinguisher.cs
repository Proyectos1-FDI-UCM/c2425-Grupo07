//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------


using UnityEngine;
using UnityEngine.InputSystem;

public class FireExtinguisher : MonoBehaviour
{
    [Header("Configuración del extintor")]
    [SerializeField] private ParticleSystem extinguisherParticles; // Sistema de partículas del extintor
    [SerializeField] private Collider2D fireCollider; // El Collider2D del fuego

    private bool _isUsing = false;

    private void Update()
    {
        // Activar el sistema de partículas solo cuando se use el extintor
        if (_isUsing && !extinguisherParticles.isPlaying)
        {
            extinguisherParticles.Play();
            Debug.Log("Partículas activadas");
        }
        else if (!_isUsing && extinguisherParticles.isPlaying)
        {
            extinguisherParticles.Stop();
            Debug.Log("Partículas detenidas");
        }
    }

    // Se activa al presionar la tecla asignada al extintor
    public void OnUseExtinguisher(InputAction.CallbackContext context)
    {
        _isUsing = context.performed;
        Debug.Log($"Extintor activado: {_isUsing}");
    }

    // Método que se ejecuta cuando las partículas colisionan con el fuego
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Fire"))
        {
            Debug.Log("¡Partículas detectadas!");
            // Llama al método del horno para apagar el fuego
            transform.parent?.SendMessage("OnExtinguish", SendMessageOptions.DontRequireReceiver);
            // Desactiva el fuego cuando las partículas colisionen
            other.SetActive(false);
        }
    }
}






