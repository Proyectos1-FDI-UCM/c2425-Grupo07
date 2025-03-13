//---------------------------------------------------------
// Contiene dos métodos para salir y reiniciar la escena
// Guillermo 
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// 
/// Este script sirve para que sus métodos sean accedidos por los botones de salir de juego y de ir a la selección de personajes
/// </summary>
public class SceneManager : MonoBehaviour
{

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController4

    //Cierra el juego
    public void ExitGame()
    {
        Application.Quit();
    }
    //Abre la escena de selección de personajes
    public void RestartGame()
    {
        Application.LoadLevel("SelectionMenu");
    }
    #endregion


} // class SceneManager 
// namespace
