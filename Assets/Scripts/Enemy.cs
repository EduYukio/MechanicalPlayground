using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float maxHealth = 25f;
    public Animator animator;
    [HideInInspector] public Rigidbody2D rb;
    float currentHealth;

    void Start() {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {

    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        animator.SetTrigger("BeingHit");

        if (currentHealth <= 0) {
            Die();
        }

    }

    void Die() {
        animator.SetBool("isDead", true);
        rb.velocity = new Vector3(2f, 4f, 0);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.angularVelocity = -35f;
        rb.gravityScale = 1f;
        GetComponent<Collider2D>().enabled = false;
        Destroy(transform.gameObject, 1f);
    }
}
