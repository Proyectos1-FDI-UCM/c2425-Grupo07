//---------------------------------------------------------
// Archivo para destruir materiales, pone como hijos a los materiales introducidos y lo destruye.
// Cheng Xiang Ye Xu
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Clase que representa un contenedor de basura donde los jugadores pueden desechar materiales.
/// Cuando un material se establece como hijo de este objeto, se destruye automáticamente.
/// </summary>
public class BinScript : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // No hay atributos de inspector en esta clase.
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // No hay atributos privados en esta clase.
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se llama cuando la jerarquía de hijos del objeto cambia.
    /// Si se añade un objeto como hijo, se destruye automáticamente.
    /// </summary>
    private void OnTransformChildrenChanged()
    {
        if (transform.childCount > 0)
        {
            GameObject child = transform.GetChild(0).gameObject;
            DestruirMaterial(child);
        }
    }

    public void Drop(GameObject item)
    {
        if (item.GetComponent<FireExtinguisher>() == null)
        {
            item.transform.SetParent(transform);
            item.transform.localPosition = Vector3.zero; // Coloca el objeto en la posición del contenedor
        }
        else
        {
            Debug.LogWarning("No puedes tirar el extintor a la basura, ten cuidado amigo.");
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Destruye un objeto material si es necesario.
    /// Si el objeto tiene una tarea en curso, la finaliza antes de la destrucción.
    /// </summary>
    /// <param name="material">Objeto del material a destruir.</param>
    private void DestruirMaterial(GameObject material)
    {
        TaskManager taskManager = material.GetComponent<TaskManager>();

        if (taskManager != null && !taskManager.IsTaskEnded())
        {
            taskManager.EndTask(false);
        }
        material.GetComponent<ConveyorItems>().ToDestroy(gameObject);
        //Destroy(material);
        Debug.Log("Material destruido en la basura.");
    }

    #endregion
} // class BinScript
