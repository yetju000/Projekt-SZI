using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlantsArrangementLearner {
    string[,] clearField = new string[20, 20];
	string[,] mostProfitableField = new string[20, 20];

	public string[,] getPlantsArrangement() {
		return mostProfitableField;
	}

	public PlantsArrangementLearner(GameObject CrateRows)
    {
		buildClearField(CrateRows);
    }

	private void buildClearField(GameObject CrateRows) {
		for (int i = 1; i <= 20; i++) {
			for (int j = 1; j <= 20; j++) {
				clearField[i - 1, j - 1] = CrateRows.transform.Find("Row (" + i + ")").transform.Find("Crate (" + j + ")").GetComponent<Field>().type;   //do tablicy field[20][20] zapisujemy informacje o wszystkich polach 
			}
		}
	}

	public void learn(int numOfIterations) {
		int numOfIterationsDone = 0;
		List<string[,]> population = generateInitialPopulation();
		//printPopulation(population);
		Dictionary<int, string[,]> populationProfits = calculateProfitForEachPopulationIndividualIn(population);
		while (numOfIterationsDone < numOfIterations) {
			numOfIterationsDone++;
			populationProfits = populationProfits.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
			//printPopulationProfits(populationProfits);
			crossbreedBestPopulationIndividuals(20,populationProfits);
			populationProfits = populationProfits.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
			mostProfitableField = populationProfits.Values.First();
		}
		//printPopulationProfits(populationProfits);
		Debug.Log(calculatePopulationIndividualProfit(mostProfitableField));
		Debug.Log(fieldToStr(mostProfitableField));
	}

	List<string[,]> generateInitialPopulation() {
		List<string[,]> population = new List<string[,]>();
		for (int g = 0; g < 50; g++) {
			string[,] populationIndividual = generatePopulationIndividual();
			population.Add(populationIndividual);
		}
		return population;
	}


	string[,] generatePopulationIndividual() {
		string[,] populationIndividual = (string[,])clearField.Clone();
		for (int i = 0; i <= 19; i++) {
			for (int j = 0; j <= 19; j++) {
				if (populationIndividual[i, j].Equals("PlantField")) {
					populationIndividual[i, j] = getRandomPlant();
				}
			}
		}
		return populationIndividual;
	}

	string getRandomPlant() {
		int los = UnityEngine.Random.Range(1, 5);
		string randomPlant = "";
		if (los == 1)
			randomPlant = "Tulip";
		if (los == 2)
			randomPlant = "Wheat";
		if (los == 3)
			randomPlant = "Corn";
		if (los == 4)
			randomPlant = "Colza";

		return randomPlant;
	}

	void printPopulation(List<string[,]> population) {
		int i = 0;
		foreach (string[,] populationIndividual in population) {
			i++;
			Debug.Log(i + "\n" + fieldToStr(populationIndividual));
		}
	}

	string fieldToStr(string[,] field) {
		string fieldStr = "";
		for (int i = 1; i <= 20; i++) {
			for (int j = 1; j <= 20; j++) {
				fieldStr += field[i - 1, j - 1] + " ";
			}
			fieldStr += "\n";
		}
		return fieldStr;
	}

	Dictionary<int, string[,]> calculateProfitForEachPopulationIndividualIn(List<string[,]> population) {
		Dictionary<int, string[,]> populationProfit = new Dictionary<int, string[,]>();
		foreach (string[,] populationIndividual in population) {
			int populationIndividualProfit = calculatePopulationIndividualProfit(populationIndividual);
			populationProfit[populationIndividualProfit] = populationIndividual;
		}
		return populationProfit;
	}

	int calculatePopulationIndividualProfit(string[,] populationIndividual) { //fitness function
		int populationProfit = 0;
		int oneTulipProfit = 50;
		int oneColzaProfit = 25;
		int oneWheatProfit = 25;
		int oneCornProfit = 10;

		for (int i = 0; i <= 19; i++) {
			for (int j = 0; j <= 19; j++) {
				if ((!populationIndividual[i, j].Equals("PathField")) && (!populationIndividual[i, j].Equals("MudField"))) {
					if (populationIndividual[i, j].Equals("Tulip")) {
						int tulipYield = calcTulipYield(populationIndividual, i, j);
						populationProfit += tulipYield * oneTulipProfit;
					}

					if (populationIndividual[i, j].Equals("Colza")) {
						int colzaYield = calcColzaYield(populationIndividual, i, j);
						populationProfit += colzaYield * oneColzaProfit;
					}

					if (populationIndividual[i, j].Equals("Wheat")) {
						int wheatYield = calcWheatYield(populationIndividual, i, j);
						populationProfit += wheatYield * oneWheatProfit;
					}

					if (populationIndividual[i, j].Equals("Corn")) {
						int cornYield = calcWheatYield(populationIndividual, i, j);
						populationProfit += cornYield * oneCornProfit;
					}
				}
			}
		}

		return populationProfit;
	}

	int calcTulipYield(string[,] wholeField, int fieldX, int fieldY) {
		int tulipYield = 0;

		bool tulipsYieldCondition = checkPlantNeighborhoodFor("Tulip", wholeField, fieldX, fieldY);
		if (tulipsYieldCondition == true) {
			tulipYield = 1;
		}
		else {
			tulipYield = 2;
		}

		return tulipYield;
	}

	int calcColzaYield(string[,] wholeField, int fieldX, int fieldY) {
		int colzaYield = 0;

		bool colzaMudFieldCondition = checkPlantNeighborhoodFor("MudField", wholeField, fieldX, fieldY);
		bool colzaTulipCondition = checkPlantNeighborhoodFor("Tulip", wholeField, fieldX, fieldY);

		if (colzaMudFieldCondition && colzaTulipCondition) {
			colzaYield = 1;
		}
		else if (colzaMudFieldCondition || colzaTulipCondition) {
			colzaYield = 2;
		}
		else {
			colzaYield = 3;
		}

		return colzaYield;
	}

	int calcWheatYield(string[,] wholeField, int fieldX, int fieldY) {
		int wheatYield = 0;

		bool wheatMudFieldCondition = checkPlantNeighborhoodFor("MudField", wholeField, fieldX, fieldY);
		bool wheatTulipCondition = checkPlantNeighborhoodFor("Tulip", wholeField, fieldX, fieldY);

		if (wheatMudFieldCondition && wheatTulipCondition) {
			wheatYield = 1;
		}
		else {
			wheatYield = 3;
		}

		return wheatYield;
	}

	int calcCornYield(string[,] wholeField, int fieldX, int fieldY) {
		int cornYield = 0;

		bool cornPathFieldCondition = checkPlantNeighborhoodFor("PathField", wholeField, fieldX, fieldY);

		if (cornPathFieldCondition) {
			cornYield = 3;
		}
		else {
			cornYield = 5;
		}

		return cornYield;
	}

	bool checkPlantNeighborhoodFor(string fieldType, string[,] wholeField, int fieldX, int fieldY) {
		bool result = false;
		//Prawo
		if (fieldX + 1 < 20) {
			bool checkRes = checkIfFieldIs(fieldType, wholeField, fieldX + 1, fieldY);
			if (checkRes) {
				result = true;
				return result;
			}
		}
		//Dol
		if (fieldY + 1 < 20) {
			bool checkRes = checkIfFieldIs(fieldType, wholeField, fieldX + 1, fieldY);
			if (checkRes) {
				result = true;
				return result;
			}
		}
		//Lewo
		if (fieldX - 1 >= 0) {
			bool checkRes = checkIfFieldIs(fieldType, wholeField, fieldX + 1, fieldY);
			if (checkRes) {
				result = true;
				return result;
			}
		}
		//Gora
		if (fieldY - 1 >= 0) {
			bool checkRes = checkIfFieldIs(fieldType, wholeField, fieldX + 1, fieldY);
			if (checkRes) {
				result = true;
				return result;
			}
		}
		//Lewo gora
		if (fieldX - 1 >= 0 && fieldY - 1 >= 0) {
			bool checkRes = checkIfFieldIs(fieldType, wholeField, fieldX + 1, fieldY);
			if (checkRes) {
				result = true;
				return result;
			}
		}
		//Lewo dol
		if (fieldX - 1 >= 0 && fieldY + 1 < 20) {
			bool checkRes = checkIfFieldIs(fieldType, wholeField, fieldX + 1, fieldY);
			if (checkRes) {
				result = true;
				return result;
			}
		}
		//Prawo gora
		if (fieldX + 1 < 20 && fieldY - 1 >= 0) {
			bool checkRes = checkIfFieldIs(fieldType, wholeField, fieldX + 1, fieldY);
			if (checkRes) {
				result = true;
				return result;
			}
		}
		//Prawo dol
		if (fieldX + 1 < 20 && fieldY + 1 < 20) {
			bool checkRes = checkIfFieldIs(fieldType, wholeField, fieldX + 1, fieldY);
			if (checkRes) {
				result = true;
				return result;
			}
		}
		return result;
	}

	bool checkIfFieldIs(string field, string[,] wholeField, int fieldX, int fieldY) {
		if (wholeField[fieldX, fieldY].Equals(field))
			return true;
		else
			return false;
	}

	void printPopulationProfits(Dictionary<int, string[,]> populationProfits) {
		foreach (KeyValuePair<int, string[,]> entry in populationProfits) {
			Debug.Log(fieldToStr(entry.Value));
			Debug.Log(entry.Key);
		}
	}

	void crossbreedBestPopulationIndividuals(int numOfPopulationIndividualsToCross, Dictionary<int, string[,]> populationProfits) {
		List<string[,]> populationIndividualsToCrossbreed = getPopulationIndividualsToCrossbreed(numOfPopulationIndividualsToCross,populationProfits);

		for (int i = 0; i < numOfPopulationIndividualsToCross/2; i++) {
			Pair<int, int> fieldCuttingPoint = getRandomPoint();
			string [,] individual1 = populationIndividualsToCrossbreed.ElementAt(19-i);
			string [,] individual2 = populationIndividualsToCrossbreed.ElementAt(i);
			string[,] bestCrossbreedChild = crossbreedTwoIndiviudals(individual1, individual2, fieldCuttingPoint);
			int bestCrossbreedChildProfit = calculatePopulationIndividualProfit(bestCrossbreedChild);
			if (populationProfits.ContainsKey(bestCrossbreedChildProfit))
				i--;
			else {
				populationProfits.Remove(populationProfits.Keys.Last());
				populationProfits.Add(bestCrossbreedChildProfit, bestCrossbreedChild);
			}
		}
	}

	List<string[,]> getPopulationIndividualsToCrossbreed(int numOfPopulationIndividualsToCross, Dictionary<int, string[,]> populationProfits) {
		List<string[,]> populationIndividualsToCrossbreed = new List<string[,]>();
		int i = 0;
		foreach (var pair in populationProfits) {
			if (i == 20)
				break;
			populationIndividualsToCrossbreed.Add(pair.Value);
			i++;
		}
		return populationIndividualsToCrossbreed;
	}

	Pair<int, int> getRandomPoint() {
		int randomNumber1 = UnityEngine.Random.Range(0, 19);
		int randomNumber2 = UnityEngine.Random.Range(0, 19);
		return new Pair<int, int>(randomNumber1, randomNumber2);
	}

	string[,] crossbreedTwoIndiviudals(string[,] individual1, string[,] individual2, Pair<int,int> cuttingPoint) {
		string[,] individual1Rectangle = getFieldRectangle(individual1, cuttingPoint);
		string[,] individual1Rest = getFieldRest(individual1, cuttingPoint);
		string[,] individual2Rectangle = getFieldRectangle(individual2, cuttingPoint);
		string[,] individual2Rest = getFieldRest(individual2, cuttingPoint);

		string[,] child1 = combineTwoFieldPieces(individual1Rectangle, individual2Rest,cuttingPoint);
		string[,] child2 = combineTwoFieldPieces(individual2Rectangle, individual1Rest,cuttingPoint);

		child1 = mutateWithProb(5, child1);
		child2 = mutateWithProb(5, child2);

		Dictionary<int, string[,]> crossbreedChilds = new Dictionary<int, string[,]>();
		int child1Profit = calculatePopulationIndividualProfit(child1);
		int child2Profit = calculatePopulationIndividualProfit(child2);
		/*
		Debug.Log(cuttingPoint.getX() + " " + cuttingPoint.getY());
		Debug.Log(fieldToStr(individual1));
		Debug.Log(fieldToStr(individual2));
		Debug.Log(child1Profit);
		Debug.Log(fieldToStr(child1));
		Debug.Log(child2Profit);
		Debug.Log(fieldToStr(child2));
		Debug.Log("=========================================================================");
		*/

		if (child1Profit == child2Profit) {
			return child1;
		}

		crossbreedChilds.Add(child1Profit, child1);
		crossbreedChilds.Add(child2Profit, child2);
		crossbreedChilds = crossbreedChilds.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
		return crossbreedChilds.First().Value;
	}

	string[,] getFieldRectangle(string[,] field, Pair<int, int> cuttingPoint) {
		int pointX = cuttingPoint.getX();
		int pointY = cuttingPoint.getY();
		string[,] fieldRectangle = new string[pointX + 1, pointY + 1];
		for (int i = 0; i <= pointX; i++) {
			for (int j = 0; j <= pointY; j++) {
				fieldRectangle[i, j] = field[i, j];
			}
		}
		return fieldRectangle;
	}

	string[,] getFieldRest(string[,] field, Pair<int, int> cuttingPoint) {
		int pointX = cuttingPoint.getX();
		int pointY = cuttingPoint.getY();
		string[,] fieldRest = (string[,])field.Clone(); ;

		for (int i = 0; i <= pointX; i++) {
			for (int j = 0; j <= pointY; j++) {
				fieldRest[i, j] = "toOverwrite";
			}
		}

		return fieldRest;
	}

	string[,] combineTwoFieldPieces(string[,] rectangle, string[,] rest, Pair<int, int> cuttingPoint) {
		string[,] child = rest;
		int cuttingPointX = cuttingPoint.getX();
		int cuttingPointY = cuttingPoint.getY();
		for (int i = 0; i <= cuttingPointX; i++) {
			for (int j = 0; j <= cuttingPointY; j++) {
				child[i, j] = rectangle[i, j];
			}
		}
		return child;
	}

	string [,] mutateWithProb(int chanceForMutation, string[,] child) {
		string[,] mutatedChild = (string [,])child.Clone();
		if (UnityEngine.Random.Range(1, 100) < chanceForMutation) {
			Pair<int, int> mutationPoint = getRandomPoint();
			int mutationPointX = mutationPoint.getX();
			int mutationPointY = mutationPoint.getY();
			while (!((mutatedChild[mutationPointX, mutationPointY] != "PathField") && (mutatedChild[mutationPointX, mutationPointY] != "MudField"))) {
				mutationPoint = getRandomPoint();
				mutationPointX = mutationPoint.getX();
				mutationPointY = mutationPoint.getY();
			}
			mutatedChild[mutationPointX, mutationPointY] = getRandomPlant();
		}
		return mutatedChild;
	}

}
