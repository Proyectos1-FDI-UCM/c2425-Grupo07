//---------------------------------------------------------
// Se programa la función de la mesa de trabajo, donde se inserta el material a la array del objecto
// Liling Chen
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class CraftingTableScript : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Componente que se encarga de reproducir un sonido cuando se introduzca un material en un objeto.
    /// </summary>
    [SerializeField] private AudioSource CraftingTableSFX;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private Objects _scriptObject; //_scriptObject es para tener referencia al script de los objetos de aquí obtener los datos necesarios
    private MaterialType[] _materials; //Se almacena la array de los materiales del objeto


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
    /// Al llamar a este, se realiza un ducktying para ver si hay algo referenciado en _scriptObject antes de usar alguno
    /// de sus métodos y si es así, se obtiene la booleana del _scriptObject y se realiza una busqueda dentro del array del material
    /// para encontrar un hueco en este, al ser agregado, este retorna el array al objecto y complueba si esta completado
    /// el objecto, sino, avanza en la array hasta el final.
    /// </summary>
    /// <param name="material"> Material al que se añade al objeto</param>
    /// <returns>Retorna falso si no fue aglegado el material, verdadero si se a insertado el material a la array</returns>
    public bool AddMaterial(MaterialType material)
    {
        bool agregado = false;
        if (_scriptObject.GetComponent<Objects>() != null)
        {
            if (_scriptObject.GetCanBeSent()) // Si el objeto puede ser enviado
            {
                int i = 0;
                while (!agregado && i < _materials.Length)
                {
                    if (_materials[i] == MaterialType.Otro)
                    {
                        _materials[i] = material;
                        agregado = true;
                        ReturnMaterials(_materials);
                        _scriptObject.ChangeSkin();
                        if (CraftingTableSFX != null)
                        {
                            CraftingTableSFX.Play();
                            Debug.Log("SuenaMESACRAFTEO");
                        }
                    }
                    else { i++; }
                }
                return agregado;
            }
            else
            {
                Debug.Log(" No se puede añadir material, se acabó el tiempo del pedido");
                return agregado;
            }
        }
        else return agregado;
    }

    //Asigna la array actualizada al array del objeto
    public void ReturnMaterials (MaterialType[] materials)
    {
        if(_scriptObject != null)
        {
            _scriptObject.SetMaterials(materials);
        }
    }

    /// <summary>
    /// Comprueba que se trata de un objecto y llama después al Player vision para asignar el objeto a la mesa de trabajo
    /// despues de asigna los atributos de _scriptObject y _materials.
    /// </summary>
    /// <param name="item">En este caso es el objecto</param>
    public void Drop(GameObject item)
    {
        if (item.GetComponent<Objects>() != null)
        {
            Objects objects = item.GetComponent<Objects>();
            if (objects != null)
            {
                item.GetComponentInParent<PlayerVision>().Drop(false);
                _scriptObject = objects;
                _materials = _scriptObject.GetCurrentMaterial();
            }
            else Debug.Log("No se puede introducir este material en esta estacion de trabajo");
        }
    }

    /// <summary>
    /// Se resetea los atributos cuando el objeto es recojido por el jugador
    /// </summary>
    public void Pick()
    {
        _scriptObject = null;
        _materials = null;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    /// <summary>
    /// Cuando se coloca la batidora en el tutorial se indicará que se le tiene que colocar unos materiales
    /// </summary>
    private void OnTransformChildrenChanged()
    {
        if (GetComponent<ArrowTutorial>() != null)
        {
            GetComponent<ArrowTutorial>().DeactivateArrow(1); // desactiva el anterior
            GetComponent<ArrowTutorial>().ActiveArrow(0);
            GameManager.Instance.SetTutorialString("OK, now process the materials and place them <color=\"red\" >in order<color=\"white\" > on top of the object to repair <color=\"lightblue\" >" +
                "on the table<color=\"white\" >.");
        }
    }
    #endregion

} // class CraftingTableScript 
// namespace
