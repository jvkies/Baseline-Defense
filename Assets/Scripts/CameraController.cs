
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float panSpeed = 5f;
	public float panBorderThickness = 10f;

	public float scrollSpeed = 1f;
	public float minZoom = 3f;
	public float maxZoom = 6f;

	public float minYzoomIn = -5f;
	public float minYzoomOut = -1.93f;

	public float maxYzoomIn = 5.46f;
	public float maxYzoomOut = 2.4f;

	public float minXzoomIn = -5f;
	public float minXzoomOut = 0.82f;

	public float maxXzoomIn = 6.64f;
	public float maxXzoomOut = 2.5f;

	// Update is called once per frame
	void LateUpdate () {

		if (GameManager.instance.isGameLost)
		{
			this.enabled = false;
			return;
		}

		if (Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - panBorderThickness)
		{
			if (transform.position.y < lerpZoom(GetComponent<Camera> ().orthographicSize, maxYzoomIn, maxYzoomOut))
				transform.Translate(Vector3.up * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= panBorderThickness)
		{
			if (transform.position.y > lerpZoom(GetComponent<Camera> ().orthographicSize, minYzoomIn, minYzoomOut))
				transform.Translate(Vector3.down * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - panBorderThickness)
		{
			if (transform.position.x < lerpZoom(GetComponent<Camera> ().orthographicSize, maxXzoomIn, maxXzoomOut))
				transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= panBorderThickness)
		{
			if (transform.position.x > lerpZoom(GetComponent<Camera> ().orthographicSize, minXzoomIn, minXzoomOut))
				transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
		}

		float scroll = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
		float newSize = GetComponent<Camera> ().orthographicSize += scroll;
		GetComponent<Camera> ().orthographicSize = Mathf.Clamp(newSize,minZoom,maxZoom);

		//Vector3 pos = transform.position;
		//pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
		//pos.y = Mathf.Clamp(pos.y, minY, maxY);

		//transform.position = pos;
		//Mathf.clam
	}

	private float lerpZoom ( float _zoom, float _zoomIn, float _zoomOut) {
		return Mathf.Lerp(_zoomIn, _zoomOut, (_zoom/3)-1);
	}


	private float zoomToMaxY ( float _zoom) {
		return Mathf.Lerp(maxYzoomIn, maxYzoomOut, (_zoom/3)-1);
	}

	private float zoomToMinY ( float _zoom) {
		return Mathf.Lerp(minYzoomIn, minYzoomOut, (_zoom/3)-1);
	}

	private float zoomToMinX ( float _zoom) {
		return Mathf.Lerp(minXzoomIn, minXzoomOut, (_zoom/3)-1);
	}


}


// y 5.46 : 3
// y 2.5 : 6

// y -2 : 6
// y -5 : 3

// x 0.82 : 6
// x -5 : 3