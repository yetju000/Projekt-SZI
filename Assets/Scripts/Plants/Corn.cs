using UnityEngine;
public class Corn : Plant
{

	public Corn()
	{
		name = "Kukurydza";
		state = true;
		HowMuchGrowed = 0;
		sick = false;
		HowManyTimesSick = 0;
		chanceForSick = 5;
		HowManyTimesLackOfMinerals = 0;
	}

	public override bool Grow(bool irrigation, int minerals)
	{
		if (HowManyTimesLackOfWater > 10 || HowManyTimesSick > 10 || HowManyTimesLackOfMinerals > 10)
			state = false;

		if (minerals > 0)
			HowManyTimesLackOfMinerals = 0;

		if (minerals == 0)
			HowManyTimesLackOfMinerals += 1;

		if (irrigation)
		{
			HowManyTimesLackOfWater = 0;
			HowMuchGrowed += Random.Range(3, 5) + minerals;
		}
		if (!irrigation)
			HowManyTimesLackOfWater += 1;

		if (!sick)
		{
			HowManyTimesSick = 0;
			if (Random.Range(1, 100) < chanceForSick)
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
