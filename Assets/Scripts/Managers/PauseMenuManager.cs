//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Alicia Sarahi Sanchez Varela
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] private GameObject ResetPanel; //Solo se usa en la escena de menuLevelSelect para desactivar el panel cuando se pulsa ESC
    [SerializeField] private Button CloseTutorial; //Boton de cerrar el tutorial
    [SerializeField] private AudioClip ButtonSound; //Sonido de los botones
    [SerializeField] private bool _paused = false;  //Indica si el juego esta pausado o no.
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private bool _controlPannelActive = false; //indica si la imagen de los controles esta activa o no.
    private bool _tutorial = false; //Indica si el tutorial esta activado
    private PlayerDash _playerDash; // Para bloquear al jugador de activar un dash si está en el menú de pausa.
    private IndicatorChange _indicatorChange;
    private LevelManager _levelManager; // Referencia al script LevelManager

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour


    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        if (FindAnyObjectByType<LevelManager>() != null)
        {
            _levelManager = FindAnyObjectByType<LevelManager>();
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// Comprueba si el jugador ha pulsado el botón de pausar para 
    /// realizar la acción
    /// </summary>
    void Update()
    {
        if (InputManager.Instance != null && InputManager.Instance.PauseWasPressedThisFrame() && _levelManager == null || 
            (_levelManager != null && _levelManager.GetCurrentSecondsLeft() > 0 && InputManager.Instance != null && InputManager.Instance.PauseWasPressedThisFrame()))
        {
            HandleInput();
        }

        if(FindAnyObjectByType<IndicatorChange>() != null)
        {
            _indicatorChange = FindAnyObjectByType<IndicatorChange>();
            _tutorial = _indicatorChange.ReturnActive();
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


    public void Resume()
    {
        SettingsManager.Instance.PlaySFX(ButtonSound);
        HandleInput();
        ControlsUI.SetActive(false);
    }

    /// <summary>
    /// Reincia la escena del nivel
    /// </summary>
    public void RestartLevel()
    {
        string actualSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SettingsManager.Instance.PlaySFX(ButtonSound);
        StartCoroutine(DelayOnSceneChange(actualSceneName, true));
    }

    /// <summary>
    /// Cambia a la escena que este escrita en el editor, sirve para cualquier boton que cambie a una escena.
    /// </summary>
    /// <param name="nameScene"></param>
    public void ChangeScenesButtons(string nameScene)
    {
        SettingsManager.Instance.PlaySFX(ButtonSound);
        StartCoroutine(DelayOnSceneChange(nameScene,true));
    }

    /// <summary>
    /// Muestra la imagen de los controles y acrtiva su booleana correspondiente.
    /// </summary>
    public void ToggleControlPanel()
    {
        SettingsManager.Instance.PlaySFX(ButtonSound);
        if (!_controlPannelActive)
        {
            ControlsUI.SetActive(true);
            _controlPannelActive = true;
            EventSystem.current.SetSelectedGameObject(ControlsUI.GetComponentInChildren<Button>().gameObject); // Selecciona el primer boton de la UI de controles.
        }
        else
        {
            ControlsUI.SetActive(false);
            _controlPannelActive = false;
            EventSystem.current.SetSelectedGameObject(PauseMenuFirstButton); // Selecciona el primer boton del menu de pausa.
        }
    }

    /// <summary>
    ///  Controla la activacion y d esactivacion del menu de pausa. Si el menu no esta activo y se da el input correspondiente
    ///  el menu de pausa se activa y se pausa el juego. Si no esta activo, ve si los controles estan activos, si no lo estan cierra
    ///  el menu de pausa. Si lo estan, los desactiva para que la siguiente vez que se pulse el input correspondiente se cierre el menu de pausa
    ///  asi primero se cierran los controles y luego se cierra el menu.
    ///  Activa el Input de Player del jugador si está en el menú de pausa, activando el Input del UI (Esta línea fue hecha por Guillermo)
    /// </summary>
    public void HandleInput()
    {
        if (!_paused)
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.EnableActionMap("UI");
            }
            PauseMenuUI.SetActive(true);
            Time.timeScale = 0f;

            _paused = true;

            EventSystem.current.SetSelectedGameObject(PauseMenuFirstButton);

        }
        else
        {
            if (_controlPannelActive)
            {
                ToggleControlPanel(); // Si los controles están activos, los desactiva.
            }
            else if (SettingsManager.Instance != null && SettingsManager.Instance.IsCanvasOpen())
            {
                ToggleSettingsPanel(); // Si los ajustes están activos, los desactiva.
            }
            else if (_tutorial)
            {
                CloseTutorial.onClick.Invoke();
            }
            else
            {
                InputManager.Instance.EnableActionMap("Player");
                PauseMenuUI.SetActive(false);
                Time.timeScale = 1f;
                _paused = false;

                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        if (ResetPanel != null)
        {
            ResetPanel.SetActive(false);
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
    /// <summary>
    /// Hecho por Guillermo
    /// Intercambia entre abrir y cerrar el panel de ajustes
    /// Accede al SettingsManager para abrirlo
    /// </summary>
    public void ToggleSettingsPanel()
    {
        SettingsManager.Instance.PlaySFX(ButtonSound);
        SettingsManager.Instance.TogglePanel();
    }
    /// <summary>
    /// Hecho por Guillermo
    /// Intercambia entre abrir y cerrar el panel de ajustes
    /// Accede al SettingsManager para abrirlo
    /// </summary>
    public void ResetProgress()
    {
        GameManager.Instance.ResetProgress();
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Changes the scene after a delay
    /// </summary>
    /// <param name="nameScene"></param>
    /// <param name="Unpause"></param>
    /// <param name="delay"></param>
    /// <returns></returns>

    private IEnumerator DelayOnSceneChange(int sceneIndex, bool Unpause = true, float delay = 0.5f)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (Unpause) { Time.timeScale = 1f; }
        SceneManager.LoadScene(sceneIndex);
    }

    /// <summary>
    /// Changes the scene after a delay. (DEFAULT : = 0.5 seconds)
    /// It can also unpause the time when the game was paused.
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

    void OnApplicationPause()
    {
        if (Time.timeScale != 0)
        {
            HandleInput();
        }
    }
    #endregion
}// class PauseMenuManager 
// namespace
