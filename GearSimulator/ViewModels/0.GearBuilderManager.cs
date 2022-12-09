using Caliburn.Micro;
using GearManager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GearSimulator.ViewModels
{
    public class GearBuilderManager : PropertyChangedBase, IHandle<int>
    {
        private readonly IEventAggregator _eventAggregator;

        public GearBuilderManager(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            tr = new TooltipRenderer();
            gb = new GearBuilder();
        }

        public void Handle(int message)
        {
            // Scroll changed
            if(message == 4)
            {
                // If starforce (yellow)
                if(gb.StarTypeYellow) ApplyStar(gb.Star, gb.StarTypeYellow, 0);
            }
        }

        private void UpdateTooltipImage()
        {
            shouldRenderTooltip = true;
            _eventAggregator.PublishOnUIThread(1);
        }

        private TooltipRenderer tr;

        private bool shouldRenderTooltip;
        private GearBuilder gb;

        public string Name { get { return gb.Name; } }
        public GearType Type { get { return gb.Type; } }
        public int ReqLevel { get { return gb.GetPropValue(GearPropType.reqLevel); } }
        public bool StarType { get { return gb.StarTypeYellow; } }
        private bool _reboot;
        public bool Reboot
        {
            get { return _reboot; }
            set
            {
                _reboot = value;
                if(_reboot) gb.ResetScroll();
                UpdateTooltipImage();
            }
        }

        public Bitmap TooltipBitmap { get; private set; }
        private BitmapSource _tooltipImage;
        public BitmapSource TooltipImage
        {
            get
            {
                if(shouldRenderTooltip)
                {
                    tr.Gear = gb.ToGear();
                    TooltipBitmap = tr.Render(Reboot);
                    _tooltipImage = Convert(TooltipBitmap);
                    shouldRenderTooltip = false;
                }
                return _tooltipImage;
            }
        }
        public string GearString { get { return gb.ToGear().ToString(); } }

        public bool SaveImage(string filename)
        {
            TooltipBitmap.Save(filename);
            return true;
        }

        public void SetGear(int code)
        {
            gb = GearBuilder.FromGearCode(code);
            UpdateTooltipImage();

            // Update dependency
            _eventAggregator.PublishOnUIThread(2); // Updated gear code

            // Update SoulWeapon Visibility
            if(Gear.IsWeapon(gb.Type))
                _eventAggregator.PublishOnUIThread(10); // gear is Weapon
            else
                _eventAggregator.PublishOnUIThread(11); // gear is not Weapon
        }

        public void ApplyAdditionalOption(Dictionary<string, int> typeGradePairs)
        {
            List<string> AddOptionString = new List<string>
            { "STR", "DEX", "INT", "LUK", "STR+DEX", "STR+INT", "STR+LUK", "DEX+INT", "DEX+LUK", "INT+LUK", "최대 HP", "최대 MP", "공격력", "마력", "방어력", "이동속도", "점프력", "보스 데미지", "데미지", "올스탯%", "착용 레벨 감소", };
            GearPropType[][] AddOptionTypes = new GearPropType[][]
            {
                new GearPropType[] { GearPropType.incSTR, },
                new GearPropType[] { GearPropType.incDEX, },
                new GearPropType[] { GearPropType.incINT, },
                new GearPropType[] { GearPropType.incLUK, },
                new GearPropType[] { GearPropType.incSTR, GearPropType.incDEX, },
                new GearPropType[] { GearPropType.incSTR, GearPropType.incINT, },
                new GearPropType[] { GearPropType.incSTR, GearPropType.incLUK, },
                new GearPropType[] { GearPropType.incDEX, GearPropType.incINT, },
                new GearPropType[] { GearPropType.incDEX, GearPropType.incLUK, },
                new GearPropType[] { GearPropType.incINT, GearPropType.incLUK, },
                new GearPropType[] { GearPropType.incMHP, },
                new GearPropType[] { GearPropType.incMMP, },
                new GearPropType[] { GearPropType.incPAD, },
                new GearPropType[] { GearPropType.incMAD, },
                new GearPropType[] { GearPropType.incPDD, },
                new GearPropType[] { GearPropType.incSpeed, },
                new GearPropType[] { GearPropType.incJump, },
                new GearPropType[] { GearPropType.bdR, },
                new GearPropType[] { GearPropType.damR, },
                new GearPropType[] { GearPropType.incAllStat, },
                new GearPropType[] { GearPropType.reduceReq, },
            };
            bool bossReward = gb.GetPropValue(GearPropType.bossReward) > 0;
            gb.AdditionalStat.Clear();
            foreach(var ao in typeGradePairs)
            {
                int idx = AddOptionString.IndexOf(ao.Key);
                bool isDoubleAdd = false;
                if(AddOptionTypes[idx].Length > 1)
                {
                    isDoubleAdd = true;
                    gb.ApplyAdditionalStat(AddOptionTypes[idx][1], ao.Value, isDoubleAdd, bossReward);
                }
                gb.ApplyAdditionalStat(AddOptionTypes[idx][0], ao.Value, isDoubleAdd, bossReward);
            }
            UpdateTooltipImage();
        }

        public void ApplyScroll(Scroll scroll)
        {
            if(Reboot) return;
            if(gb.ApplyScroll(scroll, 1))
            {
                UpdateTooltipImage();
            }
        }
        public void ApplyInnocent()
        {
            if(gb.ResetScroll())
            {
                UpdateTooltipImage();
            }
        }
        public void ApplyHammer()
        {
            if(gb.ApplyHammer())
            {
                UpdateTooltipImage();
            }
        }
        public void ApplyCleanSlate()
        {
            int idx = gb.ScrollList.FindIndex(scr => scr.Type == ScrollType.fail);
            if(idx >= 0)
            {
                gb.ScrollList.RemoveAt(idx);
                UpdateTooltipImage();
            }
        }
        public void ApplyFail()
        {
            if(gb.ApplyScroll(new Scroll(ScrollType.fail), 1))
            {
                UpdateTooltipImage();
            }
        }

        public void ApplyStar(int star, bool starTypeYellow, int bonusRate)
        {
            if(gb.ApplyStar(star, starTypeYellow, bonusRate: bonusRate))
            {
                UpdateTooltipImage();
            }
        }

        public void ApplyPotential(GearGrade grade, int code1, int code2, int code3)
        {
            gb.Grade = grade;
            if(code1 > 0) gb.Options[0] = Potential.FromCode(code1, ReqLevel);
            if(code2 > 0) gb.Options[1] = Potential.FromCode(code2, ReqLevel);
            if(code3 > 0) gb.Options[2] = Potential.FromCode(code3, ReqLevel);
            UpdateTooltipImage();
        }

        public void ApplyAdditionalPotential(GearGrade grade, int code1, int code2, int code3)
        {
            gb.AdditionGrade = grade;
            if(code1 > 0) gb.AdditionalOptions[0] = Potential.FromCode(code1, ReqLevel);
            if(code2 > 0) gb.AdditionalOptions[1] = Potential.FromCode(code2, ReqLevel);
            if(code3 > 0) gb.AdditionalOptions[2] = Potential.FromCode(code3, ReqLevel);
            UpdateTooltipImage();
        }

        public void ApplySoul(string soulName, string greatSoulOptionStrings)
        {
            Soul soul;
            if((soul = Soul.CreateFromName(soulName, greatSoulOptionStrings)) == null) return;
            gb.SoulEnchanted = true;
            gb.Soul = soul;
            UpdateTooltipImage();
        }

        public void RemoveSoul()
        {
            gb.SoulEnchanted = false;
            gb.Soul = null;
            UpdateTooltipImage();
        }

        public void ApplySoulCharge(int soulCharge)
        {
            gb.Soul.Charge = soulCharge;
            UpdateTooltipImage();
        }

        public int GetTradableIndex()
        {
            if(gb.GetPropValue(GearPropType.tradeBlock) > 0) return 1;
            if(gb.GetPropValue(GearPropType.equipTradeBlock) > 0) return 2;
            if(gb.GetPropValue(GearPropType.tradeOnce) > 0) return 3;
            return 0;
        }

        public void SetTradable(int typeIdx)
        {
            gb.Props.Remove(GearPropType.tradeBlock);
            gb.Props.Remove(GearPropType.equipTradeBlock);
            gb.Props.Remove(GearPropType.tradeOnce);
            switch(typeIdx)
            {
                case 1:
                    gb.Props.Add(GearPropType.tradeBlock, 1);
                    break;
                case 2:
                    gb.Props.Add(GearPropType.equipTradeBlock, 1);
                    break;
                case 3:
                    gb.Props.Add(GearPropType.tradeOnce, 1);
                    break;
            }
            UpdateTooltipImage();
        }

        public void ApplyKarma(int karmaType, int karmaLeft)
        {
            switch(karmaType)
            {
                case 0:
                    gb.Props.Remove(GearPropType.tradeAvailable);
                    gb.Props.Remove(GearPropType.karmaLeft);
                    break;
                default:
                    gb.Props[GearPropType.tradeAvailable] = karmaType;
                    gb.Props[GearPropType.karmaLeft] = karmaLeft;
                    break;
            }
            UpdateTooltipImage();
        }

        public void ApplyLock(DateTime dt)
        {
            gb.GreenLock = false;
            gb.Lock = dt;
            UpdateTooltipImage();
        }
        public void ApplyGreenLock()
        {
            gb.Lock = default;
            gb.GreenLock = true;
            UpdateTooltipImage();
        }
        public void RemoveLock()
        {
            gb.Lock = new DateTime();
            gb.GreenLock = false;
            UpdateTooltipImage();
        }

        public void ApplyIncline(int value)
        {
            gb.Incline = value;
            UpdateTooltipImage();
        }

        public int[] GetEquipEXP()
        {
            return new int[6]
            {
                gb.GetPropValue(GearPropType.charismaEXP),
                gb.GetPropValue(GearPropType.insightEXP),
                gb.GetPropValue(GearPropType.willEXP),
                gb.GetPropValue(GearPropType.craftEXP),
                gb.GetPropValue(GearPropType.senseEXP),
                gb.GetPropValue(GearPropType.charmEXP),
            };
        }

        public void ApplyEquipEXP(int[] expValues)
        {
            GearPropType[] propTypes = new GearPropType[]
            {
                GearPropType.charismaEXP,
                GearPropType.insightEXP,
                GearPropType.willEXP,
                GearPropType.craftEXP,
                GearPropType.senseEXP,
                GearPropType.charmEXP,
            };
            for(int i = 0; i < expValues.Length; i++)
            {
                if(expValues[i] > 0) gb.Props[propTypes[i]] = expValues[i];
                else gb.Props.Remove(propTypes[i]);
            }
            UpdateTooltipImage();
        }

        public void ApplyAnvil(string gearName)
        {
            gb.FusionAnvil = gearName;
            UpdateTooltipImage();
        }

        public static BitmapSource Convert(Bitmap bitmap)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                  bitmap.GetHbitmap(),
                  IntPtr.Zero,
                  Int32Rect.Empty,
                  BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
