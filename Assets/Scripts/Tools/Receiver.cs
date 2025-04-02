//---------------------------------------------------------
// Sistema de Recepción y Entrega de Objetos
// Este script maneja la lógica para recibir objetos rotos y
// entregar objetos reparados en el juego.
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using

/// <summary>
/// Estados posibles del receptor:
/// - Receiving: Listo para recibir nuevos objetos rotos
/// - Delivering: Listo para aceptar objetos reparados
/// - Idle: Estado neutral, sin actividad
/// </summary>
public enum receiverState
{
    Receiving,
    Delivering,
    Idle
}
/// <summary>
/// Clase que gestiona la recepción y entrega de objetos en el juego.
/// Se encarga de:
/// - Recibir objetos rotos que necesitan reparación
/// - Verificar y aceptar objetos reparados
/// - Mostrar interfaz visual del estado actual
/// - Gestionar el límite de tareas activas
/// </summary>
public class Receiver : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Referencia a la acción de interacción del jugador
    /// </summary>
    [SerializeField] private InputActionReference InteractActionReference;

    /// <summary>
    /// Referencia al manager de nivel
    /// </summary>
    [SerializeField] private LevelManager _levelManager;

    /// <summary>
    /// Array de elementos UI que muestran información sobre los objetos cuando el jugador está mirando al recibidor
    /// </summary>
    [SerializeField] private GameObject[] ObjectsUI;

    /// <summary>
    /// Posición donde se crearán las tareas en la UI
    /// </summary>
    [SerializeField] private Transform TaskPosition;

    /// <summary>
    /// Lista de prefabs de objetos que pueden ser recibidos
    /// </summary>
    [SerializeField] private GameObject[] ReceivingObjects;

    /// <summary>
    /// Determina si el receptor funciona en modo infinito, en false, los arrays avanzan en orden, de 1 en uno, en true, el array se accede aleatoriamente.
    /// </summary>
    [SerializeField] private bool InfiniteMode;

    /// <summary>
    /// Indicador visual de entrega correcta
    /// </summary>
    [SerializeField] private GameObject _correctAlert;

    /// <summary>
    /// Indicador visual de entrega incorrecta
    /// </summary>
    [SerializeField] private GameObject _wrongAlert;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Referencia al sistema de visión del jugador
    /// </summary>
    private PlayerVision _playerVision;

    /// <summary>
    /// Objeto que está siendo entregado actualmente
    /// </summary>
    public Objects _deliveredObject;

    /// <summary>
    /// Estado actual del receptor
    /// </summary>
    private receiverState _state;

    /// <summary>
    /// Índice actual en el array de objetos por recibir
    /// </summary>
    public int _indexer = 0;

    /// <summary>
    /// UI que muestra el objeto actual en entrega
    /// </summary>
    private GameObject _actualDeliveryUI;



    /// <summary>
    /// Contador de tareas activas actualmente
    /// </summary>
    private int activeTasks = 0;

    /// <summary>
    /// Contador de pedidos entregados
    /// </summary>
    private int _deliveredObjectsNumber = 0;

    /// <summary>
    /// Contador de pedidos fallidos
    /// </summary>
    private int _failedDeliveriesNumber = 0;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se ejecuta cuando el script se habilita.
    /// Configura el sistema de entrada para la interacción del jugador.
    /// </summary>
    void Awake()
    {
        InteractActionReference.action.performed += ctx => HandleInput();
        InteractActionReference.action.Enable();
    }

    /// <summary>
    /// Inicializa los componentes necesarios y configura el estado inicial.
    /// - Encuentra la posición de los pedidos
    /// - Configura las alertas visuales
    /// - Inicia el sistema de UI de objetos
    /// - Establece el estado inicial como Idle
    /// </summary>
    private void Start()
    {
        TaskPosition = GameObject.Find("Pedidos").transform;
        _correctAlert.SetActive(false);
        _wrongAlert.SetActive(false);
        InstatiateObjectUI(false);
        _state = receiverState.Idle;
        _levelManager = FindAnyObjectByType<LevelManager>();
    }

    /// <summary>
    /// Maneja la interacción del jugador con el receptor.
    /// - En modo Receiving: Genera nuevos objetos para reparar
    /// - En modo Delivering: Procesa la entrega de objetos reparados
    /// Solo funciona cuando el jugador está mirando al recibidor
    /// </summary>
    private void HandleInput()
    {
        if (_playerVision.GetActualMesa() != null && _playerVision.GetActualMesa().GetComponent<Receiver>() != null)
        {
            if (_state == receiverState.Receiving)
            {
                if (_indexer < ReceivingObjects.Length)
                {
                    Receive();
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

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Cambia el estado actual del receptor y actualiza la UI correspondiente.
    /// </summary>
    /// <param name="newState">Nuevo estado a establecer</param>

    /// <summary>
    /// Obtiene el estado actual del receptor.
    /// </summary>
    /// <returns>Estado actual del receptor</returns>
    public receiverState getState()
    {
        return _state;
    }

    /// <summary>
    /// Establece la referencia al sistema de visión del jugador.
    /// </summary>
    /// <param name="script">Componente PlayerVision del jugador</param>
    public void GetPlayerVision(PlayerVision script) { _playerVision = script; }

    /// <summary>
    /// Verifica si un objeto está listo para ser entregado.
    /// Solo acepta objetos que estén completamente reparados.
    /// </summary>
    /// <param name="heldObject">Objeto a analizar</param>
    public void AnalizeDeliveredObject(GameObject heldObject)
    {
        if (heldObject.GetComponent<Objects>() != null)
        {
            Objects analized = heldObject.GetComponent<Objects>();
            if (!analized.IsCompleted())
            {
                _failedDeliveriesNumber++;
                Debug.Log("El objeto no está apto para la entrega");
            }
            else _deliveredObject = analized;
        }
    }

    /// <summary>
    /// Actualiza la referencia del objeto entregado directamente desde PlayerVision.
    /// Sigue el mismo patrón que OvenScript y SawScript.
    /// </summary>
    /// <param name="deliveredObject">El objeto que se está entregando o null si se retira</param>
    public void UpdateDeliveredObjectReference(GameObject deliveredObject)
    {
        if (deliveredObject != null && deliveredObject.GetComponent<Objects>() != null)
        {
            Objects analized = deliveredObject.GetComponent<Objects>();
            if (!analized.IsCompleted())
            {
                Debug.Log("El objeto no está apto para la entrega");
                _deliveredObject = null;
            }
            else
            {
                _deliveredObject = analized;
            }
        }
        else
        {
            _deliveredObject = null;
        }

        // Actualizar el estado visual según la nueva referencia
        if (_state == receiverState.Delivering)
        {
            SetDeliveryMode();
        }
    }

    /// <summary>
    /// Modifica el contador de subtareas activas.
    /// </summary>
    /// <param name="amount">Cantidad a añadir (puede ser negativa)</param>
    public void AddSubTaskCount(int amount)
    {
        activeTasks += amount;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Activa el modo de recepción de objetos.
    /// Muestra la UI correspondiente y oculta las alertas.
    /// </summary>
    public void SetReceivingMode()
    {
        _state = receiverState.Receiving;
        Debug.Log("Modo de recogida activado");
        _actualDeliveryUI.SetActive(true);
        _correctAlert.SetActive(false);
        _wrongAlert.SetActive(false);
    }

    /// <summary>
    /// Activa el modo de entrega de objetos.
    /// Muestra las alertas según si hay un objeto válido para entregar.
    /// </summary>
    public void SetDeliveryMode()
    {
        _state = receiverState.Delivering;
        _actualDeliveryUI.SetActive(false);
        if (_deliveredObject != null)
        {
            _correctAlert.SetActive(true);
            _wrongAlert.SetActive(false);
        }
        else
        {
            _correctAlert.SetActive(false);
            _wrongAlert.SetActive(true);
        }
    }

    /// <summary>
    /// Devuelve _deliveredObjectsNumber
    /// </summary>
    /// <returns></returns>
    public int GetDeliveredObjectsNumber()
    {
        return _deliveredObjectsNumber;
    }

    /// <summary>
    /// Devuelve _failedDeliveriesNumber
    /// </summary>
    /// <returns></returns>
    public int GetFailedDeliveriesNumber()
    {
        return _failedDeliveriesNumber;
    }

    /// <summary>
    /// Actualiza el índice del array de objetos.
    /// Si InfiniteMode es false, incrementa el índice secuencialmente.
    /// Si InfiniteMode es true, genera un índice aleatorio.
    /// </summary>
    private void UpdateIndexer()
    {
        if (!InfiniteMode)
        {
            _indexer++;
        }
        else
        {
            _indexer = Random.Range(0, ReceivingObjects.Length);
        }
    }

    /// <summary>
    /// Genera un nuevo objeto para reparar si hay espacio disponible.
    /// Limita el número de tareas activas a 5.
    /// </summary>
    private void Receive()
    {
        if (activeTasks < 5)
        {
            GameObject broken_object = Instantiate(ReceivingObjects[_indexer], transform.position, Quaternion.identity);
            broken_object.GetComponent<TaskManager>().GetReceiver(this);
            UpdateIndexer();
            InstatiateObjectUI(false);
            _playerVision.Pick(broken_object);
            broken_object.GetComponent<TaskManager>().AddTask(TaskPosition);
            SetDeliveryMode();
        }
        else Debug.Log("No puedes recibir más pedidos");
    }

    /// <summary>
    /// Procesa la entrega de un objeto reparado.
    /// Elimina el objeto entregado y actualiza la UI.
    /// </summary>
    private void Deliver()
    {
        _deliveredObject.gameObject.GetComponent<TaskManager>().EndTask(true); // termina la tarea satisfactoriamente.
        _playerVision.SetIsBeingPicked(false); // para que el jugador pueda soltar el objeto y no salten errores de nullreference :)
        Destroy(_deliveredObject.gameObject);
        _deliveredObjectsNumber++;
        SetReceivingMode();
        /* InstatiateObjectUI(true);*/ // Hago que se vea el siguiente pedido instanteneamente, se puede quitar sin problema.
                                       // aqui se pondrá el resto del codigo más adelante

    }
    public void AddMoney(int amount)
    {
        if (_levelManager != null)
        {
            _levelManager.SumMoney(amount);
        }
        else Debug.Log("No se ha encontrado el manager de nivel, recuerda asignarlo en el inspector");
    }

    /// <summary>
    /// Establece el modo inactivo del receptor.
    /// Oculta todas las UI y alertas.
    /// </summary>
    public void SetIdleMode()
    {
        _state = receiverState.Idle;
        _actualDeliveryUI.SetActive(false);
        _correctAlert.SetActive(false);
        _wrongAlert.SetActive(false);
    }

    /// <summary>
    /// Gestiona la UI de los objetos que se pueden recibir.
    /// </summary>
    /// <param name="visible">Determina si la UI debe ser visible</param>
    private void InstatiateObjectUI(bool visible)
    {
        if (!InfiniteMode) // Más adelante implementamos el modo infinito :) 
        {
            if (_actualDeliveryUI != null) Destroy(_actualDeliveryUI); // quita el cartel anterior
            _actualDeliveryUI = Instantiate(ObjectsUI[_indexer], transform); // Instancia el popUp visual del siguiente Objeto a reparar
            _actualDeliveryUI.SetActive(visible);
        }
    }
    #endregion   
} // class Receiver 
// namespace
