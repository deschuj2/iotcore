namespace ifm.IoTCore.Common.UnitTests;

using NUnit.Framework;

[TestFixture]
public class ElementAddressTests
{
    [Test]
    public void ValidateIdentifiers()
    {
        Assert.That(ElementAddress.ValidateIdentifier("0123456789-abcdefghijklmnopqrstuvwxyz-ABCDEFGHIJKLMNOPQRSTUVWXYZ-[]"));
        
        Assert.That(ElementAddress.ValidateIdentifier(null),Is.False);
        Assert.That(ElementAddress.ValidateIdentifier(string.Empty),Is.False);
        Assert.That(ElementAddress.ValidateIdentifier("ÖÄÜ"),Is.False);
        Assert.That(ElementAddress.ValidateIdentifier("hu/hu"),Is.False);
        Assert.That(ElementAddress.ValidateIdentifier("hu\\hu"),Is.False);
        Assert.That(ElementAddress.ValidateIdentifier("hu hu"),Is.False);
    }

    [Test]
    public void ReplaceInvalidIdentifierCharacters()
    {
        Assert.That(ElementAddress.ValidateIdentifier(ElementAddress.ReplaceInvalidCharacters("ÖÄÜ")));
        Assert.That(ElementAddress.ValidateIdentifier(ElementAddress.ReplaceInvalidCharacters("hu/hu")));
        Assert.That(ElementAddress.ValidateIdentifier(ElementAddress.ReplaceInvalidCharacters("hu\\hu")));
        Assert.That(ElementAddress.ValidateIdentifier(ElementAddress.ReplaceInvalidCharacters("hu hu")));
    }

    [Test]
    public void CreateAddress()
    {
        Assert.That(ElementAddress.Create(null, "dev") == "dev");
        Assert.That(ElementAddress.Create(string.Empty, "dev") == "dev");
        Assert.That(ElementAddress.Create("dev", "el1") == "dev/el1");
        Assert.That(ElementAddress.Create("dev/el1", "el2") == "dev/el1/el2");
    }

    [Test]
    public void ValidateAddress()
    {
        Assert.That(ElementAddress.ValidateAddress(string.Empty));
        Assert.That(ElementAddress.ValidateAddress("dev"));
        Assert.That(ElementAddress.ValidateAddress("/el1"));
        Assert.That(ElementAddress.ValidateAddress("dev/el1"));
        Assert.That(ElementAddress.ValidateAddress("dev/el1/el2"));
        Assert.That(ElementAddress.ValidateAddress("dev/el1/el2/el3"));

        Assert.That(ElementAddress.ValidateAddress(null),Is.False);
        Assert.That(ElementAddress.ValidateAddress("dev/"),Is.False);
        Assert.That(ElementAddress.ValidateAddress("/el1/"),Is.False);
        Assert.That(ElementAddress.ValidateAddress("dev/el1/"),Is.False);
        Assert.That(ElementAddress.ValidateAddress("dev/el1/el2/"),Is.False);
        Assert.That(ElementAddress.ValidateAddress("dev/el1/el2/el3/"),Is.False);
    }

    [Test]
    public void Split()
    {
        var tokens = ElementAddress.Split("dev/el1/el2/el3");
        Assert.That(tokens.Length, Is.EqualTo(4));
        Assert.That(tokens[0], Is.EqualTo("dev"));
        Assert.That(tokens[1], Is.EqualTo("el1"));
        Assert.That(tokens[2], Is.EqualTo("el2"));
        Assert.That(tokens[3], Is.EqualTo("el3"));

        tokens = ElementAddress.Split("/el1/el2/el3");
        Assert.That(tokens.Length, Is.EqualTo(4));
        Assert.That(tokens[0], Is.EqualTo(string.Empty));
        Assert.That(tokens[1], Is.EqualTo("el1"));
        Assert.That(tokens[2], Is.EqualTo("el2"));
        Assert.That(tokens[3], Is.EqualTo("el3"));
    }

    [Test]
    public void GetLastIdentifier()
    {
        Assert.That(ElementAddress.GetLastIdentifier("dev"), Is.EqualTo("dev"));
        Assert.That(ElementAddress.GetLastIdentifier("dev/el1/el2/el3"), Is.EqualTo("el3"));
        Assert.That(ElementAddress.GetLastIdentifier("/el1/el2/el3"), Is.EqualTo("el3"));
    }

    [Test]
    public void GetParentAddress()
    {
        Assert.That(ElementAddress.GetParentAddress("dev/el1/el2/el3"), Is.EqualTo("dev/el1/el2"));
        Assert.That(ElementAddress.GetParentAddress("/el1/el2/el3"), Is.EqualTo("/el1/el2"));
    }
}