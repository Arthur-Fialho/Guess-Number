namespace GuessNumber.Interfaces
{
    public interface IRandomNumberProvider
    {
        // Interface para geração de números aleatórios
        int Next(int minValue, int maxValue);
    }
}