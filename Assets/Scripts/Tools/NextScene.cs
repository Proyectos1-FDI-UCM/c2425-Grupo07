//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Guillermo Isaac Ramos Medina
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class NextScene : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] bool ChangesCam;
    [SerializeField] bool ChangesScene;
    [SerializeField] MoveToNextRoom MoveScenario;
    [SerializeField] TextMeshProUGUI TutorialText;
    [SerializeField] string NextTutorialText;
    [SerializeField] float MoveDistance;
    [SerializeField] Vector3 NextPosition;

    #endregion

     // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
     // Documentar cada atributo que aparece aquí.
     // El convenio de nombres de Unity recomienda que los atributos
     // privados se nombren en formato _camelCase (comienza con _, 
     // primera palabra en minúsculas y el resto con la 
     // primera letra en mayúsculas)
     // Ejemplo: _maxHealthPoints
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
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerManager>() != null && ChangesCam)
        {
            other.gameObject.transform.position = NextPosition;
            MoveScenario.Move(MoveDistance);
        }
        if (other.GetComponent<PlayerManager>() != null && ChangesScene)
        {
            SceneManager.LoadScene("MenuLevelSelection");
        }
        if (TutorialText != null)
        {
            TutorialText.text = NextTutorialText;
        }
    }
    #endregion   

} // class NextScene 
// namespace
