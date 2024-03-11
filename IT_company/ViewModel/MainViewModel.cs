using IT_company.Commands;
using IT_company.Model;
using IT_company.View;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IT_company.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
        private string _selectedSearchCriteria;
        public string SelectedSearchCriteria
        {
            get => _selectedSearchCriteria;
            set
            {
                _selectedSearchCriteria = value;
                OnPropertyChanged(nameof(SelectedSearchCriteria));
            }
        }

        public List<string> SearchCriteria { get; } = new List<string> { "Имя", "Фамилия", "Должность" };
        public ObservableCollection<Employee> Employees { get; set; }
        public ObservableCollection<Position> Positions { get; set; }

        public ICommand AddEmployeeCommand { get; }
        public ICommand EditEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }
        public ICommand AddPositionCommand { get; }
        public ICommand EditPositionCommand { get; }
        public ICommand DeletePositionCommand { get; }
        public ICommand SearchCommand { get; }

        public MainViewModel()
        {
            Employees = new ObservableCollection<Employee>();
            Positions = new ObservableCollection<Position>();
            LoadData();

            AddEmployeeCommand = new DelegateCommand(AddEmployee, (object param) => true);
            EditEmployeeCommand = new DelegateCommand(EditEmployee, (object param) => true);
            DeleteEmployeeCommand = new DelegateCommand(DeleteEmployee, (object param) => true);
            AddPositionCommand = new DelegateCommand(AddPosition, (object param) => true);
            EditPositionCommand = new DelegateCommand(EditPosition, (object param) => true);
            DeletePositionCommand = new DelegateCommand(DeletePosition, (object param) => true);
            SearchCommand = new DelegateCommand(PerformSearch, (object param) => true);
        }

        private void LoadData()
        {
            using (var context = new CompanyContext())
            {
                context.Database.EnsureCreated();
                context.InitializeDb();

                var employeesList = context.Employees
           .Include(e => e.Position) 
           .ToList();
                Employees.Clear();

                foreach (var employee in employeesList)
                {
                    Employees.Add(employee);
                }
            }

        }

        private void AddEmployee(object parameter)
        {
            var WinAdd = new AddEmploeeWin();
            WinAdd.Show();
            LoadData();
        }

        private void EditEmployee(object parameter)
        {
            var WinEdit = new EditEmploeeWin();
            WinEdit.Show();
            LoadData();
        }

        private void DeleteEmployee(object parameter)
        {
            var DeleteWin = new DeleteEmploeeWin();
            DeleteWin.Show();
            LoadData();
        }

        private void AddPosition(object parameter)
        {
            var WinAdd = new AddPositionWin();
            WinAdd.Show();
            LoadData();
        }

        private void EditPosition(object parameter)
        {
            var WinEdit = new EditPositionWin();
            WinEdit.Show();
            LoadData();
        }

        private void DeletePosition(object parameter)
        {
            var DeleteWin = new DeletPositionWin();
            DeleteWin.Show();
            LoadData();
        }
        private void PerformSearch(object parameter)
        {
            using (var context = new CompanyContext())
            {
                IQueryable<Employee> query = context.Employees.Include(e => e.Position);

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    switch (SelectedSearchCriteria)
                    {
                        case "Имя":
                            query = query.Where(e => e.Name.StartsWith(SearchText));
                            break;
                        case "Фамилия":
                            query = query.Where(e => e.Surname.StartsWith(SearchText));
                            break;
                        case "Должность":
                            query = query.Where(e => e.Position.Title.StartsWith(SearchText));
                            break;
                    }
                }

                var searchResult = query.ToList();

                Employees.Clear();
                foreach (var employee in searchResult)
                {
                    Employees.Add(employee);
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
