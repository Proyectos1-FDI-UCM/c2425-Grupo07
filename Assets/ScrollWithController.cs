//---------------------------------------------------------
// Este script permite navegar el menú desplegable con el mando
// Guillermo Ramos
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
// Añadir aquí el resto de directivas using


/// <summary>
/// Accederemos a la Clase de la UI que cada vez que se seleccione un item
/// para hacer una operación que se asignará a la barra de deslizamiento, 
/// cambiando su selección con el mando o teclas
/// </summary>
public class ScrollWithController : MonoBehaviour, ISelectHandler //Clase de la UI que cada vez que se seleccione un item 
                                                                    // Del dropdown se ejecuta el código
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private ScrollRect scrollRect; // Contiene la información para deslizar
    private float scrollPosition = 1; // la posición que se seleccionará al principio
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// En resumen, accedemos a scrollrect (barra de deslizamiento), a los items para desplazarse y 
    /// el item actual para hacer una operación que se asignará a la barra de deslizamiento cada 
    /// vez que se selecciona
    /// </summary>
    void Start()
    {
        scrollRect = GetComponentInParent<ScrollRect>(true); // Accedemos al scrollRect si existe
        int childCount = scrollRect.content.transform.childCount - 1; // Accedemos a cuántos items hay para movernos
        int childIndex = transform.GetSiblingIndex(); // El item en el que estamos

        if (childIndex < ((float)childCount / 2f))
        // Si childIndex no está centrado con la barra de scroll se baja una unidad
        {
            childIndex -= 1;
        }
        scrollPosition = 1 - ((float)childIndex / childCount); // La posición de deslizamiento será entre 1 (el final) a 0 (el principio)
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    /// <summary>
    /// Si scrollRect Existe moveremos la posición de este al cálculo que hemos hecho
    /// en el start
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSelect(BaseEventData eventData)
    {
        if (scrollRect != null)
        {
            scrollRect.verticalScrollbar.value = scrollPosition; 
        }

    }
    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class ScrollWithController 
// namespace
