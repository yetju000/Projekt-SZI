﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using System;
using System.IO;
using Microsoft;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


public class Tractor : MonoBehaviour {

	int actualPositionX = 0;
	int actualPositionY = 7;
	public GameObject CrateRows; //Field Rows from Unity
	public GameObject TargetPosition; //Where tractor will be going
	Field [,] field; //Whole field
	Field actualField; //Active field
	int [,] PriorityTable;
	int [,] SpeedTable;
	public Grid grid; 
	float timer = 0.0f;
	public Pathfinding path;
	public float speed;
	public float step;
	Vector3 GoWhere;
	private bool learned = false;
	private string [,] plantsArrangement;
	// priorytet - wartosc pola
	int priorytetX = 0;
	int priorytetY = 0;
	public int numOfLearningIterations;

	Dictionary<string,int> yields = new Dictionary<string, int>();

	int predictionCount = 0;
	int predictionGood = 0;

	void initYields()
	{
		yields.Add("Tulip", 0);
		yields.Add("Colza", 0);
		yields.Add("Wheat", 0);
		yields.Add("Corn", 0);
	}

	void initField(Field[,] field)
	{
		for (int i = 1; i <= 20; i++)
		{
			for (int j = 1; j <= 20; j++)
			{
				field[i - 1, j - 1] = CrateRows.transform.Find("Row (" + i + ")").transform.Find("Crate (" + j + ")").GetComponent<Field>();   //do tablicy field[20][20] zapisujemy informacje o wszystkich polach 
			}
		}
	}

	void initSpeedTable(int [,] SpeedTable)
	{
		for (int i = 0; i < 20; i++)
		{
			for (int j = 0; j < 20; j++)
			{
				SpeedTable[i, j] = (int)field[i, j].FieldSpeed;
			}
		}
	}

	void FindFieldNeighbours(Field[,] wholeField, Field field, int fieldX, int fieldY)
	{
		//Prawo
		if (fieldX + 1 < 20)
		{
			field.addFieldNeigbour(wholeField[fieldX + 1, fieldY]);
		}
		//Dol
		if (fieldY + 1 < 20)
		{
			field.addFieldNeigbour(wholeField[fieldX, fieldY + 1]);
		}
		//Lewo
		if (fieldX - 1 >= 0)
		{
			field.addFieldNeigbour(wholeField[fieldX - 1, fieldY]);
		}
		//Gora
		if (fieldY - 1 >= 0)
		{
			field.addFieldNeigbour(wholeField[fieldX, fieldY - 1]);
		}
		//Lewo gora
		if (fieldX - 1 >= 0 && fieldY - 1 >= 0)
		{
			field.addFieldNeigbour(wholeField[fieldX - 1, fieldY - 1]);
		}
		//Lewo dol
		if (fieldX - 1 >= 0 && fieldY + 1 < 20)
		{
			field.addFieldNeigbour(wholeField[fieldX - 1, fieldY + 1]);
		}
		//Prawo gora
		if (fieldX + 1 < 20 && fieldY - 1 >= 0)
		{
			field.addFieldNeigbour(wholeField[fieldX + 1, fieldY - 1]);
		}
		//Prawo dol
		if (fieldX + 1 < 20 && fieldY + 1 < 20)
		{
			field.addFieldNeigbour(wholeField[fieldX + 1, fieldY + 1]);
		}
	}

	void findNeighboursOfEveryField(Field [,] field)
	{
		for (int i = 1; i <= 20; i++)
		{
			for (int j = 1; j <= 20; j++)
			{
				FindFieldNeighbours(field, field[i - 1, j - 1], i-1, j-1);
			}
		}
	}

	// Use this for initialization
	void Start()
	{
		GoWhere = new Vector3();
		PriorityTable = new int[20, 20];
		SpeedTable = new int[20, 20];
		field = new Field[20, 20];
		initYields();
		initField(field);
		initSpeedTable(SpeedTable);
		speed = field[actualPositionX, actualPositionY].FieldSpeed;
		step = speed * Time.deltaTime;
		findNeighboursOfEveryField(field);
		plantsArrangement = new string[20, 20];
	}

	// Update is called once per frame
	void Update()
	{
		if (learned == false) {
			PlantsArrangementLearner pa = new PlantsArrangementLearner(CrateRows);
			pa.learn(numOfLearningIterations);
			learned = true;
			plantsArrangement = pa.getPlantsArrangement();
		}

		speed = field[actualPositionX, actualPositionY].FieldSpeed;
		step = speed * Time.deltaTime;


		if (grid.FinalPath.Count > 0)
		{
			GoWhere = field[grid.FinalPath[0].iGridY, grid.FinalPath[0].iGridX].transform.position;
			changeField(grid.FinalPath[0].iGridY, grid.FinalPath[0].iGridX);
			actualPositionX = grid.FinalPath[0].iGridY;
			actualPositionY = grid.FinalPath[0].iGridX;
			if (transform.position.x < field[grid.FinalPath[0].iGridY, grid.FinalPath[0].iGridX].transform.position.x)
				transform.localEulerAngles = new Vector3(0, 90, 0);
			if (transform.position.x > field[grid.FinalPath[0].iGridY, grid.FinalPath[0].iGridX].transform.position.x)
				transform.localEulerAngles = new Vector3(0, -90, 0);
			if (transform.position.z < field[grid.FinalPath[0].iGridY, grid.FinalPath[0].iGridX].transform.position.z)
				transform.localEulerAngles = new Vector3(0, 0, 0);
			if (transform.position.z > field[grid.FinalPath[0].iGridY, grid.FinalPath[0].iGridX].transform.position.z)
				transform.localEulerAngles = new Vector3(0, 180, 0);
		}

		if (transform.position == GoWhere)
		{
			updatePriority();

			if (!actualField.getState() && actualField.type.Equals("PlantField"))
			{
				actualField.CheckDead();
				//plantRandomPlant();
				plantPlannedPlant();
				irrigateFieldIfDry();

			}else if (actualField.getState() && actualField.type.Equals("PlantField"))
			{
				if (actualField.checkForCollect())
				{
					actualField.priority = -10;
					actualField.MakeGrass();
					string yieldName = actualField.plant.getName();
					int fieldYield = actualField.Collect(field);
					yields[yieldName] += fieldYield;
					calculateProfit();
					irrigateFieldIfDry();
					addMineralsToFieldIfNotEnough();
					Debug.Log("(" + actualPositionX + "," + actualPositionY + "): " + "Collecting " + fieldYield + " " + yieldName);
					if (!actualField.predistion.Equals(""))
					{
						if (actualField.predistion.Contains(actualField.plant.getName()))
						{
							predictionCount++;
							predictionGood++;
							Debug.Log("Predicted plant: " + actualField.predistion + "             .Real plant:" + actualField.plant.getName() + " OK");
						}
						else {
							predictionCount++;
							Debug.Log("Predicted plant: " + actualField.predistion + "             .Real plant:" + actualField.plant.getName() + " BAD PREDICTION");
						}
					}
					Debug.Log("accuracy : " + predictionGood + "/" + predictionCount);
				}
				if (!actualField.checkForCollect() && !actualField.type.Equals(null))
				{
					actualField.CheckDead();
					irrigateFieldIfDry();
					addMineralsToFieldIfNotEnough();
					savePlantIfSick();
				}
			}

			if (actualField.getState() && actualField.type.Equals("PlantField"))
			{
				addMineralsToFieldIfNotEnough();
				irrigateFieldIfDry();
				savePlantIfSick();

				if (actualField.checkForCollect() == true)
				{
					string yieldName = actualField.plant.getName();
					int fieldYield = actualField.Collect(field);
					yields[yieldName] += fieldYield;
					calculateProfit();
					plantPlannedPlant();
					//plantRandomPlant();
				}

			}

			priorytetX = updatePriorityPoint()[0];
			priorytetY = updatePriorityPoint()[1];
			TargetPosition.transform.position = field[priorytetX, priorytetY].transform.position;
		}

		transform.position = Vector3.MoveTowards(transform.position, GoWhere, step);

		timer -= Time.deltaTime;
		if (timer < 0)
		{
			//updatePriority ();
			timer = 1;
		}

	}

	void calculateProfit()
	{
		int profit = 0;
		foreach (KeyValuePair<string, int> entry in yields)
		{
			switch (entry.Key)
			{
			case "Tulip":
				profit += entry.Value * 50;
				break;
			case "Colza":
				profit += entry.Value * 25;
				break;
			case "Wheat":
				profit += entry.Value * 25;
				break;
			case "Corn":
				profit += entry.Value * 10;
				break;
			}
		}
		Debug.Log("Profit:" + profit);
	}

	private void irrigateFieldIfDry() {
		if (!actualField.checkIrrigation()) {
			actualField.Irrigate();
			Debug.Log("(" + actualPositionX + "," + actualPositionY + "): Irrigating");
			if (!actualField.predistion.Equals(""))
			{
				if (actualField.predistion.Contains(actualField.plant.getName()))
				{
					predictionCount++;
					predictionGood++;
					Debug.Log("Predicted plant: " + actualField.predistion + "    .Real plant:" + actualField.plant.getName() + " OK");
				}
				else {
					Debug.Log("Predicted plant: " + actualField.predistion + "    .Real plant:" + actualField.plant.getName() + " BAD PREDICTION");
					predictionCount++;
				}
			}
			Debug.Log("accuracy : " + predictionGood + "/" + predictionCount);
		}

	}


	private void addMineralsToFieldIfNotEnough() {
		int mineralsAddedAmount=0;
		int fieldMinerals = actualField.checkMinerals();
		if (fieldMinerals < 3) {
			switch (fieldMinerals) {
			case 0:
				mineralsAddedAmount = 3;
				break;
			case 1:
				mineralsAddedAmount = 2;
				break;
			case 2:
				mineralsAddedAmount = 1;
				break;
			}
			actualField.AddMinerals(mineralsAddedAmount);
			Debug.Log("(" + actualPositionX + "," + actualPositionY + "): " + "Adding " + mineralsAddedAmount + " minerals");
			if (!actualField.predistion.Equals(""))
			{
				if (actualField.predistion.Contains(actualField.plant.getName()))
				{
					predictionCount++;
					predictionGood++;
					Debug.Log("Predicted plant: " + actualField.predistion + "       .Real plant:" + actualField.plant.getName() + " OK");
				}
				else {
					predictionCount++;
					Debug.Log("Predicted plant: " + actualField.predistion + "       .Real plant:" + actualField.plant.getName() + " BAD PREDICTION");
				}
			}
			Debug.Log("accuracy : " + predictionGood + "/" + predictionCount);
		}

	}

	private void savePlantIfSick() {
		if (actualField.checkSick() == true) {
			actualField.SavePlant();
			Debug.Log("(" + actualPositionX + "," + actualPositionY + "): " + "Saving ");
			if (!actualField.predistion.Equals(""))
			{
				if (actualField.predistion.Contains(actualField.plant.getName()))
				{
					predictionCount++;
					predictionGood++;
					Debug.Log("Predicted plant: " + actualField.predistion + "          .Real plant:" + actualField.plant.getName() + " OK");
				}
				else {
					predictionCount++;
					Debug.Log("Predicted plant: " + actualField.predistion + "          .Real plant:" + actualField.plant.getName() + " BAD PREDICTION");
				}
			}
			Debug.Log("accuracy : " + predictionGood + "/" + predictionCount);
		}
	}

	private void plantRandomPlant() {
		// zmienilem na UnityEngine bo nie sie kłóciło z System.Random
		int los = UnityEngine.Random.Range(1, 5);
		String plantName = "";
		if (los == 1)
			plantName = "Tulip";
		if (los == 2)
			plantName = "Wheat";
		if (los == 3)
			plantName = "Corn";
		if (los == 4)
			plantName = "Colza";

		actualField.PlantIt(plantName);
		Debug.Log("(" + actualPositionX + ", " + actualPositionY + "): Planting " + plantName);
	}

	private void plantPlannedPlant() {
		string plantName = plantsArrangement[actualPositionX, actualPositionY];
		actualField.PlantIt(plantName);
		Debug.Log("(" + actualPositionX + ", " + actualPositionY + "): Planting " + plantName);
	}

	public void changeField(int x, int y) {
		actualField = field [x,y];
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