using NUnit.Framework;
using SafeVault.Security;

namespace SafeVault.Tests;

public class SecurityTests
{
    [Test]
    public void TestForXSS_RemovesAngleBrackets()
    {
        var input = "<script>alert('xss')</script>";
        var cleaned = InputSanitizer.Clean(input);

        Assert.IsFalse(cleaned.Contains("<"));
        Assert.IsFalse(cleaned.Contains(">"));
    }

    [Test]
    public void TestForXSS_DetectsScriptLikeInput()
    {
        var input = "<script>alert(1)</script>";
        Assert.IsTrue(InputSanitizer.LooksLikeScript(input));
    }

    [Test]
    public void TestForSQLInjection_PayloadDoesNotBreakSanitizer()
    {
        var payload = "' OR 1=1 --";
        var cleaned = InputSanitizer.Clean(payload);

        Assert.IsNotNull(cleaned);
        Assert.IsTrue(cleaned.Length > 0);
    }
}
