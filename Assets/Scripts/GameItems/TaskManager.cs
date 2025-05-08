//---------------------------------------------------------
// Este script gestiona la relación entre objetos y su tarea en el HUD.
// Óliver Garcia Aguado
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using

/// <summary>
/// Clase que gestiona la relación entre objetos y sus paneles de tarea.
/// Se encarga de:
/// - Crear y destruir paneles de tarea en la UI
/// - Gestionar alertas visuales cuando el objeto está cerca del contenedor
/// - Mantener el conteo de tareas activas en el receptor
/// - Actualizar la barra de progreso según el tiempo restante del pedido
/// - Añadir dinero al recibidor según el estado del pedido
/// - Penalizar si el pedido no se entrega a tiempo o se tira a la basura.
/// </summary>
public class TaskManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Prefab del panel de tarea que se mostrará en el HUD
    /// </summary>
    [SerializeField] GameObject HUDTaskPanelPrefab; // Panel de tarea que se mostrará en el HUD cuando se acepte el pedido

    /// <summary>
    /// Tiempo que tardará el pedido en acabarse
    /// </summary>
    [SerializeField] int Tasktime; 

    /// <summary>
    /// Primer color de la barra de progreso (Pedido perfecto)
    /// </summary>
    [SerializeField] Color color1; 

    /// <summary>
    /// Segundo color de la barra de progreso (Pedido normal)
    /// </summary>
    [SerializeField] Color color2; 

    /// <summary>
    /// Tercer color de la barra de progreso (Pedido tarde)
    /// </summary>
    [SerializeField] Color color3; 

    /// <summary>
    /// Base de pago del pedido
    /// </summary>
    [SerializeField] int BasePayment;

    /// <summary>
    /// Si se encuentra en el tutorial
    /// </summary>
    [SerializeField] bool onTutorial;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Referencia al panel de tarea actual en la UI
    /// </summary>
    private GameObject _actualPanel;

    /// <summary>
    /// Alerta visual que se activa cuando el objeto está cerca de la basura
    /// </summary>
    private GameObject _binAlert;

    /// <summary>
    /// Referencia a la barra de progreso
    /// </summary>
    private Image _progressBar;

    /// <summary>
    /// Referencia al recibidor
    /// </summary>
    private Receiver _receiver;

    /// <summary>
    /// Variable estática para saber si el modo es infinito o no, se usa para penalizar al jugador por no entregar el pedido a tiempo o tirarlo a la basura.
    /// </summary>
    private static bool _isInfiniteMode = false;

    /// <summary>
    /// Referencia al script Objects
    /// </summary>
    private Objects _object;


    private void Start()
    {
        _object = gameObject.GetComponent<Objects>();

        // Por lo que sea los colores que se introducen por serializefield se ponen con alfa en 0,  con esto se evita ese problema.
        color1.a = 1f;
        color2.a = 1f;
        color3.a = 1f;

        // Se actualiza _isInfiniteMode para saber si el modo es infinito o no, se usa para penalizar al jugador por no entregar el pedido a tiempo o tirarlo a la basura.
        if (GameManager.Instance != null)
        {
            _isInfiniteMode = GameManager.Instance.isInfiniteMode();
        }
    }
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Activa la alerta visual si el objeto entra en la zona de la basura y la tarea no ha acabado.
    /// </summary>
    /// <param name="other">Collider que ha entrado en contacto</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BinScript>() != null && !IsTaskEnded())
        {
            _binAlert.SetActive(true);
        }
    }

    /// <summary>
    /// Desactiva la alerta visual si el objeto sale de la zona de la basura y la tarea no ha acabado.
    /// </summary>
    /// <param name="other">Collider que ha dejado de estar en contacto</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<BinScript>() != null && !IsTaskEnded())
        {
            _binAlert.SetActive(false);
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (IsTaskEnded())
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.83f, 0.24f, 0.24f, 1);
            _object.ChangeIndicatorsColor();
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Crea un nuevo panel de tarea en la UI y lo vincula con este objeto.
    /// Incrementa el contador de tareas activas en el receptor.
    /// </summary>
    /// <param name="position">Posición donde se creará el panel en la UI</param>
    public void AddTask(Transform position)
    {
        GetComponent<Objects>().SetCanBeSent(true);
        _actualPanel = Instantiate(HUDTaskPanelPrefab, position);
        _actualPanel.transform.SetParent(position, false);
        _binAlert = _actualPanel.transform.Find("BinAlert").gameObject;
        _progressBar = _actualPanel.transform.Find("ProgressBar").GetComponent<Image>();
        _binAlert.SetActive(false);
        _receiver.AddSubTaskCount(1);
        StartCoroutine(ProgressBar(Tasktime));
    }

        /// <summary>
        /// Elimina el panel de tarea asociado y decrementa el contador de tareas activas.
        /// Se llama cuando el objeto es entregado o destruido.
        /// Además impide que se pueda entregar el pedido o que se puedan añadir materiales si ya se ha acabado el tiempo.
        /// </summary>
    public void EndTask(bool delivered=false)
    {
        if (delivered && !IsTaskEnded() && !onTutorial) // le da el dinero correspondiente al estado con el que lo haya enviado (dependiendo del color de la barra) si está en verde el 100%, amarillo el 75%, el naranja el 50% y el rojo el 25%
        {

            if (!_isInfiniteMode) // Si se juega el nivel principal
            {
                //Los colores representan como de avanzado está el pedido (cuanto le queda)
                if (_progressBar.color == color1)
                {
                    Debug.Log("Entregado Correctamente, 100%");
                    _receiver.AddMoney(BasePayment, color1);                
                }
                else if (_progressBar.color == color2)
                {
                    Debug.Log("Entregado Correctamente, 75%");
                    _receiver.AddMoney((int)(BasePayment * 0.75f), color2);
                }
                else if (_progressBar.color == color3)
                {
                    Debug.Log("Entregado Correctamente, 25%");
                    _receiver.AddMoney((int)(BasePayment * 0.25f), color3);
                    
                }
            }
            else
            {
                float gainedTime = ConvertPaymentToTime(BasePayment); // Si se juega el nivel infinito, se convierte el dinero base a tiempo (no le afecta el color de la barra)
                Debug.Log(gainedTime);
                _receiver.AddTime(gainedTime); //Se añade el tiempo al juego
            }
            
        }
        else if (!IsTaskEnded() && !onTutorial) //penalización no conseguir entregar el pedido a tiempo o tirarlo a la basura
        {
            if (_isInfiniteMode)
            {
                Debug.Log("No entregado, penalización de 20 segundos");
                _receiver.AddTime(-ConvertPaymentToTime(100f));
            }
            else
            {
                Debug.Log("No entregado, penalización de 50€");
                _receiver.AddMoney(-50, Color.red);
            }
            _receiver.IncreaseFailedDeliveriesNumber();
        }
        GetComponent<Objects>().SetCanBeSent(false);
        Destroy(_actualPanel);
        _receiver.AddSubTaskCount(-1);
         StopAllCoroutines();
    }
    /// <summary>
    /// Comprueba si la tarea ha acabado verificando si el panel de tarea está activo o ha desaparecido.
    /// </summary>
    /// <returns>True si la tarea ha acabado, False en caso contrario.</returns>
    public bool IsTaskEnded()
    {
        return _actualPanel == null;
    }

    /// <summary>
    /// Metodo encargado de convertir una cántidad de dinero a tiempo
    /// de forma subjetiva y completamente arbitraria.
    /// No se tiene el cuenta el porcentaje del tiempo para la obtención de tiempo
    /// </summary>
    /// <param name="payAmount"></param>
    /// <returns></returns>
    private float ConvertPaymentToTime(float payAmount)
    {
        const float factorDeConversion = 0.2f;
        return payAmount * factorDeConversion;
    }

/// <summary>
/// Esta corrutina va decreciendo el tiempo de la barra de progreso y va cambiando el color de la barra según el tiempo restante del pedido.
/// </summary>
/// <param name="Tasktime">Tiempo que tardará el pedido en acabarse.</param>
/// <returns></returns>
    private IEnumerator ProgressBar(int Tasktime, bool infiniteMode = false)
    {
        float time = 0f;
        float cuarto = Tasktime / 4;
        float fillAmount = 1f;
        
        // Primera etapa - Pedido perfecto - Color verde (100% - 75%)
        _progressBar.color = color1;
        while (time < cuarto)
        {
            time += Time.deltaTime;
            fillAmount = 1 - (time / Tasktime);
            _progressBar.fillAmount = fillAmount;
            yield return null;
        }
        
        // Segunda etapa - Pedido normal - Color amarillo (75% - 50%)
        _progressBar.color = color2;
        while (time < cuarto * 3)
        {
            time += Time.deltaTime;
            fillAmount = 1 - (time / Tasktime);
            _progressBar.fillAmount = fillAmount;
            yield return null;
        }
        
        // Tercera etapa - Pedido tarde - Color rojo (25% - 0%)
        _progressBar.color = color3;
        while (time < Tasktime)
        {
            time += Time.deltaTime;
            fillAmount = 1 - (time / Tasktime);
            _progressBar.fillAmount = fillAmount;
            yield return null;
        }
        

        EndTask(false);// Penaliza al jugador por no entregar el pedido a tiempo, le resta 50 de dinero o 20 segundos si es el modo infinito
        // la primera boleana es para saber si se ha entregado el pedido o no, la segunda es para saber si es el modo infinito o no.
        
    }

    /// <summary>
    /// Recoge la referencia al recibidor que creó este objeto.
    /// </summary>
    /// <param name="receiver">Receptor que generó este objeto</param>
    public void GetReceiver(Receiver receiver)
    {
        _receiver = receiver;
    }
    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    #endregion   

} // class TaskManager 
// namespace
