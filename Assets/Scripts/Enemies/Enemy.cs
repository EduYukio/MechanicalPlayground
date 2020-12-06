using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody2D rb;

    public float maxHealth = 25f;
    public float currentHealth;

    private static PlayerFSM player;

    private void Start() {
        Enemy.player = GameObject.FindObjectOfType<PlayerFSM>();
    }

    public virtual void TakeDamage(float damage) {
    }

    public static void DieAction(GameObject enemy) {
        int direction = 1;
        if (player != null) direction = player.lastDirection;

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = new Vector3(direction * 3f, 6f, 0);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.angularVelocity = direction * -40f;
        rb.gravityScale = 2f;

        enemy.GetComponent<Collider2D>().enabled = false;
        MonoBehaviour.Destroy(enemy, 0.75f);
    }
}
