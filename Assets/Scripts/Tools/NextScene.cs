//---------------------------------------------------------
// Se encarga de gestionar las acciones de salas del tutorial
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
/// 
/// Comprueba que si ha colisionado con el jugador este ha completado una sala
/// Las salas pueden reproducir un texto, desplazar la cámara(en realidad es el escenario y el jugador para 
/// dar un efecto de desplazamiento) o cambiar de escena, a veces se pueden mezclar varias acciones.
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
    [SerializeField] bool ChangesCam; // Si cambia la cámara del nivel
    [SerializeField] bool ChangesScene; // Si cambia la escena del nivel
    [SerializeField] MoveToNextRoom MoveScenario; // Mueve al escenario accediendo a su script público
    [SerializeField] TextMeshProUGUI TutorialText; // Texto mostrado en el canvas
    [SerializeField] string NextTutorialText; // String mostrado en la sala
    [SerializeField] float MoveDistance; // Distancia que se moverá el escenario en el eje x
    [SerializeField] Vector2 NextPosition; // Posición que se moverá el jugador

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
    void Start()
    {
        GameManager.Instance.SetTutorialUIText(TutorialText);
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
    /// Si se han completado las salas o se ha salido por el menú de pausa se dará por
    /// completado al tutorial
    /// </summary>
    public void DoneTutorial()
    {
        SceneManager.LoadScene("MenuLevelSelection");
        GameManager.Instance.PlayerDidTutorial();
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    /// <summary>
    /// Encargado de detectar la posición del jugador al llegar a la siguiente sala
    /// Si cambia la cámara se desplaza al jugador y al escenario
    /// Si cambia la escena da el tutorial por completado
    /// Si se cambia el texto del canvas se reproduce un nuevo string
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerManager>() != null && ChangesCam)
        {
            other.GetComponent<PlayerManager>().SetPosition(NextPosition);
            MoveScenario.Move(MoveDistance);
        }
        if (other.GetComponent<PlayerManager>() != null && ChangesScene)
        {
            DoneTutorial();
        }
        if (TutorialText != null)
        {
            TutorialText.text = NextTutorialText;
        }
        gameObject.SetActive(false);
    }
    #endregion   

} // class NextScene 
// namespace
