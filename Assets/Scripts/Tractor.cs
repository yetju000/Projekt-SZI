using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tractor : MonoBehaviour {

	int bin = 0;  //bin for dead plants
	int minerals = 25;  // how many minerals
	int water = 25;  //how many water
	int medicine = 15;  //how many medicine
	int collectingPlace = 0;  //how many place left for collecting. max 50
	int newTulips = 15; //how many new plants
	int newCorns = 15; //how many new plants
	int newWheat = 15; //how many new plants
	int newColza = 15; //how many new plants
	int actualPositionX = 1; //actual position of tractor
	int actualPositionY = 8;
	public GameObject Crate;
	Field actualField;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		}
	public void changeField(int x, int y) {
		actualField = Crate.transform.Find ("Row ("+x+")").transform.Find ("Crate ("+y+")").GetComponent<Field> ();
	}
	public void GoField(int x, int y) {
		//TODO
	}
	public void Plant(string name) {
		if (name.Equals ("Tulip")) {
			if (newTulips > 0) {
				actualField.PlantIt (name);
				newTulips -= 1;
			}
		}
		if (name.Equals ("Corn")) {
			if (newCorns > 0) {
				actualField.PlantIt (name);
				newCorns -= 1;
			}
		}
		if (name.Equals ("Wheat")) {
			if (newWheat > 0) {
				actualField.PlantIt (name);
				newWheat -= 1;
			}
		}
		if (name.Equals ("Colza")) {
			if (newColza > 0) {
				actualField.PlantIt (name);
				newColza -= 1;
			}
		}
	}
	public bool CheckGroundForPlant () {  //if true , we can plant
		if (actualField != null) {
			if (!actualField.getState ())
				return true;
			else
				return false;
		} else
			return false;
	}
	public bool CheckForIrrigation() {  //if false - need to refill water
		if (actualField != null) {
			return actualField.checkIrrigation ();
		} else
			return true;
	}
	public int CheckForMinerals() {  //if 2 - its ok , 1 - medium , 0 need minerals quickly
		if (actualField != null) {
			return actualField.checkMinerals ();
		} else
			return 2;
	}
	public bool CheckSick() { //if true - needs medicine , if false its ok
		if (actualField != null) {
			return actualField.checkSick ();
		} else
			return false;
	}
	public bool CheckForCollect() { //if yes - need to be collected
		if (actualField != null) {
			return actualField.checkForCollect();
		} else
			return false;
	}

	public void RefillWater() {
		water -= 1;
		actualField.Irrigate ();
	}
	public void RefillMinerals(int HowMany) {
		minerals -= HowMany;
		actualField.AddMinerals (HowMany);
	}
	public void SavePlantWhenSick() {
		medicine -= 1;
		actualField.SavePlant ();
	}
	public void Collect() {
		collectingPlace += actualField.Collect ();
	}
	public void TractorInBaseRefill () { //when in base
		bin = 0;  
		minerals = 25;  
		water = 25;  
		medicine = 15;  
		collectingPlace = 0;  
		newTulips = 15; 
		newCorns = 15; 
		newWheat = 15; 
		newColza = 15; 
	}

}

