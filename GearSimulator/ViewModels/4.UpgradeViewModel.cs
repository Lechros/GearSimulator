using Caliburn.Micro;
using GearManager;
using System.Collections.Generic;
using System.Linq;

namespace GearSimulator.ViewModels
{
    public class UpgradeViewModel : Screen, IHandle<int>
    {
        private readonly IEventAggregator _eventAggregator;
        private GearBuilderManager _gb;

        public UpgradeViewModel(IEventAggregator eventAggregator, GearBuilderManager gb)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            _gb = gb;
            InitializeSpecialScrolls();
        }

        public void Handle(int message)
        {
            if(message == 2) UpdateAvailableScrolls();
        }
        public void NotifyOfScrollChanged()
        {
            _eventAggregator.PublishOnUIThread(4);
        }

        private BindableCollection<ScrollPresenter> _specialScrolls;
        public BindableCollection<ScrollPresenter> SpecialScrolls
        {
            get { return _specialScrolls; }
            set
            {
                _specialScrolls = value;
                NotifyOfPropertyChange(() => SpecialScrolls);
            }
        }
        private ScrollPresenter _selectedSpecialScrolls;
        public ScrollPresenter SelectedSpecialScrolls
        {
            get { return _selectedSpecialScrolls; }
            set
            {
                _selectedSpecialScrolls = value;
                NotifyOfPropertyChange(() => SelectedSpecialScrolls);
            }
        }

        private string _icSTR;
        public string icSTR
        {
            get { return _icSTR; }
            set
            {
                _icSTR = value;
                NotifyOfPropertyChange(() => icSTR);
            }
        }
        private string _icDEX;
        public string icDEX
        {
            get { return _icDEX; }
            set
            {
                _icDEX = value;
                NotifyOfPropertyChange(() => icDEX);
            }
        }
        private string _icINT;
        public string icINT
        {
            get { return _icINT; }
            set
            {
                _icINT = value;
                NotifyOfPropertyChange(() => icINT);
            }
        }
        private string _icLUK;
        public string icLUK
        {
            get { return _icLUK; }
            set
            {
                _icLUK = value;
                NotifyOfPropertyChange(() => icLUK);
            }
        }
        private string _icMHP;
        public string icMHP
        {
            get { return _icMHP; }
            set
            {
                _icMHP = value;
                NotifyOfPropertyChange(() => icMHP);
            }
        }
        private string _icMMP;
        public string icMMP
        {
            get { return _icMMP; }
            set
            {
                _icMMP = value;
                NotifyOfPropertyChange(() => icMMP);
            }
        }
        private string _icPAD;
        public string icPAD
        {
            get { return _icPAD; }
            set
            {
                _icPAD = value;
                NotifyOfPropertyChange(() => icPAD);
            }
        }
        private string _icMAD;
        public string icMAD
        {
            get { return _icMAD; }
            set
            {
                _icMAD = value;
                NotifyOfPropertyChange(() => icMAD);
            }
        }
        private string _icPDD;
        public string icPDD
        {
            get { return _icPDD; }
            set
            {
                _icPDD = value;
                NotifyOfPropertyChange(() => icPDD);
            }
        }
        private string _icSpeed;
        public string icSpeed
        {
            get { return _icSpeed; }
            set
            {
                _icSpeed = value;
                NotifyOfPropertyChange(() => icSpeed);
            }
        }
        private string _icJump;
        public string icJump
        {
            get { return _icJump; }
            set
            {
                _icJump = value;
                NotifyOfPropertyChange(() => icJump);
            }
        }

        private BindableCollection<ScrollPresenter> _availableScrolls;
        public BindableCollection<ScrollPresenter> AvailableScrolls
        {
            get { return _availableScrolls; }
            set
            {
                _availableScrolls = value;
                NotifyOfPropertyChange(() => AvailableScrolls);
            }
        }
        private ScrollPresenter _selectedAvailableScrolls;
        public ScrollPresenter SelectedAvailableScrolls
        {
            get { return _selectedAvailableScrolls; }
            set
            {
                _selectedAvailableScrolls = value;
                NotifyOfPropertyChange(() => SelectedAvailableScrolls);
            }
        }

        public void ApplySpecialScroll(int selectedIndex)
        {
            switch(selectedIndex)
            {
                case 0: _gb.ApplyInnocent(); break;
                case 1: _gb.ApplyHammer(); break;
                case 2: _gb.ApplyCleanSlate(); break;
                case 3: _gb.ApplyFail(); break;
            }
            NotifyOfScrollChanged();
        }
        public void ApplyScroll(ScrollPresenter selectedItem)
        {
            _gb.ApplyScroll(selectedItem.Scr);
            NotifyOfScrollChanged();
        }

        private void InitializeSpecialScrolls()
        {
            SpecialScrolls = new BindableCollection<ScrollPresenter>
            {
                new ScrollPresenter(0),
                new ScrollPresenter(1),
                new ScrollPresenter(2),
                new ScrollPresenter(3),
            };
        }
        private void UpdateAvailableScrolls()
        {
            AvailableScrolls = new BindableCollection<ScrollPresenter>();

            var spellTraceTypes = new List<ScrollType> { ScrollType.spellTrace100, ScrollType.spellTrace70, ScrollType.spellTrace30 };
            if(Gear.IsWeapon(_gb.Type) || _gb.Type == GearType.katara) spellTraceTypes.Add(ScrollType.spellTrace15);

            for(int i = 0; i <= 4; i++)
            {
                foreach(var type in spellTraceTypes)
                {
                    AvailableScrolls.Add(new ScrollPresenter(_gb, new Scroll(type, i)));
                }
            }

            if(Gear.IsArmor(_gb.Type))
            {
                AvailableScrolls.AddRange(new ScrollPresenter[]
                {
                    new ScrollPresenter(_gb, new Scroll(ScrollType.enhance100)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.enhance50)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.armorPAD, 1)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.armorPAD, 2)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.armorMAD, 1)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.armorMAD, 2)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.miracleArmorPAD, 2)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.miracleArmorPAD, 3)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.miracleArmorMAD, 2)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.miracleArmorMAD, 3)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.armorPADScroll)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.armorMADScroll)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.ultimateArmor)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.tenthAnnivArmor, 5)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.tenthAnnivArmor, 6)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.tenthAnnivPrimeArmor)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.happytimeArmorATT)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.frostyArmorEnhance)),
                });
            }
            else if(Gear.IsAccessory(_gb.Type))
            {
                AvailableScrolls.AddRange(new ScrollPresenter[]
                {
                    new ScrollPresenter(_gb, new Scroll(ScrollType.enhance100)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.enhance50)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.accPAD, 1)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.accPAD, 2)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.accMAD, 1)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.accMAD, 2)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.miracleAccPAD, 1)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.miracleAccPAD, 2)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.miracleAccPAD, 3)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.miracleAccPAD, 4)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.miracleAccMAD, 1)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.miracleAccMAD, 2)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.miracleAccMAD, 3)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.miracleAccMAD, 4)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.accPADScroll, 2)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.accPADScroll, 3)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.accPADScroll, 4)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.accMADScroll, 2)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.accMADScroll, 3)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.accMADScroll, 4)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.premiumAccPADScroll, 4)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.premiumAccPADScroll, 5)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.premiumAccMADScroll, 4)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.premiumAccMADScroll, 5)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.tenthAnnivAcc, 5)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.tenthAnnivAcc, 6)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.tenthAnnivPrimeAcc)),
                });
            }
            else if(Gear.IsWeapon(_gb.Type) || _gb.Type == GearType.katara || _gb.Type == GearType.machineHeart)
            {
                AvailableScrolls.AddRange(new ScrollPresenter[]
                {
                    new ScrollPresenter(_gb, new Scroll(ScrollType.magicalPAD, 9)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.magicalPAD, 10)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.magicalPAD, 11)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.magicalMAD, 9)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.magicalMAD, 10)),
                    new ScrollPresenter(_gb, new Scroll(ScrollType.magicalMAD, 11)),
                });
            }

            if(_gb.Type == GearType.earrings)
            {
                AvailableScrolls.AddRange(new ScrollPresenter[]
                {
                    new ScrollPresenter(_gb, new Scroll(ScrollType.earringINT10)),
                });
            }

            if(_gb.Name == "혼테일의 목걸이" || _gb.Name == "카오스 혼테일의 목걸이")
            {
                AvailableScrolls.AddRange(new ScrollPresenter[]
                {
                    new ScrollPresenter(_gb, new Scroll(ScrollType.dragonStone)),
                });
            }
            if(_gb.Name == "도미네이터 펜던트")
            {
                AvailableScrolls.AddRange(new ScrollPresenter[]
                {
                    new ScrollPresenter(_gb, new Scroll(ScrollType.fragmentOfTwistedTime)),
                });
            }

            if(100 <= _gb.ReqLevel && _gb.ReqLevel < 120)
            {
                foreach(ScrollType type in new ScrollType[] { ScrollType.yggdrasilSTR, ScrollType.yggdrasilDEX, ScrollType.yggdrasilINT, ScrollType.yggdrasilLUK })
                {
                    for(int value = 3; value <= 9; value++)
                    {
                        AvailableScrolls.Add(new ScrollPresenter(_gb, new Scroll(type, value)));
                    }
                }
            }
            else if(_gb.ReqLevel >= 120)
            {
                foreach(ScrollType type in new ScrollType[] { ScrollType.yggdrasilSTR, ScrollType.yggdrasilDEX, ScrollType.yggdrasilINT, ScrollType.yggdrasilLUK })
                {
                    for(int value = 5; value <= 15; value++)
                    {
                        AvailableScrolls.Add(new ScrollPresenter(_gb, new Scroll(type, value)));
                    }
                }
            }
        }

        public void ApplyChaosScroll()
        {
            GearPropType[] Types = new GearPropType[]
            { GearPropType.incSTR, GearPropType.incDEX, GearPropType.incINT, GearPropType.incLUK, GearPropType.incMHP, GearPropType.incMMP, GearPropType.incPAD, GearPropType.incMAD, GearPropType.incPDD, GearPropType.incSpeed, GearPropType.incJump, };
            string[] list = new string[]
            { icSTR, icDEX, icINT, icLUK, icMHP, icMMP, icPAD, icMAD, icPDD, icSpeed, icJump, };

            Dictionary<GearPropType, int> chaosStat = new Dictionary<GearPropType, int>();
            for(int i = 0; i < list.Length; i++)
            {
                if(int.TryParse(list[i], out int value) && value != 0)
                {
                    if(value > 6) value = 6;
                    else if(value < -6) value = -6;
                    if(Types[i] == GearPropType.incMHP || Types[i] == GearPropType.incMMP) value *= 10;
                    chaosStat[Types[i]] = value;
                }
            }
            _gb.ApplyScroll(new Scroll(ScrollType.incredibleChaos, chaosStat));
            NotifyOfScrollChanged();
        }
    }

    public struct ScrollPresenter
    {
        private static Dictionary<GearPropType, string> StatName = new Dictionary<GearPropType, string>
        {
            { GearPropType.incSTR, "STR" },
            { GearPropType.incDEX, "DEX" },
            { GearPropType.incINT, "INT" },
            { GearPropType.incLUK, "LUK" },
            { GearPropType.incAllStat, "올스탯" },
            { GearPropType.incMHP, "최대 HP" },
            { GearPropType.incMMP, "최대 MP" },
            { GearPropType.incPAD, "공격력" },
            { GearPropType.incMAD, "마력" },
            { GearPropType.incPDD, "방어력" },
            { GearPropType.incSpeed, "이동속도" },
            { GearPropType.incJump, "점프력" },
        };

        private static string[] SpecialScrollNames = new string[]
        {
            "아크 이노센트 주문서",
            "황금 망치",
            "순백의 주문서",
            "실패",
        };
        private static string[] SpecialScrollOptions = new string[]
        {
            "장비의 옵션을 표준 능력치로 초기화한다.",
            "업그레이드 가능 횟수를 1회 추가해준다.",
            "주문서 적용 실패로 차감된 업그레이드 가능 횟수를 회복시킨다.",
            "업그레이드 가능 횟수를 1회 감소시킨다."
        };

        public ScrollPresenter(int specialScrollIndex)
        {
            _specialIndex = specialScrollIndex;
            _type = GearType.cap;
            _reqLevel = 0;
            Scr = new Scroll();
        }

        public ScrollPresenter(GearBuilderManager gb, Scroll scroll)
        {
            _specialIndex = -1;
            _type = gb.Type;
            _reqLevel = gb.ReqLevel;
            Scr = scroll;
        }

        private GearType _type;
        private int _reqLevel;
        private int _specialIndex;

        public Scroll Scr { get; }
        public string Name
        {
            get
            {
                if(_specialIndex >= 0) return SpecialScrollNames[_specialIndex];
                return Scr.ToString(_type, _reqLevel);
            }
        }
        public string Options
        {
            get
            {
                if(_specialIndex >= 0) return SpecialScrollOptions[_specialIndex];
                string temp = "";
                var stats = Scr.GetStat(_type, _reqLevel).ToList();
                stats.Sort(delegate (KeyValuePair<GearPropType, int> s1, KeyValuePair<GearPropType, int> s2)
                {
                    int i1 = s1.Key == GearPropType.incAllStat ? 9 : (int)s1.Key;
                    int i2 = s2.Key == GearPropType.incAllStat ? 9 : (int)s2.Key;
                    return i1.CompareTo(i2);
                });
                foreach(var kv in stats)
                {
                    if(kv.Value != 0) temp += StatName[kv.Key] + " +" + kv.Value.ToString() + ", ";
                }
                if(string.IsNullOrWhiteSpace(temp)) return "-";
                return temp.Remove(temp.Length - 2);
            }
        }
    }
}
