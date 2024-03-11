using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using IT_company.Commands; // Убедитесь, что здесь указан правильный путь к вашим командам
using IT_company.Model;

namespace IT_company.ViewModel
{
    public class AddEmployeeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Position> Positions { get; private set; }
        public ICommand AddEmployeeCommand { get; private set; }
        public ObservableCollection<Position> AvailablePositions { get; set; } = new ObservableCollection<Position>();


        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _surname;
        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
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

        public AddEmployeeViewModel()
        {
            AddEmployeeCommand = new DelegateCommand(AddEmployee, CanAddEmployee);
            Positions = new ObservableCollection<Position>();
            LoadPositions();
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
        private bool CanAddEmployee(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Surname) &&
                   SelectedPosition != null;
        }

        private void AddEmployee(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Surname) || SelectedPosition == null)
            {
                MessageBox.Show("Все поля должны быть заполнены.", "Ошибка добавления");
                return;
            }

            var newEmployee = new Employee
            {
                Name = Name,
                Surname = Surname,
                PositionId = SelectedPosition.PositionId 
            };

            try
            {
                using (var context = new CompanyContext())
                {
                    context.Employees.Add(newEmployee);
                    context.SaveChanges();
                }
                Name = string.Empty;
                Surname = string.Empty;
                SelectedPosition = null;

                MessageBox.Show("Сотрудник успешно добавлен.", "Успех");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                MessageBox.Show($"Произошла ошибка при добавлении сотрудника: {ex.Message}", "Ошибка");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
