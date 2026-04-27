using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 3f;
    public float knockbackForce = 12f;
    public float cooldown = 7f;

    public GameObject explosionPrefab; // 🔥 particle effect

    private float lastAttackTime;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time < lastAttackTime + cooldown)
                return;

            Attack();
            lastAttackTime = Time.time;
        }
    }

    void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                // 💥 Spawn explosion at enemy position
                if (explosionPrefab != null)
                {
                    GameObject fx = Instantiate(explosionPrefab, hit.transform.position, Quaternion.identity);

                    ParticleSystem ps = fx.GetComponent<ParticleSystem>();
                    if (ps != null)
                    {
                        ps.Play();
                    }
                }

                Rigidbody rb = hit.attachedRigidbody;

                if (rb != null)
                {
                    Vector3 dir = hit.transform.position - transform.position;
                    dir.y = 0f;
                    dir = dir.normalized;

                    rb.linearVelocity = Vector3.zero; // clears resistance
                    rb.AddForce(dir * knockbackForce, ForceMode.Impulse);
                }
            }
        }
    }
<<<<<<< Updated upstream
}   
=======
}
>>>>>>> Stashed changes
