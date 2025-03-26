//---------------------------------------------------------
// Este script es el responsable de la mecánica PickDrop y de la visión del jugador
// Este script almacena la informacìón de la mesa que el jugador está mirando para que otros scripts puedan aprovecharlo
// Óliver García Aguado
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------


using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// 
/// Esta clase es la encargada de la mecánica del PickDrop (coger y soltar objetos) 
/// y tambien es la encargada de la visión del jugador. Analiza las mesas cercanas al jugador para determinar cual es la más cercana que este siendo mirada por el jugador.
/// GetActualMesa() devuelve el gameobject de la mesa que el jugador esté mirando.
/// 
/// </summary>
/// 
public class PlayerVision : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private InputActionReference PickDropActionReference; // La acción que realiza el pickDrop

    [SerializeField] LayerMask DetectedTilesLayer; // Esta mascara permite que la detección de mesas solo detecte a gameObjects que tengan esta Layer
    [SerializeField] Color MesaTint = Color.yellow; // El color con el cual se tintará la mesa que esté siendo selecionada/vista por el jugador (_actualmesa)
    [SerializeField] private Collider2D _visionCollider; // Collider que se encarga de detectar las mesas cercanas al jugador.   
    [SerializeField] private float _visionDistance; // La distancia del circulo de detección de mesas con respecto al centro del jugador
    
    [SerializeField] private float _offSetSpeed = 0.1f; // Velocidad a la que se mueve el offset del collider de visión
    

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    public GameObject _actualMesa; // La mesa que está siendo mirada/seleccionada por el jugador, esta mesa se verá tintada de un color, pudiendo ser diferenciada del resto de mesas.
    private GameObject _lookedObject; // El item que se encuentre en la mesa al momento de realizar la mecánica de PickDrop, si es null no hay objecto alguno.
    private GameObject _heldObject;  // El item que está en las manos del jugador.
    public bool _isBeingPicked = false; // determina cuando un objeto está siendo sujetado
    private PlayerMovement _playerMovement; //Referencia al playerMovement para calcular la posicion de los objetos en la mano del jugador.                            
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        Receiver temp = FindAnyObjectByType<Receiver>(); // para el buen funcionamiento del recibidor :)

        if (temp != null)
        {
            temp.GetPlayerVision(this);
        }
        else
        {
            Debug.Log("No hay recibidor en escena");
        }
        
    }
    /// <summary>
    /// Se encarga de la detección de mesas, se da cada detectionRate.
    /// </summary>
    void FixedUpdate()
    {

        _visionCollider.offset = Vector2.Lerp(_visionCollider.offset, _playerMovement.GetLastMove().normalized * _visionDistance , _offSetSpeed * Time.deltaTime); 
        if (_isBeingPicked)
        {
            _heldObject.transform.position = Vector2.Lerp(_heldObject.transform.position,(Vector2)transform.position + _playerMovement.GetLastMove().normalized,_offSetSpeed * Time.deltaTime );
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // me aseguro que solo el collider de la visión del jugador es el que actúa.
        if (_visionCollider.IsTouching(collision) && collision.GetComponent<Mesa>() != null) 
        {
            if (_actualMesa == null) 
            {
                _actualMesa = collision.gameObject;
            }
            else if (collision.gameObject != _actualMesa)
            {
                if (Vector2.Distance(transform.position, collision.transform.position) < Vector2.Distance(transform.position, _actualMesa.transform.position))
                {
                    _actualMesa.GetComponent<SpriteRenderer>().color = Color.white;
                    _actualMesa = collision.gameObject;
                }
            }
            _actualMesa.GetComponent<SpriteRenderer>().color = MesaTint;
            if (_actualMesa.GetComponent<Receiver>() != null)
            {
                if(_heldObject != null)
                {
                Debug.Log("Delivery mode");
                _actualMesa.GetComponent<Receiver>().SetDeliveryMode();
                }
                else
                {
                Debug.Log("Receiving mode");
                _actualMesa.GetComponent<Receiver>().SetReceivingMode();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Mesa>() != null && collision.gameObject == _actualMesa)
        {
            _actualMesa.GetComponent<SpriteRenderer>().color = Color.white;
            if (_actualMesa.GetComponent<Receiver>() != null)
            {
                 Debug.Log("Idle mode");
                _actualMesa.GetComponent<Receiver>().SetIdleMode();
            }
            _actualMesa = null;
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

    // Devuelve actualMesa
    public GameObject GetActualMesa()
    {
        ChangeVelocity(); // Cuando un jugador mira la herramienta cambia la velocidad de esta
        return _actualMesa;
    }
    /// <summary>
    /// Este metodo se encarga de la recogida del item que esté en la mesa que esté mirando el jugador.
    /// </summary>
    public void Pick(GameObject lookedObject)
    {
        if (_heldObject == null)
        {
            _heldObject = lookedObject;
            _heldObject.transform.SetParent(gameObject.transform);
            _isBeingPicked = true;
            _lookedObject = null;
        }
        else Debug.LogError("Ya tienes un objeto en la mano");
    }

    /// <summary>
    /// Este metodo se encarga de el soltado de objetos en la mesa que esté mirando el jugador.
    /// </summary>
    public void Drop()
    {
        _isBeingPicked = false;
        _heldObject.transform.position = _actualMesa.transform.position;
        _heldObject.transform.rotation = Quaternion.identity;
        _heldObject.transform.SetParent(_actualMesa.transform);
        _heldObject = null;
    }

    /// <summary>
    /// Metodo que se encarga de retornar la condicion de si el objecto ha sido recogido para la animacion
    /// </summary>
    /// <returns></returns>
    public bool IsBeingPicked()
    { return _isBeingPicked; }

    public void SetIsBeingPicked(bool isBeingPicked)
    {
        _isBeingPicked = isBeingPicked;
    }


    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
      



    // Las suscripciones al InputActionReference
    private void OnEnable()
    {
        PickDropActionReference.action.performed += PickDrop;
        PickDropActionReference.action.Enable();

    }

    private void OnDisable()
    {
        PickDropActionReference.action.performed -= PickDrop;
        PickDropActionReference.action.Disable();
    }

    /// <summary>
    /// ++(Drop) Si hay un objeto en la mano (heldObject) y hay una herramienta interactiva,
    /// se intenta soltar el elemento:
    ///     Si el objeto en la mano es el correcto y la mesa actual es una herramienta no adecuada, no se permite
    ///     soltar el material y muestra un mensaje por Debug. De lo contrario, suelta el objeto que tenga
    ///     en heldObject
    /// ++(InsertMaterial) Si hay un objeto en la mano (heldObject) y hay objeto en donde mira (lookedObject)
    /// se llama a InsertMaterial
    /// </summary>
    /// <param name="context"></param>
    private void PickDrop(InputAction.CallbackContext context)
    {
        if (_actualMesa != null) 
        {
            ContentAnalizer();
       
            if (_heldObject != null && _lookedObject == null) // EL jugador tiene un objeto en la mano y no hay objeto en la mesa : Soltar el objeto
            {
                #region Antiguo sistema de comprobacion de si se puede soltar el objeto, descartado
                

                // if (_heldObject.GetComponent<Material>() && (_actualMesa.tag == "CraftingTable" && !_heldObject.GetComponent<Objects>() || 
                //     _actualMesa.tag == "Prensa" && (!_heldObject.GetComponent<Objects>() || _heldObject.GetComponent<Objects>().ThereIsMaterial(_heldObject)) ||
                //     (_heldObject.GetComponent<Material>().MaterialTypeReturn() != MaterialType.Arena && _heldObject.GetComponent<Material>().MaterialTypeReturn() != MaterialType.MetalRoca) && _actualMesa.GetComponent<OvenScript>() != null ||
                //     _heldObject.GetComponent<Material>().MaterialTypeReturn() != MaterialType.Madera && _actualMesa.GetComponent<SawScript>() != null ||
                //     _heldObject.GetComponent<Material>().MaterialTypeReturn() != 
                // MaterialType.MetalMineral && _actualMesa.GetComponent<WelderScript>() != null) || _actualMesa.GetComponent<Receiver>() != null)
                // { Debug.Log("No se puede dropear aquí"); }
                #endregion

                if (IsMesaATool()) // Si la mesa es una herramienta
                {
                  _actualMesa.SendMessage("Drop", _heldObject,SendMessageOptions.DontRequireReceiver); // Actualizar referencias y soltar el objeto si es posible
                  if (_heldObject != null) Debug.Log("No se puede soltar el material aquí"); // si el jugador sigue teniendo el objeto es por que no ha podido soltarlo
                }
                else if (_actualMesa.GetComponent<Receiver>() == null) 
                {
                Drop(); // Soltar el objeto
                }
            }
            else if (_heldObject == null && _lookedObject != null) // El jugador no tiene un objeto en la mano y hay un objeto en la mesa : Recoger el objeto
            {
                if ((_actualMesa.GetComponent<OvenScript>() != null && _actualMesa.GetComponent<OvenScript>().ReturnBurnt()) || (_actualMesa.GetComponent<SawScript>() != null && _actualMesa.GetComponent<SawScript>().GetUnpickable()))
                { Debug.Log("No se puede recoger el material"); }
                else
                {
                    // Limpiar referencias antes de recoger el objeto
                    if (IsMesaATool()) 
                    {
                    _actualMesa.SendMessage("Pick",SendMessageOptions.DontRequireReceiver);
                    }
                    Pick(_lookedObject);
                }
            }
            else if (_heldObject != null && _lookedObject != null) InsertMaterial();
        }
    }

    private bool IsMesaATool()
    {
        return _actualMesa.GetComponent<OvenScript>() != null || _actualMesa.GetComponent<SawScript>() != null || _actualMesa.GetComponent<WelderScript>() != null;
    }


    /// <summary>
    /// Este metodo analiza el contenido de la mesa, en busca de actualizar lookedObject para trabajar con él posteriormente.
    /// </summary>
    private void ContentAnalizer()
    {
        _lookedObject = null;

        if (_actualMesa.transform.childCount >= 1)
        {
           _lookedObject = _actualMesa.transform.GetChild(0).gameObject;

            if (_lookedObject.GetComponent<Collider2D>() == null)
            {
            _lookedObject = null;
            }
        }

        
    }



    /// <summary>
    /// Change Velocity se encarga de asignar la velocidad de las herramientas acorde a qué personaje se acerque a estas
    /// En el horno solo funciona cuando se coloca el material
    /// En la sierra y la soldadora cuando se interactue
    /// Hecho por Guillermo Ramos
    /// </summary>

    private void ChangeVelocity()
    {
        gameObject.GetComponent<PlayerManager>().SetVel(this.gameObject.GetComponent<PlayerManager>().PlayerNum());
        if (_actualMesa != null && _actualMesa.GetComponent<OvenScript>() != null && !_actualMesa.GetComponent<OvenScript>().ReturnInProgress())
        {
            _actualMesa.GetComponent<OvenScript>().ChangeVelocity(gameObject.GetComponent<PlayerManager>().ReturnOven());
        }
        else if (_actualMesa != null && _actualMesa.GetComponent<SawScript>() != null)
        {
            _actualMesa.GetComponent<SawScript>().ChangeMaxClicks(gameObject.GetComponent<PlayerManager>().ReturnSaw());
        }
        else if (_actualMesa != null && _actualMesa.GetComponent<WelderScript>() != null)
        {
            _actualMesa.GetComponent<WelderScript>().ChangeMaxTime(gameObject.GetComponent<PlayerManager>().ReturnWelder());
        }
    }

    /// <summary>
    /// Método que inserta el material al objeto llamando al AddMaterial del script de Objets, siempre y cuando si el lookedObject
    /// está en una mesa con el tag de "CraftingTable", también mira si el objeto añadido es otro objeto, si es así no se realizará
    /// el AddMaterial y si el objeto en el que se le añade el material está lleno, se notifica de dicho detalle.
    /// </summary>
    private void InsertMaterial()
    {
        if (_heldObject.GetComponent<Objects>() && _lookedObject.GetComponent<Objects>())
        {
            Debug.Log("No puedes insertar un objeto dentro de otro objeto");
        }
        else if (_lookedObject.GetComponent<Objects>())
        {
            if (_actualMesa != null && _actualMesa)
            {
                Objects objetoScript = _lookedObject.GetComponent<Objects>();
                bool materialAdded = objetoScript.AddMaterial(_heldObject);
                if (materialAdded)
                {
                    _heldObject.SetActive(false);
                    _heldObject = null; // El material ha sido introducido, por lo que ya no está en la mano.
                    _isBeingPicked = false;
                }
                else Debug.Log("No se pudo añadir el material al objeto");
            }
            else Debug.Log("Solo insertar materiales en objetos que estén en la mesa de trabajo");
        }
    }


    #endregion


}
// class PlayerVision 
// namespace
