using UnityEngine;

public class Slash : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.name);
        // Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, hitBox, 0f);
        // foreach (Collider2D enemy in hitEnemies) {
        //     enemy.GetComponent<Enemy>()?.TakeDamage(player.config.attackDamage);
        // }
    }
}