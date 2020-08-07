using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{

    public bool enemy;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        //        print(collision.gameObject.name);

        if (collision.gameObject.name == "Player" && enemy)
        {
            collision.gameObject.GetComponent<PlayerController>().DealDamage(damage, true);
        }
        if (collision.gameObject.tag == "Enemy" && !enemy && collision.gameObject.GetComponent<PrimitiveEnemyScript>() != null)
        {
            collision.gameObject.GetComponent<PrimitiveEnemyScript>().InflictDamage(damage, false);
        }
        if (collision.gameObject.tag == "Enemy" && !enemy && collision.gameObject.GetComponent<FlyEnemyScript>() != null)
        {
            collision.gameObject.GetComponent<FlyEnemyScript>().InflictDamage(damage);
        }
        if (!enemy && collision.gameObject.layer != 9)
            Destroy(gameObject);
        if (enemy && collision.gameObject.tag != "Enemy")
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy)
        {
            Vector3 rf = new Vector3(Random.Range(-200f, 200f), Random.Range(-20f, 20f), Random.Range(-200f, 200f));
            GetComponent<Rigidbody>().AddForce(rf);
        }
    }
}
