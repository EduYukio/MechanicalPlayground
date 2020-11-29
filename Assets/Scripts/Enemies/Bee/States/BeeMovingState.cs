using UnityEngine;

public class BeeMovingState : BeeBaseState {
    float initialY;
    Vector2 targetPosition;

    public override void EnterState(BeeFSM bee) {
        bee.animator.Play("BeeMoving");
        Setup(bee);
    }

    public override void Update(BeeFSM bee) {
        MoveAction(bee);
    }

    void Setup(BeeFSM bee) {
        initialY = bee.transform.position.y;
        targetPosition = new Vector2(bee.transform.position.x, initialY + bee.distanceToMove);
    }

    void MoveAction(BeeFSM bee) {
        float step = bee.moveSpeed * Time.deltaTime;
        bee.transform.position = Vector2.MoveTowards(bee.transform.position, targetPosition, step);

        if (Vector2.Distance(bee.transform.position, targetPosition) < 0.01f) {
            bee.distanceToMove *= -1f;
            targetPosition = new Vector2(bee.transform.position.x, initialY + bee.distanceToMove);
        }
    }
}