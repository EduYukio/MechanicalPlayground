using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeeFSM : MonoBehaviour {
    private BeeBaseState currentState;

    public readonly BeeIdleState IdleState = new BeeIdleState();
    public readonly BeeAttackingState AttackingState = new BeeAttackingState();
    public readonly BeeBeingHitState HitState = new BeeBeingHitState();

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        TransitionToState(IdleState);
    }

    private void Update() {
        currentState.Update(this);
    }

    public void TransitionToState(BeeBaseState state) {
        currentState = state;
        currentState.EnterState(this);
    }
}