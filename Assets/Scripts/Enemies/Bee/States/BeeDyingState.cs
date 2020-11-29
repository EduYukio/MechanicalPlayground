using UnityEngine;

public class BeeDyingState : BeeBaseState {
    public override void EnterState(BeeFSM bee) {
        DieAction(bee);
    }

    public override void Update(BeeFSM bee) {

    }

    void DieAction(BeeFSM bee) {
        PlayerFSM player = GameObject.FindObjectOfType<PlayerFSM>();
        int direction = 1;
        if (player != null) direction = player.lastDirection;
        bee.rb.bodyType = RigidbodyType2D.Dynamic;
        bee.rb.velocity = new Vector3(direction * 3f, 6f, 0);
        bee.rb.constraints = RigidbodyConstraints2D.None;
        bee.rb.angularVelocity = direction * -40f;
        bee.rb.gravityScale = 2f;
        bee.GetComponent<Collider2D>().enabled = false;
        MonoBehaviour.Destroy(bee.gameObject, 0.75f);
    }
}