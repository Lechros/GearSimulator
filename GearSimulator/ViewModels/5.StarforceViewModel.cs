using Caliburn.Micro;
using System;
using System.Windows.Input;

namespace GearSimulator.ViewModels
{
    public class StarforceViewModel : Screen, IHandle<int>
    {
        private readonly IEventAggregator _eventAggregator;
        private GearBuilderManager _gb;

        public StarforceViewModel(IEventAggregator eventAggregator, GearBuilderManager gb)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            _gb = gb;
        }

        public void Handle(int message)
        {
        }

        private bool _yellow;
        public bool Yellow
        {
            get { return _yellow; }
            set
            {
                _yellow = value;
                NotifyOfPropertyChange(() => Yellow);
            }
        }
        private bool _blue;
        public bool Blue
        {
            get { return _blue; }
            set
            {
                _blue = value;
                BonusIsVisible = _blue;
                NotifyOfPropertyChange(() => Blue);
            }
        }
        private string _starCount;
        public string StarCount
        {
            get { return _starCount; }
            set
            {
                _starCount = value;
                NotifyOfPropertyChange(() => StarCount);
            }
        }
        private bool _bonusIsVisible;
        public bool BonusIsVisible
        {
            get { return _bonusIsVisible; }
            set
            {
                _bonusIsVisible = value;
                NotifyOfPropertyChange(() => BonusIsVisible);
            }
        }
        private string _bonusRate;
        public string BonusRate
        {
            get { return _bonusRate; }
            set
            {
                _bonusRate = value;
                NotifyOfPropertyChange(() => BonusRate);
            }
        }

        public void ExecuteApplyStar(ActionExecutionContext context)
        {
            var keyArgs = context.EventArgs as KeyEventArgs;

            if(keyArgs != null && keyArgs.Key == Key.Enter)
            {
                ApplyStar();
            }
        }

        public void ApplyStar()
        {
            int star, bonusRate = 0;
            if(!int.TryParse(StarCount, out star) || star < 0) return;
            if(!string.IsNullOrWhiteSpace(BonusRate) && !int.TryParse(BonusRate, out bonusRate)) return;
            bool starType = true;
            if(Yellow) starType = true;
            if(Blue) starType = false;
            bonusRate = Math.Min(100, Math.Max(0, bonusRate));
            _gb.ApplyStar(star, starType, bonusRate);
        }
    }
}
