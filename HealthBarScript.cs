using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{

    public RectTransform green;
    public float health;
    public float maxHealth;
    public bool seeCam;

    private float ogWidth;

    // Start is called before the first frame update
    void Start()
    {
        ogWidth = green.sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
        green.sizeDelta = new Vector2(ogWidth * (health / maxHealth), green.sizeDelta.y);

        if (seeCam)
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
