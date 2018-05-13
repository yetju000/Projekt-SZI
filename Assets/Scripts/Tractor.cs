using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;

public class Tractor : MonoBehaviour {

	int actualPositionX = 0; //actual position of tractor
	int actualPositionY = 7;
	public GameObject CrateRows;
	public GameObject TargetPosition;
	Field [,] field;
	int [,] PriorityTable;
	int [,] SpeedTable;
	Field actualField;
	public Grid grid; 
	float timer = 0.0f;
	//
	public Pathfinding path;
	public float speed;
	public float step;
	Vector3 GoWhere;

	// priorytet - wartosc pola
	int priorytetX = 0;
	int priorytetY = 0;

	// Use this for initialization
	void Start () {
		GoWhere = new Vector3();
		PriorityTable = new int [20,20];
		SpeedTable = new int[20,20];

		//field = new Field[20][20];
		field = new Field[20,20];
		for (int i = 1; i <= 20; i++) {
			for (int j = 1; j <= 20; j++) {
				field[i-1,j-1] = CrateRows.transform.Find ("Row ("+i+")").transform.Find ("Crate ("+j+")").GetComponent<Field> ();   //do tablicy field[20][20] zapisujemy informacje o wszystkich polach 
			}
		}

		for (int i = 0; i < 20; i++) {
			for (int j = 0; j < 20; j++) {
				SpeedTable [i, j] = (int)field [i, j].fieldSpeed;
			}
		}

		speed = field[actualPositionX,actualPositionY].fieldSpeed;
		step = speed * Time.deltaTime;

		// do debugowania
		//priorytet = actualField.getPriority ();
		//Debug.Log("Priorytet poczatkowy: " + priorytet);
	}
	
	// Update is called once per frame
	void Update () {
		speed = field[actualPositionX,actualPositionY].fieldSpeed;
		step = speed * Time.deltaTime;


		if (grid.FinalPath.Count > 0) {
			GoWhere = field [grid.FinalPath [0].iGridY, grid.FinalPath [0].iGridX].transform.position;
			changeField (grid.FinalPath [0].iGridY, grid.FinalPath [0].iGridX);
			actualPositionX = grid.FinalPath [0].iGridY;
			actualPositionY = grid.FinalPath [0].iGridX;
			if (transform.position.x < field [grid.FinalPath [0].iGridY, grid.FinalPath [0].iGridX].transform.position.x)
				transform.localEulerAngles = new Vector3(0, 90, 0);
			if (transform.position.x > field [grid.FinalPath [0].iGridY, grid.FinalPath [0].iGridX].transform.position.x)
				transform.localEulerAngles = new Vector3(0, -90, 0);
			if (transform.position.z < field [grid.FinalPath [0].iGridY, grid.FinalPath [0].iGridX].transform.position.z)
				transform.localEulerAngles = new Vector3(0, 0, 0);
			if (transform.position.z > field [grid.FinalPath [0].iGridY, grid.FinalPath [0].iGridX].transform.position.z)
				transform.localEulerAngles = new Vector3(0, 180, 0);
		}


		/// testuje field.priority ktore bede wrzucal do jakiejś listy, z ktorej pozniej bede pobieral wspolrzedne dokąd pojechac traktorem
		/// field.getPriority() i z tej listy chyba by trzeba wybierać MAX i to wrzucac do kolejki priorytetowej 
		/// max = field.getPriority()


		/// checkMinerals() zwraca ilosc mineralow na polu(0-chujowo, 1-ok , 2 to max)
		/// AddMinerals(int) podajesz ile mineralow chcesz dodac. jesli checkMinerals zwrocilo 2 to nic nie dodajesz , jesli zwrocilo 1 to dodajesz 1 a jesli zwrocilo 0 to dodajesz 2
		/// checkIrrigation() jesli zwrocilo false to znaczy ze nie jest nawodniona , jesli true to znaczy ze jest ok
		/// Irrigate() nawadnia. uzyj jesli checkIrrigation() da false
		/// checkForCollect sprawdza czy roslinka jest gotowa do zebrania.
		/// Collect() zbiera roslinke
		/// checkSick jesli zwrocilo false to ok a jesli true to znaczy ze jest chora
		/// SavePlant ratuje roslinke jesli jest chora
		/// 
		/// ///////////////////////////////////////////////////////////////////////////////////////////////

		// if plant
		// 		then
		// if not plan
		//	 	then posadź random plant
		// if state == true to znaczy ze cos rosnie

		if (transform.position == GoWhere) {  
			updatePriority ();
			// HERE WE MAKE TASKS FOR TRACTOR FOR DESTINATION POINT. FOR EXAMPLE PLANT RANDOM PLANT
			///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			if (!actualField.getState () && actualField.type.Equals ("PlantField")) {
				actualField.CheckDead ();
				// zmienilem na UnityEngine bo nie sie kłóciło z System.Random
				int los = UnityEngine.Random.Range (1, 4);
				if (los == 1)
					actualField.PlantIt ("Tulip");
				if (los == 2)
					actualField.PlantIt ("Wheat");
				if (los == 3)
					actualField.PlantIt ("Corn");
				if (los == 4)
					actualField.PlantIt ("Colza");

				if (!actualField.checkIrrigation ()) {
					actualField.Irrigate ();
				}
				if (actualField.checkMinerals () < 2) {
					if (actualField.checkMinerals () == 1)
						actualField.AddMinerals (1);
					else
						actualField.AddMinerals (2);
				}
			} else if (actualField.type.Equals ("PlantField")){
				if (actualField.checkForCollect ()) {
					actualField.priority = -10;
					actualField.MakeGrass ();
					actualField.Collect ();
					if (!actualField.checkIrrigation ()) {
						actualField.Irrigate ();
					}
					if (actualField.checkMinerals () < 2) {
						if (actualField.checkMinerals () == 1)
							actualField.AddMinerals (1);
						else
							actualField.AddMinerals (2);
					}
				} else {
					actualField.CheckDead ();
					if (!actualField.checkIrrigation ()) {
						actualField.Irrigate ();
					}
					if (actualField.checkMinerals () < 2) {
						if (actualField.checkMinerals () == 1)
							actualField.AddMinerals (1);
						else
							actualField.AddMinerals (2);
					}
					if (actualField.checkSick ()) {
						actualField.SavePlant ();
					}

				}
			}

			// jeżeli jest już jakas roślinka
			if (actualField.getState () && actualField.type.Equals ("PlantField")) {
				// sprawdzam mineraly i wode po dojechaniu na miejsce
				if (actualField.checkMinerals() < 1)
					actualField.AddMinerals (3);
				if (actualField.checkIrrigation() == false)
					actualField.Irrigate ();	

				// sprawdzamy jak się ma roślinka
				if (actualField.checkSick () == true)
					actualField.SavePlant ();
				
				if (actualField.checkForCollect () == true){
					actualField.Collect ();
					// zbieramy i sadzimy nową
					int los = UnityEngine.Random.Range (1, 4);
					if (los == 1)
						actualField.PlantIt("Tulip");
					if (los == 2)
						actualField.PlantIt("Wheat");
					if (los == 3)
						actualField.PlantIt("Corn");
					if (los == 4)
						actualField.PlantIt("Colza");
				}

			}

			/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


			// HERE WE MAKE NEXT DESTINATION POINT EXAMPLE : TargetPosition.transform.position = field[0,0].transform.position; //goes to field [0 0] field
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


			// kolejna pozycja z kolejki priorytetowej
			// zamiast field[10, 19] -> i oraz j tam gdzie priority jest max w PriorityTable
			// i chyba też SpeedTable
			//int [] prioritypoints = new int[2];
			//prioritypoints = updatePriorityPoint ();
			priorytetX = updatePriorityPoint()[0];
			priorytetY = updatePriorityPoint()[1];
			TargetPosition.transform.position = field[priorytetX, priorytetY].transform.position;
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		}

		transform.position = Vector3.MoveTowards (transform.position,GoWhere , step);

		timer -= Time.deltaTime;
		if (timer < 0) {
			//updatePriority ();
			timer = 1;
		}
	}

	public void changeField(int x, int y) {
		actualField = field [x,y]; //because we only can use functions Plant , Refillwater , Refilminerals, SavePlantWhenSick , Collect on actualField 
	}

	public void updatePriority(){
		for (int i = 0; i < 20; i++) {
			for (int j = 0; j < 20; j++) {
				PriorityTable [i, j] = field [i, j].priority;

			}
		}
	}

	//pozycja elementu z max priority w tablicy 20x20
	public int[] updatePriorityPoint(){
		// pozycja i, j elementu max
		int[] pozMax = new int[2];
		// max priorytet
		int maxP = -1;

		for (int i = 0; i < 20; i++) {
			for (int j = 0; j < 20; j++) {
				if (field [i, j].priority > maxP) {
					pozMax[0] = i;
					pozMax [1] = j;
					maxP = field [i, j].priority;
				}
			}
		}
		return pozMax;
	}
		

}