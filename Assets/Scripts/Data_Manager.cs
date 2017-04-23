using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Manager : MonoBehaviour {

    // This class is a data manager. It keeps data through scene transitions.
    // It also holds static methods as well as global variables.

    // 0 = speeder, 1 = cruiser, 2 = tank
    public static int shipType = 1;
    // 0 = super easy, 1 = normal, 2 = hard, 3 = impossibru!!!!
    public static int difficulty = 1;

}
