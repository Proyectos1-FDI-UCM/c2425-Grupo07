//---------------------------------------------------------
// Archivo para destruir materiales, pone como hijos a los materiales introducidos y lo destruye.
// Cheng Xiang Ye Xu
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Clase que representa un contenedor de basura donde los jugadores pueden desechar materiales.
/// Cuando un material se establece como hijo de este objeto, se destruye automáticamente.
/// </summary>
public class BinScript : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private Animator BinAnimator; // Referencia al componente Animator para controlar la animación del contenedor de basura.
    [SerializeField] int VecesDisminuido = 10;
    [SerializeField] ParticleSystem Smoke;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private float _velMat = 10; //La velocidad que se moverá el material hacia la basura y en la que disminuirá (lo que se repite la función
                                //Disminuir)
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

    /// <summary>
    /// Se llama cuando un collider entra en el área del trigger.
    /// Abre la tapa de la basura mediante animación.
    /// </summary>

    private void OnTriggerEnter2D(Collider2D collision)
    {
       BinAnimator.SetBool("Opened",true);
    }

    /// <summary>
    /// Se llama cuando un collider sale del área del trigger.
    /// Cierra la tapa de la basura mediante animación.
    /// </summary>
   
    private void OnTriggerExit2D(Collider2D collision)
    {
        BinAnimator.SetBool("Opened",false);
        if (Smoke != null )
        {
            Instantiate(Smoke, transform.position, Quaternion.identity);
        }
    }

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos Públicos

    /// <summary>
    /// Método para soltar un objeto dentro de la basura. 
    /// Si el objeto no es un extintor, se destruye.
    /// </summary>
    /// <param name="item">El objeto a desechar.</param>
    public void Drop(GameObject item)
    {
        if (item.GetComponent<FireExtinguisher>() == null)
        {
            item.GetComponentInParent<PlayerVision>().Drop(false);
            item.transform.SetParent(gameObject.transform);
            item.transform.localPosition = Vector3.zero; // Coloca el objeto en la posición del contenedor
        }
        else
        {
            Debug.LogWarning("No puedes tirar el extintor a la basura, ten cuidado amigo.");
        }
    }

    #endregion

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
        if (material != null)
        {
            StartCoroutine(DisminuyeMat(material.GetComponent<SizeAnimation>()));
        }
        Debug.Log("Material destruido en la basura.");
    }
    /// <summary>
    /// Hecho por Guillermo
    /// Inicia una secuencia que disminuye el tamaño del material hasta ser destruido
    /// rateDisminuye es velocidad que disminuye, mucho que se aumentará
    /// </summary>
    /// <param name="material">Script del objeto del material a disminuir.</param>
    IEnumerator DisminuyeMat(SizeAnimation material)
    {
        int i = 0;
        while (i < VecesDisminuido)
        {
            float rateDisminuye = (float)i / 10;
            float tiempoDisminuye = (float)i / 100;
            yield return new WaitForSeconds(tiempoDisminuye);
            if (material != null && material.ReturnSize() > 0.3)
            {
                material.SetPosition(Vector2.MoveTowards(material.ReturnPosition(), transform.position, _velMat * Time.deltaTime));
                material.MultiplySize(1 - rateDisminuye);
                material.SetPosition(Vector2.Lerp(material.ReturnPosition(), transform.position, rateDisminuye)); // Desplaza el material hacia abajo mientras disminuye su tamaño.
            }
            else
            {
                i = VecesDisminuido;
            }
            i++;
        }
        if (material!= null)
        {
            Destroy(material.gameObject);
        }
    }

    #endregion
} // class BinScript
