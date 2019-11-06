using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public MyTable Element;
        public AddWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty(Tb1.Text) || String.IsNullOrEmpty(Tb2.Text))
            {
                MessageBox.Show("Введіть дані у всі поля");
                return;
            }
            Element = new MyTable
            {
                ID = Tb1.Text.Trim(),
                Value = Tb2.Text.Trim()
            };
            Close();
        }
    }
}
