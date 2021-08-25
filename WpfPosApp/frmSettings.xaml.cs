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

namespace WpfPosApp
{
    /// <summary>
    /// Interaction logic for frmSettings.xaml
    /// </summary>
    public partial class frmSettings : UserControl
    {

        frmCashierDashboard m;

        public frmSettings(frmCashierDashboard mainForm)
        {
            m = mainForm;
            InitializeComponent();
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            SolidColorBrush wbrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            SolidColorBrush pbrush = new SolidColorBrush(Color.FromRgb(115, 137, 195));


            m.menuTopGrid.Background = new SolidColorBrush(Color.FromRgb(26, 26, 39));
            m.GridMenu.Background = new SolidColorBrush(Color.FromRgb(30, 30, 45));
            m.lblloggedUsr.Foreground = pbrush;
            m.dshIcn.Foreground = pbrush;
           
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SolidColorBrush wbrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            SolidColorBrush pbrush = new SolidColorBrush(Color.FromRgb(115, 137, 195));

            m.menuTopGrid.Background = wbrush;
            m.GridMenu.Background = wbrush;
            m.lblloggedUsr.Foreground = pbrush;
            m.dshIcn.Foreground = pbrush;
            m.dshText.Foreground = pbrush;
        }

       
        private void rb2_Checked(object sender, RoutedEventArgs e)
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Color.FromRgb(71, 137, 255), 0));
            brush.GradientStops.Add(new GradientStop(Color.FromRgb(83, 63, 212), 1));

            SolidColorBrush wbrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            m.gridMain.Background = new SolidColorBrush(Color.FromRgb(21, 21, 27));
        }

        private void rbwhite_Checked(object sender, RoutedEventArgs e)
        {
            SolidColorBrush bbrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            SolidColorBrush wbrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            m.gridMain.Background = new SolidColorBrush(Color.FromRgb(241, 241, 241));

        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            m.topBarGrid.Background = new SolidColorBrush(Color.FromRgb(26, 26, 39));

        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            m.topBarGrid.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            //set the new value of SavedSetting1
            //checkBox.IsChecked = Properties.Settings.Default.CheckBox;
            //Saves Form Colors
            bool check = btnTheme.IsChecked ?? true;
            Properties.Settings.Default.UCheckBox = check;
            //Saves Panel Colors
            bool pnlCheck1 = rb2.IsChecked ?? true;
            Properties.Settings.Default.UpnlCheck2 = pnlCheck1;

            bool pnlCheck = rbwhite.IsChecked ?? true;
            Properties.Settings.Default.UpnlCheck = pnlCheck;

            bool check2 = btnBtheme.IsChecked ?? true;
            Properties.Settings.Default.UCheckBox2 = check2;
         
            //apply the changes to the settings file
            Properties.Settings.Default.Save();

            MessageBoxResult result = MessageBox.Show("Changes Successfully Saved!");

        }

        private void BtnClose_Click(object sender, MouseButtonEventArgs e)
        {
            frmCshdb frm = new frmCshdb();
            m.pnlMain.Children.Remove(this);
            m.pnlMain.Children.Add(frm);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            frmCshdb frm = new frmCshdb();
            m.pnlMain.Children.Remove(this);
            m.pnlMain.Children.Add(frm);
        }

       
    }
}
