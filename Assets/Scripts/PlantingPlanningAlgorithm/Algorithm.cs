using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm : MonoBehaviour{

    /*    
    public GameObject CrateRows;
    List<Field[,]> population = new List<Field[,]>();
    Field[,] populationIndividual = new Field[20, 20];
   

    private string plantRandomPlant()
    {
        // zmienilem na UnityEngine bo nie sie kłóciło z System.Random
        int los = UnityEngine.Random.Range(1, 4);
        string plantName = null; //Napewno nie zwróci null
        if (los == 1)
            plantName = "Tulipan";
        if (los == 2)
            plantName = "Pszenica";
        if (los == 3)
            plantName = "Kukurydza";
        if (los == 4)
            plantName = "Rzepak";

        return plantName;
    }

    void generatePopulationIndividual()
    {
        for (int i = 1; i <= 20; i++)
        {
            for (int j = 1; j <= 20; j++)
            {
                populationIndividual[i - 1, j - 1] = CrateRows.transform.Find("Row (" + i + ")").transform.Find("Crate (" + j + ")").GetComponent<Field>();   //do tablicy field[20][20] zapisujemy informacje o wszystkich polach 
            }
        }


        for (int i = 1; i <= 20; i++)
        {
            for (int j = 1; j <= 20; j++)
            {
                if (populationIndividual[i - 1, j - 1].name.Equals("PlantField"))
                {
                    populationIndividual[i - 1, j - 1].PlantIt(plantRandomPlant());
                }
            }
        }
    }

    void generateInitialPopulation()
    {
        for (int g = 0; g < 50; g++)
        {
            generatePopulationIndividual();
            population.Add(populationIndividual);
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

    public int getPotentialYield(Field[,] wholeField, Field field)
    {
        int howmany = field.plant.Collect();

        if (field.plant.getName().Equals("Tulipan"))
        {
            foreach (var f in field.fieldNeighbours)
            {
                if (f.plant.getName().Equals("Tulipan"))
                {
                    return 1;
                }
            }
            return howmany;
        }

        if (field.lastPlantedPlant.Equals(field.plant.getName()))
        {
            field.lastPlantedPlant = field.plant.getName();
            return 1;
        }
        else if (field.checkIfFieldIsCloseToMud(field))
        {
            field.lastPlantedPlant = field.plant.getName();
            if (howmany >= 2)
                return 2;
            else
                return howmany;
        }
        else
        {
            field.lastPlantedPlant = field.plant.getName();
            return howmany;
        }
    }

    int calculatePotentialProfit(Field[,] populationIndividual) //Fitness function
    {
        int potentialProfit = 0;
        for (int i = 1; i <= 20; i++)
        {
            for (int j = 1; j <= 20; j++)
            {
                FindFieldNeighbours(populationIndividual, populationIndividual[i - 1, j - 1], i - 1, j - 1);
            }
        }
        
        foreach (Field f in populationIndividual)
        {
            int potentialFieldYield = 0;
            if (f.type.Equals("PlantField"))
            {
                potentialFieldYield = getPotentialYield(populationIndividual, f);

                switch (f.plant.getName())
                {
                    case "Tulipan":
                        potentialProfit += potentialFieldYield * 50;
                        break;
                    case "Rzepak":
                        potentialProfit += potentialFieldYield * 25;
                        break;
                    case "Pszenica":
                        potentialProfit += potentialFieldYield * 25;
                        break;
                    case "Kukurydza":
                        potentialProfit += potentialFieldYield * 10;
                        break;
                }
                potentialFieldYield = 0;
            }
        }
        return potentialProfit;
    }

    public void learn()
    {
        generateInitialPopulation();
        Dictionary<Field[,], int> populationYields = new Dictionary<Field[,], int>();

        int i = 0;
        if (population.Count != 0) {
            foreach (Field[,] populationIndividual in population)
            {
                i++;
                Debug.Log(i);
                int populationProfit = calculatePotentialProfit(populationIndividual);
                populationYields[populationIndividual] = populationProfit;
                Debug.Log(i + ": " + populationProfit);
            }
        }

    }

    void Main()
    { 
        learn();
    }
    */
    public GameObject CrateRows;
    string[,] clearField = new string[20, 20];

    public Algorithm()
    {
        for (int i = 1; i <= 20; i++)
        {
            for (int j = 1; j <= 20; j++)
            {
                clearField[i - 1, j - 1] = "Ala";// CrateRows.transform.Find("Row (" + i + ")").transform.Find("Crate (" + j + ")").GetComponent<Field>().type;   //do tablicy field[20][20] zapisujemy informacje o wszystkich polach 
            }
        }
    }

    string fieldToStr(string [,] field)
    {
        string fieldStr="";
        for (int i = 1; i <= 20; i++)
        {
            for (int j = 1; j <= 20; j++)
            {
                fieldStr.Insert(fieldStr.Length, field[i-1,j-1] + " ");
            }
            fieldStr.Insert(fieldStr.Length, "\n");
        }
        return fieldStr;
    }

    void Main()
    {  
        Debug.Log(fieldToStr(clearField));
    }

}
