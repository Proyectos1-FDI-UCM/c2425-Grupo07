//---------------------------------------------------------
// Se programa en el script la forma en que la música se va a cambiar dependiendo de la escena en el que está el jugador
// Liling Chen
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.SceneManagement;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class SetMusic : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] private AudioClip[] MusicClip; //todos los audios musicales para el juego
    [SerializeField] private AudioSource MusicSource; //el audio a cambiar
    [SerializeField] private float PitchOne = 1.1f; //Valor para el primer cambio de Pitch
    [SerializeField] private float PitchTwo = 1.4f; //Segundo valor para el segundo cambio de Pitch

    [SerializeField] private float SecondsLeft; // Segundos que quedan del nivel por acabarse
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private string[] _sceneNames; //Nombre de todas las escenas de la built
    private string _actualScene; //Nombre de la escena actual

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    
    /// <summary>
    /// Awake is called on the frame when a script is enabled.
    /// </summary>
    void Awake()
    {
        ObtainAllScenes();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "NivelPrincipal" || SceneManager.GetActiveScene().name == "NivelInfinito")
        {
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            SecondsLeft = levelManager.GetCurrentSecondsLeft();
            ChangePitch(SecondsLeft);
        }
        else { MusicSource.pitch = 1f; }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Obtiene los nombres de las escenas en un array privado
    /// </summary>
    private void ObtainAllScenes()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        _sceneNames = new string[sceneCount];

        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            _sceneNames[i] = System.IO.Path.GetFileNameWithoutExtension(scenePath);
        }
    }

    /// <summary>
    /// Busca la escena activa para cambiar la música
    /// </summary>
    private void SearchForPlayer()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        _actualScene = currentScene.name;
        
        int i = 0;

        while(_actualScene != _sceneNames[i] && i < _sceneNames.Length)
        {
            i++;
        }

        SetNewMusic(i);
    }


    /// <summary>
    /// Cambia la música de la escena y es sustituida por la que está en la array de MusicClip con el parametro entrante i
    /// </summary>
    /// <param name="i"></param>
    private void SetNewMusic(int i)
    {
        if(MusicClip[i] != null)
        {
            MusicSource.clip = MusicClip[i];
            MusicSource.Play();
        }
        else
        {
            MusicSource.clip = null;
        }
    }

    /// <summary>
    /// Cada vez que se carga la escena se llama a este método
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SearchForPlayer();
    }

    /// <summary>
    /// Cambia el pitch para los niveles cuando es menos de 60 segundos y 10 segundos
    /// </summary>
    /// <param name="seconds"></param>
    private void ChangePitch(float seconds)
    {
        if (seconds > 60f) { MusicSource.pitch = 1f; }
        if (seconds < 60f && seconds > 10f) MusicSource.pitch = PitchOne;
        else if(seconds < 10f) MusicSource.pitch = PitchTwo;
    }
    #endregion

} // class SetMusic 
// namespace
