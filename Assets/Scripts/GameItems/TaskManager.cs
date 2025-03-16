//---------------------------------------------------------
// Este script vincula los objetos que el jugador recibe con sus
// respectivos paneles de tarea en el HUD.
// Óliver Garcia Aguado
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
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
    /// Referencia al recibidor
    /// </summary>
    private Receiver _receiver;
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
        _binAlert.SetActive(false);
        _receiver.AddSubTaskCount(1);
    }

        /// <summary>
        /// Elimina el panel de tarea asociado y decrementa el contador de tareas activas.
        /// Se llama cuando el objeto es entregado o destruido.
        /// </summary>
    public void EndTask()
    {
        Destroy(_actualPanel);
        _receiver.AddSubTaskCount(-1);
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
