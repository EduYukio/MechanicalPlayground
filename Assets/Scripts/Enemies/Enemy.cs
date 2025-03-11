using UnityEngine;

public class Enemy : MonoBehaviour
{
    private static PlayerFSM player;

    [SerializeField] private Color particleColor = Color.white;
    [SerializeField] private GameObject beingHitParticlesObj = null;

    public float maxHealth = 25f;
    public float currentHealth;

    public Animator animator { get; set; }
    public Rigidbody2D rb { get; set; }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        ThrowCustomParticles();
    }

    public static void DieAction(GameObject enemy)
    {
        if (player == null) player = GameObject.FindObjectOfType<PlayerFSM>();
        int direction = player.lookingDirection;
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = new Vector3(direction * 3f, 6f, 0);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.angularVelocity = direction * -40f;
        rb.gravityScale = 2f;

        enemy.GetComponent<Collider2D>().enabled = false;
        MonoBehaviour.Destroy(enemy, 0.75f);
        Manager.audio.PlayDelayed("EnemyDying", 0.75f);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerFSM player = other.gameObject.GetComponent<PlayerFSM>();
            player.TransitionToState(player.DyingState);
        }
    }

    public void ThrowCustomParticles()
    {
        GameObject particlesObj = Instantiate(beingHitParticlesObj, transform.position, Quaternion.identity);
        ParticleSystem particlesSystem = particlesObj.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule particlesMainModule = particlesSystem.main;

        particlesMainModule.startColor = new ParticleSystem.MinMaxGradient(particleColor);
    }
}
