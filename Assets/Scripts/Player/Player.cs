using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerStatus
{
    Idle,
    Dead,
    None
}

public class Player : Entity
{
    private Movement _movement;
    private Control _control;

    public static Transform PlayerTransform;

    private PlayerStatus status = PlayerStatus.None;
    public PlayerStatus Status => status;

    public VignetteController vignetteController;  

    public int Health
    {
        get { return _life; }
        set
        {
            _life = Mathf.Max(value, 0);
            if (_life <= 0 && status != PlayerStatus.Dead)
            {
                status = PlayerStatus.Dead;
                HandlePlayerDeath();
            }
        }
    }

    private void Awake()
    {
        PlayerTransform = transform;
    }

    private void Start()
    {
        base.Start(); 

        status = PlayerStatus.Idle;
        _movement = new Movement(GetComponent<Rigidbody>(), speed);
        _control = new Control(_movement, Camera.main);
    }


    private void FixedUpdate()
    {
        _control.ArtificialUpdate();
    }

    public override void TakeDamage(int dmg)
    {
        if (status == PlayerStatus.Dead) return;  

        _life -= dmg;
        _life = Mathf.Max(_life, 0);

        if (vignetteController != null)
            vignetteController.TriggerVignette();

        if (_life <= 0)
        {
            status = PlayerStatus.Dead;
            HandlePlayerDeath();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            _movement.ResetJump();
    }

    public void HandlePlayerDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
