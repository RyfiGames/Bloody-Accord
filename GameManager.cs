using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public PlayerMovement movement;
    public GameObject heart;

    public Transform enemyParent;
    public Transform[] spawns;

    public GameObject spiderPF;
    public GameObject doombaPF;
    public GameObject dronePF;
    public GameObject batPF;
    public GameObject sorcererPF;
    //public Material[] spiderMatByDiff;
    public Material[] colorByDiff;

    public float[] healthByDiff;
    public float[] speedByDiff;

    public float time;
    public int times;

    // Start is called before the first frame update
    void Start()
    {
        //SpawnBat(5, 1);
        //SpawnDrone(1, 1);
    }

    public void SpawnSpider(int difficulty, int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            int s = Random.Range(0, spawns.Length);
            GameObject enemy = Instantiate(spiderPF, spawns[s].position, spawns[s].rotation, enemyParent);
            if (difficulty == 0)
            {
                enemy.transform.localScale /= 2f;
            }

            enemy.GetComponent<PrimitiveEnemyScript>().health = healthByDiff[difficulty];
            enemy.GetComponent<PrimitiveEnemyScript>().hb.maxHealth = healthByDiff[difficulty];

            enemy.GetComponent<PrimitiveEnemyScript>().speed = speedByDiff[difficulty];

        }
        //enemy.transform.GetChild(1).GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[1] = colorByDiff[difficulty];
    }


    public void SpawnDoomba(int count, float health)
    {
        for (int i = 0; i < count; i++)
        {
            int s = Random.Range(0, spawns.Length);
            GameObject enemy = Instantiate(doombaPF, spawns[s].position, spawns[s].rotation, enemyParent);
            enemy.GetComponent<PrimitiveEnemyScript>().health = health;
            enemy.GetComponent<PrimitiveEnemyScript>().hb.maxHealth = health;
        }
    }
    public void SpawnDrone(int count, float health)
    {
        for (int i = 0; i < count; i++)
        {
            int s = Random.Range(0, spawns.Length);
            GameObject enemy = Instantiate(dronePF, spawns[s].position, spawns[s].rotation, enemyParent);
            enemy.GetComponent<FlyEnemyScript>().health = health;
            enemy.GetComponent<FlyEnemyScript>().hb.maxHealth = health;
        }
    }

    public void SpawnBat(int count, float health)
    {
        for (int i = 0; i < count; i++)
        {
            int s = Random.Range(0, spawns.Length);
            GameObject enemy = Instantiate(batPF, spawns[s].position, spawns[s].rotation, enemyParent);
            enemy.GetComponent<FlyEnemyScript>().health = health;
            enemy.GetComponent<FlyEnemyScript>().hb.maxHealth = health;
        }
    }

    public void SpawnSorcerer(int count, float health)
    {
        for (int i = 0; i < count; i++)
        {
            int s = Random.Range(0, spawns.Length);
            GameObject enemy = Instantiate(sorcererPF, spawns[s].position, spawns[s].rotation, enemyParent);
            enemy.GetComponent<FlyEnemyScript>().health = health;
            enemy.GetComponent<FlyEnemyScript>().hb.maxHealth = health;
        }
    }

    public void NewTimes()
    {
        time = 0;
        times++;
        if (Random.Range(0, 2) == 0)
        {
            heart.SetActive(true);
        }
        else
        {
            movement.player.health += 5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!movement.paused)
        {
            time += Time.deltaTime;

            if (time > 5f && times < 3)
            {
                SpawnSpider(0);
                //SpawnBat(1, 1);
                //SpawnDrone(1, 10);
                NewTimes();
            }
            else
                if (time > 10f && times < 5 && times >= 3)
            {
                //SpawnSpider(0);
                SpawnDoomba(1, 3f);
                NewTimes();
            }
            else
                if (time > 10f && times < 6 && times >= 5)
            {
                //SpawnSpider(0);
                SpawnBat(10, 1f);
                NewTimes();
            }
            else if (time > 20f && times < 7 && times >= 6)
            {
                //SpawnSpider(0);
                SpawnDrone(2, 3f);
                NewTimes();
            }
            else if (time > 20f && times < 8 && times >= 7)
            {
                //SpawnSpider(0);
                SpawnSorcerer(1, 10f);
                NewTimes();
            }
            else if (time > 20f && times < 10 && times >= 8)
            {
                //SpawnSpider(0);
                SpawnSpider(1, 2);
                SpawnDoomba(1, 5f);
                NewTimes();
            }
            else if (time > 20f && times >= 10)
            {
                for (int i = 0; i < times / 2; i++)
                {
                    int e = Random.Range(0, 6);
                    switch (e)
                    {
                        case 0:

                            break;
                        case 1:
                            SpawnSpider(1);
                            break;
                        case 2:
                            SpawnDoomba(1, times / 5f);
                            break;
                        case 3:
                            SpawnBat(1, 1f);
                            break;
                        case 4:
                            SpawnDrone(1, times / 5f);
                            break;
                        case 5:
                            SpawnSorcerer(1, times);
                            i++;
                            break;
                    }
                }

                NewTimes();
            }
            /*            if (time > 10f && times < 5 && times >= 3)
                        {
                            SpawnSpider(0);
                            SpawnDoomba(1, 1f);
                            NewTimes();
                        }
                        else
                        if (time > 10f && times < 10 && times >= 5)
                        {
                            SpawnSpider(1);
                            NewTimes();
                        }
                        else
                            if (time > 10f && times < 20 && times >= 10)
                        {
                            SpawnSpider(1, 2);
                            NewTimes();
                        }
                        else if (time > 10f && times < 21 && times >= 20)
                        {
                            SpawnDoomba(1, 3f);
                            NewTimes();
                        }
                        if (time > 10f && times < 25 && times >= 21)
                        {
                            SpawnDrone(1, 3f);
                            NewTimes();
                        }
                        if (time > 15f && times < 30 && times >= 25)
                        {
                            SpawnDrone(1, 3f);
                            SpawnSpider(1, 2);
                            NewTimes();
                        }
                        if (time > 15f && times < 33 && times >= 30)
                        {
                            SpawnDrone(1, 10f);
                            NewTimes();
                        }
                        if (time > 2f && times < 40 && times >= 33)
                        {
                            NewTimes();
                        }
                        if (time > 15f && times < 41 && times >= 40)
                        {
                            SpawnBat(10, 1f);
                            NewTimes();
                        }
                        if (time > 15f && times < 42 && times >= 41)
                        {
                            SpawnBat(10, 1f);
                            SpawnDoomba(1, 10f);
                            NewTimes();
                        }
                        if (time > 5f && times < 42 && times >= 50)
                        {
                            SpawnBat(2, 1f);
                            SpawnSpider(1, 1);
                            SpawnDoomba(1, 3f);
                            NewTimes();
                        }
                        if (time > 10f && times < 41 && times >= 40)
                        {
                            SpawnBat(3, 1f);
                            SpawnSpider(1, 3);
                            SpawnDrone(1, 5f);
                            NewTimes();
                        } */
        }
    }
}
