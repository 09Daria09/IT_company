using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IT_company.Model;
using IT_company.Commands;
using System.ComponentModel;

namespace IT_company.ViewModel
{
    public class DeleteEmployeeViewModel : INotifyPropertyChanged
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
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }

        public ICommand DeleteEmployeeCommand { get; private set; }

        public DeleteEmployeeViewModel()
        {
            Employees = new ObservableCollection<Employee>();
            LoadEmployees();
            DeleteEmployeeCommand = new DelegateCommand(DeleteEmployee, CanDeleteEmployee);
        }

        private void LoadEmployees()
        {
            using (var context = new CompanyContext())
            {
                var employeesList = context.Employees.ToList();
                Employees.Clear();
                foreach (var employee in employeesList)
                {
                    Employees.Add(employee);
                }
            }
        }

        private bool CanDeleteEmployee(object parameter)
        {
            return SelectedEmployee != null;
        }

        private void DeleteEmployee(object parameter)
        {
            if (SelectedEmployee == null) return;

            var result = MessageBox.Show($"Вы уверены, что хотите удалить сотрудника {SelectedEmployee.Name} {SelectedEmployee.Surname}?", "Подтверждение", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                using (var context = new CompanyContext())
                {
                    var employee = context.Employees.Find(SelectedEmployee.EmployeeId);
                    if (employee != null)
                    {
                        context.Employees.Remove(employee);
                        context.SaveChanges();
                        Employees.Remove(SelectedEmployee);
                    }
                }

                MessageBox.Show("Сотрудник удален");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
