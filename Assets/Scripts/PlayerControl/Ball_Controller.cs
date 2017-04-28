using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// This camera control FAR from perfect, but frankly I don't have much time/manpower to fix it.

public class Ball_Controller : MonoBehaviour {
    
    private Transform tran;
    private GameObject player;
    private float[] splines;
    private int curIndexOfSpline = 0;
    private int lastIndexOfSpline = 0;
    private float curInterp = 0.0f;
    private Vector3 lookAtAvg;
    private int forwardSpeed = 10;

    public GameObject[] playerModels;

	void Awake()
	{
		tran = gameObject.GetComponent(typeof(Transform)) as Transform;
        player = Instantiate(playerModels[Data_Manager.shipType], tran.position, tran.rotation);
		player.transform.SetParent(tran);
	}

    void Start()
    {
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
        if (Data_Manager.gameOver || !Data_Manager.underPlayerControl || Data_Manager.inMenu) return;
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

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == 8)
        {
            ResetCameraPosition();
        }
    }

    void ResetCameraPosition()
    {
		tran.position = player.transform.position - new Vector3(0, player.transform.position.y - 25f);
        tran.rotation = player.transform.rotation;
    }

}
