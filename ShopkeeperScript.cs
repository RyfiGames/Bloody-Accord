using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem
{
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public GameObject itemPrefab;
}

public class ShopkeeperScript : MonoBehaviour
{

    public string shopName;
    public string characterInfo;
    public string shopDescription;

    public List<GameItem> items;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}