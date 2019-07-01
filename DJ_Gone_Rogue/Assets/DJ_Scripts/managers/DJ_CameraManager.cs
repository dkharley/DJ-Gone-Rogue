using UnityEngine;
using System.Collections;

public class DJ_CameraManager : MonoBehaviour {
	public static bool startOfLevel = false;
	private static bool prevStartLevel = false;
	private static bool prevEndOfLevel = false;
	[Range(.001f, 10.0f)]
	public float AnimationDuration;
	public float CurrTime;

	public float cameraStartHeight;
	public float cameraStartDistZ;
	public float cameraStartDistX;

    public float cameraEndHeight;
    public float cameraEndDistZ;
    public float cameraEndDistX;

	public static Vector3 camFinalOffsets;

    public static bool speedThingsUp = false;

	// Use this for initialization
	void Start ()
	{
		startOfLevel = true;
		prevStartLevel = false;
		prevEndOfLevel = false;
		CurrTime = 0.0f;
		m_camera = Camera.Instantiate(cameraPrefab) as Camera;
		camComponent = m_camera.GetComponent<DJ_Camera>();
		m_camera.transform.parent = transform;
		camComponent.pos.x = cameraStartDistX;
		camComponent.pos.y = cameraStartHeight;
		camComponent.pos.z = cameraStartDistZ;

		m_camera.transform.position = camComponent.pos;

		camFinalOffsets = new Vector3(cameraDistX, cameraHeight, cameraDistZ);
        speedThingsUp = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		camFinalOffsets.x = cameraDistX;
		camFinalOffsets.y = cameraHeight;
		camFinalOffsets.z = cameraDistZ;

		if(updateCam)
		{
			m_camera.GetComponent<DJ_Camera>().targetLookAtPos = DJ_PlayerManager.player.transform.position;

			if(startOfLevel && !prevStartLevel)
			{
				CurrTime = 0.0f;
				camComponent.pos.x = cameraStartDistX;
				camComponent.pos.y = cameraStartHeight;
				camComponent.pos.z = cameraStartDistZ;
			}

			if(DJ_PlayerManager.PlayerReachedEndOfLevel && ! prevEndOfLevel)
			{
				CurrTime = AnimationDuration;
			}

			if(startOfLevel && prevStartLevel)
			{
				if(CurrTime > AnimationDuration)
				{
					CurrTime = AnimationDuration;
					startOfLevel = false;
				}

				float x1 = cameraStartDistX + camComponent.targetLookAtPos.x;
				float x2 = cameraDistX + camComponent.targetLookAtPos.x;
				camComponent.pos.x = x1 + (x2 - x1) * CurrTime / AnimationDuration * CurrTime / AnimationDuration;

				float z1 = cameraStartDistZ + camComponent.targetLookAtPos.z;
				float z2 = cameraDistZ + camComponent.targetLookAtPos.z;
				camComponent.pos.z = z1 + (z2 - z1) * CurrTime / AnimationDuration * CurrTime / AnimationDuration;

				float h1 = cameraStartHeight;
				float h2 = cameraHeight;
				camComponent.pos.y = h1 + (h2 - h1) * CurrTime / AnimationDuration * CurrTime / AnimationDuration;

				CurrTime += Time.deltaTime;
                if (speedThingsUp)
                    CurrTime += Time.deltaTime * 2.0f;
			}
			else if(DJ_PlayerManager.PlayerReachedEndOfLevel  && prevEndOfLevel)
			{
				if(CurrTime < 0.0f)
				{
					CurrTime = 0.0f;
				}

				float x1 = cameraEndDistX + camComponent.targetLookAtPos.x;
				float x2 = cameraDistX + camComponent.targetLookAtPos.x;
				camComponent.pos.x = x1 + (x2 - x1) * CurrTime / AnimationDuration * CurrTime / AnimationDuration;
				
				float z1 = cameraEndDistZ + camComponent.targetLookAtPos.z;
				float z2 = cameraDistZ + camComponent.targetLookAtPos.z;
				camComponent.pos.z = z1 + (z2 - z1) * CurrTime / AnimationDuration * CurrTime / AnimationDuration;
				
				float h1 = cameraEndHeight;
				float h2 = cameraHeight;
				camComponent.pos.y = h1 + (h2 - h1) * CurrTime / AnimationDuration * CurrTime / AnimationDuration;
				
				CurrTime -= Time.deltaTime;
			}
			else
			{
				camComponent.pos.x = camComponent.targetLookAtPos.x + cameraDistX;
				camComponent.pos.y = cameraHeight;
				camComponent.pos.z = camComponent.targetLookAtPos.z + cameraDistZ;

                speedThingsUp = false;
			}

			prevStartLevel = startOfLevel;
			prevEndOfLevel = DJ_PlayerManager.PlayerReachedEndOfLevel;
		}
	}

	private DJ_Camera camComponent;

	public float cameraHeight = 70.0f;
	public float cameraDistZ = 75.0f;
	public float cameraDistX = 0.0f;

	public Camera m_camera;
	public Camera cameraPrefab;
	private DJ_CameraManager camManagerComponent;

	public bool updateCam = true;
}
