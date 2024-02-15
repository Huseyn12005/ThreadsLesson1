using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ThreadsLesson1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {


        public MainWindow()
        {

            _Processes = new ObservableCollection<Process>(Process.GetProcesses().ToList());
            BlackList = new ObservableCollection<String>();
            ThreadBlackList = new Thread(new ThreadStart(BlackListSearch));
            ThreadBlackList.IsBackground = true;
            ThreadBlackList.Start();
            InitializeComponent();
            DataContext = this;

        }

        public Thread ThreadBlackList { get; set; }

        private ObservableCollection<String> _BlackList;

        public ObservableCollection<String> BlackList
        {
            get { return _BlackList; }
            set 
            { 
                _BlackList = value; 
                OnPropertyChanged(); 
            }
        }

        private Process _selectedItem;

        public Process SelectedItem
        {
            get { return _selectedItem; }
            set 
            { 
                _selectedItem = value;
                OnPropertyChanged(); 
            }
        }

        private ObservableCollection<Process> Processes;

        public ObservableCollection<Process> _Processes
        {
            get { return Processes; }
            set
            {
                Processes = value;
                OnPropertyChanged();
            }
        }


        public void BlackListSearch()
        {
            while (true)
            {
                _Processes = new ObservableCollection<Process>(Process.GetProcesses().ToList());
                if (BlackList != null)
                {
                    var processes = _Processes.Where(p => BlackList.Any(bl => bl == p.ProcessName)).ToList();
                    foreach (Process process in processes)
                    {
                        Thread.Sleep(1000);
                        process.Kill();
                    }
                }
            }
        }



        private void EndClick(object sender, RoutedEventArgs e)
        {

                SelectedItem.Kill();

        }

        private void CreateClick(object sender, RoutedEventArgs e)
        {
                var process = new Process();
                process = Process.Start(Textbox.Text);
                _Processes.Add(process);

        }

     

        private void TaskMananger_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectedItem.Kill();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void AddClick(object sender, RoutedEventArgs e)
        {
            BlackList.Add(TextboxBlackList.Text);
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            BlackList.Remove(TextboxBlackList.Text);
        }
    }
}