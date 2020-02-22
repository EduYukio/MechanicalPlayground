using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : Jump
{
    private bool hasDoubleJumped = false;

    void Update()
    {
        if (coll.onGround)
        {
            hasDoubleJumped = false;
        }
        else
        {
            if (isJumping && Input.GetButtonDown("Jump") && !hasDoubleJumped)
            {
                JumpAction(Vector2.up, false);
                hasDoubleJumped = true;
            }

        }
    }
}
