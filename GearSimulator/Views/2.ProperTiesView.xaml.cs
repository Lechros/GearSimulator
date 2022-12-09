using System;
using System.Windows.Controls;

namespace GearSimulator.Views
{
    public partial class PropertiesView : Page
    {
        public PropertiesView()
        {
            Now = DateTime.Now.ToString("yyyy/MM/dd hh:mm");
            InitializeComponent();
        }

        public string Now { get; set; }
    }
}
