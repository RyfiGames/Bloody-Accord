using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemyScript : MonoBehaviour
{

    public PlayerMovement movement;
    public HealthBarScript hb;

    public Rigidbody rb;
    public Transform player;
    public Transform retreatPoints;
    public Transform currentRP;

    public float speed;
    public float hitStrength;

    public float health;

    public int type;
    public float stopRange;

    public bool onFire;
    public GameObject fireParticles;
    public float fireTime;

    public float freezeTime;

    public float poisonTime;
    public GameObject poisonParticles;

    [Header("Drone")]
    public GameObject arrowPF;
    public Transform arrowSpawn;
    public Transform arrowParent;
    public float arrowSpeed;
    public float cooldown;
    public float cooldownMax;

    [Header("Bat")]
    public bool run;
    public int r;

    [Header("Sorcerer")]
    public GameObject firballPF;
    public Transform fireballSpawn;
    public Animator sAnim;
    bool animPlay;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        retreatPoints = GameObject.Find("Retreat Points").transform;
        movement = player.GetComponent<PlayerMovement>();
        arrowParent = GameObject.Find("Arrows").transform;
        cooldown = cooldownMax;
    }

    public void InflictDamage(float amount)
    {
        health -= amount;
        //jumpTimer = jumpResetTime;
        //speed += 200;
        //currentRP = retreatPoints.GetChild(Random.Range(0, retreatPoints.childCount));

        //if (kb)
        //rb.AddForce(new Vector3(transform.position.x - player.position.x, 0f, transform.position.z - player.position.z) * knockback);
    }

    void Hover(float mult)
    {
        float up = 20;
        float down = 20;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 2, Vector3.up, out hit, Mathf.Infinity, 9))
        {
            up = hit.distance + transform.position.y;
        }
        if (Physics.Raycast(transform.position + Vector3.down * 2, Vector3.down, out hit, Mathf.Infinity, 9))
        {
            down = transform.position.y - hit.distance;
        }

        //print((up + down) / 2f);

        Vector3 goTo = new Vector3(player.position.x, ((up + down) / 2f), player.position.z);
        //rb.AddForce(mult * Vector3.MoveTowards(transform.position, goTo, speed * Time.deltaTime));
        //Vector3 goTo2 = new Vector3(transform.position.x, ((up + down) / 2f), transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, goTo, -mult * speed * Time.deltaTime);
    }

    void FireBow()
    {
        if (cooldown <= 0)
        {
            GameObject go = Instantiate(arrowPF, arrowSpawn.position, arrowSpawn.rotation, arrowParent);
            go.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * arrowSpeed);
            go.GetComponent<ArrowScript>().enemy = true;
            go.GetComponent<ArrowScript>().damage = (hitStrength);
            go.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            //print(go.transform.GetChild(1).GetChild(0).gameObject.name);

            cooldown = cooldownMax;
        }
        else
        {
            cooldown -= Time.deltaTime;
        }
    }

    void FireBall()
    {
        if (cooldown < 1.6f && !animPlay)
        {
            sAnim.SetTrigger("Fire");
            animPlay = true;
        }
        string c = sAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (animPlay && c == "metarig|IdleSorcerer" && cooldown <= 0f)
        {
            fireballSpawn.LookAt(player.position);
            GameObject go = Instantiate(firballPF, fireballSpawn.position, fireballSpawn.rotation, arrowParent);
            //go.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * arrowSpeed);
            go.GetComponent<FireballScript>().enemy = true;
            //go.GetComponent<FireballScript>().damage = (hitStrength);
            //go.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            //print(go.transform.GetChild(1).GetChild(0).gameObject.name);

            cooldown = cooldownMax;
            animPlay = false;
        }
        else
        {
            cooldown -= Time.deltaTime;
        }
    }

    /*   void LookAt(Vector3 v)
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

           transform.GetChild(0).eulerAngles = new Vector3(transform.eulerAngles.x, -angle + 180f, transform.eulerAngles.z);
       } */

    void DroneUpdate()
    {
        if (!movement.player.back)
        {
            if (Vector3.Distance(player.position, transform.position) > stopRange)
            {
                Hover(-1);
            }
            else
            {
                FireBow();
            }

            if (Vector3.Distance(player.position, transform.position) < stopRange - 2)
            {
                Hover(1);
            }

            //LookAt(player.position);
            transform.GetChild(0).LookAt(player.position + (Vector3.up * 2f));
        }
        else
        {

        }
        Vector3 rf = new Vector3(Random.Range(-20f, 20f), Random.Range(-2f, 2f), Random.Range(-20f, 20f));
        rb.AddForce(rf);
    }

    void BatUpdate()
    {
        if (!movement.player.back)
        {
            if (Vector3.Distance(player.position, transform.position) > stopRange)
            {
                Hover(-1);
                transform.LookAt(player);
                if (run)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.position, speed * 2f * Time.deltaTime);
                    run = true;
                }
            }
            else
            {
                transform.LookAt(player);
                r = Random.Range(0, 500);
                //print(r);
                if (run || r == 0)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.position, speed * 2f * Time.deltaTime);
                    run = true;
                }
            }
            if (!run && Vector3.Distance(player.position, transform.position) < stopRange - 10)
            {
                Hover(1);
            }
        }

        Vector3 rf = new Vector3(Random.Range(-20f, 20f), Random.Range(-2f, 2f), Random.Range(-20f, 20f));
        rb.AddForce(rf);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (type == 1 && collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().DealDamage(20f, true);
            health = 0;
        }
    }

    void SorcererUpdate()
    {
        if (!movement.player.back)
        {
            if (Vector3.Distance(player.position, transform.position) > stopRange)
            {
                Hover(-1);
                //cooldown = cooldownMax;
                //animPlay = false;
            }
            else
            {
                FireBall();
            }

            if (Vector3.Distance(player.position, transform.position) < stopRange - 2)
            {
                Hover(1);
            }

            //LookAt(player.position);
            transform.LookAt(player.position + (Vector3.up * 2f));
        }
        else
        {

        }
        Vector3 rf = new Vector3(Random.Range(-20f, 20f), Random.Range(-2f, 2f), Random.Range(-20f, 20f));
        rb.AddForce(rf);
    }

    // Update is called once per frame
    void Update()
    {
        if (!movement.paused)
        {
            if (type == 0 && health > 0 && freezeTime <= 0)
            {
                DroneUpdate();
            }
            if (type == 1 && health > 0 && freezeTime <= 0)
            {
                BatUpdate();
            }
            if (type == 2 && health > 0 && freezeTime <= 0)
            {
                SorcererUpdate();
            }

            if (health <= 0)
            {
                rb.isKinematic = true;
                transform.position += Vector3.down * 0.25f;
                GetComponent<BoxCollider>().enabled = false;
            }
            if (transform.position.y < -50)
            {
                Destroy(gameObject);
            }

            if (fireTime > 0)
            {
                onFire = true;
                fireParticles.SetActive(true);
                InflictDamage(Time.deltaTime);
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
                    InflictDamage(1f);
                }
            }
        }


        hb.health = health;
    }
}
