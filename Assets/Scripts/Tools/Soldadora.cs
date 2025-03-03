//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// Añadir aquí el resto de directivas using
using UnityEngine.UI;


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Soldadora : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    // PlayerPosition es la posición del jugador
    [SerializeField] private Transform _playerPosition;

    // CompletionImage es la barra de compleción del proceso de refinamiento
    [SerializeField] private Image CompletionImage;

    //Rapidez de trabajo 
    [SerializeField] private float _workSpeed;

    // maxProgress es el número max necesario para completar el proceso de refinamiento
    [SerializeField] private int _maxProgress = 6;

    [SerializeField] private InputActionReference ClickActionReference;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    //
    public bool _isWorking;

    private float _progress;

    public bool canUse;

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
        _progress = 0;
        if (_playerPosition == null)
        {
            _playerPosition = GameObject.FindAnyObjectByType<PlayerMovement>().transform;
        }

        UpdateCompletionBar(_maxProgress, _progress);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
       if (_isWorking)
       {
            _progress += (Time.deltaTime * _workSpeed);
            UpdateCompletionBar(_maxProgress, _progress);
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

    //
    public void TurnOnWelder(InputAction.CallbackContext context)
    {
        if (canUse)
        {
            _isWorking = true;
            UpdateCompletionBar(_maxProgress, _progress);
        }
    }
    public void TurnOffWelder(InputAction.CallbackContext context)
    {
        if (canUse) 
        { 
            _isWorking = false;
            UpdateCompletionBar(_maxProgress, _progress);

        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    

    // Actualiza la barra de compleción de la soldadora
    private void UpdateCompletionBar(float _maxCompletion, float _currentCompletion)
    {
        float _targetCompletion = _currentCompletion / _maxCompletion;
        CompletionImage.fillAmount = _currentCompletion / _maxCompletion;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Material>() != null && collision.gameObject.GetComponent<Material>().matType == MaterialType.Metal)
        {
            _progress = collision.gameObject.GetComponent<Material>().ReturnProgress();
            
            canUse = true; 

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Material>() != null && collision.gameObject.GetComponent<Material>().matType == MaterialType.Metal)
        { canUse = false;
            collision.gameObject.GetComponent<Material>().StoreProgress(_progress);
            _progress = 0;
            UpdateCompletionBar(_maxProgress, _progress);
        }
    }

    #endregion   

} // class Soldadora 
// namespace
