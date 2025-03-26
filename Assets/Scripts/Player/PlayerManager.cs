//---------------------------------------------------------
// Se encarga de diferenciar a los dos personajes del juego y cambiar la velocidad de las herramientas
// Guillermo
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
//Separa los personajes por sus habilidades pasivas
public enum PlayerType
{
    Velocista, 
    Reparador
}

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// 
/// La clase del PlayerManager se encarga de hacer que los métodos puedan acceder al tipo de jugador 
/// Después se puede llamar al método SetVel para definir la velocidad en ciertas herramientas
/// </summary>
public class PlayerManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    // Hacer que los métodos puedan acceder al tipo de jugador 
    [SerializeField]private PlayerType pType;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    // Atributos de la velocidad deseada en:
    int _velOven;  // Horno
    int _velSaw; // Soldadora
    int _clicksAnvil; // Sierra

    #endregion


    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    // Define la velocidad en ciertas herramientas
    public void SetVel(PlayerType pType)
    {
        this.pType = pType;
        switch (pType)
        {
            case PlayerType.Reparador:
                _velOven = 30;
                _clicksAnvil = 7;
                _velSaw = 3;
                break;
            case PlayerType.Velocista:
                _velOven = 15;
                _clicksAnvil = 14;
                _velSaw = 6;
                break;
        }
    }
    // Devuelve la velocidad acorde al jugador de:
    public int ReturnOven() // El Horno
    { return _velOven; }
    public int ReturnAnvil() // La Sierra
    { return _clicksAnvil; }
    public int ReturnSaw() // La Soldadora
    { return _velSaw; }

    public PlayerType PlayerNum()
    {
        return pType;
    }
    #endregion

} // class PlayerManager 
// namespace
