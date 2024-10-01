using System;
using System.Globalization;

public class FormatNumber
{
    // Formats a number to show up to three non-zero decimal places, rounding correctly
    public static string FormatToFirstThreeNonZeroDecimal(double num, bool threePointsAfterDecimal = false)
    {
        // Use a format string that provides up to 15 decimal places
        string numString = threePointsAfterDecimal ? num.ToString("0.###") : num.ToString("0.###################");
        
        int decimalIndex = numString.IndexOfAny(new[] { ',', '.' });
    
        // Handle case where there are no non-zero decimals after the decimal point
        if (decimalIndex == -1 || decimalIndex == numString.Length - 1)
        {
            return numString;
        }

        int firstNonZeroIndex = decimalIndex + 1;

        while (firstNonZeroIndex < numString.Length && numString[firstNonZeroIndex] == '0')
        {
            firstNonZeroIndex++;
        }

        // Keep up to three non-zero decimals after the first one
        int endIndex = firstNonZeroIndex + 2;

        if (endIndex >= numString.Length || numString[endIndex] == '0')
        {
            endIndex--;
        }

        // Round correctly based on the digit after the last decimal place
        if (endIndex + 1 < numString.Length && numString[endIndex + 1] >= '5')
        {
            // Round up if the next digit is 5 or more
            numString = Math.Round(num, 3).ToString("0.###");
        }

        return numString;
    }

    // Formats a number to show up to two non-zero decimal places
    public static string FormatToFirstTwoNonZeroDecimalAptos(double value)
    {
        // Multiply by 1e10 to handle cases with more than two decimal places
        double formattedValue = value * 1e10;

        // Convert to string and remove trailing zeros
        string stringValue = formattedValue.ToString(CultureInfo.InvariantCulture);
        stringValue = stringValue.TrimEnd('0');

        // Keep only up to two decimal places
        int decimalIndex = stringValue.IndexOf('.');
        if (decimalIndex != -1 && decimalIndex + 3 < stringValue.Length)
        {
            stringValue = stringValue.Substring(0, decimalIndex + 3);
        }

        return stringValue;
    }

    // Formats a number to show up to 10 decimal places, adding an ellipsis if more than 10 decimal places
    public static string FormatToMaxTenDecimalPlaces(double value)
    {
        // Use a format string that provides up to 15 decimal places
        string numString = value.ToString("0.###################");

        int decimalIndex = numString.IndexOfAny(new[] { ',', '.' });

        if (decimalIndex == -1 || decimalIndex + 11 > numString.Length)
        {
            return numString;
        }

        // Limit to 10 decimal places
        int endIndex = decimalIndex + 11;

        if (endIndex < numString.Length && numString[endIndex] >= '5')
        {
            // Round up if the next digit is 5 or more
            numString = Math.Round(value, 10).ToString("0.##########");
        }

        // Append ellipsis if the number has more than 10 decimal places
        if (endIndex < numString.Length)
        {
            return numString.Substring(0, decimalIndex + 11) + "...";
        }

        return numString;
    }
}
