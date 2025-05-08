//---------------------------------------------------------
// Script simple que se encarga de seleccionar aleatoriamente el splashText de la pantalla de inicio del juego
// Óliver García Aguado
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using TMPro;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class SplashTextLogic : MonoBehaviour
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

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    
    /// <summary>
    /// Se encarga de establecer la string del splashText de forma aleatoria con una probabilidad de 50%
    /// </summary>
    void Start()
    {
        string splashText = "";
        System.Random random = new System.Random();
        int resultado = random.Next(3);
        switch (resultado)
        {
            case 0:
            splashText = "Also Try Astra Damnatorum";
            break;
            case 1:
            splashText = "Also Try Kingless Dungeon";
            break;
            case 2:
            splashText = "Also Try Overcooked";
            break;
        }
        gameObject.GetComponent<TMP_Text>().text = splashText; // Establece el splashText con un 33% de probabilidad
        
    }
    #endregion


} // class SplashTextLogic 
// namespace
