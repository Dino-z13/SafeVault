using System.Text.RegularExpressions;

namespace SafeVault.Security;

public static class InputSanitizer
{
    public static string Clean(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "";

        var trimmed = input.Trim();

        trimmed = trimmed.Replace("<", "")
                         .Replace(">", "")
                         .Replace("\"", "")
                         .Replace("'", "");

        trimmed = Regex.Replace(trimmed, @"\s+", " ");
        return trimmed;
    }

    public static bool LooksLikeScript(string input)
    {
        var lowered = input.ToLower();
        return lowered.Contains("script") || lowered.Contains("onerror") || lowered.Contains("onload");
    }

    public static bool IsValidEmail(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        return Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}
