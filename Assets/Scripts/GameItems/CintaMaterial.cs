//---------------------------------------------------------
// Este script se encarga de mover los items de una cinta a otra cuando se encuentran en la cinta
// Óliver García Aguado
// Clank & Clutch
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Collections;
using System;
// Añadir aquí el resto de directivas using


/// <summary>
/// Esta clase consta de un array de cintas mecánicas, un índice que indica la cinta actual y un booleano que indica si el objeto está en una cinta.
/// Este script contiene un método de tipo corrutina que se encarga de mover el objeto a la siguiente cinta. (Método MoveToNextCinta)
/// Además tiene un metodo OnTransformParentChanged que se encarga de mover el objeto a la cinta siguiente detectando que cinta es la siguiente.
/// </summary>
public class CintaMaterial : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private float Speed; // Velocidad de la cinta

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private int _cintaIndex = 0; // Es el indice del array de cintas mecánicas donde se encuentra el objeto
    private bool _onCinta = true;

    [SerializeField] private GameObject[] _cintasMecanicas; // Es el array de cintas mecánicas del mapa

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

/// <summary>
/// Este metodo comprueba si el objeto se ha introducido en la cinta por el jugador y si es así, calcula cual es su siguiente cinta para entonces llamar a la corrutina
/// En caso contrario, avanza el indice de la cinta y llama a la corrutina para mover el objeto a la siguiente cinta.
/// Si se saca el objeto de la cinta se paran las corrutinas.
/// </summary>
    private void OnTransformParentChanged()
    {
        if (_onCinta) //El objeto se encuentra en una cinta
        {
            if (transform.parent != null && transform.parent.gameObject.CompareTag("Cinta")) //El objeto se encuentra en una cinta y se ha cambiado de cinta
            {
                _cintaIndex++;
                if (_cintaIndex < _cintasMecanicas.Length)
                {
                    StartCoroutine(MoveToNextCinta(_cintasMecanicas[_cintaIndex], 1f)); // Mueve el objeto a la siguiente cinta
                }
            }
            else //El objeto se ha sacado de la cinta
            {
                StopAllCoroutines(); // Para de moverse
                _onCinta = false;
                _cintaIndex = 0;
            }
        } 
        else if (!_onCinta && transform.parent != null && transform.parent.gameObject.CompareTag("Cinta")) // Ocurre al colocar el objeto en la cinta
        {
            // Encontrar el índice de la cinta actual
            int i = 0;
            bool found = false;
            while (i < _cintasMecanicas.Length && !found)
            {
                if (_cintasMecanicas[i] == transform.parent.gameObject) found = true;
                else i++;
            }
            if (found)
            {
                _cintaIndex = i + 1; // Prepara el índice de la cinta siguiente
                _onCinta = true;
                if (_cintaIndex < _cintasMecanicas.Length)
                {
                    StartCoroutine(MoveToNextCinta(_cintasMecanicas[_cintaIndex], 1f));
                }
            }
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
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
    /// Corrutina que mueve el objeto a la posición de la cinta siguiente en un tiempo determinado, tras llegar a la cinta siguiente se cambia el padre del objeto a la cinta en cuestión.
    /// </summary>
    /// <param name="cintaTarget">Cinta a la que se moverá el objeto</param>
    /// <param name="duration">Tiempo en segundos que tardará el movimiento</param> 
    private IEnumerator MoveToNextCinta(GameObject cintaTarget, float duration)
    {
        Debug.Log("Moviendo cinta " + cintaTarget.name);
        Vector3 startPosition = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, cintaTarget.transform.position, t);
            yield return null;
        }

        transform.position = cintaTarget.transform.position;
        transform.parent = cintaTarget.transform;
    
    }
    

    #endregion

} // class CintaMaterial 
// namespace
