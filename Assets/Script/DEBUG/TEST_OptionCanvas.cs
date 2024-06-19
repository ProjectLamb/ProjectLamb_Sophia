using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_OptionCanvas : MonoBehaviour
{
    public void CloseMenu() {
        PauseMenu pauseMenu = FindFirstObjectByType<PauseMenu>();
        if(pauseMenu != null) {
            pauseMenu.CloseMenu();
        }
        else {
            gameObject.SetActive(false);
        }
    }
}
