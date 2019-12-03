using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SnapToPixel : MonoBehaviour {
    public float z = 10;
	private PixelCamera cam;
	
	float d;
	
	void Start() {
		cam = GetComponentInChildren<PixelCamera>();
		
		d = 1f / cam.pixelsPerUnit;
	}

	void LateUpdate() {
		Vector3 pos = transform.position;
		Vector3 camPos = new Vector3 (pos.x - pos.x % d, pos.y - pos.y % d, pos.z+ z);	
		cam.transform.position = camPos;
	}
}
