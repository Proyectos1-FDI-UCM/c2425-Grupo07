//---------------------------------------------------------
// Este script es el responsable de la mecánica PickDrop y de la visión del jugador
// Este script almacena la informacìón de la mesa que el jugador está mirando para que otros scripts puedan aprovecharlo
// Óliver García Aguado
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------


using UnityEngine;
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
    [SerializeField] LayerMask DetectedTilesLayer; // Esta mascara permite que la detección de mesas solo detecte a gameObjects que tengan esta Layer
    [SerializeField] Color MesaTint = Color.yellow; // El color con el cual se tintará la mesa que esté siendo selecionada/vista por el jugador (_actualmesa)
    [SerializeField] Color ItemTint; // El color con el cual se tintará el item que esté siendo selecionada/vista por el jugador (_lookedObject)
    [SerializeField] private Collider2D _visionCollider; // Collider que se encarga de detectar las mesas cercanas al jugador.   
    [SerializeField] private float _visionDistance; // La distancia del circulo de detección de mesas con respecto al centro del jugador
    
    [SerializeField] private float _offSetSpeed = 0.1f; // Velocidad a la que se mueve el offset del collider de visión

    [SerializeField] private Vector2 MaterialPositionOffset = new Vector2(0, 0);
    // Se usa para que el material se coloque en la posición correcta al soltarlo en una herramienta

    /// <summary>
    /// sonido que se reproduce cuando el jugador coge un item
    /// </summary>
    [SerializeField] private AudioClip PickSFX;

    /// <summary>
    /// sonido que se reproduce cuando el jugador suelta un item
    /// </summary>
    [SerializeField] private AudioClip DropSFX; 
    

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private GameObject _actualMesa; // La mesa que está siendo mirada/seleccionada por el jugador, esta mesa se verá tintada de un color, pudiendo ser diferenciada del resto de mesas.
    private GameObject _objectInTable; // El item que se encuentre en la mesa al momento de realizar la mecánica de PickDrop, si es null no hay objecto alguno.
    //private GameObject _lookedObject; // El item que esté mirando el jugador se tintará de un color específico
    private GameObject _heldObject;  // El item que está en las manos del jugador.
    private bool _isBeingPicked = false; // determina cuando un objeto está siendo sujetado
    private PlayerMovement _playerMovement; //Referencia al playerMovement para calcular la posicion de los objetos en la mano del jugador.   

    private AudioSource _playerAudioSource;

                        
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
    private void Start()
    {

        _playerAudioSource = GetComponent<AudioSource>();
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
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// Comprueba si el jugador ha pulsado el botón de recoger y dejar para 
    /// realizar la acción
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.PickDropWasPressedThisFrame())
        {
            PickDrop();
        }
    }
    /// <summary>
    /// Se encarga de la detección de mesas, se da cada detectionRate.
    /// Se ejecuta después de las físicas 50 veces por segundo
    /// </summary>
    void FixedUpdate()
    {
        _visionCollider.offset = Vector2.Lerp(_visionCollider.offset, InputManager.Instance.LastMovementVector.normalized * _visionDistance , _offSetSpeed * Time.deltaTime); 
        if (_heldObject!= null && _isBeingPicked)
        {
            _heldObject.transform.position = Vector2.Lerp(_heldObject.transform.position,(Vector2)transform.position + InputManager.Instance.LastMovementVector.normalized,_offSetSpeed * Time.deltaTime );
        }
    }

    /// <summary>
    /// Se encarga de detectar si el jugador ha entrado en contacto con una mesa, si es así, se guarda la referencia de la mesa en _actualMesa.
    /// Si el jugador entra en contacto con otra mesa, se compara la distancia entre el jugador y la mesa, si es menor que la distancia entre el jugador y la mesa anterior, se guarda la nueva mesa como _actualMesa.
    /// Si el jugador entra en contacto con el recibidor, se llama al metodo correspondiente en funcion de si el jugador lleva un objeto en la mano o no.
    /// </summary>
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
                    if (_actualMesa.transform.childCount>0&& _actualMesa.transform.GetChild(0)!=null) // Hecho por Guillermo, se deja de tintar el objeto cuando se deja
                                                                                                        // de tintar la mesa
                    {
                        _actualMesa.GetComponent<Mesa>().UnTintObject();
                    }
                    _actualMesa.GetComponent<Mesa>().IsBeingLooked(false);
                    _actualMesa = collision.gameObject;
                }
            }
            _actualMesa.GetComponent<SpriteRenderer>().color = MesaTint;
            //Tinta el objeto dentro de la mesa
            _actualMesa.GetComponent<Mesa>().IsBeingLooked(true);
            if (_actualMesa.transform.childCount > 0 && _actualMesa.transform.GetChild(0) != null)
            {
                _actualMesa.GetComponent<Mesa>().TintObject(_actualMesa.transform.GetChild(0).gameObject);
            }
            if (_actualMesa.GetComponent<Receiver>() != null)
            {
                if (_heldObject != null)
                {
                    Debug.Log("Delivery mode");
                    _actualMesa.GetComponent<Receiver>().AnalizeDeliveredObject(_heldObject);
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

    /// <summary>
    /// Se encarga de detectar si el jugador ha salido de contacto con una mesa, si es así, se elimina la referencia de la mesa en _actualMesa.
    /// Si el jugador sale de contacto con el recibidor, se llama a SetIdleMode() para que el recibidor vuelva a su estado normal.
    /// Desactiva el efecto objeto si se deja de mirar
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Mesa>() != null && collision.gameObject == _actualMesa)
        {
            _actualMesa.GetComponent<SpriteRenderer>().color = Color.white;
            //Se deja de tintar el objeto, por tanto se deja de mirar la mesa
            if (_actualMesa.transform.childCount > 0 && _actualMesa.transform.GetChild(0) != null)
            {
                _actualMesa.GetComponent<Mesa>().UnTintObject();
            }
            _actualMesa.GetComponent<Mesa>().IsBeingLooked(false);
            if (_actualMesa.GetComponent<Receiver>() != null)
            {
                 Debug.Log("Idle mode");
                _actualMesa.GetComponent<Receiver>().SetIdleMode();
            }
            _actualMesa = null;
            _visionCollider.enabled = false;
            _visionCollider.enabled = true;
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
            if (_playerAudioSource != null)
            {
            _playerAudioSource.PlayOneShot(PickSFX);
            }
            _heldObject = lookedObject;
            _heldObject.transform.SetParent(gameObject.transform);
            _isBeingPicked = true;
            _objectInTable = null;
        }
        else Debug.LogError("Ya tienes un objeto en la mano");
    }

    /// <summary>
    /// Este metodo se encarga de el soltado de objetos en la mesa que esté mirando el jugador.
    /// onToolPlaced permite saber si el objeto se ha soltado en una herramienta para ajustarle un offset en la disposición del material en pantalla.
    /// </summary>
    public void Drop(bool onToolPlaced = false)
    {
        if (_playerAudioSource != null)
        {
        _playerAudioSource.PlayOneShot(DropSFX);
        }
        _isBeingPicked = false;
        _heldObject.transform.position = _actualMesa.transform.position;
        _heldObject.transform.rotation = Quaternion.identity;
        _heldObject.transform.SetParent(_actualMesa.transform);
        if (onToolPlaced)
        {
            _heldObject.transform.position += (Vector3)MaterialPositionOffset;
        }
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
    private void PickDrop()
    {
        if (_actualMesa != null) 
        {
            ContentAnalizer();

            if (_heldObject != null && _objectInTable == null) // EL jugador tiene un objeto en la mano y no hay objeto en la mesa : Soltar el objeto
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
                    _actualMesa.SendMessage("Drop", _heldObject, SendMessageOptions.DontRequireReceiver); // Actualizar referencias y soltar el objeto si es posible
                    if (_heldObject != null) Debug.Log("No se puede soltar el material aquí"); // si el jugador sigue teniendo el objeto es por que no ha podido soltarlo
                }
                else if (_actualMesa.GetComponent<Receiver>() == null && !(_heldObject.GetComponent<FireExtinguisher>() != null && _actualMesa.GetComponent<Mesa>().TableTypeReturn() == Mesa.TableType.Conveyor))
                {
                    Drop(); // Soltar el objeto
                }
            }
            else if (_actualMesa.GetComponent<OvenScript>() == null && _heldObject == null && _objectInTable != null)
            {
                // Limpiar referencias antes de recoger el objeto
                if (IsMesaATool())
                {
                    _actualMesa.SendMessage("Pick", SendMessageOptions.DontRequireReceiver);
                }
                Pick(_objectInTable);
            }
            else if (_actualMesa.GetComponent<OvenScript>() != null && !_actualMesa.GetComponent<OvenScript>().ReturnBurnt() && _heldObject == null && _objectInTable != null)
            {
                // Limpiar referencias antes de recoger el objeto
                if (IsMesaATool())
                {
                    _actualMesa.SendMessage("Pick", SendMessageOptions.DontRequireReceiver);
                }
                Pick(_objectInTable);
            }
            //else if (_heldObject == null && _lookedObject != null) // El jugador no tiene un objeto en la mano y hay un objeto en la mesa : Recoger el objeto
            //{
 
            //}
            else if (_heldObject != null && _objectInTable != null) InsertMaterial();
        }
    }

    /// <summary>
    ///  Devuelve una boleana que indica si la mesa actual es una herramienta o no.
    /// </summary>
    /// <returns></returns>
    private bool IsMesaATool()
    {
        return _actualMesa.GetComponent<Mesa>().TableTypeReturn() == Mesa.TableType.Tool;
    }


    /// <summary>
    /// Este metodo analiza el contenido de la mesa, en busca de actualizar lookedObject para trabajar con él posteriormente.
    /// </summary>
    private void ContentAnalizer()
    {
        _objectInTable = null;

        if (_actualMesa.transform.childCount >= 1)
        {
           _objectInTable = _actualMesa.transform.GetChild(0).gameObject;

            if (_objectInTable.GetComponent<Collider2D>() == null)
            {
            _objectInTable = null;
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
        else if (_actualMesa != null && _actualMesa.GetComponent<AnvilScript>() != null)
        {
            _actualMesa.GetComponent<AnvilScript>().ChangeMaxClicks(gameObject.GetComponent<PlayerManager>().ReturnAnvil());
        }
        else if (_actualMesa != null && _actualMesa.GetComponent<SawScript>() != null)
        {
            _actualMesa.GetComponent<SawScript>().ChangeMaxTime(gameObject.GetComponent<PlayerManager>().ReturnSaw());
        }
    }

    /// <summary>
    /// Método que inserta el material al objeto llamando al AddMaterial del script de Objets, siempre y cuando si el lookedObject
    /// está en una mesa con el tag de "CraftingTable", también mira si el objeto añadido es otro objeto, si es así no se realizará
    /// el AddMaterial y si el objeto en el que se le añade el material está lleno, se notifica de dicho detalle.
    /// </summary>
    private void InsertMaterial()
    {
        if (_heldObject.GetComponent<Objects>() && _objectInTable.GetComponent<Objects>())
        {
            Debug.Log("No puedes insertar un objeto dentro de otro objeto");
        }
        else if (_objectInTable.GetComponent<Objects>())
        {
            if (_actualMesa != null && _actualMesa.GetComponent<CraftingTableScript>() != null)
            {
                CraftingTableScript craftingScript = _actualMesa.GetComponent<CraftingTableScript>();
                bool materialAdded = craftingScript.AddMaterial(_heldObject.GetComponent<Material>().MaterialTypeReturn());
                if (materialAdded)
                {
                    // _heldObject.SetActive(false);
                    
                    Destroy(_heldObject);
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
