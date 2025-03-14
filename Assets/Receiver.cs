//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using

public enum receiverState
{
    Receiving,
    Delivering,
    Idle
}
/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Receiver : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private InputActionReference InteractActionReference;
    [SerializeField] private GameObject[] ObjectsUI;
    [SerializeField] private GameObject[] ReceivingObjects;
    [SerializeField] private bool InfiniteMode;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private PlayerVision _playerVision;
    public Objects _deliveredObject;
    private receiverState _state;
    private int _indexer = 0; // esta variable lleva el tracking del array de objetos por recibir
    private GameObject _actualDeliveryUI;


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
    void Awake()
    {
        InteractActionReference.action.performed += ctx => HandleInput();
        InteractActionReference.action.Enable();
    }

    private void Start()
    {
        SetObjectUI(true);
        _actualDeliveryUI.SetActive(false);
        _state = receiverState.Idle;
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
    }

    private void HandleInput()
    {
        if (_playerVision.GetActualMesa().CompareTag("Recibidor"))
        {
            if (_state == receiverState.Receiving)
            {
                if (_indexer < ReceivingObjects.Length)
                {
                    SetObjectUI(true);
                    _playerVision.Pick(Instantiate(ReceivingObjects[_indexer]));
                    _indexer++;
                }
                else Debug.Log("No quedan más pedidos");
            }
            else if (_state == receiverState.Delivering)
            {
                if (_deliveredObject != null) Deliver();
                else Debug.Log("No puedes enviar esto");
            }
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
    public void setState(receiverState newState)
    {
        _state = newState;
        if (_state == receiverState.Receiving) SetReceivingMode();
        else if (_state == receiverState.Delivering) SetDeliveryMode();
        else SetIdleMode();
    }
    public receiverState getState()
    {
        return _state;
    }
    public void GetPlayerVision(PlayerVision script) { _playerVision = script; }
    public void AnalizeDeliveredObject(GameObject heldObject)
    {
        if (heldObject.GetComponent<Objects>() != null)
        {
            Objects analized = heldObject.GetComponent<Objects>();
            if (!analized.IsCompleted()) Debug.Log("El objeto no está apto para la entrega");
            else _deliveredObject = analized;
        }
    }
    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)


    // de momento dejo estos siguientes metodos anticipando que quizás añada más lineas,
    // si no es así, reemplazo la llamadas de estos metodos por la única instrucción.
    private void SetReceivingMode() 
    {
        Debug.Log("Modo de recogida activado");
        _actualDeliveryUI.SetActive(true);
        
    }
    
    private void SetDeliveryMode()
    {
        _actualDeliveryUI.SetActive(false);
    }
    private void Deliver()
    {
        Destroy(_deliveredObject.gameObject);
        SetObjectUI(false); // Hago que se vea el siguiente pedido instanteneamente, se puede quitar sin problema.
        // aqui se pondrá el resto del codigo más adelante
    }
    private void SetIdleMode()
    {
        _actualDeliveryUI.SetActive(false);
    }
    private void SetObjectUI(bool hide)
    {
        if (!InfiniteMode) // Más adelante implementamos el modo infinito :)
        {
            if (_actualDeliveryUI != null) Destroy(_actualDeliveryUI); // quita el cartel anterior
            _actualDeliveryUI = Instantiate(ObjectsUI[_indexer], transform); // Instancia el popUp visual del siguiente Objeto a reparar
            _actualDeliveryUI.SetActive(!hide);
        }
    }

    #endregion   

} // class Receiver 
// namespace
