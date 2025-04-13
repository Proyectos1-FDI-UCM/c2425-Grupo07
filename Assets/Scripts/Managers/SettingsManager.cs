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
using TMPro;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
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
    [SerializeField] GameObject SettingsCanvas; // El panel de la interfaz (UI) de los ajustes
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
    bool _canvasOpen = true; // Booleano que comprueba si está el panel de la interfaz (UI) para activarlo / desactivarlo en el método
    int _currentResolutionWidth; // Valor de la resolución actual en la anchura
    int _currentResolutionHeight; // Valor de la resolución actual en la altura
    Resolution[] _resolutionsList; // Lista de resoluciones de Unity
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

        int _currentResolutionIndex=0;
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
        ResolutionDropdown.value = _currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();

        //1920 / 3 = 640, 1080 / 3 = 360
        Application.targetFrameRate = 60;
        //_cam.orthographicSize = ((_currentResolutionHeight) / (_currentResolutionWidth / 640 * 37)) * 0.5f;// 32 pixeles por unidad
        _isOnFullscreen = true;
        _currentResolutionWidth = Screen.currentResolution.width;
        _currentResolutionHeight = Screen.currentResolution.height;
        Screen.SetResolution(_currentResolutionWidth, _currentResolutionHeight, FullScreenMode.MaximizedWindow, Screen.currentResolution.refreshRateRatio);
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
    /// <summary>
    /// Se encarga de reproducir música
    /// </summary>
    /// <param name="clip"></param>
    public void PlayMusic(AudioClip clip)
    {
        MusicSource.PlayOneShot(clip);
    }
    /// <summary>
    /// Ajusta el volumen de la música entre un valor de -80 y 0 
    /// (decibelios mínimos y máximos)
    /// </summary>
    /// <param name="Mvol"></param>
    public void AjustaMus(float Mvol)
    {
        AudioMix.SetFloat("Music", Mvol);
    }
    /// <summary>
    /// Ajusta el volumen de los efectos de sonido entre un valor de -80 y 0 
    /// (decibelios mínimos y máximos) 
    /// </summary>
    /// <param name="Svol"></param>
    public void AjustaSf(float Svol)
    {
        AudioMix.SetFloat("SFX", Svol);
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
        }
        else
        {
            Screen.SetResolution(_currentResolutionWidth, _currentResolutionHeight, FullScreenMode.Windowed, Screen.currentResolution.refreshRateRatio);
            _isOnFullscreen = false;
        }
    }
    /// <summary>
    /// Ajusta la resolución de la pantalla dependiendo si está en pantalla completa o no,
    /// Escoge el elemento de la lista de resoluciones con la deseada en el menú desplegable
    /// </summary>
    /// <param name="resolutionIndex"></param>
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutionsList[resolutionIndex];
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
        }
        else
        {
            SettingsCanvas.SetActive(true);
        }
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
