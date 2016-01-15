using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {

    public bool isAlive;
    public bool nextState;

    MeshRenderer planeChild;
    private Color DEAD_COLOR = Color.black;
    private Color ALIVE_COLOR = Color.cyan;

	// Use this for initialization
	void Start () {
        isAlive = false;
        nextState = false;
        planeChild = gameObject.GetComponentInChildren<MeshRenderer>();
        planeChild.material.color = DEAD_COLOR;
	}
	
	// Update is called once per frame
	void Update () {
	   
	}
   
    public void ToggleAlive()
    {
        //print("ToggleAlive");
        isAlive = !isAlive;

        if (isAlive)
            planeChild.material.color = ALIVE_COLOR;        
        else
            planeChild.material.color = DEAD_COLOR;
    }

    void OnMouseDown()
    {
        //print("MouseDown");
        ToggleAlive();        
    }

}
