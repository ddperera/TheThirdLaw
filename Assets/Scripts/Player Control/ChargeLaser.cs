using UnityEngine;
using System.Collections;

public class ChargeLaser : MonoBehaviour {

	public float laserRange;
	public Color laserColor;
	public LineRenderer lineRenderer;
	public Transform muzzlePoint;

	[HideInInspector]
	public float opacity;

	// Use this for initialization
	void Start () {
		lineRenderer.SetColors(Color.clear, Color.clear);
		lineRenderer.SetVertexCount(4);
	}

	// Update is called once per frame
	void Update () {
		lineRenderer.SetPosition(0, transform.position);
		RaycastHit2D hit1 = Physics2D.Raycast(muzzlePoint.position, muzzlePoint.up, laserRange);
		if (hit1.collider == null){
			for (int i=1; i<4; i++){
				lineRenderer.SetPosition(i,transform.position+transform.up*laserRange);
			}
		} else {
			lineRenderer.SetPosition(1, hit1.point);
			Vector3 reflectedVec = -Vector3.Reflect(muzzlePoint.up.normalized, new Vector3(hit1.normal.x, hit1.normal.y, 0f));
			RaycastHit2D hit2 = Physics2D.Raycast(hit1.point, reflectedVec, laserRange-hit1.distance);
			if (hit2.collider == null){
				lineRenderer.SetPosition(2, hit1.point+((Vector2)reflectedVec*(laserRange-hit1.distance)));
				lineRenderer.SetPosition(3, hit1.point+((Vector2)reflectedVec*(laserRange-hit1.distance)));
				if (opacity > .5f) Debug.Break();
			} else {
				lineRenderer.SetPosition(2, hit2.point);
				lineRenderer.SetPosition(3, hit2.point + ((Vector2)(Vector3.Reflect(reflectedVec.normalized, new Vector3(hit2.normal.x, hit2.normal.y))*
				                         (laserRange-hit2.distance-hit1.distance))));
				//if (opacity > .5f) Debug.Break();
			}
		}
		laserColor.a = opacity;
		lineRenderer.SetColors(laserColor, laserColor);
	}
}
