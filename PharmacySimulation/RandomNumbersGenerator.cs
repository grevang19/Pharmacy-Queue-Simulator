using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpCity.Tools.RandomNumberGenerators;

namespace PharmacySimulation
{
    public class RandomNumbersGenerator
    {
        //private Random random;
        private UniformRandomGenerator arrivalTimeUniformRandom;
        private NormalRandomGenerator regularDrugsServiceTimeNormalRandom;
        private NormalRandomGenerator hardDrugsServiceTimeNormalRandom;
        private NormalRandomGenerator typeOfRecipeNormalRandom;

        public RandomNumbersGenerator()
        {
            //random = new Random();
            arrivalTimeUniformRandom = new UniformRandomGenerator();
            regularDrugsServiceTimeNormalRandom = new NormalRandomGenerator(600, 1300);
            regularDrugsServiceTimeNormalRandom.Mean = 900;
            hardDrugsServiceTimeNormalRandom = new NormalRandomGenerator(900, 1830);
            hardDrugsServiceTimeNormalRandom.Mean = 1400;
            typeOfRecipeNormalRandom = new NormalRandomGenerator(0,10);
            typeOfRecipeNormalRandom.Mean = 3;
        }

        //public int getRandomValue(int maxValue)
        //{
        //    return random.Next(maxValue)+1;
        //}

        public int getRandomArrivalTime()
        {
            return arrivalTimeUniformRandom.Next(0, 230);
        }

        private int getRandomRegularDrugs()
        {
            return regularDrugsServiceTimeNormalRandom.Next();
        }

        private int getRandomHardDrugs()
        {
            return hardDrugsServiceTimeNormalRandom.Next();
        }

        public int getRandomServiceTime(int tipeOfRecipes)
        {
            
            if (tipeOfRecipes == 0)
            {
                return getRandomRegularDrugs();
            }
            else
            {
                return getRandomHardDrugs();
            }
        }

        public int getRandomTypeOfRecipe()
        {
            if (typeOfRecipeNormalRandom.Next() < 5)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
