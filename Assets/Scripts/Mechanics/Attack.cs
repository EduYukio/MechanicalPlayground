using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
    private Player player;
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public float attackDamage = 10f;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public GameObject slashEffect;

    void Start() {
        player = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
        attackPoint = GameObject.FindWithTag("AttackPoint").GetComponent<Transform>();
    }

    void Update() {
        ProcessAttackRequest();
    }

    void ProcessAttackRequest() {
        if (Input.GetButtonDown("Attack") && Time.time >= nextAttackTime) {
            AttackAction();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    Vector3 CalculateAttackPosition() {
        Vector3 playerPos = player.gameObject.transform.position;
        float xOffset = player.lastDirection * attackPoint.localPosition.x;
        Vector3 attackPosition = playerPos + new Vector3(xOffset, attackPoint.localPosition.y, 0);

        slashEffect.transform.position = attackPosition;
        slashEffect.GetComponent<SpriteRenderer>().flipX = player.spriteRenderer.flipX;
        return attackPosition;
    }

    void AttackAction() {
        animator.SetTrigger("Attack");
        Vector3 attackPosition = CalculateAttackPosition();

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies) {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected() {
        if (attackPoint == null) return;
        Vector3 attackPosition = transform.position + new Vector3(attackPoint.localPosition.x, attackPoint.localPosition.y, 0);
        if (player) {
            attackPosition = transform.position + new Vector3(player.lastDirection * attackPoint.localPosition.x, attackPoint.localPosition.y, 0);
        }
        Gizmos.DrawWireSphere(attackPosition, attackRange);
    }
}