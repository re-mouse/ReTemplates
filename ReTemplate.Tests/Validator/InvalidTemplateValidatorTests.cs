namespace ReTemplate.Tests;

public class InvalidTemplateValidatorTests
{
    [Test]
    public void TestShouldNotValidateArrayPlaceholderAfterArray()
    {
        AssertInvalidValidationThrowsException("ArrayPlaceholderAfterArray");
    }
    
    [Test]
    public void TestShouldNotValidateArrayPlaceholderBeforeArray()
    {
        AssertInvalidValidationThrowsException("ArrayPlaceholderBeforeArray");
    }
    
    [Test]
    public void TestShouldNotValidateClosingPlaceholder()
    {
        AssertInvalidValidationThrowsException("ClosingPlaceholder");
    }
    
    [Test]
    public void TestShouldNotValidateDuplicationClosingArray()
    {
        AssertInvalidValidationThrowsException("DuplicationClosingArray");
    }

    [Test]
    public void TestShouldNotValidateDuplicationClosingCondition()
    {
        AssertInvalidValidationThrowsException("DuplicationClosingCondition");
    }
    
    [Test]
    public void TestShouldNotValidateDuplicationClosingSubArray()
    {
        AssertInvalidValidationThrowsException("DuplicationClosingSubArray");
    }
    
    [Test]
    public void TestShouldNotValidateDuplicationOpeningArray()
    {
        AssertInvalidValidationThrowsException("DuplicationOpeningArray");
    }
    
    [Test]
    public void TestShouldNotValidateDuplicationOpeningSubArray()
    {
        AssertInvalidValidationThrowsException("DuplicationOpeningSubArray");
    }
    
    [Test]
    public void TestShouldNotValidateSubArrayOutsideOfArray()
    {
        AssertInvalidValidationThrowsException("SubArrayOutsideOfArray");
    }
    
    [Test]
    public void TestShouldNotValidateDuplicationOpeningCondition()
    {
        AssertInvalidValidationThrowsException("DuplicationOpeningArray");
    }

    [Test]
    public void TestShouldNotValidateUnclosedArrayTag()
    {
        AssertInvalidValidationThrowsException("UnclosedArrayTag");
    }
    
    [Test]
    public void TestShouldNotValidateUnclosedConditionTag()
    {
        AssertInvalidValidationThrowsException("UnclosedConditionTag");
    }
    
    [Test]
    public void TestShouldNotValidateUnclosedSubArrayTag()
    {
        AssertInvalidValidationThrowsException("UnclosedSubArrayTag");
    }

    [Test]
    public void TestShouldNotValidateConditionOpenedInArrayAndClosedOutside()
    {
        AssertInvalidValidationThrowsException("ConditionOpenedInArrayAndClosedOutside");
    }

    
    [Test]
    public void TestShouldNotValidateArrayOpenedInConditionAndClosedOutside()
    {
        AssertInvalidValidationThrowsException("ArrayOpenedInConditionAndClosedOutside");
    }
    
    [Test]
    public void TestShouldNotValidateUnclosedConditionInArray()
    {
        AssertInvalidValidationThrowsException("UnclosedConditionInArray");
    }
    
    private void AssertInvalidValidationThrowsException(string invalidFileName)
    {
        var path = GetInvalidFilePath(invalidFileName);
        var text = File.ReadAllText(path);
        var templateFile = new TemplateItem(invalidFileName, text);
        try
        {
            new TemplateValidator().Validate(templateFile);
            Assert.Fail("Expected format exception");
        }
        catch (FormatException formatException)
        {
            TestContext.Out.Write($"Got correct exception. Message: {formatException.Message}");
        }
    } 

    private string GetInvalidFilePath(string fileName)
    {
        return Path.Combine(TestContext.CurrentContext.TestDirectory, "Validator/InvalidTemplates", fileName);
    }
}