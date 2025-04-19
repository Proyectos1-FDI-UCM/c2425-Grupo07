//---------------------------------------------------------
// Responsable de regular el volumen del juego y ajustar las resoluciones, además de activar la pantalla completa y
// Y desactivar el panel de la UI
// Guillermo Isaac Ramos Medina
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
//using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
// Añadir aquí el resto de directivas using


/// <summary>
/// Se controla el sonido a través de un AudioMix, responsable de modificar la música y los 
/// efectos de sonido en un mismo componente. También se piden los componentes que reproducen
/// el sonido para implementarlo mejor más tarde. También, para controlar el panel de ajustes se 
/// necesitará acceder al menú desplegable para meterle las resoluciones y poder cambiarlas y al botón
/// que se seleccionará al abrir el panel.
/// Se comprobará si la pantalla está completa para cambiar el modo de pantalla y el panel de la 
/// interfaz para abrir y cerrarlo.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    //Audio
    [SerializeField] AudioSource MusicSource; // La fuente de audio de la que se reproduce el contenido musical
    [SerializeField] AudioSource SfxSource; // La fuente de audio de la que se reproducen los efectos de sonido
    [SerializeField] AudioMixer AudioMix; // El mezclador de audio que contiene la separación de sonidos
    [SerializeField] TMP_Dropdown ResolutionDropdown; // El elemento de la interfaz (UI) que contiene la lista de resoluciones del juego
    [SerializeField] Button BackButton; // Botón que se seleccionará al abrir el panel
    [SerializeField] GameObject SettingsCanvas; // El panel de la interfaz (UI) de los ajustes
    [SerializeField] GameObject ScreenUI; // El UI de la pantalla
    //Elementos del UI para configurarlos con PlayerPrefs
    [SerializeField] Slider MusicSlider; // El slider de la música
    [SerializeField] Slider SFXSlider; // El slider de los efectos de sonido
    [SerializeField] Toggle ToggleButton; // El cuadrado que indica si está en pantalla completa o no
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    bool _isOnFullscreen; // Booleano que comprueba si está en pantalla completa para activarlo / desactivarlo en el método
    bool _canvasOpen = false; // Booleano que comprueba si está el panel de la interfaz (UI) para activarlo / desactivarlo en el método
    int _currentResolutionWidth; // Valor de la resolución actual en la anchura
    int _currentResolutionHeight; // Valor de la resolución actual en la altura
    Resolution[] _resolutionsList; // Lista de resoluciones de Unity
    private static SettingsManager _instance; // Instancia única de la clase (singleton).

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// Además se encarga de asignar las resoluciones disponibles para meter en el menú desplegable 
    /// Asigna la tasa de frames y activa la pantalla completa al empezar el juego
    /// </summary>
    void Start()
    {
        _resolutionsList = Screen.resolutions;
        ResolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int _currentResolutionIndex = 0;
        for (int i = 0; i < _resolutionsList.Length; i++)
        {
            string option = _resolutionsList[i].width + " x " + _resolutionsList[i].height + " " + _resolutionsList[i].refreshRateRatio + "hz";
            options.Add(option);
            if (_resolutionsList[i].width == Screen.currentResolution.width && _resolutionsList[i].height == Screen.currentResolution.height)
            {
                _currentResolutionIndex = i;
            }
        }
        ResolutionDropdown.AddOptions(options);
        //ResolutionDropdown.value = _currentResolutionIndex;
        //ResolutionDropdown.RefreshShownValue();

        //1920 / 3 = 640, 1080 / 3 = 360
        Application.targetFrameRate = 60;

        //_cam.orthographicSize = ((_currentResolutionHeight) / (_currentResolutionWidth / 640 * 37)) * 0.5f;// 32 pixeles por unidad
        //_isOnFullscreen = true;
        //_currentResolutionWidth = Screen.currentResolution.width;
        //_currentResolutionHeight = Screen.currentResolution.height;
        //Screen.SetResolution(_currentResolutionWidth, _currentResolutionHeight, FullScreenMode.MaximizedWindow, Screen.currentResolution.refreshRateRatio);

        //Inicializar con las preferencias
        MusicSlider.value = PlayerPrefs.GetFloat("musicVolume", -30);
        SFXSlider.value = PlayerPrefs.GetFloat("effectsVolume", -30);
        if (PlayerPrefs.GetInt("IsFullScreen", 0) == 0)
        {
            //_isOnFullscreen = false;
            ToggleButton.isOn = true;
        }
        else
        {
            //_isOnFullscreen = true;
            ToggleButton.isOn = false;
        }
        ToggleFullScreen();
        Debug.Log(_currentResolutionIndex);
        _currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", _currentResolutionIndex);
        SetResolution(_currentResolutionIndex);
        Debug.Log(_currentResolutionIndex);
        ResolutionDropdown.value = _currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();

        if (Application.isMobilePlatform)
        {
            ScreenUI.SetActive(false);
        }
    }


    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static SettingsManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    protected void Awake()
    {
        if (_instance != null)
        {
            // No somos la primera instancia. Se supone que somos un
            // SettingsManager de una escena que acaba de cargarse, pero
            // ya había otro en DontDestroyOnLoad que se ha registrado
            // como la única instancia.
            // Si es necesario, transferimos la configuración que es
            // dependiente de la escena. Esto permitirá al SettingsManager
            // real mantener su estado interno pero acceder a los elementos
            // de la escena particulares o bien olvidar los de la escena
            // previa de la que venimos para que sean efectivamente liberados.
            Debug.Log("Ya existe una instancia de SettingsManager. Destruyendo esta instancia.");

            // Y ahora nos destruímos del todo. DestroyImmediate y no Destroy para evitar
            // que se inicialicen el resto de componentes del GameObject para luego ser
            // destruídos. Esto es importante dependiendo de si hay o no más managers
            // en el GameObject.
            DestroyImmediate(this.gameObject);
        }
        else
        {
            // Somos el primer SettingsManager.
            // Queremos sobrevivir a cambios de escena.
            Debug.Log("Inicializando SettingsManager.");
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        } // if-else somos instancia nueva o no.
    }
    /// <summary>
    /// Método llamado cuando se destruye el componente.
    /// </summary>
    protected void OnDestroy()
    {
        if (this == _instance)
        {
            // Éramos la instancia de verdad, no un clon.
            _instance = null;
        } // if somos la instancia principal
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
    /// Se encarga de reproducir un efecto de sonido
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySFX(AudioClip clip)
    {
        SfxSource.PlayOneShot(clip);
    }

    public void AudioSliderTest()
    {
        if (!SfxSource.isPlaying){
             SfxSource.Play();
        }
       
    }

    public void AnvilSFX(AudioClip clip)
    {
        AudioClip prev_clip = SfxSource.clip;
        SfxSource.clip = clip;
        SfxSource.Play();
        SfxSource.clip = prev_clip;
    }
    /// <summary>
    /// Se encarga de reproducir música
    /// </summary>
    /// <param name="clip"></param>
    public void PlayMusic(AudioClip clip)
    {
        MusicSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Se encarga de parar toda la musica y sonidos que estén reproduciéndose
    /// </summary>
    public void StopAllSounds()
    {
        MusicSource.Stop();
    }


    /// <summary>
    /// Ajusta el volumen de la música entre un valor de -80 y 0 
    /// (decibelios mínimos y máximos)
    /// </summary>
    /// <param name="Mvol"></param>
    public void AjustaMus(float Mvol)
    {

        AudioMix.SetFloat("Music", Mvol);
        PlayerPrefs.SetFloat("musicVolume", Mvol);

    }
    /// <summary>
    /// Ajusta el volumen de los efectos de sonido entre un valor de -80 y 0 
    /// (decibelios mínimos y máximos) 
    /// </summary>
    /// <param name="Svol"></param>
    public void AjustaSf(float Svol)
    {
        AudioMix.SetFloat("SFX", Svol);
        PlayerPrefs.SetFloat("effectsVolume", Svol);
    }
    /// <summary>
    /// Intercambia entre pantalla completa y ventana
    /// Depende del booleano _isOnFullscreen
    /// </summary>
    public void ToggleFullScreen ()
    {
        if (!_isOnFullscreen)
        {
            Screen.SetResolution(_currentResolutionWidth, _currentResolutionHeight, FullScreenMode.MaximizedWindow, Screen.currentResolution.refreshRateRatio);
            _isOnFullscreen=true;
            PlayerPrefs.SetInt("IsFullScreen", 0); // 0 true
        }
        else
        {
            Screen.SetResolution(_currentResolutionWidth, _currentResolutionHeight, FullScreenMode.Windowed, Screen.currentResolution.refreshRateRatio);
            _isOnFullscreen = false;
            PlayerPrefs.SetInt("IsFullScreen", 1); // 1 false
        }
    }
    /// <summary>
    /// Ajusta la resolución de la pantalla dependiendo si está en pantalla completa o no,
    /// Escoge el elemento de la lista de resoluciones con la deseada en el menú desplegable
    /// </summary>
    /// <param name="setResolutionIndex"></param>
    public void SetResolution(int setResolutionIndex)
    {
        Resolution resolution = _resolutionsList[setResolutionIndex];
        _currentResolutionHeight = resolution.height;
        _currentResolutionWidth = resolution.width;
        if (!_isOnFullscreen)
        {
            Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.Windowed, Screen.currentResolution.refreshRateRatio);
        }
        else
        {
            Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.MaximizedWindow, Screen.currentResolution.refreshRateRatio);
        }
        PlayerPrefs.SetInt("ResolutionIndex", setResolutionIndex); 
    }
    /// <summary>
    /// Intercambia entre abrir y cerrar el panel
    /// Depende del booleano _canvasOpen
    /// </summary>
    public void TogglePanel()
    {
        if (_canvasOpen)
        {
            SettingsCanvas.SetActive(false);
            _canvasOpen = false;
            EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject); // Selecciona el primer botón del canvas que encuentre para el funcionamiento del mando
        }
        else
        {
            SettingsCanvas.SetActive(true);
            _canvasOpen = true;
            BackButton.Select();
        }
    }
    /// <summary>
    /// Devuelve si el panel está abierto para abrir y cerrarlo en el pause UI
    /// </summary>
    /// <returns></returns>
    public bool IsCanvasOpen()
    {
        return _canvasOpen;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class AudioManager 
// namespace
