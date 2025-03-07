using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EroSplorerX.Data;

public class MainPageViewModel : INotifyPropertyChanged
{
    public bool ShowPlayed
    {
        get => _showPlayed;
        set
        {
            _showPlayed = value;
            OnPropertyChanged();
        }
    }
    private bool _showPlayed;



    #region PropertyChanged members

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}
