using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract class inherited by all enemies

public abstract class EnemyAbstract_script : MonoBehaviour
{
    protected abstract void Movement();
    protected abstract void DisableColliders();
    protected abstract void Stomped();
    protected abstract void TouchedWithFireBall();
    protected abstract void TouchedByInvincibleMario();
    public bool Damaged;
    bool flipExecuted;
    protected Rigidbody2D EnemyRigidbody;
    AudioSource audio;
    protected Scoore_script scooreScript;
    protected bool freezeMovement;
    public static bool GlobalFreezMovement { get; set; }
    protected int flipDirection;
    int protectedFlipDirection;
    public static int stompsInARow { get; set; }
    int rotationCount;
    protected bool flip;
    protected int movementDirection;
    public const float stuckTimerConst = 0.05f;
    public float stuckTimer = stuckTimerConst;
    public bool boxColliderTouch;
    // Flip is performed each time Mario enemy collides with fire ball or with Mario in an invincible form (although some enemies ignore fire balls)
    protected void Flip(Transform objectTransform)
    {

        EnemyRigidbody.freezeRotation = false;
        if (!flipExecuted)
        {
            EnemyRigidbody.gravityScale = 2;
            EnemyRigidbody.velocity = new Vector2(protectedFlipDirection * -1, 5);

            EnemyRigidbody.freezeRotation = false;
            freezeMovement = true;
            protectedFlipDirection = flipDirection;
            //PlayFlipAudio();

            DisableColliders();

            flipExecuted = true;

        }

        if (rotationCount < 180)
            objectTransform.Rotate(Vector3.forward, 15 * protectedFlipDirection);
        else
            EnemyRigidbody.freezeRotation = true;

        rotationCount += 15;

    }

    public void StartFlip(int direction)
    {
        flipDirection = direction;
        flip = true;
    }

    protected void SetScoore(string scoore)
    {
        scooreScript.SetScoore(scoore);

    }
    protected void CheckIfStuck()
    {
        if (EnemyRigidbody.velocity.x == 0 && EnemyRigidbody.velocity.y == 0)
        {
            stuckTimer -= Time.deltaTime;
            if (stuckTimer <= 0)
            {
                stuckTimer = stuckTimerConst;
                if (boxColliderTouch)
                {
                    boxColliderTouch = false;
                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
                }
                else
                {
                    movementDirection = movementDirection * -1;
                }
            }
        }
        else
        {
            stuckTimer = stuckTimerConst;
        }
    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        boxColliderTouch = collision.otherCollider is BoxCollider2D;

        if (collision.gameObject.tag == "PlayerGroundCheck" && !Mario_script.MarioDamaged)
        {
            Stomped();
        }

        else if (collision.gameObject.tag == "FireBall")
        {
            TouchedWithFireBall();
            if (collision.transform.position.x > transform.position.x)
                flipDirection = -1;
            else
                flipDirection = 1;
        }

        else if (collision.gameObject.tag == "Player" && Mario_script.InvincibleMario)
        {
            if (collision.transform.position.x > transform.position.x)
                flipDirection = -1;
            else
                flipDirection = 1;
            TouchedByInvincibleMario();
        }
    }
    
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerGroundCheck" && !Mario_script.MarioDamaged)
        {
            Stomped();
        }

        else if (collision.gameObject.tag == "FireBall")
        {
            TouchedWithFireBall();
            if (collision.transform.position.x > transform.position.x)
                flipDirection = -1;
            else
                flipDirection = 1;
        }

        else if (collision.gameObject.tag == "Player" && Mario_script.InvincibleMario)
        {
            if (collision.transform.position.x > transform.position.x)
                flipDirection = -1;
            else
                flipDirection = 1;
            TouchedByInvincibleMario();
        }
    }

}