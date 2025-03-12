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

    private bool _isUsing = false;

    private void Update()
    {
        // Activa el Particle System cuando el extintor está siendo usado
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

    // Detecta las colisiones con el fuego y lo apaga
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Fire"))
        {
            Debug.Log("Fuego apagado");
            other.SetActive(false); // Desactiva el objeto fuego (o realiza la acción de apagar)

            // Resetear el estado del horno
            Transform parent = other.transform.parent;
            if (parent != null)
            {
                parent.SendMessage("OnExtinguish", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
