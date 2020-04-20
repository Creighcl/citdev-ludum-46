﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BadGuyMotor : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody;
    private GameSceneManager mgr;
    public float movementSpeed = 10f;
    public int damageToPlant = 10;
    private bool isChasing = true;
    private bool entering = true;
    private float maxSpeed = 6.4f;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mgr = GameSceneManager.Instance;
        animator.SetBool("IsEntering", true);
        rigidbody.isKinematic = true;
    }

    public void Die() {
        animator.SetBool("IsDead", true);
        movementSpeed = 0f;
        isChasing = false;
        gameObject.layer = LayerMask.NameToLayer("TheDead");
        EventManager.Instance.ReportBadGuyDied(transform.position);
    }

    void FixedUpdate() {
        if (isChasing && !entering) {
            float targetX = mgr.GetTarget().position.x;
            bool goLeft = targetX < transform.position.x;
            spriteRenderer.flipX = !goLeft;
            if (rigidbody.velocity.magnitude < maxSpeed) {
                float force = (goLeft ? -1 : 1) * movementSpeed * 10;
                rigidbody.AddForce(new Vector2(force, 0.1f));
            }
        }
    }

    void Despawn() {
        gameObject.layer = LayerMask.NameToLayer("TheDead");
        isChasing = false;
        movementSpeed = 0f;
        rigidbody.velocity = Vector2.zero;
        animator.SetBool("IsDead", true);

    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (LayerMask.LayerToName(coll.gameObject.layer) == "Plant") {
            Despawn();
            GameSceneManager.Instance.ReportBadGuyDamagedPlant(damageToPlant);
            EventManager.Instance.ReportBadGuyDealtDamage();
        }
    }
    public void AnimationComplete() {
        StartCoroutine(Activate());
    }

    IEnumerator Activate() {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("IsEntering", false);
        entering = false;
        rigidbody.isKinematic = false;
    }
}
