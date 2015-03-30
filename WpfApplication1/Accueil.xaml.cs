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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public static UserControl1 userControl1 { get; set; } //Resharper? C'est peut être du à ta license
        public Page1()
        {
            InitializeComponent();
        }                   

        private void Question_Click(object sender, RoutedEventArgs e)
        {
            userControl1.myFrame.Source=new Uri("/QuestionView.xaml", UriKind.RelativeOrAbsolute);
        }

        private void Trainer_Click(object sender, RoutedEventArgs e)
        {
            userControl1.myFrame.Source = new Uri("/TrainerView.xaml", UriKind.Relative);
        }

        private void Validation_Click(object sender, RoutedEventArgs e)
        {
            userControl1.myFrame.Source = new Uri("/ValidationView.xaml", UriKind.Relative);
        }

    }
}
