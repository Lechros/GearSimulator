using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GearSimulator.Views
{
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();
        }

        private void ShowIndex(int index)
        {
            Canvas[] list = new Canvas[] { t1, t2, t3, t4, t5, t6, t7, t8, t9 };
            for(int i = 0; i < list.Length; i++)
            {
                if(i == index) list[i].Visibility = Visibility.Visible;
                else list[i].Visibility = Visibility.Hidden;
            }
        }

        private void ShowGearInfoPage_Click(object sender, RoutedEventArgs e)
        {
            ShowIndex(0);
        }

        private void ShowPropertiesPage_Click(object sender, RoutedEventArgs e)
        {
            ShowIndex(1);
        }

        private void ShowAddOptionPage_Click(object sender, RoutedEventArgs e)
        {
            ShowIndex(2);
        }

        private void ShowUpgradePage_Click(object sender, RoutedEventArgs e)
        {
            ShowIndex(3);
        }

        private void ShowStarforcePage_Click(object sender, RoutedEventArgs e)
        {
            ShowIndex(4);
        }

        private void ShowPotentialPage_Click(object sender, RoutedEventArgs e)
        {
            ShowIndex(5);
        }

        private void ShowAddPotentialPage_Click(object sender, RoutedEventArgs e)
        {
            ShowIndex(6);
        }

        private void ShowSoulWeaponPage_Click(object sender, RoutedEventArgs e)
        {
            ShowIndex(7);
        }

        private void ShowInfoPage_Click(object sender, RoutedEventArgs e)
        {
            ShowIndex(8);
        }
    }
}