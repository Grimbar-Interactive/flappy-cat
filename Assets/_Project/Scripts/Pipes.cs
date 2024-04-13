using UnityEngine;

/// <summary>
/// Handles random placement and movement logic for pipes obstacles.
/// </summary>
public class Pipes : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float yVariability = 2f;
    
    [SerializeField] private float chanceOfYMovement = 0.1f;
    [SerializeField] private float yMovementDiff = 1f;
    [SerializeField] private float period = 3f;

    private float _yOrigin;
    private bool _doYMovement = false;
    
    private void Awake()
    {
        _yOrigin = Random.Range(-yVariability, yVariability);
        transform.position = new Vector3(transform.position.x, _yOrigin, 0f);
        
        if (Random.Range(0f, 1f) < chanceOfYMovement)
        {
            _doYMovement = true;
            _yOrigin = Mathf.Max(_yOrigin, -yMovementDiff);
        }
    }

    private void Update()
    {
        transform.position += Vector3.left * (movementSpeed * Time.deltaTime);
        if (_doYMovement)
        {
            transform.position = new Vector3(transform.position.x, _yOrigin + Mathf.Sin(Time.time * Mathf.PI / period) * yMovementDiff, 0f);
        }
    }
}
