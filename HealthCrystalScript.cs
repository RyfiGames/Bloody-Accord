using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCrystalScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().DealDamage(-70f, true);
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += Vector3.up;
    }
}
