//---------------------------------------------------------
// Se programa el funcionamiento de la prensa
// Liling Chen
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PressScript : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] private float PressTime = 5f; // Tiempo que tarda la prensa en devolver el objeto a su estado original.
    [SerializeField] private GameObject ProgressBar; // Barra de progreso visual (opcional).
    [SerializeField] private GameObject[] OriginalStatePrefabs; // Lista de prefabs de los objetos en su estado original.

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private float _currentPressTime = 0f; // Tiempo actual de prensado.
    private bool _isPressing = false; // Indica si la prensa está activa.
    private GameObject _currentObject; // Objeto actual en la prensa.
    private Vector3 _originalPosition; // Posición original del objeto (para devolverlo después).

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_isPressing)
        {
            _currentPressTime += Time.deltaTime;

            // Actualiza la barra de progreso (si existe).
            if (ProgressBar != null)
            {
                float progress = _currentPressTime / PressTime;
                ProgressBar.transform.localScale = new Vector3(progress, 1, 1);
            }

            // Si se completa el tiempo de prensado, devuelve el objeto a su estado original.
            if (_currentPressTime >= PressTime)
            {
                ResetObject();
            }
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

    public void StartPress(GameObject objectToPress)
    {
        if (!_isPressing && objectToPress != null)
        {
            _currentObject = objectToPress;
            _originalPosition = _currentObject.transform.position;
            _isPressing = true;
            _currentPressTime = 0f;

            Debug.Log("Prensado iniciado.");
        }
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    private void ResetObject()
    {
        if (_currentObject != null)
        {
            // Busca el prefab correspondiente al objeto actual.
            GameObject originalPrefab = FindOriginal(_currentObject);

            if (originalPrefab != null)
            {
                // Destruye el objeto actual.
                Destroy(_currentObject);

                // Instancia el objeto en su estado original.
                GameObject newObject = Instantiate(originalPrefab, _originalPosition, Quaternion.identity);
                newObject.name = originalPrefab.name; // Mantén el nombre original.

                Debug.Log("Objeto devuelto a su estado original: " + originalPrefab.name);
            }
            else
            {
                Debug.LogWarning("No se encontró el prefab original para el objeto: " + _currentObject.name);
            }

            // Reinicia la prensa.
            _isPressing = false;
            _currentObject = null;
            _currentPressTime = 0f;

            // Reinicia la barra de progreso (si existe).
            if (ProgressBar != null)
            {
                ProgressBar.transform.localScale = new Vector3(0, 1, 1);
            }
        }
    }

    private GameObject FindOriginal(GameObject currentObject)
    {
        foreach (GameObject prefab in OriginalStatePrefabs)
        {
            if (prefab.name == currentObject.name)
            {
                return prefab;
            }
        }
        return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isPressing)
        {
            StartPress(other.gameObject);
        }
    }

    #endregion   

} // class PressScript 
// namespace
