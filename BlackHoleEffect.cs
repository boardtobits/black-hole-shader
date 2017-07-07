using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class BlackHoleEffect : MonoBehaviour {

	//public settings
	public Shader shader;
	public Transform blackHole;
	public float ratio; //aspect ratio of the screen
	public float radius; //size of black hole in scene

	//private settings
	Camera cam;
	Material _material; //will be procedurally generated

	Material material {
		get { 
			if (_material == null) {
				_material = new Material (shader);
				_material.hideFlags = HideFlags.HideAndDontSave;
			}
			return _material;
		}
	}

	void OnEnable(){
		cam = GetComponent<Camera> ();
		ratio = 1f / cam.aspect;
	}

	void OnDisable(){
		if (_material) {
			DestroyImmediate (_material);
		}
	}

	Vector3 wtsp;
	Vector2 pos;

	void OnRenderImage(RenderTexture source, RenderTexture destination){
		//processing happens here
		if (shader && material && blackHole){
			wtsp = cam.WorldToScreenPoint (blackHole.position);

			//is the black hole in front of the camera
			if (wtsp.z > 0){
				pos = new Vector2 (wtsp.x / cam.pixelWidth, 1 - (wtsp.y / cam.pixelHeight));
				//apply shader parameters
				_material.SetVector("_Position", pos);
				_material.SetFloat ("_Ratio", ratio);
				_material.SetFloat ("_Rad", radius);
				_material.SetFloat ("_Distance", Vector3.Distance(blackHole.position, transform.position));

				//apply the shader to the image
				Graphics.Blit(source, destination, material);
			}
		}
	}




}
