//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
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

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private GameObject _object;
    private Objects _scriptObject;
    private Material[] _materials;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (transform.childCount > 0)
        {
            _object = transform.GetChild(0).gameObject;
            if(_object.GetComponent<Objects>()!= null)
            {
                _scriptObject = _object.GetComponent<Objects>();
                //_materials = _object.GetCurrentMaterial();
            }
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    public bool AddMaterial(Material material)
    {
        if (_scriptObject.GetComponent<Objects>() != null)
        {
            if (true /*_scriptObject.GetCanBeSent()*/) // Si el objeto puede ser enviado
            {
                bool agregado = false;
                int i = 0;
                while (!agregado && i < _materials.Length)
                {
                    if (_materials[i] == null)
                    {
                        _materials[i] = material;
                        agregado = true;
                        _scriptObject.IsCompleted();
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
        else return false;
    }

    public void ReturnMaterials (Material[] materials)
    {
        if(_scriptObject != null)
        {
            //_scriptObject.SetMaterials(materials);
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

} // class CraftingTableScript 
// namespace
