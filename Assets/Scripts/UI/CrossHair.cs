using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour {
    // Reference to our rect
    private RectTransform rect;

    private void Start()
    {
        rect = gameObject.GetComponent(typeof(RectTransform)) as RectTransform;
    }

    private void FixedUpdate()
    {
        // If we are playing, and the xboxCursor is toggled, move the cursor based on the right joystick movement.
        if (Data_Manager.underPlayerControl && !Data_Manager.inMenu && !Data_Manager.gameOver && Data_Manager.xboxCursor)
        {
            // If the rect is off screen, reset anchors to the middle and set the pos to 0,0,0
            if (rect.anchoredPosition == new Vector2(-15, 15))
            {
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchoredPosition = Vector3.zero;
            }
            var invertedY = Input.GetAxis("XBox-Y");
            if (!Data_Manager.inverted) invertedY *= -1;

            rect.anchoredPosition += new Vector2(Input.GetAxis("XBox-X") * Time.fixedDeltaTime* 125f, invertedY * Time.fixedDeltaTime * 125f);

        }
        // If we aren't playing or the cursor is off, set the rect off screen.
        else
        {
            rect.anchorMax = new Vector2(0, 1);
            rect.anchorMin = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(-15, 15);
        }
    }
}
