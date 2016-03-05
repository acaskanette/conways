using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Stores the agents and manages their behaviour
public class AgentManager : MonoBehaviour {

	[SerializeField]
	private GameObject AgentType;		// The prefab to spawn for each agent
	
	private GameObject[,] agents;		// The 2D-array of Agents
	private Agent[,] agentScripts;		// References to each Agent's script
	
	[SerializeField]
	private Text SimulationText;		// On screen GUI element to communicate game state
	
	private const int width = 40;		// width of the grid of Agents
	private const int height = 18;		// height of the grid of Agents
	
	private float timeElapsed = 0.0f;	// Timer variable
	private const float TIME_BETWEEN_UPDATES = 0.2f;	// How long to wait between update cycles
	
	bool simulating;			// Whether you have started the simulation
	
	// Initialize Array and game state
	void Start () {       
		
		simulating = false;
		
		// Set up array of Agents
		agents = new GameObject[width,height];
		agentScripts = new Agent[width, height];
		
		// Fill array of Agents and Agent scripts
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				// Make a new agent
				agents[i, j] = (GameObject)GameObject.Instantiate(AgentType, new Vector3(i - width/2, 0, j - height/2), Quaternion.identity);
				// And store its script reference
				agentScripts[i, j] = agents[i, j].GetComponent<Agent>();
			}
		}
	
	}
	
	// Update list of Agents
	void Update () {
	
		// Toggle Simulation on/off
		if (Input.GetKeyDown(KeyCode.Space)) {
			simulating = !simulating;
		}
		
		// If Simulation is on...
		if (simulating)	{
			// Enable the UI text to tell user simulation has started
			if (!SimulationText.enabled)
				SimulationText.enabled = true;
			
			// Keep track of time
			timeElapsed += Time.deltaTime;
			
			// If timer is long enough
			if (timeElapsed >= TIME_BETWEEN_UPDATES) {
				// Figure out which agents will change state
				FlagAgentsForToggle();
				// Change their state
				ToggleAgents();
				// Reset timer
				timeElapsed = 0.0f;
			}
		}
		else { // !simulating ; Simulation is off
			// Disable UI text 
			if (SimulationText.enabled)
				SimulationText.enabled = false;
		}
	
	}
	
	
	// Goes through the list and set the next State based on the conditions of Conway's Game of Life
	void FlagAgentsForToggle() {        
	
		for (int i = 0; i < width; i++) {
		
			for (int j = 0; j < height; j++) {
			
				bool living = agentScripts[i, j].isAlive; 	// Whether the agent is alive or not, for readability
				int count = NumberOfNeighbours(i, j);		// How many neighbours the agent has, for readbility
				
				bool result = false;	// Whether to swap states for this agent
				
				if (living && count < 2)	// condition 1: Agent is alive and has less than 2 living neighbours
					result = false;		// result: Agent dies from underpopulation
				if (living && count == 2 || count == 3)	// condition 2: Agent is alive and has 2 or 3 living neighbours
					result = true;		// result: Agent stays alive
				if (living && count > 3)	// condition 3: Agent is alive and has more than 3 living neighbours
					result = false;		// result: Agent dies from overpopulation
				if (!living && count == 3)	// condition 4: Agent was dead yet has exactly 3 living neighbours
					result = true;		// result: Agent becomes alive through reproduction
				
				// Set the agents next state to the result, flagging it for swap
				agentScripts[i, j].nextState = result;
			
			}
		}
		
	}
	
	// Returns the number of neighbours alive next to the current agent
	int NumberOfNeighbours(int x, int y)	{
	
	
		// Adjust indexes to avoid overflow; allows agents to wrap around the game board
		int arx = (x + 1 + width) % width;      // adjust x for bounds on right side
		int aby = (y + 1 + height) % height;    // adjust y for bounds on bottom side
		int alx = (x - 1 + width) % width;      // adjust x for bounds on left side
		int aty = (y - 1 + height) % height;    // adjust y for bounds on top side
	
		// Counter for number of live neighbours
		int liveNeighbours = 0;
		
		// Counts neighbour in each direction
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
		
		// Return the neighbour count
		return liveNeighbours;
	}
	
	// Toggles all Agent and checks for stagnation
	void ToggleAgents() {
		// Flag for whether all agents are dead
		bool allDead = true;
		// Loop through the Agents
		for (int i = 0; i < width; i++)	{
			for (int j = 0; j < height; j++) {
				// If next state is different from current state
				if (agentScripts[i,j].nextState != agentScripts[i,j].isAlive) {
					// We're not all dead!
					allDead = false;
					// Toggle that agent
					agentScripts[i, j].ToggleAlive();                    
				}
			}
		}
		// Turn off the simulation if no state changes occur
		if (allDead)
			simulating = !simulating;
	}


}
