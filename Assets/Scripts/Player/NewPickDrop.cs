//---------------------------------------------------------
// Sistema de recogida y colocación de objetos
// Responsable de detectar tiles cercanas e interactuar con ellas
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
// Añadir aquí el resto de directivas using


/// <summary>
/// Controla la mecánica de recoger y soltar objetos.
/// Detecta las tiles cercanas al jugador y permite interactuar con ellas.
/// </summary>
public class NewPickDrop : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Capa que contiene las tiles con las que se puede interactuar
    [SerializeField] private LayerMask targetLayerMask;
    // Radio de detección alrededor del jugador
    [SerializeField] private float detectionRadius = 3f;
    // Desplazamiento en la dirección frontal del jugador para el punto de detección
    [SerializeField] private float detectionOffset = 2f;
    // Referencia al objeto que el jugador tiene en las manos
    [SerializeField] private GameObject heldObject;
    // Depuración: colliders detectados en el último escaneo
    [SerializeField] private Collider2D[] hitColliders;
    // Intervalo de actualización de la detección (en segundos)
    [SerializeField] private float detectionRate = 0.25f;
    // Activar/desactivar visualización de depuración en tiempo de ejecución
    [SerializeField] private bool showDebugInfo = true;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Referencia a la última tile detectada
    private TileBase _lastDetectedTile;
    // Posición de la última tile detectada
    private Vector3Int _lastDetectedTilePosition;
    // Referencia al último tilemap detectado
    private Tilemap _lastDetectedTilemap;
    // Control del tiempo para la detección periódica
    private float _detectionTimer = 0f;
    // Indicador de si hay una tile en rango de interacción
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // Actualizar el temporizador
        _detectionTimer += Time.deltaTime;

        // Verificar si es momento de actualizar la detección
        if (_detectionTimer >= detectionRate)
        {
            UpdateNearestTileDetection();
            _detectionTimer = 0f;
        }
    }

    /// <summary>
    /// Dibuja gizmos cuando el objeto está seleccionado en el editor
    /// para visualizar el área de detección
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Calcular el punto de detección con offset
        Vector3 detectionPoint = transform.position + transform.up * detectionOffset;

        // Dibujar una línea desde el jugador hasta el punto de detección
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, detectionPoint);

        // Dibujar el círculo de detección
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(detectionPoint, detectionRadius);

        // Si se ha detectado una tile, resaltarla
        if (_lastDetectedTilemap != null && Application.isPlaying && showDebugInfo)
        {
            Gizmos.color = Color.green;
            Vector3 tileCenter = _lastDetectedTilemap.GetCellCenterWorld(_lastDetectedTilePosition);
            Gizmos.DrawCube(tileCenter, Vector3.one * 0.5f);
        }
    }

    /// <summary>
    /// Dibuja información de depuración en la pantalla durante el juego
    /// </summary>

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Maneja la interacción con las tiles cercanas.
    /// Se llama cuando el jugador pulsa el botón de interacción.
    /// </summary>
    /// <param name="context">Contexto de la acción de input</param>
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            // La detección ya está actualizada gracias al Update
            if (_lastDetectedTile != null)
            {
                Debug.Log("Interactuando con tile: " + _lastDetectedTile.name);
                // Aquí iría la lógica de interacción con la tile
            }
            else
            {
                Debug.Log("No hay tile en rango para interactuar.");
            }
        }
    }

    /// <summary>
    /// Estructura para devolver todos los datos relevantes de la detección de tiles
    /// </summary>
    public struct TileDetectionResult
    {
        public TileBase tile;
        public Vector3Int cellPosition;
    }

    /// <summary>
    /// Encuentra la tile más cercana al jugador dentro del radio de detección
    /// y que corresponda a la layermask especificada.
    /// </summary>
    /// <returns>Datos de la tile más cercana, o una estructura con valores nulos si no se encuentra ninguna</returns>
    public TileDetectionResult GetNearestTile()
    {
        // Calcular el punto de detección con offset en la dirección del jugador
        Vector2 detectionPoint = (Vector2)transform.position + (Vector2)(transform.up * detectionOffset);

        // Obtener todos los colliders cercanos
        hitColliders = Physics2D.OverlapCircleAll(detectionPoint, detectionRadius, targetLayerMask);

        TileDetectionResult result = new TileDetectionResult();
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D collider in hitColliders)
        {
            // Intentar obtener TilemapCollider2D del objeto
            TilemapCollider2D tilemapCollider = collider.GetComponent<TilemapCollider2D>();

            if (tilemapCollider != null)
            {
                // Obtener el Tilemap y el Grid
                Tilemap tilemap = tilemapCollider.GetComponent<Tilemap>();
                if (tilemap != null)
                {
                    Grid grid = tilemap.GetComponentInParent<Grid>();
                    if (grid != null)
                    {
                        // Convertir la posición mundial a posición de celda
                        Vector3Int cellPosition = grid.WorldToCell(detectionPoint);

                        // Verificar si hay una tile en esa posición
                        TileBase tile = tilemap.GetTile(cellPosition);

                        if (tile != null)
                        {
                            // Calcular el centro de la celda en coordenadas del mundo
                            Vector3 cellCenterWorld = grid.GetCellCenterWorld(cellPosition);

                            // Calcular la distancia al jugador
                            float distance = Vector2.Distance(transform.position, cellCenterWorld);

                            // Si esta tile está más cerca que la anterior, actualizamos
                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                result.tile = tile;
                                result.cellPosition = cellPosition;

                            }
                        }

                    }

                    
                }


                
            }
        }

        return result;
    }


    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Actualiza la detección de la tile más cercana
    /// </summary>
    private void UpdateNearestTileDetection()
    {
        TileDetectionResult result = GetNearestTile();

        // Actualizar las variables con el resultado
        _lastDetectedTile = result.tile;
        _lastDetectedTilePosition = result.cellPosition;

        // Puedes añadir eventos o callbacks aquí si algo debe ocurrir
        // cuando cambia la tile detectada
    }
    #endregion
} // class NewPickDrop