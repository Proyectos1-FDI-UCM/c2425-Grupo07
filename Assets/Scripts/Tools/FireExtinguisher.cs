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
    [SerializeField] private ParticleSystem extinguisherParticles;
    [SerializeField] private Collider2D extinguisherTrigger;  // Área de acción del extintor

    private bool _isUsing = false;

    private void Update()
    {
        if (_isUsing && !extinguisherParticles.isPlaying)
        {
            extinguisherParticles.Play();  // Reproducimos las partículas del extintor
        }
        else if (!_isUsing && extinguisherParticles.isPlaying)
        {
            extinguisherParticles.Stop();  // Detenemos las partículas cuando no se usa
        }
    }

    // Se llama cuando el jugador usa el extintor (presiona el botón asignado)
    public void OnUseExtinguisher(InputAction.CallbackContext context)
    {
        _isUsing = context.performed;
    }

    // Verifica si el extintor está asociado con un padre válido (el jugador)
    public bool IsExtinguisherAssociatedWithValidParent()
    {
        // Verificamos si el objeto padre tiene la etiqueta "Player"
        Transform parentTransform = transform.parent;

        if (parentTransform != null)
        {
            return parentTransform.CompareTag("Player");
        }

        return false;  // Si no tiene un padre válido, no es un extintor asociado al jugador
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Si estamos usando el extintor y tocamos algo con la etiqueta "Fire"
        if (_isUsing && other.CompareTag("Fire"))
        {
            Debug.Log("¡Extinguiendo fuego!");
            other.gameObject.SetActive(false);  // Apagamos el fuego

            // Buscamos un horno en la escena y lo llamamos
            OvenScript horno = FindObjectOfType<OvenScript>();
            if (horno != null)
            {
                Debug.Log("Horno encontrado, llamando a OnExtinguish()");
                horno.OnExtinguish();  // Apagamos el horno
            }
            else
            {
                Debug.LogError("No se encontró el script OvenScript en la escena.");
            }
        }
    }
}






