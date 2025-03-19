//---------------------------------------------------------
// Este script vincula los objetos que el jugador recibe con sus
// respectivos paneles de tarea en el HUD.
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
/// </summary>
public class TaskManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Prefab del panel de tarea que se mostrará en el HUD
    /// </summary>
    [SerializeField] GameObject HUDTaskPanelPrefab;
    [SerializeField] int Tasktime;

    [SerializeField] Color color1;
    [SerializeField] Color color2;
    [SerializeField] Color color3;
     [SerializeField] Color color4;
     [SerializeField] int BasePayment;
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

    private void Start()
    {
        // Por lo que sea los colores que se introducen por serializefield se ponen con alfa en 0,  con esto se evita ese problema.
        color1.a = 1f;
        color2.a = 1f;
        color3.a = 1f;
        color4.a = 1f;
    }
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Activa la alerta visual si el objeto entra en la zona de la basura.
    /// </summary>
    /// <param name="other">Collider que ha entrado en contacto</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BinScript>() != null)
        {
            _binAlert.SetActive(true);
        }
    }

    /// <summary>
    /// Desactiva la alerta visual si el objeto sale de la zona de la basura.
    /// </summary>
    /// <param name="other">Collider que ha dejado de estar en contacto</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<BinScript>() != null)
        {
            _binAlert.SetActive(false);
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
        /// </summary>
    public void EndTask(bool delivered)
    {
        if (delivered) // le da el dinero correspondiente al estado con el que lo haya enviado (dependiendo del color de la barra) si está en verde el 100%, amarillo el 75%, el naranja el 50% y el rojo el 25%
        {
            if (_progressBar.color == color1)
            {
                Debug.Log("Entregado Correctamente, 100%");
                _receiver.AddMoney(BasePayment);
            }
            else if (_progressBar.color == color2)
            {
                Debug.Log("Entregado Correctamente, 75%");
                _receiver.AddMoney((int)(BasePayment * 0.75f));
            }
            else if (_progressBar.color == color3)
            {
                Debug.Log("Entregado Correctamente, 50%");
                _receiver.AddMoney((int)(BasePayment * 0.5f));
            }
            else if (_progressBar.color == color4)
            {
                Debug.Log("Entregado Correctamente, 25%");
                _receiver.AddMoney((int)(BasePayment * 0.25f));
            }
        }
        else //penalización no conseguir entregar el pedido a tiempo o tirarlo a la basura
        {
            Debug.Log("No entregado, penalización de 50€");
            _receiver.AddMoney(-50);
        }
        Destroy(_actualPanel);
        _receiver.AddSubTaskCount(-1);
         StopAllCoroutines();
    }
    public bool IsTaskEnded()
    {
        return _actualPanel == null;
    }

/// <summary>
/// Esta corrutina va decreciendo el
/// </summary>
/// <param name="Tasktime"></param>
/// <returns></returns>
    private IEnumerator ProgressBar(int Tasktime)
    {
        float time = 0f;
        float cuarto = Tasktime / 4;
        float fillAmount = 1f;
        
        // Primer cuarto - Color inicial
        _progressBar.color = color1;
        while (time < cuarto)
        {
            time += Time.deltaTime;
            fillAmount = 1 - (time / Tasktime);
            _progressBar.fillAmount = fillAmount;
            yield return null;
        }
        
        // Segundo cuarto - Color gris
        _progressBar.color = color2;
        while (time < cuarto * 2)
        {
            time += Time.deltaTime;
            fillAmount = 1 - (time / Tasktime);
            _progressBar.fillAmount = fillAmount;
            yield return null;
        }
        
        // Tercer cuarto - Color amarillo
        _progressBar.color = color3;
        while (time < cuarto * 3)
        {
            time += Time.deltaTime;
            fillAmount = 1 - (time / Tasktime);
            _progressBar.fillAmount = fillAmount;
            yield return null;
        }
        
        // Último cuarto - Color rojo
        _progressBar.color = color4;
        while (time < Tasktime)
        {
            time += Time.deltaTime;
            fillAmount = 1 - (time / Tasktime);
            _progressBar.fillAmount = fillAmount;
            yield return null;
        }
        
        EndTask(false);
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
