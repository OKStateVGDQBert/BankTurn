using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Death : MonoBehaviour {

	void OnCollisionEnter(Collision coll)
    {
        Debug.Log("U Ded!");
    }

}
