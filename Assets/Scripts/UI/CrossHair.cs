using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour {

    private RectTransform rect;

    private void Start()
    {
        rect = gameObject.GetComponent(typeof(RectTransform)) as RectTransform;
    }

    private void FixedUpdate()
    {
        if (Data_Manager.underPlayerControl && !Data_Manager.inMenu && !Data_Manager.gameOver && Data_Manager.xboxCursor)
        {
            if (rect.anchoredPosition == new Vector2(-15, 15))
            {
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchoredPosition = Vector3.zero;
            }
            var invertedY = Input.GetAxis("XBox-Y");
            if (!Data_Manager.inverted) invertedY *= -1;

            rect.anchoredPosition += new Vector2(Input.GetAxis("XBox-X") * Time.fixedDeltaTime* 125f, invertedY * Time.fixedDeltaTime * 125f);

        } else
        {
            rect.anchorMax = new Vector2(0, 1);
            rect.anchorMin = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(-15, 15);
        }
    }
}
