//---------------------------------------------------------
// Este script sirve para que el jugador pueda hacer click sobre la sierra y que por tanto esta funcione
// Ferran
// Clank&Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.InputSystem;


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerSierra : MonoBehaviour
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

    // Referencia al script Sierra
    [SerializeField] private Sierra SierraClick;

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
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
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

    // Llama al método Click() del script Sierra cuando se hace click y el jugador está dentro del rango de interacción
    // de la sierra llevando madera y haya hecho menos clicks de los necesarios para completar el proceso de refinamiento
    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        //Debug.Log("Click");

        Vector2 _mousePosition = Mouse.current.position.ReadValue();

        Vector2 _worldPoint = Camera.main.ScreenToWorldPoint(_mousePosition);

        RaycastHit2D _hit = Physics2D.Raycast(_worldPoint, Vector2.zero);

        if (_hit.collider != null)
        {
            SierraClick = _hit.collider.gameObject.GetComponent<Sierra>();
            if (SierraClick != null && SierraClick.IsOnRange && SierraClick.CarriesWood && SierraClick.CurrentClicks < SierraClick.MaxClicks)
            {
                SierraClick.Click();
            }
        }
    }

    #endregion   

} // class PlayerSierra 
// namespace
