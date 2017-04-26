using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Manager : MonoBehaviour {

    // This class is a data manager. It keeps data through scene transitions.
    // It also holds static methods as well as global variables.

    // 0 = speeder, 1 = cruiser, 2 = tank
    public static int shipType = 0;
    // 0 = super easy, 1 = normal, 2 = hard, 3 = impossibru!!!!
    public static int difficulty = 1;
    // If the game is under the players control
    public static bool underPlayerControl = false;
    // If the player is in the menus.
    public static bool inMenu = false;
    // If the player has died/reached the end.
    public static bool gameOver = false;

    public static bool IsPlayer(Collider coll)
    {
        // If our Collider is on a GameObject that has a parent.
        if (coll.gameObject.transform.parent != null)
        {
            // Grab that parent's tag and check to see if they're a player.
            if (coll.gameObject.transform.parent.gameObject.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

}
