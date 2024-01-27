using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingCat : MonoBehaviour
{
    public GameObject catIdle;
    public GameObject[] catwalks;
    public GameObject[] catJumpReady;
    public GameObject catJump;

    private float catWalkSpeed = 0.5f;

    public Transform target;

    private float walkAnimationTimer = 0f;
    private int walkAnimationIndex = 0;

    private float catInterest = 0f;

    private readonly float catInterestRadius = 3.5f;
    private readonly float catJumpRadius = 2.5f;

    private readonly float jumpHeight = 3f;
    private readonly float jumpDistance = 1.5f;
    private readonly float jumpSpeed = 3f;
    private float jumpDestinationY = 0f;
    private float jumpDestinationX = 0f;
    private bool isJumping = false;
    private bool isJumpingReady = false;
    
    void Start()
    {
        catIdle.SetActive(true);
        for(int i = 0; i < catwalks.Length; i++)
        {
            catwalks[i].SetActive(false);
        }

        for(int i = 0; i < catJumpReady.Length; i++)
        {
            catJumpReady[i].SetActive(false);
        }
        catJump.SetActive(false);

        StartCoroutine(UpdateRoutine());
    }

    private IEnumerator UpdateRoutine()
    {
        while(true) {
            var distance = Vector2.Distance(transform.position, target.position);
            if(distance < catInterestRadius)
            {
                catInterest += Time.deltaTime * 10f;
                Debug.Log("Cat interest: " + catInterest);
            }

            // idle
            if(catInterest < 40f) {
                // 기본적으로 idle
                catIdle.SetActive(true);

                // move random 

                yield return null;
            }

            if(catInterest >= 70f && distance < catJumpRadius)
            {
                if(isJumping || isJumpingReady)
                {
                    yield return null;
                }

                // fake jump
                if(catInterest < 100f)
                {
                    yield return StartCoroutine(JumpReady());
                    yield return null;
                }

                // jump
                if(catInterest > 100f)
                {
                    yield return StartCoroutine(JumpReady());
                    yield return StartCoroutine(Jump());
                    yield return null;
                }
            } else if(catInterest >= 40f)
            {
                MoveTowardsTarget();
            }

            yield return null;
        }
    }

    private IEnumerator Jump() 
    {
        // check x
        var estimatedX = (transform.position.x + (transform.localScale.x * jumpDistance * -1)) * 2;
        if(estimatedX > 7.8f || estimatedX < -7.8f)
        {
            yield return null;
        }

        isJumping = true;

        catIdle.SetActive(false);
        catJump.SetActive(true);

        for(int i = 0; i < catwalks.Length; i++)
        {
            catwalks[i].SetActive(false);
        }

        for(int i = 0; i < catJumpReady.Length; i++)
        {
            catJumpReady[i].SetActive(false);
        }

        var startPosY = transform.position.y;
        jumpDestinationY = transform.position.y + jumpHeight;
        jumpDestinationX = transform.position.x + (transform.localScale.x * jumpDistance * -1);

        while(transform.position.y < jumpDestinationY)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(jumpDestinationX, jumpDestinationY), jumpSpeed * Time.deltaTime);
            yield return null;
        }

        // go down to before y
        jumpDestinationX = transform.position.x + (transform.localScale.x * jumpDistance * -1) * 0.5f;
        while(transform.position.y > startPosY)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(jumpDestinationX, startPosY), jumpSpeed * Time.deltaTime * 2f);
            yield return null;
        }


        catJump.SetActive(false);
        catwalks[0].SetActive(true);

        isJumping = false;
        yield return null;
    }

    private IEnumerator JumpReady() 
    {
        catInterest += 15f;
        isJumpingReady = true;

        catIdle.SetActive(false);
        catJump.SetActive(false);
        for(int i = 0; i < catwalks.Length; i++)
        {
            catwalks[i].SetActive(false);
        }

        catJumpReady[0].SetActive(true);
        catJumpReady[1].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        catJumpReady[0].SetActive(false);
        catJumpReady[1].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        catJumpReady[0].SetActive(true);
        catJumpReady[1].SetActive(false);
        
        yield return new WaitForSeconds(1f);

        catJumpReady[0].SetActive(false);
        catJumpReady[1].SetActive(false);
        catwalks[0].SetActive(true);

        isJumpingReady = false;
        yield return null;
    }

    private void MoveTowardsTarget()
    {
        catIdle.SetActive(false);

        if(walkAnimationTimer > 0)
        {
            walkAnimationTimer -= Time.deltaTime;
        }
        else
        {
            walkAnimationTimer = 0.5f;
            walkAnimationIndex++;
            if(walkAnimationIndex >= catwalks.Length)
            {
                walkAnimationIndex = 0;
            }
            for(int i = 0; i < catwalks.Length; i++)
            {
                catwalks[i].SetActive(false);
            }
            catwalks[walkAnimationIndex].SetActive(true);
        }

        var targetPos = target.position;
        var moveVector = targetPos - transform.position;
        var estimatedX = transform.position.x + (moveVector.normalized.x * catWalkSpeed * Time.deltaTime);
        var estimatedY = transform.position.y + (moveVector.normalized.y * catWalkSpeed * Time.deltaTime);
        if(estimatedX > 7.8f || estimatedX < -7.8f) 
        {
            targetPos.x = transform.position.x;
        }

        if(estimatedY > 1.6f || estimatedY < -2.1f) 
        {
            targetPos.y = transform.position.y;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPos, catWalkSpeed * Time.deltaTime);
        // flip x scale if target is to the left
        if (targetPos.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
