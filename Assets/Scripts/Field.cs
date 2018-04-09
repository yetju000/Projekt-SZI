using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Field : MonoBehaviour {

	bool state = false; // if anything grows here. 0 = no , 1 = yes
	bool irrigation = true; // if needs water = 0 , if its ok = 1;
	int minerals = 2; //0 if no minerals , 1 if medium , 2 if its ok
	int chanceForDewater = 5; // % for lack of water
	int chanceForLessMinerals = 5;
	Plant plant = null;
	float timer = 0.0f;
	MeshRenderer mesh;
	public Material grass;
	public Material smallTulips;
	public Material bigTulips;
	// Use this for initializationj
	void Start () {
		timer = Random.Range (3,6);
		mesh = this.GetComponent<MeshRenderer>();

	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (plant == null)
			setState (false);
		
		if (timer < 0) {
			if (plant != null) {
				if (!plant.getState()) {
					setState (false);
					plant = null;
				}
				if (plant.checkForCollect ()) {
					Material[] ma = mesh.materials;
					ma [0] = bigTulips;
					mesh.materials = ma;
				}
				plant.Grow (irrigation,minerals);
				plant.checkOvergroved ();
				}
			Dewater ();
			LessMinerals ();

			timer = Random.Range (3,6);
		}

		if (Input.GetKeyDown(KeyCode.P))
		{if (!state) {
				Debug.Log (state);
				PlantIt("Tulip");
			}}
		
		if (Input.GetKeyDown (KeyCode.C)) {
			if (plant.checkForCollect ()) {
				plant = null;
				Material[] ma = mesh.materials;
				ma [0] = grass;
				mesh.materials = ma;
				Debug.Log (plant.Collect ());
			}
		}
	}

	public bool getState() {
		return this.state;
	}
	private void setState(bool state) {
		this.state = state;
	}
	public int checkMinerals() {
		return minerals;
	}
	public void AddMinerals(int HowMuch) {
		minerals += HowMuch;
	}
	private void LessMinerals() {
		if (Random.Range(1,100)<chanceForLessMinerals)
			minerals -= 1;
	}
	public bool checkIrrigation() {
		return irrigation;
	}
	public bool checkSick() {
		return plant.checkSickness ();
	}
	public void Irrigate() {
		irrigation = true;
	}
	public void SavePlant() {
		plant.SavePlant ();
	}
	private void Dewater() {
		if (Random.Range(1,100)<chanceForDewater)
			irrigation = false;
	}
	public bool checkForCollect() {
		return plant.checkForCollect ();
	}
	public int Collect() {
		return plant.Collect ();
	}
	public void PlantIt(string name) {

		state = true;
		if (name.Equals("Tulip")){
			Material[] ma = mesh.materials;
			ma [0] = smallTulips;
			mesh.materials = ma;
			plant = new Tulip();
		}
		if (name.Equals("Corn")){
			plant = new Corn();
		}
		if (name.Equals("Wheat")){
			plant = new Wheat();
		}
		if (name.Equals("Colza")){
			plant = new Colza();
		}
	}

}

public abstract class Plant {
	protected string name; //plant name
	protected bool state; //0 = dead , 1 = alive

	protected int HowMuchGrowed; //1 = small , 100 = need to be colected , 150 = overgroved

	protected int HowManyTimesLackOfWater; //0 = ok , more than 10 = plant is dead
	protected int HowManyTimesLackOfMinerals;
	protected int HowManyTimesSick; //how long is plant sick. if more than 10 plant is dead

	protected bool sick; // 0 if no , 1 if yes
	protected int chanceForSick; // % for sick

	public string getName(){
		return name;
	}

	public bool getState(){
		return state;
	}

	public int getHowMuchGrowed() {
		return HowMuchGrowed;
	}

	public bool checkSickness() {
		return sick;
	}


	public virtual void Grow(bool irrigation,int minerals){
	}

	public void SavePlant() {
		HowManyTimesSick = 0;
		sick = false;
	}

	public int Collect() {
		state = false;
		return Random.Range (1, 3);
	}

	public void checkOvergroved() {
		if (HowMuchGrowed > 150)
			state = false;
	}

	public bool checkForCollect() {
		if (HowMuchGrowed >= 100)
			return true;
		else
			return false;
	}
}

public class Tulip : Plant {
	
	public Tulip(){
		name = "Tulipan";
		state = true;
		HowMuchGrowed = 80;
		sick = false;
		HowManyTimesSick = 0;
		chanceForSick = 5;
		HowManyTimesLackOfMinerals = 0;
	}

	public override void Grow(bool irrigation,int minerals) {
		if (HowManyTimesLackOfWater > 10 || HowManyTimesSick > 10 || HowManyTimesLackOfMinerals > 10)
			state = false;

		if (minerals > 0)
			HowManyTimesLackOfMinerals = 0;

		if (minerals == 0)
			HowManyTimesLackOfMinerals += 1;

		if (irrigation) {
			HowManyTimesLackOfWater = 0;
			HowMuchGrowed += Random.Range (3,5)+ minerals;
		}
		if (!irrigation) 
				HowManyTimesLackOfWater += 1;
		
		if (!sick) {
			HowManyTimesSick = 0;
			if (Random.Range (1, 100) < chanceForSick)
				sick = true;
				
		}
		else if (sick) 
			HowManyTimesSick += 1;
		
	}
} 

public class Corn : Plant {

	public Corn(){
		name = "Kukurydza";
		state = true;
		HowMuchGrowed = 0;
		sick = false;
		HowManyTimesSick = 0;
		chanceForSick = 5;
		HowManyTimesLackOfMinerals = 0;
	}

	public override void Grow(bool irrigation,int minerals) {
		if (HowManyTimesLackOfWater > 10 || HowManyTimesSick > 10 || HowManyTimesLackOfMinerals > 10)
			state = false;

		if (minerals > 0)
			HowManyTimesLackOfMinerals = 0;

		if (minerals == 0)
			HowManyTimesLackOfMinerals += 1;

		if (irrigation) {
			HowManyTimesLackOfWater = 0;
			HowMuchGrowed += Random.Range (3,5)+ minerals;
		}
		if (!irrigation) 
			HowManyTimesLackOfWater += 1;

		if (!sick) {
			HowManyTimesSick = 0;
			if (Random.Range (1, 100) < chanceForSick)
				sick = true;

		}
		else if (sick) 
			HowManyTimesSick += 1;

	}
} 

public class Wheat : Plant {

	public Wheat(){
		name = "Pszenica";
		state = true;
		HowMuchGrowed = 0;
		sick = false;
		HowManyTimesSick = 0;
		chanceForSick = 5;
		HowManyTimesLackOfMinerals = 0;
	}

	public override void Grow(bool irrigation,int minerals) {
		if (HowManyTimesLackOfWater > 10 || HowManyTimesSick > 10 || HowManyTimesLackOfMinerals > 10)
			state = false;

		if (minerals > 0)
			HowManyTimesLackOfMinerals = 0;

		if (minerals == 0)
			HowManyTimesLackOfMinerals += 1;

		if (irrigation) {
			HowManyTimesLackOfWater = 0;
			HowMuchGrowed += Random.Range (3,5)+ minerals;
		}
		if (!irrigation) 
			HowManyTimesLackOfWater += 1;

		if (!sick) {
			HowManyTimesSick = 0;
			if (Random.Range (1, 100) < chanceForSick)
				sick = true;

		}
		else if (sick) 
			HowManyTimesSick += 1;

	}
} 

public class Colza : Plant {

	public Colza(){
		name = "Rzepak";
		state = true;
		HowMuchGrowed = 0;
		sick = false;
		HowManyTimesSick = 0;
		chanceForSick = 5;
		HowManyTimesLackOfMinerals = 0;
	}

	public override void Grow(bool irrigation,int minerals) {
		if (HowManyTimesLackOfWater > 10 || HowManyTimesSick > 10 || HowManyTimesLackOfMinerals > 10)
			state = false;

		if (minerals > 0)
			HowManyTimesLackOfMinerals = 0;

		if (minerals == 0)
			HowManyTimesLackOfMinerals += 1;

		if (irrigation) {
			HowManyTimesLackOfWater = 0;
			HowMuchGrowed += Random.Range (3,5)+ minerals;
		}
		if (!irrigation) 
			HowManyTimesLackOfWater += 1;

		if (!sick) {
			HowManyTimesSick = 0;
			if (Random.Range (1, 100) < chanceForSick)
				sick = true;

		}
		else if (sick) 
			HowManyTimesSick += 1;

	}
} 

