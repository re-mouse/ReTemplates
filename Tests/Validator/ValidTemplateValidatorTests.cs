namespace ReTemplate.Tests;

public class ValidTemplateValidatorTests
{
    [Test]
    public void ShouldValidateEmpty()
    {
        AssertValidValidationDoesntThrowsException("Empty");
    }
    
    [Test]
    public void ShouldValidateNoTags()
    {
        AssertValidValidationDoesntThrowsException("NoTags");
    }
    
    [Test]
    public void ShouldValidatePlaceholder()
    {
        AssertValidValidationDoesntThrowsException("Placeholder");
    }
    
    
    [Test]
    public void ShouldValidateArrayInCondition()
    {
        AssertValidValidationDoesntThrowsException("ArrayInCondition");
    }
    
    [Test]
    public void ShouldValidateConditionsInArrayInCondition()
    {
        AssertValidValidationDoesntThrowsException("ConditionsInArrayInCondition");
    }
    
    [Test]
    public void ShouldValidateMultiplePlaceholders()
    {
        AssertValidValidationDoesntThrowsException("MultiplePlaceholders");
    }
    
    [Test]
    public void ShouldValidateArray()
    {
        AssertValidValidationDoesntThrowsException("Array");
    }
    
    [Test]
    public void ShouldValidateMultipleArrays()
    {
        AssertValidValidationDoesntThrowsException("MultipleArrays");
    }
    
    [Test]
    public void ShouldValidateArrayWithSubArray()
    {
        AssertValidValidationDoesntThrowsException("ArrayWithSubArray");
    }
    
    [Test]
    public void ShouldValidateArrayWithPlaceholder()
    {
        AssertValidValidationDoesntThrowsException("ArrayWithPlaceholder");
    }
    
    [Test]
    public void ShouldValidateArrayWithPlaceholderAndSubArray()
    {
        AssertValidValidationDoesntThrowsException("ArrayWithPlaceholderAndSubArray");
    }
    
    [Test]
    public void ShouldValidateArrayWithNestedSubArrays()
    {
        AssertValidValidationDoesntThrowsException("ArrayWithNestedSubArrays");
    }
    
    [Test]
    public void ShouldValidateConditions()
    {
        AssertValidValidationDoesntThrowsException("Conditions");
    }
    
    [Test]
    public void ShouldValidateConditionsWithPlaceholders()
    {
        AssertValidValidationDoesntThrowsException("ConditionsWithPlaceholders");
    }
    
    [Test]
    public void ShouldValidateConditionsInArray()
    {
        AssertValidValidationDoesntThrowsException("ConditionsInArray");
    }
    
    [Test]
    public void ShouldValidateGlobalConditionsWithConditionsInArray()
    {
        AssertValidValidationDoesntThrowsException("GlobalConditionsWithConditionsInArray");
    }
    
    [Test]
    public void ShouldValidateArrayConditionsWithNestedSubArraysAndPlaceholder()
    {
        AssertValidValidationDoesntThrowsException("ArrayWithNestedSubArraysAndPlaceholder");
    }

    private void AssertValidValidationDoesntThrowsException(string validFileName)
    {
        var path = GetValidFilePath(validFileName);
        var text = File.ReadAllText(path);
        var templateFile = new TemplateFile(validFileName, text);
        Assert.DoesNotThrow(() => new TemplateValidator().Validate(templateFile));
    }

    private string GetValidFilePath(string fileName)
    {
        return Path.Combine(TestContext.CurrentContext.TestDirectory, "Validator/ValidTemplates", fileName);
    }
}