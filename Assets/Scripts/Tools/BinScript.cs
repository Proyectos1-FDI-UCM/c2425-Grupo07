//---------------------------------------------------------
// Archivo para destruir materiales, pone como hijos a los materiales introducidos y lo destruye
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
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Destruye automáticamente cualquier objeto que se convierta en hijo de la basura.
    /// </summary>
  
    private void OnTransformChildrenChanged()
    {
        if (transform.childCount > 0)
        {
            GameObject child = transform.GetChild(0).gameObject;
            DestruirMaterial(child);
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Destruye el objeto de material inmediatamente.
    /// </summary>
    /// <param name="material">Objeto del material a destruir.</param>
    private void DestruirMaterial(GameObject material)
    {
        if (material.GetComponent<TaskManager>() != null)
        {
            material.GetComponent<TaskManager>().EndTask();
        }
        Destroy(material);
        Debug.Log("Material destruido en la basura.");
    }

    #endregion
} // class Basura
