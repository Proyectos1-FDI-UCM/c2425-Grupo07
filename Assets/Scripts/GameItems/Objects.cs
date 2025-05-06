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
    //Array de GameObjects que representan los materiales que el objeto puede contener.
    [SerializeField] private MaterialType[] Materials;
    //Array de GameObject que define el orden correcto de los materiales para completar el objeto.
    [SerializeField] private MaterialType[] OrdenPedidos;
    //Array de GameObjects que son indicadores y representan los huecos que tiene el objeto
    [SerializeField] private Renderer[] CapacityAmount = new Renderer[3];
    //Array de imagenes para el estado del objeto
    [SerializeField] private Sprite[] ObjectImage;


    #endregion
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private bool _canBeSent = true; //Booleana que representa si se puede enviar o no un objeto
    [SerializeField] private int _nGood = 0; //numero de veces que se a hecho bien el jugador al colocar el objeto
    private SpriteRenderer _skin; //Referencia al sprite renderer del objecto para cambiarlo más tarde
    private int _nInsertados; //numero de objetos insertados
    private bool _isFull;

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
    private void Start()
    {
        _skin = gameObject.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_nInsertados < Materials.Length)
        {
            CapacityIndicator();
        }
        else FinalColor();
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
    /// Comprueba que haya material dentro de la array del objeto
    /// </summary>
    /// <param name="material"></param>
    /// <returns>retorna true si no hay material en el objeto, false si hay material</returns>
    public bool ThereIsMaterial()
    {
        return Materials[0] == MaterialType.Otro;
    }

    /// <summary>
    /// Verifica si los materiales están en el orden correcto y si el objeto está completado.
    /// </summary>
    /// <returns>True si los materiales están en el orden correcto, False en caso contrario.</returns>
    public bool IsCompleted()
    {
        bool complete = false;
        if (_canBeSent) // Si el objeto puede ser enviado
        {
            int n = 0;

            // Recorre cada objeto requerido en el pedido
            foreach (MaterialType required in OrdenPedidos)
            {
                // Si no hay más materiales o el tipo de material no coincide con el requerido, retorna false
                if (!IsSameMaterialType(Materials[n], required))
                {
                    Debug.Log("FALSE Material no coincide con el pedido");
                }
                else n++; // Avanza al siguiente material
            }

            return complete = n == OrdenPedidos.Length;
        }
        else
        {
            Debug.Log("No se puede enviar, se acabó el tiempo del pedido");
            return complete;
        }
    }
    /// <summary>
    /// Al llamar al método, pone todos los huecos de la array a null
    /// </summary>
    public void ResetObject()
    {
        for (int i = 0; i < Materials.Length; i++)
        {
            Materials[i] = MaterialType.Otro;
        }
        _nGood = 0;
        _nInsertados = 0;
    }
    /// <summary>
    /// Establece si el objeto puede ser enviado o no para evitar que el jugador intente añadir materiales o que intente enviar el objeto después de que se acabó el tiempo del pedido.
    /// </summary>
    /// <param name="canBeSent">True si el objeto puede ser enviado, False en caso contrario.</param>
    public void SetCanBeSent(bool canBeSent)
    {
        _canBeSent = canBeSent;
    }

    //Retorna la booleana _canBeSent
    public bool GetCanBeSent() { return _canBeSent; }

    //Asigna los materiales actualizados a la array del objeto
    public void SetMaterials(MaterialType[] materials)
    {
        Materials = materials;
    }

    //Retorna el array del objecto
    public MaterialType[] GetCurrentMaterial()
    {
        return Materials;
    }

    /// <summary>
    /// Cambia el sprite del objeto cuando uno de los materiales intersados coincide con el orden determinado
    /// </summary>
    public void ChangeSkin()
    {
        if (IsSameMaterialType(Materials[_nInsertados], OrdenPedidos[_nInsertados]))
        {
            _nGood++;
        }
        if(_nInsertados<OrdenPedidos.Length) _nInsertados++;
        _skin.sprite = ObjectImage[_nGood];
    }

    /// <summary>
    /// Cambia el color de los indicadores de capacidad a rojo (este método es llamado por TaskManager cuando se acaba el tiempo de un pedido)
    /// </summary>
    public void ChangeIndicatorsColor()
    {
        for (int i = 0; i < Materials.Length; i++)
        {
            CapacityAmount[i].material.color = Color.red; // Cambia a color de erróneo.
        }
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
                if (Materials[i] != MaterialType.Otro)
                {
                    CapacityAmount[i].material.color = Color.gray; // Cambia a color de ocupado.
                }
                else
                {
                    CapacityAmount[i].material.color = Color.white; // Cambia a color de vacío.
                }
            }
        }
    }

    /// <summary>
    /// Cuando el objeto esta lleno, se le indicará al jugador si está bien o no completado
    /// </summary>
    private void FinalColor()
    {
        bool complete = IsCompleted();
        for (int i = 0; i < Materials.Length; i++)
        {
            if (complete)
            {
                CapacityAmount[i].material.color = Color.green; // Cambia a color de completo.
            }
            else
            {
                CapacityAmount[i].material.color = Color.red; // Cambia a color de erroneo.
            }
        }
    }

    /// <summary>
    /// Método que verifica que los materiales del requerido del array de OrdenPedidos y del array del Materiales del objeto sean iguales
    /// dependiendo del emun (tipo) que es de su script Material
    /// </summary>
    /// <param name="material"> Contenido del array de Materiales que el objeto almacena </param>
    /// <param name="required"> Contenido del array de OrdenPedidos que son la condición para que se complete el objeto </param>
    /// <returns>Retorna false cuando el contenido de material y required son nulos o cuando son distintos por su emun en el script Material,
    /// si son iguales retorna true </returns>
    private bool IsSameMaterialType(MaterialType material, MaterialType required)
    {
        // Compara los tipos de material
        Debug.Log(material == required);
        return material == required;
    }

    

    #endregion   

} // class Objets 
// namespace
