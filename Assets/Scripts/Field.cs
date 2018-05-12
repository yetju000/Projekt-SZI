using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {
	//there can be 3 type of fields : MudField ,PlantField and  PathField

	public int priority; 

	// if (state false = 0, state true = 1) , (irrigation false = 3 , irrigation true = 0) , (minerals 2 = 0 , minerals 1 = 2 , minerals 0 = 3) , (sick 1 = 5 , sick 0 = 0) , 
	//checkForCollect true = 10 , checkForCollect false = 0
	bool state = false; // if anything grows here. 0 = no , 1 = yes
	bool irrigation = true; // if needs water = 0 , if its ok = 1;
	int minerals = 2; //0 if no minerals , 1 if medium , 2 if its ok
	int chanceForDewater = 5; // % for lack of water
	int chanceForLessMinerals = 5;

	Plant plant = null;
	float timer = 0.0f;
	public MeshRenderer mesh;
	public string type;

	// textury
	public Material grass;
	public Material smallTulips;
	public Material bigTulips;
	public Material smallCorn;
	public Material bigCorn;
	public Material smallWheat;
	public Material bigWheat;
	public Material smallColza;
	public Material bigColza;
	public Material deadPlant;

	public float fieldSpeed;

	// Use this for initialization
	void Start () {
		timer = Random.Range (3,6);
		mesh = this.GetComponent<MeshRenderer>();
<<<<<<< HEAD

		if (type.Equals ("PlantField")) {
			fieldSpeed = 20f;
			priority = 1;
		}
			
		if (type.Equals ("PathField")) {
			fieldSpeed = 30f;
			priority = -1;
		}
			
		if (type.Equals ("MudField")) {
=======
		priority = 0;

		if (type.Equals ("PlantField"))
			fieldSpeed = 5f;
		if (type.Equals ("PathField"))
>>>>>>> af17bfe2f5f713e918294b8b7619ad257e88cf56
			fieldSpeed = 10f;
			priority = -1;
		}
	}
		
	// Update is called once per frame
	void Update () {
		if (type.Equals("PlantField")) {
			
			if (plant.getState ())
				priority = 0;
			if (!checkIrrigation()) 
				priority = priority + 3;
			if (checkMinerals () == 1)
				priority = priority + 2;
			if (checkMinerals () == 0)
				priority = priority + 3;
			if (checkSick ())
				priority = priority + 5;
			if (checkForCollect ())
				priority = priority + 10;
		timer -= Time.deltaTime;
			if (plant == null) {
				setState (false);
				priority = 1;
			}
		if (timer < 0) {
			if (plant != null) {
					priority = 0;

				if (!plant.getState()) {
					setState (false);
					plant = null;

					Material[] ma = mesh.materials;
					ma [0] = deadPlant;
					mesh.materials = ma;
						
				}
<<<<<<< HEAD
				

						


					if (plant.Grow (irrigation, minerals)) {
						if (plant.getName ().Equals ("Tulipan")) {
							Material[] ma = mesh.materials;
							ma [0] = bigTulips;
							mesh.materials = ma;
						}
						if (plant.getName ().Equals ("Pszenica")) {
							Material[] ma = mesh.materials;
							ma [0] = bigWheat;
							mesh.materials = ma;
						}
						if (plant.getName ().Equals ("Kukurydza")) {
							Material[] ma = mesh.materials;
							ma [0] = bigCorn;
							mesh.materials = ma;
						}
						if (plant.getName ().Equals ("Rzepak")) {
							Material[] ma = mesh.materials;
							ma [0] = bigColza;
							mesh.materials = ma;
						}
					}
=======
				if (plant.checkForCollect ()) {
					if (plant.getName ().Equals ("Tulipan")) {
						Material[] ma = mesh.materials;
						ma [0] = bigTulips;
						mesh.materials = ma;
					}
					if (plant.getName ().Equals ("Pszenica")) {
						Material[] ma = mesh.materials;
						ma [0] = bigWheat;
						mesh.materials = ma;
					}
					if (plant.getName ().Equals ("Kukurydza")) {
						Material[] ma = mesh.materials;
						ma [0] = bigCorn;
						mesh.materials = ma;
					}
					if (plant.getName ().Equals ("Rzepak")) {
						Material[] ma = mesh.materials;
						ma [0] = bigColza;
						mesh.materials = ma;
					}
				}
				plant.Grow (irrigation,minerals);
>>>>>>> af17bfe2f5f713e918294b8b7619ad257e88cf56
				plant.checkOvergroved ();

			}

			Dewater ();
			LessMinerals ();

<<<<<<< HEAD
			timer = Random.Range (4,7);
					

=======
			timer = Random.Range (3,6);

			
			if (!checkIrrigation()) 
				priority = 3;
			if (checkMinerals () == 1)
				priority = priority + 2;
			if (checkMinerals () == 0)
				priority = priority + 3;
			if (checkSick ())
				priority = priority + 5;
			if (checkForCollect ())
				priority = priority + 10;
>>>>>>> af17bfe2f5f713e918294b8b7619ad257e88cf56
				
		}
				
		}

	}

	public bool getState() {
		return this.state;
	}
	public void setState(bool state) {
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
		if (minerals < 0)
			minerals = 0;
	}
	public bool checkIrrigation() {
		return irrigation;
	}
	public bool checkSick() {
		return plant.checkSickness();
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
		int howmany = plant.Collect();
		plant = null;
		return howmany;
	}
	public void MakeGrass() {
		Material[] ma = mesh.materials;
		ma [0] = grass;
		mesh.materials = ma;
	}
	public void CheckDead() {
		Material[] ma = mesh.materials;
		if (mesh.materials [0].name.Equals ("DeadPlant") || mesh.materials [0].name.Equals ("deadPlant")) {
			ma [0] = grass;
			mesh.materials = ma;
		}
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
			Material[] ma = mesh.materials;
			ma [0] = smallCorn;
			mesh.materials = ma;
			plant = new Corn();
		}
		if (name.Equals("Wheat")){
			Material[] ma = mesh.materials;
			ma [0] = smallWheat;
			mesh.materials = ma;
			plant = new Wheat();
		}
		if (name.Equals("Colza")){
			Material[] ma = mesh.materials;
			ma [0] = smallColza;
			mesh.materials = ma;
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


	public virtual bool Grow(bool irrigation,int minerals){
		return false;
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
		HowMuchGrowed = 0;
		sick = false;
		HowManyTimesSick = 0;
		chanceForSick = 5;
		HowManyTimesLackOfMinerals = 0;
	}

	public override bool Grow(bool irrigation,int minerals) {
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

		if (HowMuchGrowed >= 100)
			return true;
		else
			return false;
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

	public override bool Grow(bool irrigation,int minerals) {
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
		if (HowMuchGrowed >= 100)
			return true;
		else
			return false;
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

	public override bool Grow(bool irrigation,int minerals) {
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
		if (HowMuchGrowed >= 100)
			return true;
		else
			return false;
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

	public override bool Grow(bool irrigation,int minerals) {
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
		if (HowMuchGrowed >= 100)
			return true;
		else
			return false;
	}
} 



