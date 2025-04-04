//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Security;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// BackgroundMenu se encarga de relaizar el efecto de Parallax en el Fondo del Menu
/// </summary>
public class BackgroundMenu : MonoBehaviour
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
    [SerializeField] private float Limit;// El limite hasta donde llega la camara antes de volver a su posicion incial
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
    private float _distanceTravelled;//la distancia que se ha recorrido 
    private Vector3 _camStartPosition;//La posicion incial de la camara
    private Rigidbody2D rb; //El rigidbody para recoger el de la camara
    
    
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
        _camStartPosition = Camera.transform.position;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
        rb = Camera.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// El Update se encarga de calcular la distancia que se ha recorrido, Ver cuando se repiten las capas, 
    /// el movimiento de la camara que hace posible el movimiento del fondo y regresar la camara a la posicion incial
    /// si llega al limite puesto
    /// </summary>
    void Update()
    {
        _distanceTravelled = Camera.transform.position.x * ParallaxEffect;
        float repeat = Camera.transform.position.x * (1 - ParallaxEffect);
        transform.position = new Vector2(_startPos.x + _distanceTravelled, transform.position.y);

        if(repeat > _startPos.x + _length)
        {
            _startPos.x += _length;
        }
        else if (repeat < _startPos.x - _length)
        {
            _startPos.x -= _length;
        }

        rb.velocity = new Vector3(-1,0,0) * CameraSpeed;

        if (Camera.transform.position.x < Limit)
        {
            transform.position = _startPos;
            Camera.transform.position = _camStartPosition;
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

    #endregion   

} // class Background 
// namespace
