//---------------------------------------------------------
// Este script sirve para que el jugador pueda interactuar con la sierra pulsando la tecla de accionado
// Ferran Escribá Cufí
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.InputSystem;


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// 
/// Esta clase se encarga de que el jugador pueda interactuar con la sierra de manera adecuada.
/// Solo puede interactuar si está mirando a la sierra llevando madera y habiendo hecho menos clicks de los necesarios para procesar la madera.
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

    // Referencia a la acción de click
    [SerializeField] private InputActionReference ClickActionReference;

    // Referencia al script SawScript
    [SerializeField] private SawScript SierraClick;

    // Referencia al script PlayerVision
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

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 



    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        if (GameObject.FindWithTag("Sierra") != null)
        {
            SierraClick = GameObject.FindWithTag("Sierra").GetComponent<SawScript>();
        }
        Player = GetComponent<PlayerVision>();
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

    private void OnEnable()
    {
        ClickActionReference.action.performed += OnClickPerformed;
        ClickActionReference.action.Enable();
    }

    private void OnDisable()
    {
        ClickActionReference.action.performed -= OnClickPerformed;
        ClickActionReference.action.Disable();
    }

    // Llama al método Click() del script Sierra cuando se hace click y el jugador está mirando a la sierra
    // llevando madera y haya hecho menos clicks de los necesarios para completar el proceso de refinamiento
    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        if (SierraClick != null && Player.GetActualMesa() != null && Player.GetActualMesa().CompareTag("Sierra") && SierraClick.GetHasWood() && !SierraClick.GetUnpickable() && SierraClick.GetCurrentClicks() < SierraClick.GetMaxClicks())
        {
            SierraClick.Click();
        }
    }

    #endregion   

} // class PlayerSaw 
// namespace
