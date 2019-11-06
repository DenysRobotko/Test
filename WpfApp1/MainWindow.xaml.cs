using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public static Random rnd = new Random(DateTime.Now.Millisecond);
        public static int[] arr = new int[500];

        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        public MainWindow(ObservableCollection<MyTable> tableData)
        {
            grid.ItemsSource = tableData;
        }

        private void Initialize()
        {
            for (int i = 0; i < 10; i++)
            {
                var next = 0;
                while (true)
                {
                    next = rnd.Next(500);
                    if (!Contains(arr, next)) break;
                }

                arr[i] = next;

                TestData.Add(new MyTable { ID = arr[i].ToString(), Value = GetRandomText() });
            }
            grid.ItemsSource = TestData;
        }

        static bool Contains(int[] array, int value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == value) return true;
            }
            return false;
        }

        private string GetRandomText()
        {
            return System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName());
        }

        private ObservableCollection<MyTable> _testData = new ObservableCollection<MyTable>();

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<MyTable> TestData
        {
            get { return _testData; }
            set { _testData = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (grid.SelectedIndex >= 0)
            {
                //remove the selectedItem from the collection source
                MyTable item = grid.SelectedItem as MyTable;
                TestData.Remove(item);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
            addWindow.ShowDialog();
            for(int i=0;i<TestData.Count;i++)
            {
                if (TestData[i].ID == addWindow.Element.ID)
                {
                    MessageBox.Show("Елемент з таким ключем вже існує");
                    return;
                }
            }
            TestData.Add(addWindow.Element);
        }

        private void Menu_Information(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Хеш-таблиця – це структура даних," +
                " яка часто застосовується для «словникових операцій»," +
                " тобто додавання, пошуку і видалення елементів." +
                " Хеш-таблицю можна розглядати як один з різновидів масиву." +
                " Це - структура даних, що реалізує інтерфейс асоціативного масиву," +
                " а саме, вона дозволяє зберігати пари (ключ, значення). " +
                "Тобто не обов'язково працювати з усіма полями елемента одночасно, " +
                "достатньо лише обрати лише значення ключа і виконувати операції" +
                " на основі цього значення.");
        }
    }

    public class MyTable
    {
        public string ID { get; set; }
        public string Value { get; set; }
    }

    public static class DataGridTextSearch
    {
        public static readonly DependencyProperty SearchValueProperty =
            DependencyProperty.RegisterAttached("SearchValue", typeof(string), typeof(DataGridTextSearch),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.Inherits));

        public static string GetSearchValue(DependencyObject obj)
        {
            return (string)obj.GetValue(SearchValueProperty);
        }

        public static void SetSearchValue(DependencyObject obj, string value)
        {
            obj.SetValue(SearchValueProperty, value);
        }
        
        public static readonly DependencyProperty IsTextMatchProperty =
            DependencyProperty.RegisterAttached("IsTextMatch", typeof(bool), typeof(DataGridTextSearch), new UIPropertyMetadata(false));

        public static bool GetIsTextMatch(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsTextMatchProperty);
        }

        public static void SetIsTextMatch(DependencyObject obj, bool value)
        {
            obj.SetValue(IsTextMatchProperty, value);
        }
    }

    public class SearchValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string cellText = values[0] == null ? string.Empty : values[0].ToString();
            string searchText = values[1] as string;

            if (!string.IsNullOrEmpty(searchText) && !string.IsNullOrEmpty(cellText))
            {
                return cellText.ToLower().StartsWith(searchText.ToLower());
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}