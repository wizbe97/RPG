using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;

    void OnInventory() {
        if (menuActivated) {
            InventoryMenu.SetActive(false);
            menuActivated = false;
            Time.timeScale = 1;
        }
        else if (!menuActivated) {
            Time.timeScale = 0;
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }
    }
}