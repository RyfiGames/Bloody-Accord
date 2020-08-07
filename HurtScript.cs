using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtScript : MonoBehaviour
{

    public List<GameObject> enemies;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemies.Contains(other.gameObject))
        {
            enemies.Remove(other.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
