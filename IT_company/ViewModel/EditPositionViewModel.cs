using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IT_company.Commands; 
using IT_company.Model;
namespace IT_company.ViewModel
{
    public class EditPositionViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Position> AvailablePositions { get; set; } = new ObservableCollection<Position>();
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

        private string _newPositionName;
        public string NewPositionName
        {
            get => _newPositionName;
            set
            {
                _newPositionName = value;
                OnPropertyChanged(nameof(NewPositionName));
            }
        }

        public ICommand EditPositionCommand { get; private set; }

        public EditPositionViewModel()
        {
            LoadPositions();
            EditPositionCommand = new DelegateCommand(EditPosition, CanEditPosition);
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

        private bool CanEditPosition(object parameter)
        {
            return SelectedPosition != null && !string.IsNullOrWhiteSpace(NewPositionName);
        }

        private void EditPosition(object parameter)
        {
            if (CanEditPosition(null))
            {
                using (var context = new CompanyContext())
                {
                    var positionToUpdate = context.Positions.Find(SelectedPosition.PositionId);

                    if (positionToUpdate != null)
                    {
                        positionToUpdate.Title = NewPositionName;
                        context.SaveChanges();

                        LoadPositions(); 
                        NewPositionName = string.Empty; 

                        MessageBox.Show("Должность успешно обновлена.");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка: Должность не найдена.");
                    }
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
