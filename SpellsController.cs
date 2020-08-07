using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsController : MonoBehaviour
{
    public PlayerMovement movement;
    public bool bookOpen;
    public GameObject[] menus;
    public int menuID;
    //0  public GameObject mainMenu;
    //1  public GameObject attackMenu;
    //2  public GameObject defendMenu;
    //3  public GameObject specialMenu;
    //4  public GameObject destroyMenu;

    public GameObject fireballPF;
    public HurtScript hurtCube;
    public Transform enemyParent;
    public ShieldScript shield;
    public GameObject acidRain;

    public float rainTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OpenMenu(int id)
    {
        movement.player.am.PlaySound("Magical_Noise_");

        CloseBook();
        bookOpen = true;

        menus[id].SetActive(true);
        menuID = id;
    }

    public void CloseBook()
    {
        bookOpen = false;
        foreach (GameObject m in menus)
        {
            m.SetActive(false);
        }
    }

    public void SpellLetters()
    {
        switch (menuID)
        {
            case 0:
                if (Input.GetKeyUp("r"))
                {
                    OpenMenu(1);
                }
                if (Input.GetKeyUp("y"))
                {
                    OpenMenu(2);
                }
                if (Input.GetKeyUp("f"))
                {
                    OpenMenu(3);
                }
                if (Input.GetKeyUp("i"))
                {
                    OpenMenu(4);
                }
                break;
            case 1:
                if (Input.GetKeyUp("b"))
                {
                    foreach (GameObject e in hurtCube.enemies)
                    {
                        if (e != null)
                        {
                            if (e.GetComponent<PrimitiveEnemyScript>() != null)
                            {
                                e.GetComponent<PrimitiveEnemyScript>().fireTime = 6f;
                            }
                            if (e.GetComponent<FlyEnemyScript>() != null)
                            {
                                e.GetComponent<FlyEnemyScript>().fireTime = 6f;
                            }
                        }
                    }
                    movement.player.DealDamage(8f, false);

                    OpenMenu(0);
                }
                if (Input.GetKeyUp("g"))
                {
                    for (int i = 0; i < enemyParent.childCount; i++)
                    {
                        GameObject e = enemyParent.GetChild(i).gameObject;
                        if (e != null)
                        {
                            if (e.GetComponent<PrimitiveEnemyScript>() != null)
                            {
                                e.GetComponent<PrimitiveEnemyScript>().freezeTime = 3f;
                            }
                            if (e.GetComponent<FlyEnemyScript>() != null)
                            {
                                e.GetComponent<FlyEnemyScript>().freezeTime = 3f;
                            }
                        }
                    }
                    movement.player.DealDamage(2f, false);

                    OpenMenu(0);
                }
                if (Input.GetKeyUp("u"))
                {
                    Instantiate(fireballPF, movement.player.hurtCube.transform.position, movement.player.hurtCube.transform.rotation);
                    movement.player.DealDamage(5f, false);
                    OpenMenu(0);
                }
                if (Input.GetKeyUp("y"))
                {
                    OpenMenu(0);
                }
                break;
            case 2:
                if (Input.GetKeyUp("c"))
                {
                    shield.size = 100;
                    movement.player.DealDamage(1f, false);
                    OpenMenu(0);
                }
                if (Input.GetKeyUp("l"))
                {
                    shield.size = 300;
                    movement.player.DealDamage(5f, false);
                    OpenMenu(0);
                }
                if (Input.GetKeyUp("o"))
                {
                    shield.size = 700;
                    movement.player.DealDamage(7f, false);
                    OpenMenu(0);
                }
                if (Input.GetKeyUp("k"))
                {
                    OpenMenu(0);
                }
                break;
            case 3:
                if (Input.GetKeyUp("t"))
                {
                    movement.player.enchantTime = 10f;
                    movement.player.DealDamage(9f, false);
                    OpenMenu(0);
                }
                if (Input.GetKeyUp("h"))
                {
                    movement.player.flightTime = 10f;
                    movement.player.DealDamage(7f, false);
                    OpenMenu(0);
                }
                if (Input.GetKeyUp("o"))
                {
                    movement.player.safetyTime = 10f;
                    movement.player.DealDamage(10f, false);
                    OpenMenu(0);
                }
                if (Input.GetKeyUp("r"))
                {
                    OpenMenu(0);
                }
                break;
            case 4:
                if (Input.GetKeyUp("r"))
                {
                    for (int i = 0; i < enemyParent.childCount; i++)
                    {
                        GameObject e = enemyParent.GetChild(i).gameObject;
                        if (e != null && i % 2 == 0)
                        {
                            if (e.GetComponent<PrimitiveEnemyScript>() != null)
                            {
                                e.GetComponent<PrimitiveEnemyScript>().health = 0;
                            }
                            if (e.GetComponent<FlyEnemyScript>() != null)
                            {
                                e.GetComponent<FlyEnemyScript>().health = 0;
                            }
                        }
                    }
                    movement.player.DealDamage(80f, false);
                    OpenMenu(0);
                }
                if (Input.GetKeyUp("o"))
                {
                    rainTime = 5f;
                    movement.player.DealDamage(60f, false);
                    OpenMenu(0);
                }
                if (Input.GetKeyUp("f"))
                {
                    for (int i = 0; i < enemyParent.childCount; i++)
                    {
                        GameObject e = enemyParent.GetChild(i).gameObject;
                        if (e != null && i % 3 == 0)
                        {
                            if (e.GetComponent<PrimitiveEnemyScript>() != null)
                            {
                                e.GetComponent<PrimitiveEnemyScript>().poisonTime = 1f;
                            }
                            if (e.GetComponent<FlyEnemyScript>() != null)
                            {
                                e.GetComponent<FlyEnemyScript>().poisonTime = 1f;
                            }
                        }
                    }
                    movement.player.DealDamage(30f, false);

                    OpenMenu(0);
                }
                if (Input.GetKeyUp("l"))
                {
                    OpenMenu(0);
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!movement.paused)
        {
            SpellLetters();

            acidRain.SetActive(rainTime > 0);

            if (rainTime > 0)
            {

                rainTime -= Time.deltaTime;
                for (int i = 0; i < enemyParent.childCount; i++)
                {
                    GameObject e = enemyParent.GetChild(i).gameObject;
                    if (e != null && Random.Range(0, 10) == 0)
                    {
                        if (e.GetComponent<PrimitiveEnemyScript>() != null)
                        {
                            e.GetComponent<PrimitiveEnemyScript>().InflictDamage(1f, false);
                        }
                        if (e.GetComponent<FlyEnemyScript>() != null)
                        {
                            e.GetComponent<FlyEnemyScript>().InflictDamage(1f);
                        }
                    }
                }

            }
        }


    }
}
