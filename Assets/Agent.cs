using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {

	// Properties
	
	public bool isAlive;	// Is the agent in the Alive State
	public bool nextState;	// Which state will this agent change to after next update
	
	MeshRenderer planeChild;	// Plane where the visual representation of the agent is shown
	private Color DEAD_COLOR = Color.black;	// Default Colour of an agent
	private Color ALIVE_COLOR = Color.cyan;	// Colour of an agent once activated, when isAlive is true


	// Methods

	// Initialize Agent values
	void Start () {
		// Agent default values
		isAlive = false;
		nextState = false;
		planeChild = gameObject.GetComponentInChildren<MeshRenderer>();
		planeChild.material.color = DEAD_COLOR;
	
	}
	
	
	// This method toggles the isAlive status of the agent and updates the visuals for it
	public void ToggleAlive() {
	
		//print("ToggleAlive");
		isAlive = !isAlive;
		
		if (isAlive)
	    		planeChild.material.color = ALIVE_COLOR;        
		else
			planeChild.material.color = DEAD_COLOR;
	}
	
	// This method is used when you click on an agent
	void OnMouseDown()
	{
		//print("MouseDown");
		ToggleAlive();        
	}

}
