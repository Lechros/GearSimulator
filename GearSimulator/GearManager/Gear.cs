using System;
using System.Collections.Generic;

namespace GearManager
{
    public class Gear
    {
        public Gear()
        {
            Props = new Dictionary<GearPropType, int>();
            BaseStat = new Dictionary<GearPropType, int>();
            AdditionalStat = new Dictionary<GearPropType, int>();
            EnchantStat = new Dictionary<GearPropType, int>();
            Options = new Potential[3];
            AdditionalOptions = new Potential[3];
            StarTypeYellow = true;
        }
        public string Name { get; set; }
        public BitmapOrigin IconRaw { get; set; }
        public GearType Type { get; set; }
        public string Desc { get; set; }
        public Dictionary<GearPropType, int> Props { get; set; }
        public int Incline { get; set; }
        public string NameTag { get; set; }
        public string FusionAnvil { get; set; }
        public DateTime TimeLimited { get; set; }
        public DateTime Lock { get; set; }
        public bool GreenLock { get; set; }

        public GearQuality Quality { get; set; }

        public bool StarTypeYellow { get; set; } // true=스타포스, false=놀장
        public int Star { get; set; }
        public int MaxStar { get; set; }
        public bool HasTuc { get; set; }
        public int ScrollUp { get; set; }
        public int ScrollAvailable { get; set; }
        public int ScrollRestore { get; set; }
        public int Hammer { get; set; }
        public int Yggdrasil { get; set; }

        public Dictionary<GearPropType, int> BaseStat { get; set; }
        public Dictionary<GearPropType, int> AdditionalStat { get; set; }
        public Dictionary<GearPropType, int> EnchantStat { get; set; }

        public GearGrade Grade { get; set; }
        public GearGrade AdditionGrade { get; set; }
        public Potential[] Options { get; set; }
        public Potential[] AdditionalOptions { get; set; }

        public bool SoulEnchanted { get; set; }
        public Soul Soul { get; set; }
        public int SoulCharge { get; set; }

        public static bool IsWeapon(GearType type)
        {
            return IsLeftWeapon(type) || IsDoubleHandWeapon(type);
        }

        public static bool IsLeftWeapon(GearType type)
        {
            int _type = (int)type;
            if(type == GearType.shiningRod || type == GearType.tuner)
            {
                _type = (int)type / 10;
            }
            return _type >= 121 && _type <= 139 && type != GearType.katara;
        }

        public static bool IsSubWeapon(GearType type)
        {
            switch(type)
            {
                case GearType.shield:
                case GearType.demonShield:
                case GearType.soulShield:
                    return true;

                default:
                    if((int)type / 1000 == 135)
                    {
                        return true;
                    }
                    return false;
            }
        }

        public static bool IsDoubleHandWeapon(GearType type)
        {
            int _type = (int)type;
            return (_type >= 140 && _type <= 149)
                || (_type >= 152 && _type <= 159);
        }

        public static bool IsArmor(GearType type)
        {
            int _type = (int)type;
            return (_type == 100)
                || (_type >= 104 && _type <= 110);
        }

        public static bool IsAccessory(GearType type)
        {
            int _type = (int)type;
            return (_type >= 101 && _type <= 103)
                || (_type >= 111 && _type <= 113)
                || (_type == 115);
        }

        public static bool IsMechanicGear(GearType type)
        {
            return (int)type >= 161 && (int)type <= 165;
        }

        public static bool IsDragonGear(GearType type)
        {
            return (int)type >= 194 && (int)type <= 197;
        }

        public static GearType GetGearType(int code)
        {
            switch(code / 1000)
            {
                case 1098:
                    return GearType.soulShield;
                case 1099:
                    return GearType.demonShield;
            }
            if(code / 10000 == 135)
            {
                switch(code / 100)
                {
                    case 13522:
                    case 13528:
                    case 13529:
                        return (GearType)(code / 10);

                    default:
                        return (GearType)(code / 100 * 10);
                }
            }
            if(code / 10000 == 119)
            {
                switch(code / 100)
                {
                    case 11902:
                        return (GearType)(code / 10);
                }
            }
            if(code / 10000 == 121)
            {
                switch(code / 1000)
                {
                    case 1212:
                    case 1213:
                        return (GearType)(code / 1000);
                }
            }
            return (GearType)(code / 10000);
        }

        public static int CalculateMaxStar(Gear gear, int reqLevel = -1)
        {
            if(gear.Props.TryGetValue(GearPropType.onlyUpgrade, out int value) && value > 0)
            {
                return 0;
            }
            if(!gear.HasTuc)
            {
                return 0;
            }
            if(gear.ScrollUp + gear.ScrollAvailable + gear.ScrollRestore <= 0)
            {
                return 0;
            }
            if(IsMechanicGear(gear.Type) || IsDragonGear(gear.Type))
            {
                return 0;
            }


            int[][] starData = new int[][] {
                new[]{ 0, 5, 3 },
                new[]{ 95, 8, 5 },
                new[]{ 108, 10, 8 },
                new[]{ 118, 15, 10 },
                new[]{ 128, 20, 12 },
                new[]{ 138, 25, 15 },
            };

            if(reqLevel < 0)
            {
                gear.Props.TryGetValue(GearPropType.reqLevel, out reqLevel);
            }
            int[] data = null;
            foreach(int[] item in starData)
            {
                if(reqLevel >= item[0])
                {
                    data = item;
                }
                else
                {
                    break;
                }
            }
            if(data == null)
            {
                return 0;
            }

            gear.Props.TryGetValue(GearPropType.superiorEqp, out value);
            return data[value > 0 ? 2 : 1];
        }

        public override string ToString()
        {
            int value;
            char endl = '\n';
            string dotline = "--------------------------\n";
            string summary = "";
            summary += (!string.IsNullOrEmpty(FusionAnvil) ? FusionAnvil + "의" + endl : null);
            summary += (StarTypeYellow ? "스타포스" : "놀장") + ": " + Star + "/" + MaxStar + endl;
            if(SoulEnchanted)
            {
                summary += Soul.Name.Replace(" 소울", "") + endl;
            }
            summary += Name + (ScrollUp > 0 ? " (+" + ScrollUp + ")" : null) + endl;
            var attrList = GetGearAttributeString();
            if(attrList.Count > 0)
            {
                foreach(string attrStr in attrList)
                {
                    summary += attrStr + ", ";
                }
                summary = summary.Remove(summary.Length - 2, 2);
                summary += endl;
            }
            if(TimeLimited >= DateTime.Parse("2003/4/29"))
            {
                summary += TimeLimited.ToString("yyyy년 M월 d일 HH시 mm분까지 사용가능") + endl;
            }
            if(Lock >= DateTime.Parse("2003/4/29"))
            {
                summary += Lock.ToString("yyyy년 M월 d일 HH시 mm분 봉인해제") + endl;
            }
            if(GreenLock)
            {
                summary += "그린 PC 잠금 적용";
            }
            summary += dotline;

            summary += "공격력 증가량: " + (Incline > 0 ? "+" : null) + Incline.ToString() + endl;
            Props.TryGetValue(GearPropType.reqLevel, out value);
            int reqLevel = value;
            AdditionalStat.TryGetValue(GearPropType.reduceReq, out value);
            summary += "REQ LEV : " + (reqLevel - value) + (value != 0 ? "(" + reqLevel + "-" + value + ")" : null) + endl;
            Props.TryGetValue(GearPropType.reqSTR, out value);
            summary += "REQ STR : " + value.ToString("D3") + endl;
            Props.TryGetValue(GearPropType.reqDEX, out value);
            summary += "REQ DEX : " + value.ToString("D3") + endl;
            Props.TryGetValue(GearPropType.reqINT, out value);
            summary += "REQ INT : " + value.ToString("D3") + endl;
            Props.TryGetValue(GearPropType.reqLUK, out value);
            summary += "REQ LUK : " + value.ToString("D3") + endl;

            string extraReq = StringHelper.GetExtraJobReqString(Type) ??
                (Props.TryGetValue(GearPropType.reqSpecJob, out value) ? StringHelper.GetExtraJobReqString(value) : null);
            Props.TryGetValue(GearPropType.reqJob, out int reqJob);
            string[] reqJobName = { "초보자", "전사", "마법사", "궁수", "도적", "해적" };
            for(int i = 0; i <= 5; i++)
            {
                bool enable;
                if(i == 0)
                {
                    enable = reqJob <= 0;
                    if(reqJob == 0) reqJob = 0x1f;// 0001 1111
                    if(reqJob == -1) reqJob = 0; // 0000 0000
                }
                else
                {
                    enable = (reqJob & (1 << (i - 1))) != 0;
                }
                if(enable)
                {
                    summary += reqJobName[i] + " ";
                }
                else
                {
                    summary += "-- ";
                }
            }
            summary = summary.Remove(summary.Length - 1, 1);
            summary += endl;
            if(extraReq != null)
            {
                summary += extraReq + endl;
            }
            summary += dotline;
            if(Props.TryGetValue(GearPropType.superiorEqp, out value) && value > 0)
            {
                summary += "슈페리얼" + endl;
            }
            if(Props.TryGetValue(GearPropType.growthLevel, out value) && value > 0)
            {
                string levelStr, expStr;
                if(value > 5)
                {
                    levelStr = "MAX";
                    expStr = "MAX";
                }
                else
                {
                    levelStr = value.ToString();
                    Props.TryGetValue(GearPropType.growthLevel, out value);
                    expStr = value.ToString();
                }
                summary += "성장 레벨 : " + levelStr + endl;
                summary += "성장 경험치: " + expStr + endl;
            }
            if(Props.TryGetValue(GearPropType.durability, out value))
            {
                summary += "내구도 : " + value + "%" + endl;
            }
            bool isWeapon = IsWeapon(Type);
            string typeStr = StringHelper.GetGearTypeString(Type);
            if(!string.IsNullOrEmpty(typeStr))
            {
                if(isWeapon)
                {
                    typeStr = "무기분류 : " + typeStr;
                }
                else
                {
                    typeStr = "장비분류 : " + typeStr;
                }
                if(IsLeftWeapon(Type) || Type == GearType.katara)
                {
                    typeStr += " (한손무기)";
                }
                else if(IsDoubleHandWeapon(Type))
                {
                    typeStr += " (두손무기)";
                }
                summary += typeStr + endl;
            }
            if(!Props.TryGetValue(GearPropType.attackSpeed, out value) && (Gear.IsWeapon(Type) || Type == GearType.katara))
            {
                value = 6;
            }
            if(value > 0)
            {
                summary += "공격속도 : " + StringHelper.GetAttackSpeedString(value) + " (" + value + ")" + endl;
            }

            List<GearPropType> stdProps = new List<GearPropType>();
            foreach(KeyValuePair<GearPropType, int> p in BaseStat)
            {
                if((int)p.Key < 100 && p.Value != 0)
                {
                    stdProps.Add(p.Key);
                }
            }
            foreach(KeyValuePair<GearPropType, int> p in AdditionalStat)
            {
                if(!stdProps.Contains(p.Key))
                {
                    if((int)p.Key < 100 && p.Value != 0)
                    {
                        stdProps.Add(p.Key);
                    }
                }
            }
            foreach(KeyValuePair<GearPropType, int> p in EnchantStat)
            {
                if(!stdProps.Contains(p.Key))
                {
                    if((int)p.Key < 100 && p.Value != 0)
                    {
                        stdProps.Add(p.Key);
                    }
                }
            }
            stdProps.Sort();
            foreach(GearPropType propType in stdProps)
            {
                int[] s = new int[3];
                BaseStat.TryGetValue(propType, out s[0]);
                AdditionalStat.TryGetValue(propType, out s[1]);
                EnchantStat.TryGetValue(propType, out s[2]);
                if(s[0] == 0 && s[1] == 0 && s[2] == 0) continue;
                string rate = null;
                switch(propType)
                {
                    case GearPropType.knockback:
                        summary += StringHelper.GetStatString(propType, s[0]) + endl; break;
                    case GearPropType.reduceReq:
                        summary += StringHelper.GetStatString(propType, s[0] + s[1]) + endl; break;
                    case GearPropType.incMHPr:
                    case GearPropType.incMMPr:
                    case GearPropType.bdR:
                    case GearPropType.imdR:
                    case GearPropType.damR:
                    case GearPropType.incAllStat:
                        rate = "%";
                        break;
                }
                if(propType != GearPropType.knockback && propType != GearPropType.reduceReq)
                {
                    summary += StringHelper.GetStatString(propType, 0) + ((s[0] + s[1] + s[2]) > 0 ? " +" : " ") + (s[0] + s[1] + s[2]) + rate;
                    if(s[1] != 0 || s[2] != 0)
                    {
                        summary += " (" + s[0] + rate + (s[1] >= 0 ? " +" : " ") + s[1] + rate + (s[2] >= 0 ? " +" : " ") + s[2] + rate + ")";
                    }
                    summary += endl;
                }
            }
            if(Props.TryGetValue(GearPropType.exceptUpgrade, out value) && value > 0)
            {
                summary += "강화불가" + endl;
            }
            else if(HasTuc)
            {
                summary += "업그레이드 가능 횟수 : " + ScrollAvailable + " (복구 가능 횟수 : " + ScrollRestore + ")" + endl;
            }
            if(HasTuc && Hammer > 0)
            {
                summary += "황금망치 제련 적용" + endl;
            }
            bool tradable = false;
            if(Props.TryGetValue(GearPropType.tradeBlock, out value) && value > 0)
            {
                tradable = true;
            }
            if(Props.TryGetValue(GearPropType.equipTradeBlock, out value) && value > 0)
            {
                tradable = true;
            }
            if(Props.TryGetValue(GearPropType.tradeOnce, out value) && value > 0)
            {
                tradable = true;
            }
            if(tradable && Props.TryGetValue(GearPropType.karmaLeft, out value) && value >= 0)
            {
                summary += StringHelper.GetGearPropString(GearPropType.karmaLeft, value) + endl;
            }
            int optionCount = 0;
            Props.TryGetValue(GearPropType.noPotential, out value);
            bool hasPotential = value <= 0;
            foreach(Potential potential in Options)
            {
                if(potential != null)
                {
                    optionCount++;
                }
            }
            if(hasPotential && optionCount > 0)
            {
                summary += dotline;
                summary += "잠재옵션 " + StringHelper.GetGearGradeString(Grade) + endl;
                foreach(Potential potential in Options)
                {
                    if(potential != null && !string.IsNullOrEmpty(potential.ToString()))
                    {
                        summary += "[" + potential.Code.ToString() + "] " + potential.ToString() + endl;
                    }
                }
            }
            int adOptionCount = 0;
            foreach(Potential potential in AdditionalOptions)
            {
                if(potential != null)
                {
                    adOptionCount++;
                }
            }
            if(hasPotential && adOptionCount > 0)
            {
                summary += dotline;
                summary += "에디셔널 잠재옵션 " + StringHelper.GetGearGradeString(AdditionGrade) + endl;
                foreach(Potential potential in AdditionalOptions)
                {
                    if(potential != null && !string.IsNullOrEmpty(potential.ToString()))
                    {
                        summary += "[" + potential.Code.ToString() + "] " + potential.ToString() + endl;
                    }
                }
            }
            if(SoulEnchanted)
            {
                summary += dotline;
                if(!string.IsNullOrEmpty(Soul.Name))
                {
                    int chargeAD = Soul.GetChargeAD();
                    summary += Soul.Name + " 적용" + endl;
                    summary += "소울 충전량 " + Soul.Charge + "/1000 (공:+" + chargeAD + ",마:+" + chargeAD + ")" + endl;
                    summary += Soul.OptionString + endl;
                    summary += "소울 충전 시 '" + Soul.SkillName + "' 사용가능" + endl;
                }
            }
            List<string> desc = new List<string>();
            if(!string.IsNullOrEmpty(Desc))
            {
                desc.Add(Desc);
            }
            if(Props.TryGetValue(GearPropType.tradeBlock, out value) && value > 0 && Props.TryGetValue(GearPropType.tradeAvailable, out value) && value > 0)
            {
                desc.Add(StringHelper.GetGearPropString(GearPropType.tradeAvailable, value));
            }
            GearPropType[] descTypes = new GearPropType[]{
                GearPropType.accountShareTag,
                GearPropType.jokerToSetItem,
            };
            foreach(GearPropType type in descTypes)
            {
                if(Props.TryGetValue(type, out value) && value > 0)
                {
                    desc.Add(StringHelper.GetGearPropString(type, value));
                }
            }
            string incline = null;
            GearPropType[] inclineTypes = new GearPropType[]{
                GearPropType.charismaEXP,
                GearPropType.insightEXP,
                GearPropType.willEXP,
                GearPropType.craftEXP,
                GearPropType.senseEXP,
                GearPropType.charmEXP
            };
            string[] inclineString = new string[]
            {
                "카리스마","통찰력","의지","손재주","감성","매력"
            };
            for(int i = 0; i < inclineTypes.Length; i++)
            {
                if(Props.TryGetValue(inclineTypes[i], out value) && value > 0)
                {
                    incline += ", " + inclineString[i] + " " + value;
                }
            }
            if(!string.IsNullOrEmpty(incline))
            {
                desc.Add("장착 시 1회에 한해 " + incline.Substring(2) + "의 경험치를 얻으실 수 있습니다.");
            }
            if(StarTypeYellow && Star > 0)
            {
                desc.Add("놀라운 장비강화 주문서가 사용되었습니다.");
            }
            // 주문서 효과
            GearPropType[] scrollTypes = new GearPropType[]{
                GearPropType.scrollProtect,
                GearPropType.scrollRecover,
                GearPropType.scrollReturn,
            };
            foreach(var type in scrollTypes)
            {
                if(Props.TryGetValue(type, out value) && value > 0)
                {
                    desc.Add(StringHelper.GetGearPropString(type, value));
                }
            }
            switch(Yggdrasil)
            {
                case 1: desc.Add("힘의 이그드라실의 축복 성공"); break;
                case 2: desc.Add("지력의 이그드라실의 축복 성공"); break;
                case 3: desc.Add("민첩성의 이그드라실의 축복 성공"); break;
                case 4: desc.Add("행운의 이그드라실의 축복 성공"); break;
            }
            if(desc.Count > 0)
            {
                summary += dotline;
                foreach(string s in desc)
                {
                    summary += s + endl;
                }
            }
            if(!string.IsNullOrEmpty(FusionAnvil))
            {
                summary += "신비의 모루에 의해 [" + FusionAnvil + "]의 외형이 합성됨" + endl;
            }

            return summary;
        }

        private List<string> GetGearAttributeString()
        {
            int value;
            List<string> tags = new List<string>();

            if(Props.TryGetValue(GearPropType.only, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.only, value));
            }
            if(Props.TryGetValue(GearPropType.quest, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.quest, value));
            }
            if(Props.TryGetValue(GearPropType.tradeBlock, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.tradeBlock, value));
            }
            if(Props.TryGetValue(GearPropType.equipTradeBlock, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.equipTradeBlock, value));
            }
            if(Props.TryGetValue(GearPropType.tradeOnce, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.tradeOnce, value));
            }
            if(Props.TryGetValue(GearPropType.accountSharable, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.accountSharable, value));
            }
            if(Props.TryGetValue(GearPropType.sharableOnce, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.sharableOnce, value));
            }
            if(Props.TryGetValue(GearPropType.onlyEquip, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.onlyEquip, value));
            }
            if(Props.TryGetValue(GearPropType.abilityTimeLimited, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.abilityTimeLimited, value));
                tags.Add(StringHelper.GetGearPropString(GearPropType.notExtend, value));
            }
            if(Props.TryGetValue(GearPropType.notExtend, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.notExtend, value));
            }
            if(Props.TryGetValue(GearPropType.blockGoldHammer, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.blockGoldHammer, value));
            }
            if(Props.TryGetValue(GearPropType.noPotential, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.noPotential, value));
            }
            if(Props.TryGetValue(GearPropType.fixedPotential, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.fixedPotential, value));
            }
            if(Props.TryGetValue(GearPropType.cantRepair, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.cantRepair, value));
            }

            return tags;
        }
    }
}
