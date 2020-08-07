using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{

    public GameObject explostionPF;
    public bool moving;
    public Rigidbody rb;
    public float speed;
    public bool enemy = false;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 9 && !enemy)
        {
            moving = false;
        }
        if (collision.gameObject.tag != "Enemy" && enemy)
        {
            moving = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!moving)
        {
            if (collision.gameObject.tag == "Enemy" && collision.gameObject.GetComponent<PrimitiveEnemyScript>() != null && !enemy)
            {
                collision.gameObject.GetComponent<PrimitiveEnemyScript>().InflictDamage(8f, false);
            }
            if (collision.gameObject.tag == "Enemy" && collision.gameObject.GetComponent<FlyEnemyScript>() != null && !enemy)
            {
                collision.gameObject.GetComponent<FlyEnemyScript>().InflictDamage(8f);
            }
            if (collision.gameObject.name == "Player" && enemy)
            {
                collision.gameObject.GetComponent<PlayerController>().DealDamage(8f, true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            rb.AddRelativeForce(Vector3.forward * speed);
        }
        else if (transform.localScale != Vector3.one * 20f)
        {
            transform.localScale = Vector3.one * 20f;
            GameObject go = Instantiate(explostionPF, transform.position, transform.rotation);
            Destroy(go, 3f);
            Destroy(gameObject, 0.2f);
        }
        if (enemy)
        {
            Vector3 rf = new Vector3(Random.Range(-20f, 20f), Random.Range(-2f, 2f), Random.Range(-20f, 20f));
            GetComponent<Rigidbody>().AddForce(rf);
        }
    }
}
