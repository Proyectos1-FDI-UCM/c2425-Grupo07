//---------------------------------------------------------
// Este script sirve para que el jugador pueda interactuar con la sierra pulsando la tecla de accionado
// Ferran Escribá Cufí
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

//using UnityEditor.Callbacks;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// 
/// Esta clase se encarga de que el jugador pueda interactuar con la sierra de manera adecuada.
/// Solo puede interactuar si está pulsando la tecla para interactuar, la mesa a la que está mirando es la sierra y la sierra tiene madera no procesada.
/// </summary>
public class PlayerSaw : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Referencia al script SawScript
    /// </summary>
    [SerializeField] private SawScript SierraClick;

    /// <summary>
    /// Referencia al script PlayerVision
    /// </summary>
    [SerializeField] private PlayerVision Player;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    /// <summary>
    /// _playerVision sirve para llamar luego al script de PlayerVision
    /// </summary>
    private PlayerVision _playerVision;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 



    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// Se asignan las referencias a SawScript y PlayerVision
    /// </summary>
    void Start()
    {
        if (FindAnyObjectByType<SawScript>() != null)
        {
            SierraClick = FindAnyObjectByType<SawScript>();
        }
        _playerVision = GetComponent<PlayerVision>();
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// Comprueba si el jugador ha mantenido el botón de interactuar y se enciende cada vez que
    /// se deja pulsado, se apaga cuando no
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.InteractWasPressedThisFrame() && _playerVision.GetActualMesa() != null
            && _playerVision.GetActualMesa().GetComponent<SawScript>() != null)
        {
            TurnOn();
        }
        else if ((InputManager.Instance.InteractWasReleasedThisFrame() || InputManager.Instance.MovementVector != Vector2.zero)
            && _playerVision.GetActualMesa() != null && _playerVision.GetActualMesa().GetComponent<SawScript>() != null)
        {
            TurnOff();
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    /// <summary>
    /// Hecho por Guillermo, Cambia el script de la sierra actual por la que esté mirando ahora
    /// </summary>
    /// <param name="sierra">El nuevo script de la sierra </param>
    public void ChangeActualSierra(SawScript sierra)
    {
        if (SierraClick != sierra)
        {
            SierraClick = sierra;
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Llama al método TurnOnSaw() de SawScipt
    /// </summary>
    private void TurnOn()
    {
        Debug.Log("TurnOn");
        SierraClick.TurnOnSaw();
    }

    /// <summary>
    /// Llama al método TurnOffSaw() de SawScript
    /// </summary>
    private void TurnOff()
    {
        Debug.Log("TurnOff");
        SierraClick.TurnOffSaw();
    }
    #endregion   

} // class PlayerSaw 
// namespace
