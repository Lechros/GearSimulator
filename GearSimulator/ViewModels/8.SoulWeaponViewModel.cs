using Caliburn.Micro;
using GearManager;
using System.Linq;
using System.Windows.Input;

namespace GearSimulator.ViewModels
{
    public class SoulWeaponViewModel : Screen
    {
        private GearBuilderManager _gb;

        public SoulWeaponViewModel(GearBuilderManager gb)
        {
            _gb = gb;
            SoulMobName = new BindableCollection<string>(Soul.SoulMobNameData.ToArray());
        }

        public BindableCollection<string> SoulMobName { get; set; }
        private string _selectedSoulMobName;
        public string SelectedSoulMobName
        {
            get { return _selectedSoulMobName; }
            set
            {
                _selectedSoulMobName = value;
                NotifyOfPropertyChange(() => SelectedSoulMobName);
                if(!string.IsNullOrEmpty(SelectedSoulMobName))
                {
                    SoulName = new BindableCollection<string>(Soul.GetSoulNameContains(SelectedSoulMobName));
                    SoulNameIsVisible = true;
                }
            }
        }

        private bool _soulNameIsVisible;
        public bool SoulNameIsVisible
        {
            get { return _soulNameIsVisible; }
            set
            {
                _soulNameIsVisible = value;
                NotifyOfPropertyChange(() => SoulNameIsVisible);
            }
        }
        private BindableCollection<string> _soulName;
        public BindableCollection<string> SoulName
        {
            get { return _soulName; }
            set
            {
                _soulName = value;
                NotifyOfPropertyChange(() => SoulName);
            }
        }
        private string _selectedSoulName;
        public string SelectedSoulName
        {
            get { return _selectedSoulName; }
            set
            {
                _selectedSoulName = value;
                NotifyOfPropertyChange(() => SelectedSoulName);
                if(!string.IsNullOrEmpty(SelectedSoulName) && SelectedSoulName.Contains("위대한"))
                {
                    GreatSoulOption = new BindableCollection<string>(Soul.GreatSoulOptionStrings);
                    GreatSoulOptionIsVisible = true;
                }
                else
                {
                    GreatSoulOptionIsVisible = false;
                }
            }
        }

        private bool _greatSoulOptionIsVisible;
        public bool GreatSoulOptionIsVisible
        {
            get { return _greatSoulOptionIsVisible; }
            set
            {
                _greatSoulOptionIsVisible = value;
                NotifyOfPropertyChange(() => GreatSoulOptionIsVisible);
            }
        }
        private BindableCollection<string> _greatSoulOption;
        public BindableCollection<string> GreatSoulOption
        {
            get { return _greatSoulOption; }
            set
            {
                _greatSoulOption = value;
                NotifyOfPropertyChange(() => GreatSoulOption);
            }
        }
        private string _selectedGreatSoulOption;
        public string SelectedGreatSoulOption
        {
            get { return _selectedGreatSoulOption; }
            set
            {
                _selectedGreatSoulOption = value;
                NotifyOfPropertyChange(() => SelectedGreatSoulOption);
            }
        }

        private string _soulCharge;
        public string SoulCharge
        {
            get { return _soulCharge; }
            set
            {
                _soulCharge = value;
                NotifyOfPropertyChange(() => SoulCharge);
            }
        }

        public void ExecuteApplySoul(ActionExecutionContext context)
        {
            var keyArgs = context.EventArgs as KeyEventArgs;

            if(keyArgs != null && keyArgs.Key == Key.Enter)
            {
                ApplySoul();
            }
        }

        public void ApplySoul()
        {
            if(!string.IsNullOrEmpty(SelectedSoulName))
            {
                _gb.ApplySoul(SelectedSoulName, SelectedGreatSoulOption);
                if(int.TryParse(SoulCharge, out int value))
                {
                    if(value > 1000) value = 1000;
                    if(value < 0) value = 0;
                    _gb.ApplySoulCharge(value);
                }
            }
        }

        public void RemoveSoul()
        {
            _gb.RemoveSoul();
        }
    }
}
