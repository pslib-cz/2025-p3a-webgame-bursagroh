namespace game.Server.Services
{
    public class RandomSeedGenerator : IRandomSeedGenerator
    {
        private readonly Random _random;

        public RandomSeedGenerator(Random random)
        {
            _random = random;
        }

        public int RandomSeed()
        {
            return (int)_random.NextInt64(16);
        }
    }
}
