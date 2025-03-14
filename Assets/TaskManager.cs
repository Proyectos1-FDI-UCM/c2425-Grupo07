//---------------------------------------------------------
// Este script se encargara de establecer un vínculo entre el objeto que el jugador recibe y el panel de su tarea para que cuando el pedido se entregue, se elimine el panel de la tarea.
// Responsable de la creación de este archivo
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class TaskManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] GameObject HUDTaskPanelPrefab;

    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private GameObject _actualPanel;
    private GameObject _binAlert;
    private Receiver _receiver;
    

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BinScript>() != null)
        {
            _binAlert.SetActive(true);
        }
    }
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
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    
    public void AddTask(Transform position)
    {
        _actualPanel = Instantiate(HUDTaskPanelPrefab, position);
        _actualPanel.transform.SetParent(position, false);
        _binAlert = _actualPanel.transform.Find("BinAlert").gameObject;
        _binAlert.SetActive(false);
        _receiver.AddSubTaskCount(1);

    }
    public void EndTask()
    {
        Destroy(_actualPanel);
        _receiver.AddSubTaskCount(-1);
    }

    public void GetReceiver(Receiver receiver)
    {
        _receiver = receiver;
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class TaskManager 
// namespace
