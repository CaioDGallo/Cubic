using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimPositionHandler : MonoBehaviour {

    RectTransform pos;

    private void Start()
    {
        pos = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update () {
        pos.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
	}
}
