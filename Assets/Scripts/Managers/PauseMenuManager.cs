//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Alicia Sarahi Sanchez Varela
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.EventSystems;

/// <summary>
/// Esta clase de PauseMenuManager se encarga de pausar el juego, abrir y cerrar el menu de pausa, que contiene botones para la funcion 
/// de reanudar el juego, cambiar a la escena de seleccion de niveles, a la escena de menu principal, mostrar los controles y poder navegar
/// en el menu con teclado y mando.
/// </summary>
public class PauseMenuManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] private GameObject PauseMenuUI; // para colocar el Objecto de Menu de Pausa desde el editor para su correcto funcionamiento.
    [SerializeField] private GameObject ControlsUI; // Necesitamos utilizar la imagen de los controles para poder mostarlos en el Menu.
    [SerializeField] private GameObject PauseMenuFirstButton; //El primer boton que aparece en el estado de "hovering" para oder navegar con teclas o Gamepad.


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private bool _paused = false;  //Indica si el juego esta pausado o no.
    private bool _controlPannelActive = false; //indica si la imagen de los controles esta activa o no.

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// Comprueba si el jugador ha pulsado el botón de pausar para 
    /// realizar la acción
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.PauseWasPressedThisFrame())
        {
            HandleInput();
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


    /// <summary>
    /// Reincia la escena del nivel
    /// </summary>
    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Cambia a la escena que este escrita en el editor, sirve para cualquier boton que cambie a una escena.
    /// </summary>
    /// <param name="nameScene"></param>
    public void ChangeScenesButtons(string nameScene)
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(nameScene);
    }

    /// <summary>
    /// Muestra la imagen de los controles y acrtiva su booleana correspondiente.
    /// </summary>
    public void ShowControls()
    {
        ControlsUI.SetActive(true);
        _controlPannelActive = true;
    }

    /// <summary>
    ///  Controla la activacion y d esactivacion del menu de pausa. Si el menu no esta activo y se da el input correspondiente
    ///  el menu de pausa se activa y se pausa el juego. Si no esta activo, ve si los controles estan activos, si no lo estan cierra
    ///  el menu de pausa. Si lo estan, los desactiva para que la siguiente vez que se pulse el input correspondiente se cierre el menu de pausa
    ///  asi primero se cierran los controles y luego se cierra el menu.
    /// </summary>
    public void HandleInput()
    {
        if (!_paused)
        {
            PauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            _paused = true;

            EventSystem.current.SetSelectedGameObject(PauseMenuFirstButton);
        }
        else
        {
            if (_controlPannelActive)
            {
                ControlsUI.SetActive(false);
                _controlPannelActive = false;
            }
            else
            {
                PauseMenuUI.SetActive(false);
                Time.timeScale = 1f;
                _paused = false;

                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }


    /// <summary>
    /// Para que la booleana de _pause pueda ser accedida por otros scrpits 
    /// </summary>
    /// <returns></returns>
    public bool PauseActive()
    {
        return _paused;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)



    #endregion
}// class PauseMenuManager 
// namespace
