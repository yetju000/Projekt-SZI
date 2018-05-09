using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tractor : MonoBehaviour {


	int actualPositionX = 0; //actual position of tractor
	int actualPositionY = 7;
	public GameObject Crate;
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
	// Use this for initialization
	void Start () {
		GoWhere = new Vector3();
		PriorityTable = new int [20,20];
		SpeedTable = new int[20,20];

		//field = new Field[20][20];
		field = new Field[20,20];
		for (int i = 1; i <= 20; i++) {
			for (int j = 1; j <= 20; j++) {
				field[i-1,j-1] = Crate.transform.Find ("Row ("+i+")").transform.Find ("Crate ("+j+")").GetComponent<Field> ();   //do tablicy field[20][20] zapisujemy informacje o wszystkich polach 
			}
		}

		for (int i = 0; i < 20; i++) {
			for (int j = 0; j < 20; j++) {
				SpeedTable [i, j] = (int)field [i, j].fieldSpeed;
			}
		}

		speed = field[actualPositionX,actualPositionY].fieldSpeed;
		step= speed * Time.deltaTime;
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





		////////LYSY TYLKO W TYM IFIE OGARNIAJ. JAK CHCESZ COS ZROBIC NA POLU DO KTOREG DOJEDZIESZ TO ROBISZ actualField.akcja    :D akcja moze byc : 
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
		if (transform.position == GoWhere) {  
			// HERE WE MAKE TASKS FOR TRACTOR FOR DESTINATION POINT. FOR EXAMPLE PLANT RANDOM PLANT
			///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			if (!actualField.getState() && actualField.type.Equals("PlantField")) {
				int los=	Random.Range (1, 4);
				if (los == 1)
					actualField.PlantIt("Tulip");
				if (los == 2)
					actualField.PlantIt("Wheat");
				if (los == 3)
					actualField.PlantIt("Corn");
				if (los == 4)
					actualField.PlantIt("Colza");
			}
			/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


			// HERE WE MAKE NEXT DESTINATION POINT EXAMPLE : TargetPosition.transform.position = field[0,0].transform.position; //goes to field [0 0] field
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			TargetPosition.transform.position = field[10,19].transform.position;
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		}













		transform.position = Vector3.MoveTowards (transform.position,GoWhere , step);


		timer -= Time.deltaTime;
		if (timer < 0) {
			updatePriority ();
			timer = 3;

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

