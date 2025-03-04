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
public class Basura : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Referencia al material que se destruirá si se establece como hijo de "Basura".
    /// </summary>
    [Tooltip("Referencia al material que se destruirá si es colocado en la basura.")]
    public GameObject materialPrefab;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Posición de la basura en el grid.
    /// </summary>
    private Vector2Int _posicionBasura;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Inicializa la posición de la basura en el grid.
    /// </summary>
    void Start()
    {
        _posicionBasura = ObtenerPosicionEnGrid(transform.position);
    }

    /// <summary>
    /// Detecta si un material entra en el área de la basura y lo destruye inmediatamente si está en la misma celda.
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Material>() != null)
        {
            Vector2Int posicionMaterial = ObtenerPosicionEnGrid(other.transform.position);

            if (posicionMaterial == _posicionBasura)
            {
                // Establecer el material como hijo de la basura
                other.transform.SetParent(this.transform);
                Debug.Log("Material colocado en la basura");
            }
        }
    }

    /// <summary>
    /// Detecta cuando un material sale del área de la basura.
    /// </summary>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Material>() != null)
        {
            Debug.Log("Material salió de la basura");
        }
    }

    /// <summary>
    /// Destruye los materiales que son hijos de la basura.
    /// </summary>
    void Update()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.GetComponent<Material>() != null)
            {
                DestruirMaterial(child);
            }
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Destruye el objeto de material inmediatamente después de establecerlo como hijo.
    /// </summary>
    /// <param name="material">Objeto del material a destruir.</param>
    private void DestruirMaterial(GameObject material)
    {
        Destroy(material);
        Debug.Log("Material destruido en la basura.");
    }

    /// <summary>
    /// Convierte una posición del mundo en coordenadas de la cuadrícula (grid).
    /// </summary>
    /// <param name="posicion">Posición en el mundo.</param>
    /// <returns>Coordenadas de la cuadrícula (grid).</returns>
    private Vector2Int ObtenerPosicionEnGrid(Vector3 posicion)
    {
        int x = Mathf.FloorToInt(posicion.x);
        int y = Mathf.FloorToInt(posicion.y);
        return new Vector2Int(x, y);
    }

    #endregion
} // class Basura



