using System.ComponentModel;
using System.Windows.Input;
using IT_company.Commands;
using IT_company.Model;

namespace IT_company.ViewModel
{
    public class AddPositionViewModel : INotifyPropertyChanged
    {
        private string _positionTitle;

        public string PositionTitle
        {
            get => _positionTitle;
            set
            {
                _positionTitle = value;
                OnPropertyChanged(nameof(PositionTitle));
            }
        }

        public ICommand AddPositionCommand { get; }

        public AddPositionViewModel()
        {
            AddPositionCommand = new DelegateCommand(AddPosition, CanAddPosition);
        }

        private bool CanAddPosition(object parameter)
        {
            return !string.IsNullOrWhiteSpace(PositionTitle); 
        }

        private void AddPosition(object parameter)
        {
            var positionTitle = PositionTitle.Trim(); 

            if (!CanAddPosition(positionTitle))
            {
                return;
            }

            var newPosition = new Position { Title = positionTitle };

            try
            {
                using (var context = new CompanyContext())
                {
                    context.Positions.Add(newPosition);
                    context.SaveChanges();
                }

                PositionTitle = string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
