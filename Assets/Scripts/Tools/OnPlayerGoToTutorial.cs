//---------------------------------------------------------
// Panel que se abrirá cuando el jugador quiera volver al tutorial
// Guillermo Isaac Ramos Medina
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// 
/// Si el jugador entra en colisión con la zona para ir al tutorial se abrirá un panel pausando el juego,
/// que hará que no se pueda pausar el juego. Se abrirá un panel para confirmar la decisión del jugador
/// </summary>
public class OpenGoToTutorial : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private PauseMenuManager _pauseMenu; // Menu de pausa
    private bool _openPanel = true; // Comprueba si el panel está abierto
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// Recoge el menú de pausa de la escena actual
    /// </summary>
    void Start()
    {
        _pauseMenu = FindObjectOfType<PauseMenuManager>().gameObject.GetComponent<PauseMenuManager>();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    /// <summary>
    /// Comprueba si el jugador ha entrado en la zona para abrir el panel de si quiere entrar al 
    /// tutorial
    /// </summary>
    /// <param name="other">un objeto de colisión</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerManager>() != null && _openPanel)
        {
            _pauseMenu.ToggleToTutorial();
            _openPanel = false;
        }
    }
    /// <summary>
    /// Comprueba si el jugador sigue en la zona para volver al tutorial, entonces no se abrirá el panel
    /// </summary>
    /// <param name="other">un objeto de colisión</param>
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<PlayerManager>() != null)
        {
            _openPanel = false;
        }
    }
    /// <summary>
    /// Si sale de la zona y vuelve a entrar se podrá volver a abrir el panel de confirmación de tutorial
    /// </summary>
    /// <param name="other">un objeto de colisión</param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerManager>() != null)
        {
            _openPanel = true;
        }
    }
    #endregion

} // class OnPlayerCollision 
// namespace
