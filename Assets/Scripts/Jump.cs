using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{

	public float jumpSpeed = 240f;
	public float forwardSpeed = 20f;

	private Rigidbody2D body2d;
	private InputState inputState;

	void Awake() {
		inputState = GetComponent<InputState>();
		body2d = GetComponent<Rigidbody2D>();
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inputState.standing) {
			if(inputState.actionButton) {
				body2d.velocity = new Vector2(transform.position.x < 0 ? forwardSpeed : 0, jumpSpeed);
			}
		}
    }
}
