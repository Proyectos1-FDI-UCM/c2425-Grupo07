//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Alicia Sarahi Sanchez Varela
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// La clase PauseMenuManager se encarga de gestionar el menú de pausa del juego.
/// Permite pausar el juego, reanudarlo, cambiar entre escenas, mostrar los controles,
/// y navegar en el menú utilizando teclado o mando.
/// </summary>
public class PauseMenuManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Los atributos del Inspector permiten arrastrar los objetos necesarios desde el editor.

    [SerializeField] private GameObject PauseMenuUI; // Referencia al objeto del menú de pausa.
    [SerializeField] private GameObject ControlsUI; // Imagen de los controles que se muestra en el menú.
    [SerializeField] private GameObject PauseMenuFirstButton; // El primer botón que será seleccionado por defecto al abrir el menú de pausa.
    [SerializeField] private GameObject ResetPanel; // Solo se usa en la escena de selección de niveles, desactiva el panel cuando se presiona ESC.
    [SerializeField] private GameObject GoToTutorialPanel; // Solo se usa en la escena de selección de niveles, activa el panel para ir al tutorial.
    [SerializeField] private Button CloseTutorial; // Botón para cerrar el tutorial.
    [SerializeField] private AudioClip ButtonSound; // Sonido que se reproduce al presionar botones.
    [SerializeField] private bool _paused = false; // Indica si el juego está pausado o no.
    [SerializeField] private GameObject selectionPlayerPanel; // Se usa en la selección de jugadores, referencia al panel de selección de jugador.
    [SerializeField] private IndicatorChange tutorialPannelScript;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Atributos privados que se usan internamente en el script.

    private bool _controlPannelActive = false; // Indica si el panel de controles está activo.
    private bool goesToTutorial = false; // Indica si se está yendo al tutorial.
    private bool _recipeTutorial = false; // Indica si el tutorial de recetas está activo.
    private PlayerDash _playerDash; // Referencia para controlar si el jugador puede realizar un "dash" (velocidad extra).
    private LevelManager _levelManager; // Referencia al script LevelManager para controlar el estado del nivel.
    private SceneLoader _loaderScene; // Encargado de cargar las escenas del juego
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se llama al inicio, justo antes de que cualquier método Update sea llamado.
    /// Inicializa las referencias necesarias.
    /// </summary>
    void Start()
    {
        // Busca una referencia al LevelManager si existe.
        if (FindAnyObjectByType<LevelManager>() != null)
        {
            _levelManager = FindAnyObjectByType<LevelManager>();
        }
        if(FindObjectOfType<SceneLoader>() != null)
        {
            _loaderScene = FindObjectOfType<SceneLoader>().GetComponent<SceneLoader>();
        }
        tutorialPannelScript = GetComponentInChildren<IndicatorChange>();
    }

    /// <summary>
    /// Se llama cada frame si el MonoBehaviour está habilitado.
    /// Se encarga de comprobar las entradas del jugador para pausar el juego o navegar en el menú.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance != null && InputManager.Instance.PauseWasPressedThisFrame())
        {
            if (selectionPlayerPanel != null && selectionPlayerPanel.activeSelf)
            {
                // Salir del Selection Player
                PlayerBool playerBool = selectionPlayerPanel.GetComponent<PlayerBool>();
                if (playerBool != null)
                {
                    playerBool.ExitCanva(); 
                }
                else
                {
                    // Fallback por si no encuentra el componente
                    selectionPlayerPanel.SetActive(false);
                }

                // Llamar a Resume para liberar el control y reactivar el mapa de acciones "Player"
                Resume();
            }
            else if (_levelManager == null || (_levelManager != null && _levelManager.GetCurrentSecondsLeft() > 0))
            {
                // Si no estamos en el selectionPlayerPanel, manejar el input normalmente
                HandleInput();
            }
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Reanuda el juego desactivando el menú de pausa, reactivando el mapa de acciones del jugador y reanudando el tiempo.
    /// </summary>
    public void Resume()
    {
        SettingsManager.Instance.PlaySFX(ButtonSound); // Efecto de sonido
        InputManager.Instance.EnableActionMap("Player"); // Activar el mapa de acción de Player
        PauseMenuUI.SetActive(false); // Desactivar el menú de pausa
        Time.timeScale = 1f; // Asegurarse de que el juego no esté pausado
        _paused = false; // Actualizar el estado de pausa

        EventSystem.current.SetSelectedGameObject(null); // Desactivar la selección del botón
    }

    /// <summary>
    /// Reinicia el nivel actual.
    /// </summary>
    public void RestartLevel()
    {
        string actualSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SettingsManager.Instance.PlaySFX(ButtonSound); // Reproduce el sonido del botón.
        _loaderScene.StartCoroutine(DelayOnSceneChange(actualSceneName, true));
    }

    /// <summary>
    /// Cambia a la escena indicada.
    /// </summary>
    /// <param name="nameScene">Nombre de la escena a la que se cambiará.</param>
    public void ChangeScenesButtons(string nameScene)
    {
        SettingsManager.Instance.PlaySFX(ButtonSound); // Reproduce el sonido del botón.
        _loaderScene.StartCoroutine(DelayOnSceneChange(nameScene, true));
    }

    /// <summary>
    /// Muestra u oculta el panel de controles.
    /// </summary>
    public void ToggleControlPanel()
    {
        SettingsManager.Instance.PlaySFX(ButtonSound); // Reproduce el sonido del botón.
        if (!_controlPannelActive)
        {
            ControlsUI.SetActive(true); // Muestra la UI de controles.
            _controlPannelActive = true; // Cambia el estado a activo.
            EventSystem.current.SetSelectedGameObject(ControlsUI.GetComponentInChildren<Button>().gameObject); // Selecciona el primer botón de la UI de controles.
        }
        else
        {
            ControlsUI.SetActive(false); // Oculta la UI de controles.
            _controlPannelActive = false; // Cambia el estado a inactivo.
            EventSystem.current.SetSelectedGameObject(PauseMenuFirstButton); // Selecciona el primer botón del menú de pausa.
        }
    }

    /// <summary>
    /// Maneja la entrada para pausar o reanudar el juego, dependiendo del estado actual.
    /// </summary>
    public void HandleInput()
    {
        if (!_paused)
        {
            // Si el panel de selección de jugador está activo, se desactiva al presionar ESC
            if (selectionPlayerPanel!= null&& selectionPlayerPanel.activeSelf)
            {
                selectionPlayerPanel.SetActive(false); // Desactivar el panel de selección de jugador
                InputManager.Instance.EnableActionMap("Player"); // Habilitar el mapa de acciones de jugador
            }
            else if (!goesToTutorial && tutorialPannelScript!=null && !tutorialPannelScript.ReturnActive()) 
            {
                // Si el panel de selección de jugador no está activo, mostramos el menú de pausa
                InputManager.Instance.EnableActionMap("UI");
                PauseMenuUI.SetActive(true); // Activar el menú de pausa
                Time.timeScale = 0f; // Pausar el juego
                _paused = true;
                EventSystem.current.SetSelectedGameObject(PauseMenuFirstButton); // Seleccionar el primer botón
            }
        }
        else
        {
            // Si el menú está activo y no está el panel de selección de jugador activo
            if (_controlPannelActive)
            {
                ToggleControlPanel(); // Desactivar los controles
            }
            else if (SettingsManager.Instance != null && SettingsManager.Instance.IsCanvasOpen())
            {
                ToggleSettingsPanel(); // Desactivar los ajustes si están abiertos
            }
            else
            {
                // Si no está activo nada más, desactivar el menú de pausa
                InputManager.Instance.EnableActionMap("Player");
                PauseMenuUI.SetActive(false); // Cerrar el menú de pausa
                Time.timeScale = 1f; // Reanudar el juego
                _paused = false;
                EventSystem.current.SetSelectedGameObject(null); // Desactivar la selección del botón
            }
        }

        // Aseguramos que el ResetPanel se desactive
        if (ResetPanel != null)
        {
            ResetPanel.SetActive(false);
        }
    }


    /// <summary>
    /// Devuelve el estado de pausa para poder accederlo desde otros scripts.
    /// </summary>
    /// <returns>True si el juego está pausado, false si no lo está.</returns>
    public bool PauseActive()
    {
        return _paused;
    }

    /// <summary>
    /// Método que fuerza al menu de pausa para que se cierre cuando el jugador pulsa el botón de recetas/indicaciones.
    /// Hasta a mi me da pena tener que hacer codigo tan horrible.
    /// </summary>
    public void ActiveIndicationsAdjusments()
    {
        PauseMenuUI.SetActive(false);
        _paused = false;
    }

    /// <summary>
    /// Abre o cierra el panel de ajustes.
    /// </summary>
    public void ToggleSettingsPanel()
    {
        SettingsManager.Instance.PlaySFX(ButtonSound); // Reproduce el sonido del botón.
        SettingsManager.Instance.TogglePanel(); // Activa o desactiva el panel de ajustes.
    }

    /// <summary>
    /// Abre o cierra el panel de tutorial.
    /// </summary>
    public void ToggleToTutorial()
    {
        if (!goesToTutorial)
        {
            goesToTutorial = true;
            Time.timeScale = 0f; // Pausa el juego.
            GoToTutorialPanel.SetActive(true); // Activa el panel de tutorial.
            InputManager.Instance.EnableActionMap("UI"); // Habilita las acciones del UI.
            EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject); // Selecciona el primer botón del tutorial.
        }
        else
        {
            goesToTutorial = false;
            Time.timeScale = 1f; // Reanuda el tiempo del juego.
            InputManager.Instance.EnableActionMap("Player"); // Habilita las acciones del jugador.
            GoToTutorialPanel.SetActive(false); // Activa el panel de tutorial.
            EventSystem.current.SetSelectedGameObject(null); // Desactiva la selección del botón.
        }
    }

    /// <summary>
    /// Reinicia el progreso del juego, borrando cualquier dato guardado.
    /// </summary>
    public void ResetProgress()
    {
        GameManager.Instance.ResetProgress(); // Llama a la función de reinicio del progreso en el GameManager.
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Cambia la escena después de un retraso.
    /// </summary>
    /// <param name="nameScene">Nombre de la escena a cambiar.</param>
    /// <param name="Unpause">Indica si debe despausar el juego.</param>
    /// <param name="delay">Retraso en segundos antes de cambiar de escena.</param>
    /// <returns>IEnumerator para la Coroutine.</returns>
    private IEnumerator DelayOnSceneChange(string nameScene, bool Unpause = true, float delay = 0.5f)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (Unpause) { Time.timeScale = 1f; } // Despausa el juego si es necesario.
        SceneManager.LoadScene(nameScene); // Carga la nueva escena.
    }

    #endregion

    // ---- MÉTODO DE EVENTO ----
    #region Métodos de Evento

    /// <summary>
    /// Se llama cuando la aplicación es pausada, maneja la entrada para pausar el juego.
    /// </summary>
    void OnApplicationPause()
    {
        if (!_paused && InputManager.Instance != null && !goesToTutorial && _levelManager==null ||
            _levelManager != null && _levelManager.GetCurrentSecondsLeft() > 0 && !_paused && InputManager.Instance != null)
        {
            HandleInput(); // Maneja la entrada si es necesario.
        }
    }

    #endregion
}
// class PauseMenuManager 
// namespace
