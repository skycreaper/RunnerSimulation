using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffScreen : MonoBehaviour
{

	public float offset = 16f; // que tan lejos debe estar de la pantalla antes de ser destruido
    public delegate void OnDestroy();
    public event OnDestroy DestroyCallback;

	private bool offscreen;
	public float offscreenX;
	private Rigidbody2D body2D;

	void Awake() {
		body2D = GetComponent<Rigidbody2D>();
	}

    // Start is called before the first frame update
    void Start()
    {
     offscreenX = (Screen.width/PixelPerfectCamera.pixelsToUnits)/2 + offset;   
    }

    // Update is called once per frame
    void Update()
    {
    	var posX = transform.position.x;
    	var dirX = body2D.velocity.x;

    	if (Mathf.Abs(posX) > offscreenX) {
    		if (dirX < 0 && posX < -offscreenX) {
    			offscreen = true;
    		} else if(dirX > 0 && posX > offscreenX) {
    			offscreen = true;
    		}
    	} else {
    		offscreen = false;
    	}

    	if (offscreen) {
    		OnOutBounds();
    	}
        
    }

    public void OnOutBounds() {
    	offscreen = false;
    	GameObjectUtily.Destroy(gameObject);

        if(DestroyCallback != null) {
            DestroyCallback();
        }
     }
}
