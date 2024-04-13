using UnityEngine;

/// <summary>
/// Used to spawn pipe obstacles at a set interval.
/// </summary>
public class PipesSpawner : MonoBehaviour
{
    [SerializeField] private Pipes pipesPrefab = null;
    [SerializeField] private float timeBetweenPipes = 3f;

    private float _timer = 0f;

    private void Awake()
    {
        _timer = timeBetweenPipes;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer < timeBetweenPipes) return;
        _timer = 0f;
        Instantiate(pipesPrefab, transform.position, Quaternion.identity);
    }
}
