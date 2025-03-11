//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Controla el Particle System para que siga al player de manera independiente, 
/// sin ser hijo de PickPos. Esto asegura que las partículas siempre sigan al player
/// pero no se vean afectadas por el movimiento del PickPos.
/// </summary>
public class FollowParticleSystem : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [Header("Configuración del Particle System")]
    [SerializeField] private Transform player;  // Referencia al transform del player.
    [SerializeField] private Vector3 offset;    // Offset para ajustar la posición de las partículas.

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // No hay atributos privados en este caso, ya que todo está en el Inspector.

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // Asegura que el Particle System sigue al player, con un offset (desplazamiento)
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Aquí se podrían añadir métodos públicos si fuera necesario.

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // No es necesario agregar métodos privados por ahora.

    #endregion
} // class FollowParticleSystem

