using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using IT_company.Commands;
using IT_company.Model; 
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace IT_company.ViewModel
{
    public class EditEmploeeViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Employee> _employees; 
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                if (value != null)
                {

                    Name = value?.Name;
                    Surname = value?.Surname;
                    SelectedPosition = AvailablePositions.FirstOrDefault(p => p.PositionId == value.Position.PositionId);
                    OnPropertyChanged(nameof(SelectedEmployee));
                }
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }

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

        public ICommand SaveChangesCommand { get; private set; }

        public EditEmploeeViewModel() 
        {
            Employees = new ObservableCollection<Employee>();
            AvailablePositions = new ObservableCollection<Position>();
            SaveChangesCommand = new DelegateCommand(EditEmployee, CanEditEmployee);
            LoadEmployees();
            LoadPositions();
        }

        private void LoadEmployees()
        {
            using (var context = new CompanyContext())
            {
                var employeesList = context.Employees.Include(e => e.Position).ToList();
                Employees.Clear();
                foreach (var employee in employeesList)
                {
                    Employees.Add(employee);
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

        private bool CanEditEmployee(object parameter)
        {
            return SelectedEmployee != null && !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Surname) && SelectedPosition != null;
        }

        private void EditEmployee(object parameter)
        {
            if (CanEditEmployee(null) && SelectedEmployee != null)
            {
                using (var context = new CompanyContext())
                {
                    var emptyPosition = new Position { PositionId = -1, Title = "Нет должности" };
                    var employeeToUpdate = context.Employees.FirstOrDefault(e => e.EmployeeId == SelectedEmployee.EmployeeId);

                    if (employeeToUpdate != null)
                    {
                        employeeToUpdate.Name = Name;
                        employeeToUpdate.Surname = Surname;

                        employeeToUpdate.PositionId = SelectedPosition.PositionId;

                        context.SaveChanges();

                        MessageBox.Show("Сотрудник успешно обновлён.");
                        LoadEmployees();
                        Name = string.Empty;
                        Surname = string.Empty;
                        SelectedPosition = emptyPosition;
                        OnPropertyChanged(nameof(SelectedPosition));
                    }
                    else
                    {
                        MessageBox.Show("Ошибка: Сотрудник не найден.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля и выберите сотрудника.");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
