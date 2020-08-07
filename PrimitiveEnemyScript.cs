using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimitiveEnemyScript : MonoBehaviour
{

    public PlayerMovement movement;
    public HealthBarScript hb;

    public Rigidbody rb;
    public Transform player;
    public Transform retreatPoints;
    public Transform currentRP;

    public bool jump = true;

    public float speed;
    public float hitStrength;

    public float health;

    public float jumpRange;
    public float jumpMag;
    public float jumpTimer;
    public float jumpResetTime;
    bool jumping;

    public float knockback;

    public bool onFire;
    public GameObject fireParticles;
    public float fireTime;

    public float freezeTime;

    public float poisonTime;
    public GameObject poisonParticles;

    //public float rotSpeed;

    //float layerTime = 30;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        retreatPoints = GameObject.Find("Retreat Points").transform;
        movement = player.GetComponent<PlayerMovement>();
    }

    public void MoveTowards(Vector3 v)
    {
        float xdif = Mathf.Abs(transform.position.x - v.x);
        float zdif = Mathf.Abs(transform.position.z - v.z);
        float angle = Mathf.Atan(xdif / zdif) * Mathf.Rad2Deg;
        float pn = (xdif / (transform.position.x - v.x)) * (zdif / (transform.position.z - v.z));

        if (pn > 0)
        {
            angle = 180f - angle;
        }

        if (transform.position.x - v.x > 0)
        {
            angle += 180;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, -angle + 180f, transform.eulerAngles.z);
        //transform.eulerAngles = Vector3.RotateTowards(transform.eulerAngles, new Vector3(transform.eulerAngles.x, -pn * angle, transform.eulerAngles.z), rotSpeed * Time.deltaTime, rotSpeed * Time.deltaTime);

        rb.AddRelativeForce(Vector3.forward * speed);
    }

    public void Jump()
    {
        jumpTimer = jumpResetTime;
        jumping = true;
        if (jump)
            rb.AddRelativeForce(Vector3.up * jumpMag);

        currentRP = retreatPoints.GetChild(Random.Range(0, retreatPoints.childCount));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!movement.paused)
        {
            if (collision.collider.gameObject.name == "Player")
            {
                player.GetComponent<PlayerController>().DealDamage(hitStrength, true);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!movement.paused)
        {
            if (collision.collider.gameObject.name == "Player" && jumping)
            {
                player.GetComponent<PlayerController>().DealDamage(hitStrength, true);
            }
        }
    }

    public void InflictDamage(float amount, bool kb = true)
    {
        health -= amount;
        //jumpTimer = jumpResetTime;
        //speed += 200;
        //currentRP = retreatPoints.GetChild(Random.Range(0, retreatPoints.childCount));

        if (kb)
            rb.AddForce(new Vector3(transform.position.x - player.position.x, 0f, transform.position.z - player.position.z) * knockback);
    }

    // Update is called once per frame
    void Update()
    {
        if (!movement.paused)
        {
            //           if (layerTime > 0)
            //         {
            //           layerTime -= Time.deltaTime;
            //     }
            //   if (gameObject.layer != 0 && layerTime <= 0)
            // {
            //   gameObject.layer = 0;
            //}

            if (Random.Range(0, 2000) == 0)
            {
                currentRP = retreatPoints.GetChild(Random.Range(0, retreatPoints.childCount));

                jumpTimer = jumpResetTime;
            }

            if (jumpTimer > 1 && jumpTimer < 4.5f && freezeTime <= 0)
            {
                MoveTowards(currentRP.position);
                if (Vector3.Distance(currentRP.position, transform.position) < jumpRange)
                {
                    currentRP = retreatPoints.GetChild(Random.Range(0, retreatPoints.childCount));
                }
            }
            else if (!movement.player.back && freezeTime <= 0)
            {
                MoveTowards(player.position);
            }
            else if (freezeTime <= 0)
            {
                if (currentRP == null)
                {
                    currentRP = retreatPoints.GetChild(Random.Range(0, retreatPoints.childCount));
                }

                MoveTowards(currentRP.position);
                if (Vector3.Distance(currentRP.position, transform.position) < jumpRange)
                {
                    currentRP = retreatPoints.GetChild(Random.Range(0, retreatPoints.childCount));
                }
            }

            if (Vector3.Distance(player.position, transform.position) < jumpRange && jumpTimer <= 0)
            {
                Jump();
            }
            else
            {
                if (jumpTimer > 0)
                {
                    jumpTimer -= Time.deltaTime;
                    jumping = false;
                    //speed = 1200;
                }
            }

            if (health <= 0)
            {
                rb.isKinematic = true;
                transform.position += Vector3.down * .1f;
            }
            if (transform.position.y < -50)
            {
                Destroy(gameObject);
            }

            if (fireTime > 0)
            {
                onFire = true;
                fireParticles.SetActive(true);
                InflictDamage(Time.deltaTime, false);
                fireTime -= Time.deltaTime;
            }
            else
            {
                onFire = false;
                fireParticles.SetActive(false);
            }
            if (freezeTime > 0)
            {
                freezeTime -= Time.deltaTime;
            }
            if (poisonTime > 0)
            {
                poisonTime -= Time.deltaTime;
                poisonParticles.SetActive(true);
                if (poisonTime < 0)
                {
                    poisonTime = 5f;
                    InflictDamage(1f, false);
                }
            }
        }


        hb.health = health;
    }
}
