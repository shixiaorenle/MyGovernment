using UnityEngine;
using System.Collections;

public class AutoFullScreen : Define {

	// Use this for initialization
	void Start () {
		GetComponent<RectTransform> ().sizeDelta = new Vector2 (_scaledScreenSize.x, _scaledScreenSize.y);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Test()
    {

    }
}
