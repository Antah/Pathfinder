using UnityEngine;

public class CameraController : MonoBehaviour {
    float cameraDistanceMax = 100f;
    float cameraDistanceMin = 5f;
    float cameraDistance = 30f;
    float scrollSpeed = 10f;

	public Vector3 offset = new Vector3(0,0,-40);

	// Use this for initialization
	void Start () 
	{
        ResetPosition();
    }

    void Update()
    {
        cameraDistance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);

        gameObject.GetComponent<Camera>().orthographicSize = cameraDistance;
    }

    public void ResetPosition()
    {
        Vector3 cameraPosition = new Vector3(GameManager.instance.boardScript.columns / 2, GameManager.instance.boardScript.rows / 2, -cameraDistance);
        transform.position = cameraPosition;
    }
}