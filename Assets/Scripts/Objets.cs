//---------------------------------------------------------
// Breve descripción del contenido del archivo
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
public class Objets : MonoBehaviour
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


    [SerializeField] private GameObject[] _materials = new GameObject[3]; //Array de GameObjects que representan los materiales que el objeto puede contener. 
    [SerializeField] private GameObject[] _ordenPedidos = new GameObject[3]; //Array de GameObjects que define el orden correcto de los materiales para completar el objeto.
    private bool _complete = false; //Indica si el objeto está completado correctamente.
    [SerializeField] private Renderer[] _capacityAmount = new Renderer[3]; //Array de GameObjects que son indicadores y representan los huecos que tiene el objeto

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

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    /// <summary>
    /// AddMaterial busca por el array de _materials un hueco null, si lo encuentra, inserta en i el 
    /// Gameobject y devuelve true, sino, sigue buscando por el array hasta el último, si no hay más 
    /// huevo lo notifica y agredado será false
    /// </summary>
    /// <param name="material"></param>  GameObject que será añadido a la array
    /// <returns>True si el material fue añadido correctamente, False si no hay espacio.</returns>

    public bool AddMaterial (GameObject material)
    {
        bool agregado = false;
        int i = 0;
        while (!agregado && i < _materials.Length)
        {
            if (_materials[i] == null)
            {
                _materials[i] = material;
                IsCompleted();
                agregado = true;
            }
            else { i++; }
        }
        if (!agregado) Debug.Log("No hay más hueco");
        return agregado;
    }


    /// <summary>
    /// Verifica si los materiales están en el orden correcto y si el objeto está completado.
    /// </summary>
    /// <returns>True si los materiales están en el orden correcto, False en caso contrario.</returns>
    public bool IsCompleted()
    {
        int n = 0;

        foreach (GameObject required in _ordenPedidos)
        {
            if (required == null)
            {
                continue;
            }
            // Busca el siguiente material no nulo en el array.
            while (n < _materials.Length && _materials[n] == null)
            {
                n++;
            }
            // Si no hay más materiales o el material no coincide con el requerido, retorna false.
            if (n >= _materials.Length || _materials[n] != required)
            {
                Debug.Log("FALSE Material no coincide con el pedido o está incompleto");
                return false;
            }
            n++;
        }

        while (n < _materials.Length)
        {
            if (_materials[n] != null)
            {
                Debug.Log("FALSE Hay materiales adicionales que no están en el orden de pedidos.");
                return false;
            }
            n++;
        }
        Debug.Log("TRUE COMPLETADO");

        return true;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Actualiza los indicadores de capacidad
    /// </summary>

    private void CapacityIndicator ()
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            if (_capacityAmount[i] != null)
            {
                if (_materials[i] != null)
                {
                    _capacityAmount[i].material.color = Color.green; // Cambia a color de ocupado.
                }
                else
                {
                    _capacityAmount[i].material.color = Color.white; // Cambia a color de vacío.
                }
            }
        }
    }

    #endregion   

} // class Objets 
// namespace
