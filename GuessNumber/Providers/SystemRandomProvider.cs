using GuessNumber.Interfaces;

namespace GuessNumber.Providers
{
    // Implementação da interface de geração de números aleatórios usando System.Random
    public class SystemRandomProvider : IRandomNumberProvider
    {

        private static readonly Random _random = new Random();
        public int Next(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
    }
}