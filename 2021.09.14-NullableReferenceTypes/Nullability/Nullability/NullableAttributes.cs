using System.Diagnostics.CodeAnalysis;

namespace Nullability;

public class Person
{
    //.NET 6
    private DateOnly? _DateOfBirth;
    public DateOnly? DateOfBirth 
    {
        get => _DateOfBirth;
        [MemberNotNull(nameof(Age))]
        init
        {
            if (value is { } dob)
            {
                Age = DateTime.Today.Year - dob.Year;
            }
            else
            {
                throw new ArgumentNullException(nameof(value));
            }
            _DateOfBirth = value;
        }
    }

    public int? Age { get; private set; }

    private string _MiddleName = "";
    [AllowNull]
    public string MiddleName
    {
        get => _MiddleName;
        set => _MiddleName = value ?? "";
    }

    private string? _FirstName;
    public string FirstName
    {
        get => _FirstName!;
        set => _FirstName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Person(string fullName)
    {
        if (TryParseName(fullName, out string? firstName, out string? lastName))
        {
            FirstName = firstName;
            MiddleName = null;
        }
        else
        {
            throw new ArgumentException("Full name not valid");
        }
    }

    private static bool TryParseName(string fullName, 
        [NotNullWhen(true)]
        out string? firstName,
        [NotNullWhen(true)]
        out string? lastName)
    {
        var parts = fullName.Split(' ');
        if (parts.Length == 2)
        {
            firstName = parts[0];
            lastName = parts[1];
            return true;
        }
        firstName = lastName = null;
        return false;
    }
}

/// <summary>
/// https://docs.microsoft.com/dotnet/csharp/language-reference/attributes/nullable-analysis
/// https://docs.microsoft.com/dotnet/csharp/nullable-migration-strategies
/// </summary>
public class NullableAttributes
{

}
