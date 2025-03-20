//---------------------------------------------------------
// El Objeto podrá almacenar hasta tres materiales que hayan sido insertados cuando el objeto está en la mesa de trabajo
// ,también entrá un UI que muestra el límite de capacidad que tiene el objeto que será actualizado cada vez que se le
// inserte el material. Cuando el contenido del objeto sea de igual orden y material a la condición, este objeto estará
// completado
// Liling Chen
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------




using UnityEngine;
using UnityEngine.UIElements;
// Añadir aquí el resto de directivas using

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Objects : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private GameObject[] Materials = new GameObject[3]; //Array de GameObjects que representan los materiales que el objeto puede contener. 
    [SerializeField] private GameObject[] OrdenPedidos; //Array de GameObject que define el orden correcto de los materiales para completar el objeto.
    [SerializeField] private Renderer[] CapacityAmount = new Renderer[3]; //Array de GameObjects que son indicadores y representan los huecos que tiene el objeto
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        CapacityIndicator();
    }
    #endregion

    private bool _canBeSent = true;
    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    /// <summary>
    /// AddMaterial busca por el array de Materials un hueco null, si lo encuentra, inserta en i el 
    /// Gameobject y devuelve true, sino, sigue buscando por el array hasta el último, si no hay más 
    /// hueco lo notifica y agredado (bool) será false
    /// </summary>
    /// <param name="material"></param>  GameObject que será añadido a la array
    /// <returns>True si el material fue añadido correctamente, False si no hay espacio</returns>

    public bool AddMaterial (GameObject material)
    {
        if (_canBeSent) // Si el objeto puede ser enviado
        {
        bool agregado = false;
        int i = 0;
        while (!agregado && i < Materials.Length)
        {
            if (Materials[i] == null)
            {
                Materials[i] = material;
                agregado = true;
                IsCompleted();
            }
            else { i++; }
        }
        return agregado;
        }
        else
        {
            Debug.Log(" No se puede añadir material, se acabó el tiempo del pedido");
            return false;
        }
    }


    public bool ThereIsMaterial(GameObject material)
    {
        return Materials[0] == null;
    }

    /// <summary>
    /// Verifica si los materiales están en el orden correcto y si el objeto está completado.
    /// </summary>
    /// <returns>True si los materiales están en el orden correcto, False en caso contrario.</returns>
    public bool IsCompleted()
    {
        if (_canBeSent) // Si el objeto puede ser enviado
        {
            int n = 0;

        // Recorre cada objeto requerido en el pedido
        foreach (GameObject required in OrdenPedidos)
        {
            if (required == null) continue; // Ignora objetos nulos en el pedido

            // Si no hay más materiales o el tipo de material no coincide con el requerido, retorna false
            if (!IsSameMaterialType(Materials[n], required))
            {
                Debug.Log("FALSE Material no coincide con el pedido o está incompleto");
                return false;
            }
            n++; // Avanza al siguiente material
        }

        // Verifica si hay materiales adicionales que no están en el pedido
        while (n < Materials.Length)
        {
            if (Materials[n] != null)
            {
                Debug.Log("FALSE Hay materiales adicionales que no están en el orden de pedidos.");
                return false;
            }
            n++;
        }

            Debug.Log("TRUE COMPLETADO");
            return true;
        }
        else
        {
            Debug.Log("No se puede enviar, se acabó el tiempo del pedido");
            return false;
        }
    }

    public void ResetObject()
    {
        for (int i = 0; i < Materials.Length; i++)
        {
            Materials[i] = null;
        }
    }
    /// <summary>
    /// Establece si el objeto puede ser enviado o no para evitar que el jugador intente añadir materiales o que intente enviar el objeto después de que se acabó el tiempo del pedido.
    /// </summary>
    /// <param name="canBeSent">True si el objeto puede ser enviado, False en caso contrario.</param>
    public void SetCanBeSent(bool canBeSent)
    {
        _canBeSent = canBeSent;
    }

    /// <summary>
    /// Método que verifica que los materiales del requerido del array de OrdenPedidos y del array del Materiales del objeto sean iguales
    /// dependiendo del emun (tipo) que es de su script Material
    /// </summary>
    /// <param name="material"> Contenido del array de Materiales que el objeto almacena </param>
    /// <param name="required"> Contenido del array de OrdenPedidos que son la condición para que se complete el objeto </param>
    /// <returns>Retorna false cuando el contenido de material y required son nulos o cuando son distintos por su emun en el script Material,
    /// si son iguales retorna true </returns>
    private bool IsSameMaterialType(GameObject material, GameObject required)
    {
        if (material == null || required == null)
        {
            return false;
        }

        // Obtiene el componente MaterialType de ambos objetos
        var matType1 = material.GetComponent<Material>()?.MaterialType();
        var matType2 = required.GetComponent<Material>()?.MaterialType();

        // Compara los tipos de material
        return matType1 == matType2;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Actualiza los indicadores de capacidad haciéndolos cambiar de color, de blanco a verde
    /// </summary>
    private void CapacityIndicator ()
    {
        for (int i = 0; i < Materials.Length; i++)
        {
            if (CapacityAmount[i] != null)
            {
                if (Materials[i] != null)
                {
                    CapacityAmount[i].material.color = Color.green; // Cambia a color de ocupado.
                }
                else
                {
                    CapacityAmount[i].material.color = Color.white; // Cambia a color de vacío.
                }
            }
        }
    }

    #endregion   

} // class Objets 
// namespace
