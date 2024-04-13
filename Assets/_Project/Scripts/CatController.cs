using UnityEngine;

/// <summary>
/// This is the main controller for the cat character. This is a good example of some of the
/// Unity lifetime methods, input checking, physics updates, animation, and more.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class CatController : MonoBehaviour
{
    // The [SerializeField] attribute allows us to change their values in Unity's Inspector tab.
    // We make them private so that outside classes cannot modify their values, but public fields
    // are also serialized and don't require the [SerializeField] attribute.
    [Header("Physics")]
    [SerializeField] private float minVelocity = -10f;
    [SerializeField] private float maxVelocity = 20f;
    [SerializeField] private float jumpVelocity = 20f;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip deathSFX;
    
    // These are a more optimized way of storing animator parameters rather than
    // referencing them by string each time you need to use them.
    private static readonly int FlapParameter = Animator.StringToHash("Flap");
    private static readonly int IsDeadParameter = Animator.StringToHash("Is Dead");
    
    
    // Here are some component references we'll assign in Awake().
    private Rigidbody2D _rb2d;
    private Animator _animator;
    private AudioSource _audio;
    
    
    // These are just some private variables for use in this class only.
    private bool _jump = false;
    private bool _isDead = false;
    
    
    // Awake() is called only once in the lifetime of a script instance. It is best used for gathering
    // component references and setting up properties that are internal to this class (private).
    // https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    
    // Start() is called only once in the lifetime of a script instance, but only if/when the script is enabled.
    // Typically, it should be used for setup tasks associated with other scripts or passing data to other scripts.
    // https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    private void Start()
    {
        _animator.ResetTrigger(FlapParameter);
        _animator.SetBool(IsDeadParameter, false);
    }

    
    // Update() is called every frame if the instance is enabled. It is best used for logic that needs to happen
    // on a consistent basis, like gathering input or updating timers.
    // https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    private void Update()
    {
        if (_isDead) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) ||
            (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            GameManager.Instance.StartGame();
            _jump = true;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            Quaternion.Euler(0, 0, Mathf.Clamp(_rb2d.velocity.y, -30f, 30f)),
            360f * Time.deltaTime);
    }

    
    // FixedUpdate() is similar to Update() but is frame-rate independent and gets called at fixed intervals.
    // It should be used to update physics behaviours like changing a Rigidbody's velocity or adding force.
    // https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html
    private void FixedUpdate()
    {
        if (_isDead) return;
        if (_jump)
        {
            _jump = false;
            _rb2d.velocity += Vector2.up * jumpVelocity;
            _rb2d.velocity = Vector2.up * Mathf.Max(_rb2d.velocity.y, jumpVelocity / 2f);
            _animator.SetTrigger(FlapParameter);
            _audio.PlayOneShot(jumpSFX);
        }
        _rb2d.velocity = Vector2.up * Mathf.Clamp(_rb2d.velocity.y, minVelocity, maxVelocity);
    }

    
    // OnCollisionEnter2D() is just one example of collision-related methods that are called by Unity's physics
    // system. You can use these methods to determine when and what collided with this object.
    // https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter2D.html
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_isDead || !other.gameObject.CompareTag("Death")) return;
        _isDead = true;
        _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        _animator.SetBool(IsDeadParameter, true);
        _audio.PlayOneShot(deathSFX);
        GameManager.Instance.TriggerGameOver();
    }
}
