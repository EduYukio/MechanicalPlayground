using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody2D rb;

    public float maxHealth = 25f;
    public float currentHealth;

    public virtual void TakeDamage(float damage) {
    }
}
