using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float attackRange = 2f;
    public float knockbackForce = 8f;
    public float attackCooldown = 1.2f;

    private Transform target;
    private Rigidbody rb;
    private float attackTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            target = player.transform;
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;

        // 🔴 CRITICAL: ignore vertical difference
        direction.y = 0f;

        float distance = direction.magnitude;

        attackTimer -= Time.fixedDeltaTime;

        // ✅ Always move toward player (so it touches them)
        Vector3 move = direction.normalized * moveSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        // ✅ Attack when close
        if (distance <= attackRange && attackTimer <= 0f)
        {
            Attack(direction);
            attackTimer = attackCooldown;
        }
    }

    void Attack(Vector3 direction)
    {
        Rigidbody targetRb = target.GetComponent<Rigidbody>();

        if (targetRb != null)
        {
            Vector3 knockDir = direction.normalized;

            // 🔴 CRITICAL: no upward force → prevents climbing
            knockDir.y = 0f;

            targetRb.AddForce(knockDir * knockbackForce, ForceMode.Impulse);
        }
    }
}