namespace Core;

using System.Text.RegularExpressions;

public static class Helpers
{
    public static string ValidateRegNr(string input)
    {
        if (!Regex.IsMatch(input, @"^[a-zA-Z]{3}\d{3}$"))
        {
            throw new ArgumentException(
                "RegNr must be in the format of three letters a-z and three numbers 0-9.");
        }

        return input.ToUpper();
    }
}