using UnityEngine;
public abstract class Plant
{
	protected string name; //plant name
	protected bool state; //0 = dead , 1 = alive

	protected int HowMuchGrowed; //1 = small , 100 = need to be colected , 150 = overgroved

	protected int HowManyTimesLackOfWater; //0 = ok , more than 10 = plant is dead
	protected int HowManyTimesLackOfMinerals;
	protected int HowManyTimesSick; //how long is plant sick. if more than 10 plant is dead

	protected bool sick; // 0 if no , 1 if yes
	protected int chanceForSick; // % for sick

	public string getName()
	{
		return name;
	}

	public bool getState()
	{
		return state;
	}

	public int getHowMuchGrowed()
	{
		return HowMuchGrowed;
	}

	public bool checkSickness()
	{
		return sick;
	}

	public virtual bool Grow(bool irrigation, int minerals)
	{
		return false;
	}

	public void SavePlant()
	{
		HowManyTimesSick = 0;
		sick = false;
	}

	public int Collect()
	{
		state = false;

		return Random.Range(1, 3);
	}

	public void checkOvergroved()
	{
		if (HowMuchGrowed > 150)
			state = false;
	}

	public bool checkForCollect()
	{
		if (HowMuchGrowed >= 100)
			return true;
		else
			return false;
	}
}