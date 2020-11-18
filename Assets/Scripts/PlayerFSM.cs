using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour {
    private PlayerBaseState currentState;

    //DEBUGGGG
    public string debugState;
    //DEBUGGGG

    public readonly PlayerGroundedState GroundedState = new PlayerGroundedState();
    public readonly PlayerJumpingState JumpingState = new PlayerJumpingState();
    public readonly PlayerFallingState FallingState = new PlayerFallingState();

    public PlayerConfig config;
    public Rigidbody2D rb;
    public Animator animator;
    public bool isGrounded;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        TransitionToState(GroundedState);
    }

    private void Update() {
        currentState.Update(this);
    }

    public void TransitionToState(PlayerBaseState state) {
        currentState = state;
        currentState.EnterState(this);

        //DEBUGG
        debugState = currentState.GetType().Name;
        //DEBUGG
    }
}