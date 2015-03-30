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
    /// Page d'acceuil
    public partial class UserControl1 : UserControl
    {
        private bool myBoolean;
        static public Frame mainFrame;
        public UserControl1()
        {
            InitializeComponent();
            mainFrame = myFrame;
            Page1.userControl1 = this;
        }

    }
}
