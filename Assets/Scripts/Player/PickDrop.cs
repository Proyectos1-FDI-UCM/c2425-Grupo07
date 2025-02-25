//---------------------------------------------------------
// Script para coger o soltar objetos detectados mediante un rayo en una capa específica
// Cheng Xiang Ye Xu
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

public class PickDrop : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] private Transform _grabPoint; // Posición donde se sujetará el objeto
    [SerializeField] private float _rayDistance = 2f; // Distancia del rayo para detectar objetos
    [SerializeField] private LayerMask _pickupLayer; // Capa de los objetos recogibles

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados

    private GameObject _heldObject; // Objeto actualmente recogido
    private Rigidbody2D _heldObjectRb; // Rigidbody2D del objeto recogido

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space)) // Tecla para recoger/soltar
        //{
        //    if (_heldObject == null)
        //    {
        //        TryPickUp();
        //    }
        //    else
        //    {
        //        TryDrop();
        //    }
        //}
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Intenta recoger un objeto dentro del rango de interacción con un rayo.
    /// </summary>
    private void TryPickUp()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, _rayDistance, _pickupLayer);
        if (hit.collider != null)
        {
            _heldObject = hit.collider.gameObject;
            _heldObjectRb = _heldObject.GetComponent<Rigidbody2D>();

            if (_heldObjectRb != null)
            {
                _heldObjectRb.gravityScale = 0;
                _heldObjectRb.velocity = Vector2.zero;
                _heldObjectRb.simulated = false;
                _heldObject.transform.position = _grabPoint.position;
                _heldObject.transform.SetParent(_grabPoint);
            }
        }
    }

    /// <summary>
    /// Intenta soltar el objeto en una zona válida.
    /// </summary>
    private void TryDrop()
    {
        if (_heldObject != null)
        {
            _heldObject.transform.SetParent(null);
            _heldObjectRb.gravityScale = 1;
            _heldObjectRb.simulated = true;
            _heldObject = null;
            _heldObjectRb = null;
        }
    }

    #endregion
    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public void Interact()
    {
        if (_heldObject == null)
        {
            TryPickUp();
        }
        else
        {
            TryDrop();
        }
    }
    #endregion
}
