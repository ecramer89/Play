using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour, ISubscriber
{

    public static GameState instance;

    public void Start()
    {
        instance = this;

        MyEvents.PlayerGivenItem.Subscribe(this, (Item item) => {

            playerInventory.Add(item);

            MyEvents.PlayerInventoryUpdated.Fire();

        });


        MyEvents.PlayerEnteredNewInput.Subscribe(this, (string input) => { currentPlayerInput = input; });
    }


    public List<Item> playerInventory = new List<Item>();

    public string currentPlayerInput = "";


}
