//---------------------------------------------------------
// BackgroundMenu se encarga de realizar el efecto de Parallax en el Fondo del Menu (TEST)
// Liling Chen
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Security;
using UnityEngine.U2D;
using UnityEngine;
// Añadir aquí el resto de directivas using

public class TestBackgroundLili : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] GameObject Camera; //Obtiene el objecto de la camara
    [SerializeField] private float ParallaxEffect; //Recoge el numero para distanciar las distintas capas
    [SerializeField] private float ResetThreshold = 1000f; //Distancia a la que llega la camara para volver
    [SerializeField] private float CameraSpeed;//La velocidad de la camara, y por tanto el background
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private float _length;// La longitud que avanzan las capas de sprites
    private Vector3 _startPos; //la posicion incial de las capas de spites
    private Transform _camTransform; //Referencia al transform de la cámara
    private Vector3 _camStartPosition;//La posicion incial de la camara
    private float _totalCameraDistance; //Distancia recorrida por la cámara
    private PixelPerfectCamera _pixelPerfect;


    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Incia las posiciones inciales de las capas y de la camara, asi como la longuitud y recoge el RB de la camara
    /// </summary>
    void Start()
    {
        _startPos = transform.position;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
        _camTransform = Camera.transform;
        _pixelPerfect = Camera.GetComponent<PixelPerfectCamera>();
        _totalCameraDistance = 0f;
    }

    /// <summary>
    /// El Update se encarga de calcular la distancia que se ha recorrido, Ver cuando se repiten las capas, 
    /// el movimiento de la camara que hace posible el movimiento del fondo y regresar la camara a la posicion incial
    /// si llega al limite puesto
    /// </summary>
    void Update()
    {
        // Movimiento de cámara
        float frameMovement = CameraSpeed * Time.deltaTime;
        _camTransform.Translate(Vector3.left * frameMovement);
        _totalCameraDistance += frameMovement;

        // Reset de posiciones para mantener precisión
        if (Mathf.Abs(_totalCameraDistance) > ResetThreshold)
        {
            ResetWorldPositions();
        }

        // Cálculo de parallax
        Vector2 cameraOffset = _pixelPerfect.RoundToPixel(_camTransform.position);
        float parallaxOffset = cameraOffset.x * ParallaxEffect;
        transform.position = _pixelPerfect.RoundToPixel(
            new Vector2(_startPos.x + parallaxOffset, transform.position.y));

        // Repetición del fondo
        float relativeCamPos = cameraOffset.x * (1 - ParallaxEffect);
        if (relativeCamPos > _startPos.x + _length)
        {
            _startPos.x += _length;
        }
        else if (relativeCamPos < _startPos.x - _length)
        {
            _startPos.x -= _length;
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

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    private void ResetWorldPositions()
    {
        float resetAmount = _totalCameraDistance;
        _camTransform.position -= Vector3.right * resetAmount;
        _startPos.x -= resetAmount * ParallaxEffect;
        _totalCameraDistance = 0f;
    }
    #endregion

} // class Background 
// namespace
