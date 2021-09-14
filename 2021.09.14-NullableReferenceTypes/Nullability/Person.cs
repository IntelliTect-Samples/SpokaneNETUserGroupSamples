using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nullability;

public class Person : INotifyPropertyChanged
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
