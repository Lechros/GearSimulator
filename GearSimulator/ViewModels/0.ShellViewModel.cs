using Caliburn.Micro;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GearSimulator.ViewModels
{
    public class ShellViewModel : Conductor<object>.Collection.OneActive, IHandle<int>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly GearBuilderManager _gb;

        public ShellViewModel(IEventAggregator eventAggregator, GearBuilderManager gb)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            _gb = gb;
            Items.Add(new GearInfoViewModel(_gb)); //0
            Items.Add(new PropertiesViewModel(eventAggregator, _gb)); //1
            Items.Add(new AddOptionViewModel(_gb)); //2
            Items.Add(new UpgradeViewModel(eventAggregator, _gb)); //3
            Items.Add(new StarforceViewModel(eventAggregator, _gb)); //4
            Items.Add(new PotentialViewModel(eventAggregator, _gb)); //5
            Items.Add(new AddPotentialViewModel(eventAggregator, _gb)); //6
            Items.Add(new SoulWeaponViewModel(_gb)); //7
            Items.Add(new InfoViewModel()); //8
            ShowGearInfoPage();
            InitializeGearBase();
        }

        public BitmapSource Tooltip
        {
            get { return _gb.TooltipImage; }
        }
        public void Handle(int message)
        {
            if(message == 1) NotifyOfPropertyChange(() => Tooltip);

            if(message == 10) SoulPageIsVisible = Visibility.Visible;
            else if(message == 11) SoulPageIsVisible = Visibility.Collapsed;
        }

        private string _titleText;
        public string TitleText
        {
            get { return _titleText; }
            set
            {
                _titleText = value;
                NotifyOfPropertyChange(() => TitleText);
            }
        }

        public void ShowGearInfoPage()
        {
            TitleText = MenuItems.GearInfo;
            ActivateItem(Items[0]);
        }
        public void ShowPropertiesPage()
        {
            TitleText = MenuItems.Properties;
            ActivateItem(Items[1]);
        }
        public void ShowAddOptionPage()
        {
            TitleText = MenuItems.AddOption;
            ActivateItem(Items[2]);
        }
        public void ShowUpgradePage()
        {
            TitleText = MenuItems.Upgrade;
            ActivateItem(Items[3]);
        }
        public void ShowStarforcePage()
        {
            TitleText = MenuItems.Starforce;
            ActivateItem(Items[4]);
        }
        public void ShowPotentialPage()
        {
            TitleText = MenuItems.Potential;
            ActivateItem(Items[5]);
        }
        public void ShowAddPotentialPage()
        {
            TitleText = MenuItems.AddPotential;
            ActivateItem(Items[6]);
        }
        public void ShowSoulWeaponPage()
        {
            TitleText = MenuItems.SoulWeapon;
            ActivateItem(Items[7]);
        }
        public void ShowInfoPage()
        {
            //TitleText = MenuItems.Info;
            TitleText = "아이템 시뮬레이터";
            ActivateItem(Items[8]);
        }

        public void InitializeGearBase()
        {
            int code = 1003142;
            string name = "위젯 무적 모자";
            ((GearInfoViewModel)Items[0]).ApplyGear(new System.Collections.Generic.KeyValuePair<int, string>(code, name));
        }

        private Visibility _soulPageIsVisible;
        public Visibility SoulPageIsVisible
        {
            get { return _soulPageIsVisible; }
            set
            {
                _soulPageIsVisible = value;
                NotifyOfPropertyChange(() => SoulPageIsVisible);
            }
        }
    }
}
