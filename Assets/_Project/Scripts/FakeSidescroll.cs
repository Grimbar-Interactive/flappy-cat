using UnityEngine;

/// <summary>
/// A simple script that moves a Transform across a repeatable distance.
/// </summary>
public class FakeSidescroll : MonoBehaviour
{
    [SerializeField] private float repeatDistance = 2f;
    [SerializeField] private float moveSpeed = 5f;

    private float _xOrigin;
    
    private void Awake()
    {
        _xOrigin = transform.position.x;
    }

    private void Update()
    {
        // Every frame, move this GameObject to the left.
        transform.Translate(Vector3.right * (moveSpeed * Time.deltaTime));
        
        // Calculate how far we've travelled to check if we've passed the "repeat" point.
        var extraOffset = transform.position.x - (_xOrigin + Mathf.Sign(moveSpeed) * repeatDistance);
        if (extraOffset < 0f)
        {
            transform.position = new Vector3(_xOrigin + extraOffset, transform.position.y, 0);
        }
    }
}
