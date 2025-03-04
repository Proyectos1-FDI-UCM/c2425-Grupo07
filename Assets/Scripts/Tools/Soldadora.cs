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


    // CompletionImage es la barra de compleción del proceso de refinamiento
    [SerializeField] private Image CompletionImage;
    [SerializeField] private GameObject _metalIntroducido;
    [SerializeField] private GameObject _metalProcesado;

    //Rapidez de trabajo 
    [SerializeField] private float _workSpeed;

    // maxProgress es el número max necesario para completar el proceso de refinamiento
    [SerializeField] private int _maxProgress = 6;

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
        if (_progress >= _maxProgress)
        {
            _progress = 0;
            Destroy(_metalIntroducido);
            GameObject metalProcesado = Instantiate(_metalProcesado, this.gameObject.transform.position, gameObject.transform.rotation);
            metalProcesado.transform.SetParent(this.transform);
            canUse = false;
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
        Debug.Log("aa1");
        if (canUse)
        {
            _isWorking = true;
            UpdateCompletionBar(_maxProgress, _progress);
        }
    }
    public void TurnOffWelder(InputAction.CallbackContext context)
    {
        Debug.Log("aa2");
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







    #endregion   

} // class Soldadora 
// namespace
