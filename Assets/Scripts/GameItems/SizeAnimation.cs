//---------------------------------------------------------
// Altera y devuelve el tamaño o posición de un GameObject, ya sea UI o de la escena
// Guillermo Isaac Ramos Medina
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// En este script se altera y devuelve el tamaño (ya sea por RectTransform o Transform) del objeto 
/// que contiene el script.
/// Principalmente en la basura para disminuir los objetos y
/// en el levelmanager para el efecto de recogida de dinero
/// </summary>
public class SizeAnimation : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] RectTransform RecTransform; // Componente de los elementos de la UI que
                                                 // usaremos para cambiar y devolver posición
                                                 // y tamaño
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints


    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    /// <summary>
    /// Modifica el tamaño del rectTransform
    /// </summary>
    /// <param name="_rateAumenta">Es el valor x e y de lo que se cambiará el tamaño</param>
    public void SizeUI(float _rateAumenta)
    {
        RecTransform.sizeDelta = new Vector2(_rateAumenta, _rateAumenta);
    }
    /// <summary>
    /// Devuelve el tamaño del rectTransform
    /// </summary>
    /// <returns></returns>
    public float ReturnUISize()
    {
        return RecTransform.sizeDelta.x;
    }
    /// <summary>
    /// Cambia la posición del elemento del UI
    /// </summary>
    /// <param name="endpos">la posición final para ir</param>
    public void PosUI(Vector2 endpos)
    {
        RecTransform.anchoredPosition = endpos;
    }
    /// <summary>
    /// Devuelve la posición del elemento del UI
    /// </summary>
    /// <returns></returns>
    public Vector2 ReturnUIPos()
    {
        return RecTransform.anchoredPosition;
    }
    /// <summary>
    /// Devuelve la posición de un objeto de la escena
    /// </summary>
    /// <returns></returns>
    public float ReturnSize()
    {
        return transform.localScale.x;
    }
    /// <summary>
    /// Cambia el tamaño del transform de un elemento del escenario
    /// </summary>
    /// <param name="newSize">Es el valor x e y de lo que se multiplicará el tamaño</param>
    public void MultiplySize(float newSize)
    {
        transform.localScale *= newSize;
    }
    /// <summary>
    /// Cambia la posición del objeto de la escena
    /// </summary>
    /// <param name="newPos">la posición final para ir</param>
    public void SetPosition(Vector2 newPos)
    {
        transform.position = newPos;
    }
    /// <summary>
    /// Devuelve la posición del objeto de la escena
    /// </summary>
    /// <returns></returns>
    public Vector2 ReturnPosition()
    {
        return transform.position;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class SizeAnimation 
// namespace
