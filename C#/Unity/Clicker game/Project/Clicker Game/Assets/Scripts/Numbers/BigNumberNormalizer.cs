using UnityEngine;

public class BigNumberNormalizer : MonoBehaviour
{
    private static readonly string[] suffixes = { "", "K", "M", "B", "T", "Q", "Qt", "Sx" };

    public string Normalize(double number)
    {
        if (number < 1000)
        {
            return number.ToString("F2"); // Меньше 1000 - без сокращения
        }

        int suffixIndex = 0;
        while (number >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            number /= 1000;
            suffixIndex++;
        }
        //string format = $"F{decimalPlaces}";
        return number.ToString("F2") + suffixes[suffixIndex];
    }
}
