
using UnityEngine;

public class CameraController : MonoBehaviour {

	private Vector3 dragOrigin;
	private Camera cam;

	public float panSpeed = 5f;
	public float panBorderThickness = 10f;
	public float scrollSpeed = 5f;

	public float gameFieldSizeY = 8.25f;		// not in unity meters, in orthographic camera size (unity meters / 2)
	public float gameFieldSizeX = 10.75f;		// not in unity meters, in orthographic camera size (unity meters / 2)
	public float gameFieldOffsetY = 0.25f;		// game field is offset y for a bit

	public float menuOffset = 4f;

	public float minZoom = 3f;
	public float maxZoom = 6f;

	void Start() {
		cam = Camera.main;
	}

	void LateUpdate () {

		// deactivate script when lost
		if (GameManager.instance.isGameLost)
		{
			this.enabled = false;
			return;
		}

		// zoom
		float scroll = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
		float zoom = GetComponent<Camera> ().orthographicSize;
		zoom += scroll;
		float cZoom =  Mathf.Clamp(zoom, minZoom, maxZoom);
		GetComponent<Camera> ().orthographicSize = cZoom;

		// click and drag
		if( Input.GetMouseButtonDown( 0 ) ) {
			dragOrigin = new Vector3( Input.mousePosition.x, Input.mousePosition.y, 0 );
			dragOrigin = cam.ScreenToWorldPoint( dragOrigin );
		};

		if( Input.GetMouseButton( 0 ) ) {
			Vector3 currentPos = new Vector3( Input.mousePosition.x, Input.mousePosition.y, 0 );
			currentPos = cam.ScreenToWorldPoint( currentPos );
			Vector3 movePos = dragOrigin - currentPos;

			transform.position += ( movePos );
		};

		Vector3 pos = transform.position;

		// moving with keys and borders
		if (Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - panBorderThickness) {
			pos.y += panSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= panBorderThickness) {
			pos.y -= panSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - panBorderThickness) {
			pos.x += panSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= panBorderThickness) {
			pos.x -= panSpeed * Time.deltaTime;
		}

		pos.y = Mathf.Clamp (pos.y, lerpZoom(-limitY(cZoom), -limitY(cZoom), cZoom) + gameFieldOffsetY, lerpZoom(limitY(cZoom), limitY(cZoom), cZoom) + gameFieldOffsetY);
		pos.x = Mathf.Clamp (pos.x, lerpZoom(-limitX(cZoom), -limitX(cZoom), cZoom), lerpZoom(limitX(cZoom), limitX(cZoom) + menuOffset, cZoom));

		transform.position = pos;

	}
		
	private float limitY(float zoom) {
		return gameFieldSizeY - zoom;
	}

	private float limitX(float zoom) {
		return gameFieldSizeX - (zoom * cam.aspect);
	}
		
	private float lerpZoom (float _zoomIn, float _zoomOut, float _zoom) {
		return Mathf.Lerp(_zoomIn, _zoomOut, (_zoom-minZoom) / (maxZoom - minZoom));
	}

}