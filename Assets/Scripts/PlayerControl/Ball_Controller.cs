using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ball_Controller : MonoBehaviour {
    
    // Our transform
    private Transform tran;
    // Reference to the player
    private GameObject player;
    // The splines to move the camera between. These are the Key's from the ys dictionary in Data_Manager
    private float[] splines;
    // The indeces that we're working with
    private int curIndexOfSpline = 0;
    private int lastIndexOfSpline = 0;
    private float curInterp = 0.0f;
    private Vector3 lookAtAvg;
    // Movement multiplier
    private int forwardSpeed = 10;

    // The available player characters
    public GameObject[] playerModels;

    // Create the player before Start so that other classes can access and find the player.
	void Awake()
	{
        // Store our transform
		tran = gameObject.GetComponent(typeof(Transform)) as Transform;
        // Create our player
        player = Instantiate(playerModels[Data_Manager.shipType], tran.position, tran.rotation);
		player.transform.SetParent(tran);
	}

    void Start()
    {
        // Change the movement speed based on the ship type.
        switch (Data_Manager.shipType)
        {
            case 0:
                forwardSpeed = 25;
                break;
            case 1:
                forwardSpeed = 18;
                break;
            default:
                forwardSpeed = 12;
                break;
        }


    }

    void FixedUpdate()
    {
        // If splines is null, grab the splines from the dictionary.
        if (splines == null)
        {
            var tempList = Data_Manager.ys.Keys.ToList();
            tempList.Sort();
            splines = tempList.ToArray();
            var x = tran.position.x;
            for (curIndexOfSpline = 0; curIndexOfSpline < splines.Length; curIndexOfSpline++)
            {
                lastIndexOfSpline++;
                if (x > splines[curIndexOfSpline]) break;
            }
        }
        // If we aren't playing, ignore.
        if (Data_Manager.gameOver || !Data_Manager.underPlayerControl || Data_Manager.inMenu) return;
        // This whole section works with the spline. Makes the camera face the average of the splines ahead of it.
        if (lastIndexOfSpline != curIndexOfSpline)
        {
            lookAtAvg = Vector3.zero;
            for (int i = 0; i < 7; i++)
            {
                if (curIndexOfSpline + i < splines.Length)
                {
                    float yVal;
                    Data_Manager.ys.TryGetValue(splines[curIndexOfSpline + i], out yVal);
                    Vector3 lookAt = new Vector3(splines[curIndexOfSpline + i], 25, yVal);
                    lookAtAvg = lookAtAvg + lookAt;
                } else
                {
                    lookAtAvg = lookAtAvg / (i + 1);
                    break;
                }
                if (i == 6)
                {
                    lookAtAvg = lookAtAvg / (i + 1);
                    break;
                }
            }
            lastIndexOfSpline = curIndexOfSpline;
            curInterp = 0.0f;
        }
        curInterp = Mathf.Min(curInterp + Time.fixedDeltaTime * (forwardSpeed / 10.0f), 1.0f);
        tran.rotation = Quaternion.Lerp(tran.rotation, Quaternion.LookRotation(lookAtAvg - tran.position), curInterp);
        
        tran.position = tran.position + tran.forward * Time.fixedDeltaTime * forwardSpeed;
        while (tran.position.x > splines[curIndexOfSpline])
        {
            curIndexOfSpline++;
        }
    }

}
