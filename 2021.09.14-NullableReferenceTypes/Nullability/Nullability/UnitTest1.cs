using System.ComponentModel;
using Xunit;
namespace Nullability;

public class Person2 : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged = delegate { };

    private string _FirstName;
    public string FirstName
    {
        get => _FirstName;
        set
        {
            if (_FirstName != value)
            {
                _FirstName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(FirstName)));
            }
        }
    }

    public string LastName { get; init; }

    public DateTime DateOfBirth { get; }
}

public class PersonTests
{
    public PersonTests(string fullName)
    {

    }

    public PersonTests(string firstName, string lastName, DateTime? dob = null)
    {

    }
}

public class UnitTest1
{
    [Fact]
    public void NullableValueType()
    {
        int? number = null;
        Assert.Equal(typeof(int?), typeof(Nullable<int>));
    }

    [Fact]
    public void NullableReferenceType()
    {
        string? s = null;
        s = "42";
        Assert.Equal(typeof(string), s.GetType());
    }

    [Fact]
    public void NullableValueTypeWithArrays()
    {
        int? number = null;
        int?[] nullableArray = new int?[]
        {
            number
        };
        int[] array = new int[0];
        
        Assert.Equal(typeof(int?[]), nullableArray.GetType());
        Assert.Equal(typeof(Nullable<int>[]), nullableArray.GetType());

        Assert.NotEqual(typeof(int?[]), array.GetType());
        Assert.NotEqual(typeof(Nullable<int>[]), array.GetType());
    }
}