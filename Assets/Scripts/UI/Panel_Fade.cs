using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Fade : MonoBehaviour {
    
    private Image image;
    private bool fading = false;

	// Use this for initialization
	void Start () {
        image = gameObject.GetComponent(typeof(Image)) as Image;
	}
	
	// Update is called once per frame
	void Update () {
		if (fading)
        {
            if (image.color.a <= 0.0f)
            {
                gameObject.SetActive(false);
            }
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.005f);
        }
	}

    public void StartFade()
    {
        fading = true;
    }
}
