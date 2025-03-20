//---------------------------------------------------------
// Este script se encarga de gestionar el timer del nivel y de informar al jugador en caso de que se acabe el tiempo
// Ferran Escribá Cufí
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using TMPro;


/// <summary>
/// Esta clase se encarga de gestionar el timer del nivel, con métodos públicos para que empiece y para que se detenga.
/// Muestra el tiempo en formato MM:SS e informa al jugador en caso de que se acabe el tiempo.
/// </summary>
public class LevelTimer : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    // ShowText es el texto para mostrar en partida
    [SerializeField] private TextMeshProUGUI ShowText;

    // Panel es el panel que se muestra cuando se acaba el tiempo con el mensaje de que se ha acabado el tiempo
    [SerializeField] private GameObject Panel;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    // _continue determina si el timer debe empezar (true) o no (false)
    private bool _continue = false;

    // _maxTime es el tiempo máximo que puede durar la partida
    private float _maxTime = 180;

    // _currentSecondsLeft es el tiempo restante en segundos
    private float _currentSecondsLeft;

    // _minutesShow son los minutos para mostrar en el timer del juego
    private int _minutesShow;

    // _secondsShow son los segundos para mostrar en el timer del juego
    private int _secondsShow;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_continue)
        {
            _currentSecondsLeft -= Time.deltaTime;
        }
        if (_currentSecondsLeft < 0)
        {
            StopTimer();
            _currentSecondsLeft = 0;
            Panel.SetActive(true);
            Time.timeScale = 0;
        }
        ShowTime();
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
    /// StartTimer() inicializa _currentSecondsLeft al valor de _maxTime y pone _continue a true para que el timer empiece
    /// </summary>
    public void StartTimer()
    {
        _currentSecondsLeft = _maxTime;
        _continue = true;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    // StopTimer() pone _continue a false para que el timer pare
    private void StopTimer()
    {
        _continue = false;
    }

    // ShowTime() muestra el tiempo restante en formato MM:SS
    private void ShowTime()
    {
        _minutesShow = (int)_currentSecondsLeft / 60;
        _secondsShow = (int)_currentSecondsLeft % 60;
        if (_minutesShow < 10 && _secondsShow > 9)
        {
            ShowText.text = "0" + _minutesShow + ":" + _secondsShow;
        }
        else if (_minutesShow < 10 && _secondsShow < 10)
        {
            ShowText.text = "0" + _minutesShow + ":" + "0" + _secondsShow;
        }
        else if (_minutesShow > 9 && _secondsShow > 9)
        {
            ShowText.text = _minutesShow + ":" + _secondsShow;
        }
        else if (_minutesShow > 9 && _secondsShow < 10)
        {
            ShowText.text = _minutesShow + ":" + "0" + _secondsShow;
        }
    }

    #endregion   

} // class LevelTimer 
// namespace
