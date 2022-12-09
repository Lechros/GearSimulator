using Caliburn.Micro;
using System;
using System.Data.SqlTypes;
using System.Windows.Input;

namespace GearSimulator.ViewModels
{
    public class PropertiesViewModel : Screen, IHandle<int>
    {
        private readonly IEventAggregator _eventAggregator;
        private GearBuilderManager _gb;

        public PropertiesViewModel(IEventAggregator eventAggregator, GearBuilderManager gb)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            _gb = gb;
            SetTradableDefault();
            SetDefaultEXP();
        }

        public void Handle(int message)
        {
            if(message == 2)
            {
                SetTradableDefault();
                SetDefaultEXP();
            }
        }

        private bool _tradable;
        public bool Tradable
        {
            get { return _tradable; }
            set
            {
                _tradable = value;
                NotifyOfPropertyChange(() => Tradable);
            }
        }
        private bool _tradeBlock;
        public bool TradeBlock
        {
            get { return _tradeBlock; }
            set
            {
                _tradeBlock = value;
                NotifyOfPropertyChange(() => TradeBlock);
            }
        }
        private bool _equipTradeBlock;
        public bool EquipTradeBlock
        {
            get { return _equipTradeBlock; }
            set
            {
                _equipTradeBlock = value;
                NotifyOfPropertyChange(() => EquipTradeBlock);
            }
        }
        private bool _tradeOnce;
        public bool TradeOnce
        {
            get { return _tradeOnce; }
            set
            {
                _tradeOnce = value;
                NotifyOfPropertyChange(() => TradeOnce);
            }
        }

        private void SetTradableDefault()
        {
            int tradableIdx = _gb.GetTradableIndex();
            Tradable = tradableIdx == 0;
            TradeBlock = tradableIdx == 1;
            EquipTradeBlock = tradableIdx == 2;
            TradeOnce = tradableIdx == 3;
        }

        public void ApplyTradable()
        {
            int tradableIdx = 0;
            if(Tradable) tradableIdx = 0;
            if(TradeBlock) tradableIdx = 1;
            if(EquipTradeBlock) tradableIdx = 2;
            if(TradeOnce) tradableIdx = 3;
            _gb.SetTradable(tradableIdx);
        }

        private bool _silver;
        public bool Silver
        {
            get { return _silver; }
            set
            {
                _silver = value;
                NotifyOfPropertyChange(() => Silver);
            }
        }
        private bool _platinum;
        public bool Platinum
        {
            get { return _platinum; }
            set
            {
                _platinum = value;
                NotifyOfPropertyChange(() => Platinum);
            }
        }
        private string _karmaLeft;
        public string KarmaLeft
        {
            get { return _karmaLeft; }
            set
            {
                _karmaLeft = value;
                NotifyOfPropertyChange(() => KarmaLeft);
            }
        }

        public void ExecuteKarma(ActionExecutionContext context)
        {
            var keyArgs = context.EventArgs as KeyEventArgs;

            if(keyArgs != null && keyArgs.Key == Key.Enter)
            {
                ApplyKarma();
            }
        }

        public void ApplyKarma()
        {
            int karmaType = 0;
            if(Silver) karmaType = 1;
            if(Platinum) karmaType = 2;
            if(karmaType == 0) return;
            if(!int.TryParse(KarmaLeft, out int karmaLeft)) karmaLeft = -1;
            _gb.ApplyKarma(karmaType, karmaLeft);
        }

        public void RemoveKarma()
        {
            _gb.ApplyKarma(0, 0);
        }

        private string _lockDateTime;
        public string LockDateTime
        {
            get { return _lockDateTime; }
            set
            {
                _lockDateTime = value;
                NotifyOfPropertyChange(() => LockDateTime);
            }
        }

        public void ExecuteLock(ActionExecutionContext context)
        {
            var keyArgs = context.EventArgs as KeyEventArgs;

            if(keyArgs != null && keyArgs.Key == Key.Enter)
            {
                ApplyLock();
            }
        }

        public void ApplyLock()
        {
            if(DateTime.TryParse(LockDateTime, out DateTime dt))
            {
                _gb.ApplyLock(dt);
            }
        }

        public void ApplyGreenLock()
        {
            _gb.ApplyGreenLock();
        }

        public void RemoveLock()
        {
            _gb.RemoveLock();
        }

        private string _incline;
        public string Incline
        {
            get { return _incline; }
            set
            {
                _incline = value;
                NotifyOfPropertyChange(() => Incline);
            }
        }
        public void ApplyIncline()
        {
            if(int.TryParse(Incline, out int value))
            {
                _gb.ApplyIncline(value);
            }
        }
        public void ExecuteIncline(ActionExecutionContext context)
        {
            var keyArgs = context.EventArgs as KeyEventArgs;

            if(keyArgs != null && keyArgs.Key == Key.Enter)
            {
                ApplyIncline();
            }
        }


        private string _charismaEXP;
        public string CharismaEXP
        {
            get { return _charismaEXP; }
            set
            {
                _charismaEXP = value;
                NotifyOfPropertyChange(() => CharismaEXP);
            }
        }
        private string _insightEXP;
        public string InsightEXP
        {
            get { return _insightEXP; }
            set
            {
                _insightEXP = value;
                NotifyOfPropertyChange(() => InsightEXP);
            }
        }
        private string _willEXP;
        public string WillEXP
        {
            get { return _willEXP; }
            set
            {
                _willEXP = value;
                NotifyOfPropertyChange(() => WillEXP);
            }
        }
        private string _craftEXP;
        public string CraftEXP
        {
            get { return _craftEXP; }
            set
            {
                _craftEXP = value;
                NotifyOfPropertyChange(() => CraftEXP);
            }
        }
        private string _senseEXP;
        public string SenseEXP
        {
            get { return _senseEXP; }
            set
            {
                _senseEXP = value;
                NotifyOfPropertyChange(() => SenseEXP);
            }
        }
        private string _charmEXP;
        public string CharmEXP
        {
            get { return _charmEXP; }
            set
            {
                _charmEXP = value;
                NotifyOfPropertyChange(() => CharmEXP);
            }
        }

        public void SetDefaultEXP()
        {
            int[] values = _gb.GetEquipEXP();
            CharismaEXP = values[0].ToString();
            InsightEXP = values[1].ToString();
            WillEXP = values[2].ToString();
            CraftEXP = values[3].ToString();
            SenseEXP = values[4].ToString();
            CharmEXP = values[5].ToString();
        }

        public void ApplyEXP()
        {
            int[] values = new int[6];
            int.TryParse(CharismaEXP, out values[0]);
            int.TryParse(InsightEXP, out values[1]);
            int.TryParse(WillEXP, out values[2]);
            int.TryParse(CraftEXP, out values[3]);
            int.TryParse(SenseEXP, out values[4]);
            int.TryParse(CharmEXP, out values[5]);
            _gb.ApplyEquipEXP(values);
        }

        private string _anvilGearName;
        public string AnvilGearName
        {
            get { return _anvilGearName; }
            set
            {
                _anvilGearName = value;
                NotifyOfPropertyChange(() => AnvilGearName);
            }
        }

        public void ExecuteAnvil(ActionExecutionContext context)
        {
            var keyArgs = context.EventArgs as KeyEventArgs;

            if(keyArgs != null && keyArgs.Key == Key.Enter)
            {
                ApplyAnvil();
            }
        }

        public void ApplyAnvil()
        {
            _gb.ApplyAnvil(AnvilGearName);
        }

        public void ResetAnvil()
        {
            _gb.ApplyAnvil(string.Empty);
        }
    }
}
