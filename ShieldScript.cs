using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{

    public Transform player;
    public float timeGrow;
    public float size;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (size <= 1)
        {
            transform.localScale = Vector3.one * 50f;
            transform.GetChild(0).gameObject.SetActive(false);
            GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            GetComponent<MeshRenderer>().enabled = true;
            transform.localScale += Vector3.one * Time.deltaTime * timeGrow;
            if (transform.localScale.x >= size)
            {
                size = 1;
            }
        }

        transform.position = player.position;
    }
}
