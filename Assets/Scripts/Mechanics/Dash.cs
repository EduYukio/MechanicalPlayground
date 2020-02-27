using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour {
    public Rigidbody2D rb;
    public CollisionChecker coll;

    public float dashSpeed = 20;
    public bool isDashing = false;
    public bool hasDashed = false;

    void Start() {
        coll = GetComponent<CollisionChecker>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        if (coll.onGround) {
            hasDashed = false;
        }

        if (Input.GetButtonDown("Fire2") && !hasDashed) {
            if (xRaw != 0 || yRaw != 0) {
                DashAction(xRaw, yRaw);
            }
        }
    }

    private void DashAction(float x, float y) {
        hasDashed = true;

        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);

        rb.velocity = dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait() {
        rb.gravityScale = 0;
        GetComponent<SmoothFall>().enabled = false;
        GetComponent<Walk>().enabled = false;
        //isDashing = true;

        yield return new WaitForSeconds(.1f);

        rb.gravityScale = 3;
        GetComponent<SmoothFall>().enabled = true;
        GetComponent<Walk>().enabled = true;
        //isDashing = false;
    }
}
