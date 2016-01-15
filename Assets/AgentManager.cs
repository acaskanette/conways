using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AgentManager : MonoBehaviour {

    [SerializeField]
    private GameObject AgentType;

    private GameObject[,] agents;
    private Agent[,] agentScripts;

    [SerializeField]
    private Text SimulationText;

    private const int width = 40;
    private const int height = 18;

    private float timeElapsed = 0.0f;
    private const float TIME_BETWEEN_UPDATES = 0.2f;

    bool simulating;   

	// Use this for initialization
	void Start () {       

        simulating = false;

        agents = new GameObject[width,height];
        agentScripts = new Agent[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                agents[i, j] = (GameObject)GameObject.Instantiate(AgentType, new Vector3(i - width/2, 0, j - height/2), Quaternion.identity);
                agentScripts[i, j] = agents[i, j].GetComponent<Agent>();
            }
        }
     
	}
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetKeyDown(KeyCode.Space))
        {
            simulating = !simulating;
        }

        if (simulating)
        {

            if (!SimulationText.enabled)
                SimulationText.enabled = true;


            timeElapsed += Time.deltaTime;

            if (timeElapsed >= TIME_BETWEEN_UPDATES)
            {
                FlagAgentsForToggle();
                ToggleAgents();
                timeElapsed = 0.0f;
            }
        }
        else
        {
            if (SimulationText.enabled)
                SimulationText.enabled = false;
        }

    }

    void FlagAgentsForToggle()
    {        

        for (int i = 0; i < width; i++) {

            for (int j = 0; j < height; j++) {

                bool living = agentScripts[i, j].isAlive;
                int count = NumberOfNeighbours(i, j);
                bool result = false;

                if (living && count < 2)
                    result = false;
                if (living && count == 2 || count == 3)
                    result = true;
                if (living && count > 3)
                    result = false;
                if (!living && count == 3)
                    result = true;

                agentScripts[i, j].nextState = result;

            }
        }
       
    }

    int NumberOfNeighbours(int x, int y)
    {
        int liveNeighbours = 0;

        int arx = (x + 1 + width) % width;      // adjust x for bounds on right side
        int aby = (y + 1 + height) % height;    // adjust y for bounds on bottom side
        int alx = (x - 1 + width) % width;      // adjust x for bounds on left side
        int aty = (y - 1 + height) % height;    // adjust y for bounds on top side

        // Right        
        if (agentScripts[arx, y].isAlive)
                liveNeighbours++;
        // Bottom Right
            if (agentScripts[arx, aby].isAlive)
                liveNeighbours++;
        // Bottom
            if (agentScripts[x, aby].isAlive)
                liveNeighbours++;
        // Bottom Left
            if (agentScripts[alx, aby].isAlive)
                liveNeighbours++;
        // Left
            if (agentScripts[alx, y].isAlive)
                liveNeighbours++;
        // Top Left
            if (agentScripts[alx, aty].isAlive)
                liveNeighbours++;
        // Top
            if (agentScripts[x, aty].isAlive)
                liveNeighbours++;
        // Top Right
            if (agentScripts[arx, aty].isAlive)
                liveNeighbours++;       

        return liveNeighbours;
    }
    


    void ToggleAgents()
    {
        bool allDead = true;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (agentScripts[i,j].nextState != agentScripts[i,j].isAlive)
                {
                    allDead = false;
                    agentScripts[i, j].ToggleAlive();                    
                }
            }
        }

        if (allDead)
            simulating = !simulating;
    }


}
