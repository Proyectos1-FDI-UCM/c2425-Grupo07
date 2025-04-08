//---------------------------------------------------------
// Se programa el funcionamiento del spawner
// Liling Chen
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class SpawnPlayer : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] GameObject Rack;
    [SerializeField] GameObject Albert;
    [SerializeField] GameObject Spawn;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private GameManager _gameManager;
    private GameObject _playerInScene;
    private bool _isRack;
    private Transform _spawnPosition;
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
        if (_gameManager == null)
        {
            _gameManager = GameManager.Instance;
            _isRack = _gameManager.ReturnBool();
        }

        _spawnPosition = Spawn.GetComponent<Transform>();

        if (Spawn != null)
        {
            SpawnPlayerInScene();
        }
        _gameManager.SetPlayer(_playerInScene);
        _gameManager.FirstFindPlayerComponents();

    }


    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    public void SpawnPlayerInScene()
    {
        Time.timeScale = 1f;

        if (Rack != null && Albert != null)
        {
            GameObject player = _isRack ? Rack : Albert;

            Instantiate(player, Spawn.transform.position, Quaternion.identity);

            _playerInScene = player;   
        }
        else Debug.Log("No hay personaje asignado");

    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class SpawnPlayer 
// namespace
