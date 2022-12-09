using Caliburn.Micro;
using GearManager;
using System.Collections.Generic;

namespace GearSimulator.ViewModels
{
    public class PotentialViewModel : Screen, IHandle<int>
    {
        private readonly IEventAggregator _eventAggregator;
        private GearBuilderManager _gb;

        public PotentialViewModel(IEventAggregator eventAggregator, GearBuilderManager gb)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            _gb = gb;
            InitializeGrade();
        }

        public void Handle(int message)
        {
            if(message == 2) UpdateAvailablePotentials();
        }

        public BindableCollection<PotentialPresenter> Grade { get; set; }
        private PotentialPresenter _selectedGrade;
        public PotentialPresenter SelectedGrade
        {
            get { return _selectedGrade; }
            set
            {
                _selectedGrade = value;
                NotifyOfPropertyChange(() => SelectedGrade);
                if(_selectedGrade.Value > (int)GearGrade.Normal)
                {
                    PotentialIsVisible = true;
                }
                else
                {
                    PotentialIsVisible = false;
                }
                UpdateAvailablePotentials();
            }
        }
        private bool _potentialIsVisible;
        public bool PotentialIsVisible
        {
            get { return _potentialIsVisible; }
            set
            {
                _potentialIsVisible = value;
                NotifyOfPropertyChange(() => PotentialIsVisible);
            }
        }
        private BindableCollection<PotentialPresenter> _potential1 = new BindableCollection<PotentialPresenter>();
        public BindableCollection<PotentialPresenter> Potential1
        {
            get { return _potential1; }
            set
            {
                _potential1 = value;
                NotifyOfPropertyChange(() => Potential1);
                SelectedPotential1 = new PotentialPresenter();
            }
        }
        private BindableCollection<PotentialPresenter> _potential2 = new BindableCollection<PotentialPresenter>();
        public BindableCollection<PotentialPresenter> Potential2
        {
            get { return _potential2; }
            set
            {
                _potential2 = value;
                NotifyOfPropertyChange(() => Potential2);
                SelectedPotential2 = new PotentialPresenter();
            }
        }
        private BindableCollection<PotentialPresenter> _potential3 = new BindableCollection<PotentialPresenter>();
        public BindableCollection<PotentialPresenter> Potential3
        {
            get { return _potential3; }
            set
            {
                _potential3 = value;
                NotifyOfPropertyChange(() => Potential3);
                SelectedPotential3 = new PotentialPresenter();
            }
        }
        private PotentialPresenter _selectedPotential1;
        public PotentialPresenter SelectedPotential1
        {
            get { return _selectedPotential1; }
            set
            {
                _selectedPotential1 = value;
                NotifyOfPropertyChange(() => SelectedPotential1);
            }
        }
        private PotentialPresenter _selectedPotential2;
        public PotentialPresenter SelectedPotential2
        {
            get { return _selectedPotential2; }
            set
            {
                _selectedPotential2 = value;
                NotifyOfPropertyChange(() => SelectedPotential2);
            }
        }
        private PotentialPresenter _selectedPotential3;
        public PotentialPresenter SelectedPotential3
        {
            get { return _selectedPotential3; }
            set
            {
                _selectedPotential3 = value;
                NotifyOfPropertyChange(() => SelectedPotential3);
            }
        }

        public void InitializeGrade()
        {
            Grade = new BindableCollection<PotentialPresenter>
            {
                new PotentialPresenter((int)GearGrade.Normal, "노멀"),
                new PotentialPresenter((int)GearGrade.Rare, "레어"),
                new PotentialPresenter((int)GearGrade.Epic, "에픽"),
                new PotentialPresenter((int)GearGrade.Unique, "유니크"),
                new PotentialPresenter((int)GearGrade.Legendary, "레전드리"),
            };
            SelectedGrade = Grade[0];
        }

        public void UpdateAvailablePotentials()
        {
            Potential1 = new BindableCollection<PotentialPresenter>();
            foreach(KeyValuePair<int, string> kv in Potential.AvailableOptions(_gb.Type, (GearGrade)SelectedGrade.Value, _gb.ReqLevel, false, 0))
            {
                Potential1.Add(new PotentialPresenter(kv.Key, kv.Value));
            }
            Potential2 = new BindableCollection<PotentialPresenter>();
            foreach(KeyValuePair<int, string> kv in Potential.AvailableOptions(_gb.Type, (GearGrade)SelectedGrade.Value, _gb.ReqLevel, false, 1))
            {
                Potential2.Add(new PotentialPresenter(kv.Key, kv.Value));
            }
            Potential3 = new BindableCollection<PotentialPresenter>();
            foreach(KeyValuePair<int, string> kv in Potential.AvailableOptions(_gb.Type, (GearGrade)SelectedGrade.Value, _gb.ReqLevel, false, 2))
            {
                Potential3.Add(new PotentialPresenter(kv.Key, kv.Value));
            }
        }

        public void ApplyPotential()
        {
            _gb.ApplyPotential((GearGrade)SelectedGrade.Value, SelectedPotential1.Value, SelectedPotential2.Value, SelectedPotential3.Value);
        }
    }

    public struct PotentialPresenter
    {
        public PotentialPresenter(int value, string name)
        {
            Value = value;
            Name = name;
        }

        public int Value { get; }
        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }
    }

}
