//---------------------------------------------------------
// Este script se encarga de spawnear los materiales para la cinta mecánica en orden aleatorio y de manera cíclica
// Ferran Escribá Cufí
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using System.Collections;


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// 
/// Esta clase tiene un array con todos los materiales para spawnear que desordena y recorre cíclicamente para spawnearlos.
/// Spawnea los materiales en la posición del spawner y los asigna como sus hijos.
/// Usa una corrutina para spawnear los materiales, la frecuencia con la que spawnea los materiales se puede ajustar en este código.
/// </summary>
public class SpawnMaterials : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    // Materials es un array que contiene todos los GameObjects de los materiales
    [SerializeField] GameObject[] Materials = new GameObject[4];

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    // _spawnPoint es el lugar en el que tienen que spawnear los materiales
    private Transform _spawnPoint;

    // _currentObjectIndex es el índice actual que está recorriendo el array Materials
    private int _currentObjectIndex = 0;

    // _spawnInterval es el tiempo en segundos que pasa entre cada spawn
    private float _spawnInterval = 1.5f;

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
        _spawnPoint = GetComponent<Transform>();

        ShuffleArray(Materials);

        StartCoroutine(SpawnObjects());
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    // SpawnObjects spawnea el material que corresponda respetando el intervalo de tiempo entre
    // cada spawn. Spawnea el material en la posición del spawner y lo asigna como su hijo
    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            GameObject _material = Instantiate(Materials[_currentObjectIndex], _spawnPoint.position, _spawnPoint.rotation);
            _material.transform.SetParent(transform);

            _currentObjectIndex = (_currentObjectIndex + 1) % Materials.Length;

            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    // SuffleArray desordena el array Materials para que spawneen en un orden más aleatorio
    private void ShuffleArray(GameObject[] _materials)
    {
        for (int i = _materials.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            GameObject temp = _materials[i];
            _materials[i] = _materials[j];
            _materials[j] = temp;
        }
    }

    #endregion   

} // class SpawnMaterials 
// namespace
