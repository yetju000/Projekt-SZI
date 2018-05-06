using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tractor : MonoBehaviour {


	int actualPositionX = 1; //actual position of tractor
	int actualPositionY = 8;
	public GameObject Crate;
	Field [,] field;
	Field actualField;

	// Use this for initialization
	void Start () {
		//field = new Field[20][20];
		field = new Field[20,20];
		for (int i = 2; i <= 19; i++) {
			for (int j = 2; j <= 19; j++) {
				field[i-1,j-1] = Crate.transform.Find ("Row ("+i+")").transform.Find ("Crate ("+j+")").GetComponent<Field> ();   //do tablicy field[20][20] zapisujemy informacje o wszystkich polach 
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 2; i <= 19; i++) {
			for (int j = 2; j <= 19; j++) {
				Debug.Log ("Pole["+i+"]["+j+"] : "+CheckGroundForPlant(i,j)+" "+CheckForIrrigation(i,j)+" "+CheckForMinerals(i,j)+" "+CheckSick(i,j)+" "+CheckForCollect(i,j)+" "); //check every ground. To test click 'P' to plant first rov and 'C' to collect
			}
		}


		}

	public void changeField(int x, int y) {
		actualField = field [x-1,y-1]; //because we only can use functions Plant , Refillwater , Refilminerals, SavePlantWhenSick , Collect on actualField 
	}
	public void GoField(int x, int y) {  //tractor movement
		//TODO
	}
		















	//functions below can be used at any field
	public bool CheckGroundForPlant (int x , int y) {  //if true , we can plant
		if (field[x-1,y-1] != null) {
			if (!field[x-1,y-1].getState ())
				return true;
			else
				return false;
		} else
			return false;
	}
	public bool CheckForIrrigation(int x, int y) {  //if false - need to refill water
		if (field[x-1,y-1] != null) {
			return field[x-1,y-1].checkIrrigation ();
		} else
			return true;
	}
	public int CheckForMinerals(int x , int y) {  //if 2 - its ok , 1 - medium , 0 need minerals quickly
		if (field[x-1,y-1] != null) {
			return field[x-1,y-1].checkMinerals ();
		} else
			return 2;
	}
	public bool CheckSick(int x, int y) { //if true - needs medicine , if false its ok
		if (field[x-1,y-1] != null) {
			return field[x-1,y-1].checkSick ();
		} else
			return false;
	}
	public bool CheckForCollect(int x , int y) { //if yes - need to be collected
		if (field[x-1,y-1] != null) {
			return field[x-1,y-1].checkForCollect();
		} else
			return false;
	}



	//functions below can are used on actual field only.
	public void Plant(string name) {  
		if (name.Equals ("Tulip")) {

			actualField.PlantIt (name);

		}
		if (name.Equals ("Corn")) {

			actualField.PlantIt (name);

		}
		if (name.Equals ("Wheat")) {

			actualField.PlantIt (name);

		}
		if (name.Equals ("Colza")) {

			actualField.PlantIt (name);

		}
	}
	public void RefillWater() {
		actualField.Irrigate ();
	}
	public void RefillMinerals(int HowMany) {
		actualField.AddMinerals (HowMany);
	}
	public void SavePlantWhenSick() {
		actualField.SavePlant ();
	}
	public void Collect() {
		actualField.Collect ();
	}


}

