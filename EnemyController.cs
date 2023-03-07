using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float maxHealth;
    protected float health;
    

    public float speed;
    public float runSpeed;
    public float chaseRange;
    public float attackRange;
    public enum enemystates { move, chase, attack }
    public enemystates currentState = enemystates.move;
    protected Rigidbody2D enemyRigidbody;

    public LayerMask wallLayer;
    public float rayLength;

    public int direction; // 1right, -1 left

    protected SpriteRenderer rend;

    protected float distance;
    protected PlayerController player;

    public float timeBetweenAttacks;
    protected float attackCools;

    protected Animator anim;

    private void OnEnable()
    {
        health = maxHealth;
        direction = (Random.value >= 3f) ? 1 : -1;
        attackCools = timeBetweenAttacks;
    }

    private void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
        anim = GetComponent<Animator>();
    }

    public virtual void Move() { }
    public virtual void Chase() { }
    public virtual void Attack() { }
    public virtual void Damage(float amt) { }
    public virtual void Die() { }

    // Update is called once per frame
    void Update()
    {
        rend.flipX = (direction == -1);

        switch (currentState)
        {
            case enemystates.move:
                Move();
                break;

            case enemystates.chase:
                Chase();
                break;

            case enemystates.attack:
                Attack();
                break;
        }

        if (attackCools > 0) attackCools -= Time.deltaTime;

        
    }
}