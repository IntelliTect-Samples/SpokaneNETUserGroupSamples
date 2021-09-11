using Xunit;
namespace Nullability;

public class UnitTest2
{
    [Fact]
    public void CheckingForNull()
    {
        string? isNull = null;

        Assert.True(isNull == null);
        Assert.True(ReferenceEquals(isNull, null));
        Assert.True(isNull is null);
        Assert.False(isNull is { } unused);
        Assert.False(isNull?.Length == 0);

        //can be combined with assignment ??=
        Assert.Equal("42", isNull ?? "42");
    }

    public object? Foo { get; set; }
    [Fact]
    public void CheckingForNotNull()
    {
        int? isNull = 42;

        if (isNull is { })
        {

        }
        if (isNull is object)
        {

        }
        if (isNull is not null)
        {
            //Recomended
        }
    }

    [Theory]
    [InlineData(null)]
    public void NullParameters(string nullParameters)
    {
        if (nullParameters is { } foo)
        {

        }
        Assert.Throws<NullReferenceException>(
            () => _ = nullParameters.Length
        );
        Assert.Null(nullParameters);
    }

    [Fact]
    public void NullForgivenessOperator()
    {
        Assert.Throws<ArgumentNullException>(() => DoStuff(null!));
    }

    public static void DoStuff(string data)
    {
        _ = data ?? throw new ArgumentNullException(nameof(data));
        if (data is null) throw new ArgumentNullException(nameof(data));

        if (data is { })
        {
            //Do stuff
        }
    }


}
