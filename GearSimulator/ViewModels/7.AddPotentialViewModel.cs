using Caliburn.Micro;
using GearManager;
using System.Collections.Generic;

namespace GearSimulator.ViewModels
{
    public class AddPotentialViewModel : Screen, IHandle<int>
    {
        private readonly IEventAggregator _eventAggregator;
        private GearBuilderManager _gb;

        public AddPotentialViewModel(IEventAggregator eventAggregator, GearBuilderManager gb)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            _gb = gb;
            InitializeAdditionalGrade();
        }

        public void Handle(int message)
        {
            if(message == 2) UpdateAvailableAdditionalPotentials();
        }

        public BindableCollection<PotentialPresenter> AdditionalGrade { get; set; }
        private PotentialPresenter _selectedAdditionalGrade;
        public PotentialPresenter SelectedAdditionalGrade
        {
            get { return _selectedAdditionalGrade; }
            set
            {
                _selectedAdditionalGrade = value;
                NotifyOfPropertyChange(() => SelectedAdditionalGrade);
                if(_selectedAdditionalGrade.Value > (int)GearGrade.Normal)
                {
                    AdditionalPotentialIsVisible = true;
                }
                else
                {
                    AdditionalPotentialIsVisible = false;
                }
                UpdateAvailableAdditionalPotentials();
            }
        }
        private bool _additionalPotentialIsVisible;
        public bool AdditionalPotentialIsVisible
        {
            get { return _additionalPotentialIsVisible; }
            set
            {
                _additionalPotentialIsVisible = value;
                NotifyOfPropertyChange(() => AdditionalPotentialIsVisible);
            }
        }
        private BindableCollection<PotentialPresenter> _additionalAdditionalPotential1 = new BindableCollection<PotentialPresenter>();
        public BindableCollection<PotentialPresenter> AdditionalPotential1
        {
            get { return _additionalAdditionalPotential1; }
            set
            {
                _additionalAdditionalPotential1 = value;
                NotifyOfPropertyChange(() => AdditionalPotential1);
                SelectedAdditionalPotential1 = new PotentialPresenter();
            }
        }
        private BindableCollection<PotentialPresenter> _additionalAdditionalPotential2 = new BindableCollection<PotentialPresenter>();
        public BindableCollection<PotentialPresenter> AdditionalPotential2
        {
            get { return _additionalAdditionalPotential2; }
            set
            {
                _additionalAdditionalPotential2 = value;
                NotifyOfPropertyChange(() => AdditionalPotential2);
                SelectedAdditionalPotential2 = new PotentialPresenter();
            }
        }
        private BindableCollection<PotentialPresenter> _additionalAdditionalPotential3 = new BindableCollection<PotentialPresenter>();
        public BindableCollection<PotentialPresenter> AdditionalPotential3
        {
            get { return _additionalAdditionalPotential3; }
            set
            {
                _additionalAdditionalPotential3 = value;
                NotifyOfPropertyChange(() => AdditionalPotential3);
                SelectedAdditionalPotential3 = new PotentialPresenter();
            }
        }
        private PotentialPresenter _selectedAdditionalPotential1;
        public PotentialPresenter SelectedAdditionalPotential1
        {
            get { return _selectedAdditionalPotential1; }
            set
            {
                _selectedAdditionalPotential1 = value;
                NotifyOfPropertyChange(() => SelectedAdditionalPotential1);
            }
        }
        private PotentialPresenter _selectedAdditionalPotential2;
        public PotentialPresenter SelectedAdditionalPotential2
        {
            get { return _selectedAdditionalPotential2; }
            set
            {
                _selectedAdditionalPotential2 = value;
                NotifyOfPropertyChange(() => SelectedAdditionalPotential2);
            }
        }
        private PotentialPresenter _selectedAdditionalPotential3;
        public PotentialPresenter SelectedAdditionalPotential3
        {
            get { return _selectedAdditionalPotential3; }
            set
            {
                _selectedAdditionalPotential3 = value;
                NotifyOfPropertyChange(() => SelectedAdditionalPotential3);
            }
        }

        public void InitializeAdditionalGrade()
        {
            AdditionalGrade = new BindableCollection<PotentialPresenter>
            {
                new PotentialPresenter((int)GearGrade.Normal, "노멀"),
                new PotentialPresenter((int)GearGrade.Rare, "레어"),
                new PotentialPresenter((int)GearGrade.Epic, "에픽"),
                new PotentialPresenter((int)GearGrade.Unique, "유니크"),
                new PotentialPresenter((int)GearGrade.Legendary, "레전드리"),
            };
            SelectedAdditionalGrade = AdditionalGrade[0];
        }

        public void UpdateAvailableAdditionalPotentials()
        {
            AdditionalPotential1 = new BindableCollection<PotentialPresenter>();
            foreach(KeyValuePair<int, string> kv in Potential.AvailableOptions(_gb.Type, (GearGrade)SelectedAdditionalGrade.Value, _gb.ReqLevel, true, 0))
            {
                AdditionalPotential1.Add(new PotentialPresenter(kv.Key, kv.Value));
            }
            AdditionalPotential2 = new BindableCollection<PotentialPresenter>();
            foreach(KeyValuePair<int, string> kv in Potential.AvailableOptions(_gb.Type, (GearGrade)SelectedAdditionalGrade.Value, _gb.ReqLevel, true, 1))
            {
                AdditionalPotential2.Add(new PotentialPresenter(kv.Key, kv.Value));
            }
            AdditionalPotential3 = new BindableCollection<PotentialPresenter>();
            foreach(KeyValuePair<int, string> kv in Potential.AvailableOptions(_gb.Type, (GearGrade)SelectedAdditionalGrade.Value, _gb.ReqLevel, true, 2))
            {
                AdditionalPotential3.Add(new PotentialPresenter(kv.Key, kv.Value));
            }
        }

        public void ApplyAdditionalPotential()
        {
            _gb.ApplyAdditionalPotential((GearGrade)SelectedAdditionalGrade.Value, SelectedAdditionalPotential1.Value, SelectedAdditionalPotential2.Value, SelectedAdditionalPotential3.Value);
        }
    }
}
