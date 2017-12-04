using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;
using System.Text;


public class InventoryController : MonoBehaviour, ISubscriber {

    private Text inventoryViewText;
    private GameObject inventoryPanel;

	// Use this for initialization
	void Start () {
        inventoryPanel = gameObject.FindChildWithName("InventoryPanel");
   
        inventoryPanel.SetActive(false);
        
        inventoryViewText = gameObject.FindChildWithName("InventoryText").GetComponent<Text>();

        MyEvents.PlayerInventoryUpdated.Subscribe(this, DisplayInventoryItemTextOnScreen);
    }
	
	void DisplayInventoryItemTextOnScreen()
    {
        StringBuilder inventoryStringBuilder = new StringBuilder();
        foreach(Item inventoryItem in GameState.instance.playerInventory)
        {
            inventoryStringBuilder.Append(inventoryItem.name);
            inventoryStringBuilder.Append(" : ");
            inventoryStringBuilder.Append(inventoryItem.description);
            inventoryStringBuilder.Append("\n");
        }


        inventoryViewText.text = inventoryStringBuilder.ToString();
    }



    public void ToggleInventoryPanelOpen()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }
}
