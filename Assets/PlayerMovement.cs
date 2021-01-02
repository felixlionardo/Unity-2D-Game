using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   [SerializeField]
    public float moveSpeed;

    private bool isMoving;  

    public LayerMask solid;


    public Rigidbody2D rb;

    Vector2 movement;

    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (movement.x != 0) 
            { 
                movement.y = 0; 
            }

            if (movement != Vector2.zero)
            {
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
                var targetPos = transform.position;
                targetPos.x += movement.x;
                targetPos.y += movement.y;

                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
                
            }
        }

        animator.SetBool("isMoving", isMoving);
        
    }
    
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.fixedDeltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }
    
    private bool IsWalkable(Vector3 targetPos)
    {
        if(Physics2D.OverlapCircle(targetPos, 0.1f, solid) != null) 
        {
            return false;
        }
        return true;
    }
}
