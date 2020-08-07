using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public GameObject shopMenu;
    public Text shopTitle;
    public Text shopSmallDesc;
    public Text shopDesc;
    public GameObject[] itemBoxes;

    [Header("Old Stuff")]
    public PlayerMovement movement;
    public HurtScript hurtCube;
    public SpellsController spells;

    public float health;
    public float maxHealth;
    public RectTransform hB;
    public RectTransform[] slots;
    public int pickedSlot;

    float ogWidth;

    float invulCooldown = 0;
    public float invulMax;

    public Image redCover;
    public float redFadeSpeed;

    public GameObject[] physicalItem;
    public Animator handPivot;
    public bool needToSwap;
    public float swapTime;

    public float itemCooldown;
    public float maxItemCooldown;

    public Animator bowA;
    public Transform arrowParent;
    public Transform arrowSpawn;
    public GameObject arrowPF;
    public GameObject dummyArrow;
    public float arrowSpeed;
    bool arrowPull;

    public int arrows;
    public int ammo;
    public int swordLevel;
    public int armorLevel;

    public GameObject arrowMessage;
    public GameObject ammoMessage;

    public float gunDamage;

    public bool back;
    public Vector3 telePos = new Vector3(0f, 10f, 0f);
    public Transform shopPos;

    public GameObject buyPrompt;
    public Text buyText;
    public string buyID;

    public Renderer sword;
    public Material[] swordMats;

    public Image armorMainBar;
    public Image armorSmallBar;
    public Color[] armorColors;

    public GlassCaseScript swordCase;
    public GlassCaseScript armorCase;
    public int maxSwordLevel;
    public int maxArmorLevel;
    public GameObject buyButton;

    public GameObject endScreen;
    public Text endText;
    public float totalTime;

    public float enchantTime;
    public float flightTime;
    public float safetyTime;

    public AudioManager am;

    public bool gameOver;

    public float scrollTimer;

    // Start is called before the first frame update
    void Start()
    {
        ogWidth = hB.sizeDelta.x;
    }

    void HealthBarUpdate()
    {
        hB.sizeDelta = new Vector2(ogWidth * (health / maxHealth), hB.sizeDelta.y);
    }

    void SlotUpdate()
    {
        int oldSlot = pickedSlot;

        for (int i = 1; i <= 5; i++)
        {
            if (Input.GetKey(i.ToString()))
            {
                pickedSlot = i;
            }

            slots[i - 1].sizeDelta = new Vector2(40f, 40f);
        }

        if (Input.mouseScrollDelta.y >= 1 && pickedSlot > 1)// && scrollTimer <= 0)
        {
            pickedSlot--;
        }
        if (Input.mouseScrollDelta.y <= -1 && pickedSlot < 5)// && scrollTimer <= 0)
        {
            pickedSlot++;
        }

        slots[pickedSlot - 1].sizeDelta = new Vector2(70f, 70f);

        if (oldSlot != pickedSlot)
        {
            handPivot.SetTrigger("Switch");
            needToSwap = true;

            if (pickedSlot == 4)
            {
                spells.OpenMenu(0);
                am.PlaySound("Book_Opening");
            }
            else
            {
                spells.CloseBook();
            }

            arrowPull = false;
            arrowSpeed = 1000f;
            Camera.main.fieldOfView = 60;
            movement.speed = 1200;

            scrollTimer = 0.5f;
        }
    }

    public void DealDamage(float amount, bool hit)
    {
        am.PlaySound("Hurt");

        if (safetyTime > 0 && hit && amount > 0)
        {
            amount = 0;
        }

        if (hit && amount > 0)
            amount = amount / Mathf.Pow(2, armorLevel);

        if (invulCooldown <= 0 || !hit)
        {
            health -= amount;
            invulCooldown = invulMax;
            redCover.color = new Color(1f, 0f, 0f, .5f);
        }

        if (health <= 0)
        {
            EndGame();
        }
    }

    public void ClickAction(string click)
    {
        if (click == "down")
        {
            switch (pickedSlot)
            {
                case 1:
                    if (itemCooldown <= 0 && !needToSwap && !back)
                    {
                        SwordStrike();
                        handPivot.SetTrigger("Strike");
                        itemCooldown = maxItemCooldown;
                    }
                    break;
                case 2:
                    if (!needToSwap && arrows > 0 && !back)
                    {
                        arrowPull = true;
                        movement.speed = 0;
                        bowA.SetBool("Hold Bow", true);
                        am.PlaySound("Bow_Draw_Back");
                    }
                    break;
                case 3:
                    if (ammo <= 0)
                    {
                        break;
                    }
                    if (itemCooldown <= 0 && !needToSwap && !back)
                    {
                        FireGun();
                        handPivot.SetTrigger("Fire Gun");
                        itemCooldown = maxItemCooldown;
                    }
                    break;
                case 5:
                    am.PlaySound("teleportSound");
                    if (!back)
                    {
                        telePos = transform.position;
                        transform.position = shopPos.position;
                    }
                    else
                    {
                        transform.position = telePos;
                    }
                    break;
            }

            if (back && !buyPrompt.activeInHierarchy)
            {
                RaycastHit hit;
                Vector3 eyes = Camera.main.transform.position;
                if (Physics.Raycast(eyes, hurtCube.transform.position - eyes, out hit, Mathf.Infinity, 9))
                {
                    if (hit.collider.GetComponent<GlassCaseScript>() != null)
                    {
                        string s = hit.collider.GetComponent<GlassCaseScript>().id;
                        string s2 = hit.collider.GetComponent<GlassCaseScript>().description;
                        BuyPrompt(s, s2);
                    }

                    if (hit.collider.GetComponent<ShopkeeperScript>())
                    {
                        var sks = hit.collider.GetComponent<ShopkeeperScript>();
                        shopMenu.SetActive(true);
                        shopTitle.text = sks.shopName;
                        shopSmallDesc.text = sks.characterInfo;
                        shopDesc.text = sks.shopDescription;
                        // Add items
                    }
                }
            }
        }
        if (click == "up")
        {
            switch (pickedSlot)
            {
                case 2:
                    if (arrowPull)
                    {
                        GameObject go = Instantiate(arrowPF, arrowSpawn.position, arrowSpawn.rotation, arrowParent);
                        go.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * arrowSpeed);
                        go.GetComponent<ArrowScript>().enemy = false;
                        go.GetComponent<ArrowScript>().damage = 2f * (arrowSpeed / 2500f);
                        if (enchantTime > 0)
                        {
                            go.GetComponent<ArrowScript>().damage *= 2;
                        }

                        bowA.SetBool("Hold Bow", false);

                        arrowPull = false;
                        arrowSpeed = 1000f;
                        Camera.main.fieldOfView = 60;
                        movement.speed = 1200;
                        arrows--;
                        am.PlaySound("Arrow_release");
                    }
                    break;
                case 5:
                    back = !back;
                    break;
            }
        }
        if (click == "full")
        {
            switch (pickedSlot)
            {
                case 5:

                    break;
            }
        }
    }

    public void SwordStrike()
    {
        float damage = Mathf.Pow(2, swordLevel);
        if (enchantTime > 0)
        {
            damage *= 2;
        }
        foreach (GameObject e in hurtCube.enemies)
        {
            if (e != null)
            {
                if (e.GetComponent<PrimitiveEnemyScript>() != null)
                {
                    e.GetComponent<PrimitiveEnemyScript>().InflictDamage(damage, true);
                    if (e.name.Contains("Doomba"))
                    {
                        am.PlaySound("MetalHittingMetal");
                    }
                    if (e.name.Contains("Spider"))
                    {
                        am.PlaySound("SwordHittingFlesh");
                    }
                }
                if (e.GetComponent<FlyEnemyScript>() != null)
                {
                    e.GetComponent<FlyEnemyScript>().InflictDamage(damage);
                    if (e.name.Contains("Drone"))
                    {
                        am.PlaySound("MetalHittingMetal");
                    }

                }
            }
        }
    }

    public void FireGun()
    {
        float mult = 1;
        if (enchantTime > 0)
        {
            mult = 2;
        }

        RaycastHit hit;
        Vector3 eyes = Camera.main.transform.position;
        if (Physics.Raycast(eyes, hurtCube.transform.position - eyes, out hit, Mathf.Infinity, 9))
        {
            if (hit.collider.GetComponent<PrimitiveEnemyScript>() != null)
            {
                hit.collider.GetComponent<PrimitiveEnemyScript>().InflictDamage(mult * gunDamage, false);
            }
            if (hit.collider.GetComponent<FlyEnemyScript>() != null)
            {
                hit.collider.GetComponent<FlyEnemyScript>().InflictDamage(mult * gunDamage);
            }
        }

        //        print(hit.collider.gameObject.name);
        am.PlaySound("Gunshot");
        ammo--;
    }

    public void BuyItem(string id)
    {
        switch (id)
        {
            case "arrows":
                arrows += 100;
                DealDamage(5, false);
                break;
            case "ammo":
                ammo += 100;
                DealDamage(20, false);
                break;
            case "sword upgrade":
                swordLevel++;
                DealDamage(30, false);
                break;
            case "armor upgrade":
                armorLevel++;
                DealDamage(40, false);
                break;
        }

        if (swordLevel >= maxSwordLevel)
        {
            swordCase.description = "Max Sword";
            swordCase.id = "";
        }
        if (armorLevel >= maxArmorLevel)
        {
            armorCase.description = "Max Armor";
            armorCase.id = "";
        }
    }

    public void BuyPrompt(string id, string description)
    {
        buyPrompt.SetActive(true);
        buyText.text = description;
        buyID = id;
        Cursor.lockState = CursorLockMode.None;
        movement.moveable = false;
        buyButton.SetActive(id != "");
    }

    public void RespondBuy(bool buy)
    {
        if (buy)
        {
            BuyItem(buyID);
            buyPrompt.SetActive(false);
        }
        else
        {
            buyPrompt.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        movement.moveable = true;
    }

    void EndGame()
    {
        movement.OpenMenu(true);
        movement.menuPanel.SetActive(false);
        endScreen.SetActive(true);
        endText.text = $"Game Over\nClick to Restart\nYou lasted {(int)totalTime} seconds";
        Cursor.lockState = CursorLockMode.None;
        gameOver = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 eyes = Camera.main.transform.position;
        Debug.DrawRay(eyes, (hurtCube.transform.position - eyes) * 30f);

        if (!movement.paused)
        {
            if (!back)
            {
                totalTime += Time.deltaTime;
            }

            HealthBarUpdate();
            SlotUpdate();

            if (invulCooldown > 0)
            {
                invulCooldown -= Time.deltaTime;
            }

            redCover.color = new Color(1f, 0f, 0f, redCover.color.a - Time.deltaTime * redFadeSpeed);

            if (needToSwap)
            {
                swapTime += Time.deltaTime;
                if (swapTime > 0.5f)
                {
                    needToSwap = false;
                    swapTime = 0f;
                    foreach (GameObject item in physicalItem)
                    {
                        item.SetActive(false);
                    }
                    physicalItem[pickedSlot - 1].SetActive(true);
                }
            }

            if (itemCooldown > 0)
            {
                itemCooldown -= Time.deltaTime;
            }

            if (arrowPull && arrowSpeed < 5000f)
            {
                arrowSpeed += Time.deltaTime * 2000f;
                Camera.main.fieldOfView = -0.005f * arrowSpeed + 65;
            }

            if (enchantTime > 0)
            {
                enchantTime -= Time.deltaTime;
            }
            if (flightTime > 0)
            {
                movement.rb.AddForce(Vector3.up * movement.speed);
                flightTime -= Time.deltaTime;
            }
            if (safetyTime > 0)
            {
                safetyTime -= Time.deltaTime;
            }
        }

        arrowMessage.SetActive(arrows <= 0);
        if (dummyArrow != null)
        {
            dummyArrow.SetActive(arrows > 0);
        }
        ammoMessage.SetActive(ammo <= 0);

        sword.material = swordMats[swordLevel];
        armorMainBar.gameObject.SetActive(armorLevel != 0);
        armorMainBar.color = armorColors[armorLevel * 2];
        armorSmallBar.color = armorColors[(armorLevel * 2) + 1];

        if (health > 100)
        {
            health = 100;
        }

        if (gameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Restart();
            }
        }

        if (scrollTimer > 0)
        {
            scrollTimer -= Time.deltaTime;
        }
    }
}
