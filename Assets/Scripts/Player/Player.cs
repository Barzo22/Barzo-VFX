using UnityEngine;
using UnityEngine.SceneManagement;

//TP2 - Ariadna Delpiano

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
    public PlayerStatus Status
    {
        get { return status; }
    }

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
        _life -= dmg;

        if (_life <= 0)
        {
            HandlePlayerDeath();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _movement.ResetJump();
        }
    }

    public void HandlePlayerDeath()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
