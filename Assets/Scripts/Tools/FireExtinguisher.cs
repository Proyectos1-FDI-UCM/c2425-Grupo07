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
    [SerializeField] private Collider2D extinguisherTrigger; // Área de acción del extintor

    private bool _isUsing = false;

    private void Update()
    {
        if (_isUsing && !extinguisherParticles.isPlaying)
        {
            extinguisherParticles.Play();
        }
        else if (!_isUsing && extinguisherParticles.isPlaying)
        {
            extinguisherParticles.Stop();
        }
    }

    public void OnUseExtinguisher(InputAction.CallbackContext context)
    {
        _isUsing = context.performed;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_isUsing && other.CompareTag("Fire"))
        {
            Debug.Log(" ¡Extinguiendo fuego!");
            other.gameObject.SetActive(false);

            // Busca directamente el script del horno en la escena
            OvenScript horno = FindObjectOfType<OvenScript>();
            if (horno != null)
            {
                Debug.Log(" Horno encontrado, llamando a OnExtinguish()");
                horno.OnExtinguish();
            }
            else
            {
                Debug.LogError(" No se encontró el script OvenScript en la escena.");
            }
        }
    }

}







