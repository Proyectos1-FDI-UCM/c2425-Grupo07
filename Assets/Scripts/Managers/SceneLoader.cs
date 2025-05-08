//---------------------------------------------------------
// Este Script es responsable de gestionar las escenas que se van a cargar y los paneles que se abrirán en la pantalla de inicio ("TitleScreen")
// Guillermo Isaac Ramos Medina
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    // hace posible el sonido de los botones
    [SerializeField] private AudioClip ButtonSound;
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
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    /// <summary>
    /// Cambia a la escena especificada
    /// </summary>
    /// <param name="nameScene">Nombre de la escena a la que se quiere ir</param>
    public void WarpScene(string nameScene)
    {
        SettingsManager.Instance.PlaySFX(ButtonSound);
        StartCoroutine(DelayOnSceneChange(nameScene));
    }

    public void WarpTutorialOrSelection()
    {
        string nameScene = "MenuLevelSelection";
        if (GameManager.Instance.ReturnPlayerTutorial() == 0)
        {
            nameScene = "Tutorial";
        }
        SettingsManager.Instance.PlaySFX(ButtonSound);
        StartCoroutine(DelayOnSceneChange(nameScene));
    }

    //Cierra el juego
    public void QuitGame()
    { Application.Quit(); }

    //Abre el panel de Ajustes
    public void ToggleSettingsPanel()
    {
        SettingsManager.Instance.TogglePanel();
    }

    /// <summary>
    /// Reinicia la escena actual
    /// </summary>
    public void RestartLevel()
    {
        string actualSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SettingsManager.Instance.PlaySFX(ButtonSound);
        StartCoroutine(DelayOnSceneChange(actualSceneName, true));
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Cambia a la escena dada por la string después de un delay. (default: = 0.5 segundos)
    /// También puede reanudar el tiempo cuando el juego estaba pausado
    /// </summary>
    /// <param name="nameScene"></param>
    /// <param name="Unpause"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator DelayOnSceneChange(string nameScene, bool Unpause = true, float delay = 0.5f)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (Unpause) { Time.timeScale = 1f; }
        SceneManager.LoadScene(nameScene);
    }

    #endregion

} // class SceneLoader 
// namespace
