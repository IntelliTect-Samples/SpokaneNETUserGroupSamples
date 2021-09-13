using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Xunit;
namespace Nullability;

public class TodoItem
{

    public string? Text { get; set; }

    #region NonNullable Property

    private string? _Text2;

    public string Text2
    {
        get => _Text2!;
        set => _Text2 = value ?? throw new ArgumentNullException(nameof(value));
    }

    public TodoItem(string text)
    {
        Text2 = text;
        MyProperty = Guard.NotNull(text);
    }

    #endregion NonNullable Property

    #region Auto Properties

    public string MyProperty { get; }

    private string? _Text3;
    public string Text3
    { 
        get => _Text3!; 
        init => _Text3 = Guard.NotNull(value);
    }

    #endregion

}

//Rename to be person
public class TodoItemTests
{
    [Fact]
    public void HandlesNull()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => new TodoItem("")
        {
            Text3 = null!
        });
        Assert.Equal("value", ex.ParamName);
    }
}

public static class Guard
{
    public static string NotNull(string argument,
        [CallerArgumentExpression("argument")]
         string argumentExpression = null!)
        => argument ?? throw new ArgumentNullException(argumentExpression);
}