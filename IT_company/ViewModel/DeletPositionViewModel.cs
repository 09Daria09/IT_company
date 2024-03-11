using IT_company.Commands;
using IT_company.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

public class DeletPositionViewModel : INotifyPropertyChanged 
{
    private ObservableCollection<Position> _availablePositions;
    public ObservableCollection<Position> AvailablePositions
    {
        get => _availablePositions;
        set
        {
            _availablePositions = value;
            OnPropertyChanged(nameof(AvailablePositions));
        }
    }

    private Position _selectedPosition;
    public Position SelectedPosition
    {
        get => _selectedPosition;
        set
        {
            _selectedPosition = value;
            OnPropertyChanged(nameof(SelectedPosition));
        }
    }

    public ICommand DeletePositionCommand { get; }

    public DeletPositionViewModel() 
    {
        AvailablePositions = new ObservableCollection<Position>();
        LoadPositions(); 
        DeletePositionCommand = new DelegateCommand(DeletePosition, CanDeletePosition);
    }

    private bool CanDeletePosition(object parameter)
    {
        return SelectedPosition != null;
    }

    private void DeletePosition(object parameter)
    {
        if (SelectedPosition == null)
        {
            MessageBox.Show("Пожалуйста, выберите должность для удаления.");
            return;
        }

        var result = MessageBox.Show($"Вы уверены, что хотите удалить должность '{SelectedPosition.Title}'?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            try
            {
                using (var context = new CompanyContext())
                {
                    var position = context.Positions.FirstOrDefault(p => p.PositionId == SelectedPosition.PositionId);
                    if (position != null)
                    {
                        context.Positions.Remove(position);
                        context.SaveChanges();

                        AvailablePositions.Remove(SelectedPosition);
                        SelectedPosition = null;

                        MessageBox.Show("Должность успешно удалена.");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка: Должность не найдена в базе данных.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении должности: {ex.Message}");
            }
        }
    }


    private void LoadPositions()
    {
        using (var context = new CompanyContext())
        {
            var positions = context.Positions.ToList();

            AvailablePositions.Clear();

            foreach (var position in positions)
            {
                AvailablePositions.Add(position);
            }
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
