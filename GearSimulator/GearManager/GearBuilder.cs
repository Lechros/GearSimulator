using System;
using System.Collections.Generic;
using System.Linq;

namespace GearManager
{
    public class GearBuilder
    {
        public GearBuilder()
        {
            Props = new Dictionary<GearPropType, int>();
            BaseStat = new Dictionary<GearPropType, int>();
            AdditionalStat = new Dictionary<GearPropType, int>();
            ScrollList = new List<Scroll>();
            StarStat = new Dictionary<GearPropType, int>();
            Options = new Potential[3];
            AdditionalOptions = new Potential[3];
            StarTypeYellow = true;
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if(_name != value)
                {
                    iconUpdated = false;
                    _name = value;
                }
            }
        }
        public BitmapOrigin IconRaw { get; set; }
        public GearType Type { get; set; }
        public string Desc { get; set; }
        public Dictionary<GearPropType, int> Props { get; set; }
        public int Incline { get; set; }
        public string NameTag { get; set; }
        private string _fusionAnvil;
        public string FusionAnvil
        {
            get { return _fusionAnvil; }
            set
            {
                if(_fusionAnvil != value)
                {
                    iconUpdated = false;
                    _fusionAnvil = value;
                }
            }
        }
        public DateTime TimeLimited { get; set; }
        private DateTime _lock;
        public DateTime Lock
        {
            get { return _lock; }
            set
            {
                _greenLock = false;
                _lock = value;
            }
        }
        private bool _greenLock;
        public bool GreenLock
        {
            get { return _greenLock; }
            set
            {
                _lock = default;
                _greenLock = value;
            }
        }

        public GearQuality Quality { get; set; }

        public bool StarTypeYellow { get; set; }
        public int Star { get; set; }
        public int MaxStar { get; set; }
        public int Tuc { get; set; }
        public int Hammer { get; set; }

        public Dictionary<GearPropType, int> BaseStat { get; set; }
        public Dictionary<GearPropType, int> AdditionalStat { get; set; }
        public List<Scroll> ScrollList { get; set; }
        public Dictionary<GearPropType, int> StarStat { get; set; }

        public GearGrade Grade { get; set; }
        public GearGrade AdditionGrade { get; set; }
        public Potential[] Options { get; set; }
        public Potential[] AdditionalOptions { get; set; }

        public bool SoulEnchanted { get; set; }
        public Soul Soul { get; set; }

        private bool iconUpdated = false;

        public static GearBuilder FromGearCode(int code)
        {
            return FromGear(GearResource.CreateGear(code));
        }

        public static GearBuilder FromGear(Gear gear)
        {
            if(gear == null) return new GearBuilder();

            GearBuilder gb = new GearBuilder
            {
                iconUpdated = true,
                Name = gear.Name,
                IconRaw = gear.IconRaw,
                Type = gear.Type,
                Desc = gear.Desc,
                Incline = gear.Incline,
                NameTag = gear.NameTag,
                FusionAnvil = gear.FusionAnvil,
                TimeLimited = gear.TimeLimited,
                Lock = gear.Lock,
                GreenLock = gear.GreenLock,
                Props = gear.Props.ToDictionary(x => x.Key, x => x.Value),
                MaxStar = gear.MaxStar,
                Tuc = gear.HasTuc ? (gear.ScrollUp + gear.ScrollAvailable + gear.ScrollRestore - gear.Hammer) : 0,
                BaseStat = gear.BaseStat.ToDictionary(x => x.Key, x => x.Value),
                Grade = gear.Grade,
                AdditionGrade = gear.AdditionGrade,
                Options = gear.Options.Clone() as Potential[],
                AdditionalOptions = gear.AdditionalOptions.Clone() as Potential[],
                SoulEnchanted = gear.SoulEnchanted,
                Soul = gear.Soul,
            };

            if(gb.Name.Contains("제네시스"))
            {
                int optionValue;
                if(gb.Name == "제네시스 에너지체인")
                {
                    if(gb.BaseStat.ContainsKey(GearPropType.incLUK))
                    {
                        optionValue = 3;
                    }
                    else
                    {
                        optionValue = 0;
                    }
                }
                else
                {
                    switch(gb.GetPropValue(GearPropType.reqJob))
                    {
                        case 1: optionValue = 0; break;
                        case 2: optionValue = 2; break;
                        case 4: optionValue = 1; break;
                        case 8: optionValue = 3; break;
                        case 16: optionValue = 0; break;
                        default: optionValue = 0; break;
                    }
                }
                for(int i = 0; i < 8; i++)
                {
                    gb.ScrollList.Add(new Scroll(ScrollType.spellTrace15, optionValue));
                }
                gb.ApplyStar(22, true, forceApply: true);
            }

            return gb;
        }

        public Gear ToGear()
        {
            if(!iconUpdated) SetIcon();
            Gear gear = new Gear();
            gear.Name = Name;
            gear.IconRaw = IconRaw.Clone();
            gear.Type = Type;
            gear.Desc = Desc;
            gear.Props = Props.ToDictionary(x => x.Key, x => x.Value);
            if(ScrollList.Any(scr => scr.Type == ScrollType.yggdrasilSTR || scr.Type == ScrollType.yggdrasilDEX || scr.Type == ScrollType.yggdrasilINT || scr.Type == ScrollType.yggdrasilLUK))
            {
                gear.Props[GearPropType.growthLevel] = 6;
                gear.Props[GearPropType.growthExp] = 100;
            }
            gear.Incline = Incline;
            gear.NameTag = NameTag;
            gear.FusionAnvil = FusionAnvil;
            gear.TimeLimited = TimeLimited;
            gear.Lock = Lock;
            gear.GreenLock = GreenLock;
            gear.Quality = GetGearQuality();
            gear.StarTypeYellow = StarTypeYellow;
            gear.Star = Star;
            gear.MaxStar = MaxStar;
            if(Tuc > 0)
            {
                gear.HasTuc = true;
                gear.ScrollUp = ScrollList.Count(scr => scr.Type != ScrollType.fail);
                gear.ScrollAvailable = Tuc + Hammer - ScrollList.Count;
                gear.ScrollRestore = ScrollList.Count(scr => scr.Type == ScrollType.fail);
                gear.Hammer = Hammer;
            }
            gear.BaseStat = BaseStat.ToDictionary(x => x.Key, x => x.Value);
            gear.AdditionalStat = AdditionalStat.ToDictionary(x => x.Key, x => x.Value);
            gear.EnchantStat = GetEnchantStat();

            gear.Grade = Grade;
            gear.AdditionGrade = AdditionGrade;
            gear.Options = Options.Clone() as Potential[];
            gear.AdditionalOptions = AdditionalOptions.Clone() as Potential[];

            if(Gear.IsWeapon(Type) && SoulEnchanted)
            {
                gear.SoulEnchanted = SoulEnchanted;
                gear.Soul = Soul;
            }

            return gear;
        }

        public void SetIcon()
        {
            BitmapOrigin icon;

            if(!string.IsNullOrEmpty(FusionAnvil))
            {
                icon = GearResource.GetGearIcon(FusionAnvil);
                if(!icon.IsEmpty)
                {
                    iconUpdated = true;
                    IconRaw = icon;
                    return;
                }
            }
            icon = GearResource.GetGearIcon(Name);
            if(!icon.IsEmpty)
            {
                iconUpdated = true;
                IconRaw = icon;
            }
        }

        public void ApplyAdditionalStat(GearPropType type, int grade, bool isDoubleAdd = false, bool bossReward = true)
        {
            if(grade <= 0) return;
            grade = Math.Max(0, Math.Min(7, grade));

            int reqLevel;
            if((reqLevel = GetPropValue(GearPropType.reqLevel)) > 0)
            {
                int statValue, value;
                switch(type)
                {
                    case GearPropType.incSTR:
                    case GearPropType.incDEX:
                    case GearPropType.incINT:
                    case GearPropType.incLUK:
                        statValue = isDoubleAdd ? ((reqLevel / 40 + 1) * grade) : ((reqLevel / 20 + 1) * grade);
                        break;
                    case GearPropType.incPDD:
                        statValue = (reqLevel / 20 + 1) * grade;
                        break;
                    case GearPropType.incPAD:
                    case GearPropType.incMAD:
                        if(Type == GearType.swordZB || Type == GearType.swordZL)
                        {
                            if(grade > 7) grade = 7;
                            BaseStat.TryGetValue(GearPropType.incPAD, out int weaponAtt);
                            if(Type == GearType.swordZB)
                            {
                                switch(weaponAtt)
                                {
                                    case 100: weaponAtt = 102; break;
                                    case 103: weaponAtt = 105; break;
                                    case 105: weaponAtt = 107; break;
                                    case 112: weaponAtt = 114; break;
                                    case 117: weaponAtt = 121; break;
                                    case 135: weaponAtt = 139; break;
                                    case 169: weaponAtt = 173; break;
                                    case 203: weaponAtt = 207; break;
                                    case 293: weaponAtt = 297; break;
                                    case 337: weaponAtt = 342; break;
                                }
                            }
                            double[] data = new double[] { 1, 2.2222, 3.63, 5.325, 7.32, 8.7777, 10.25 };
                            value = (reqLevel > 180) ? 6 : (reqLevel > 160) ? 5 : (reqLevel > 110) ? 4 : 3;
                            statValue = (int)Math.Ceiling(weaponAtt * data[grade - 1] * value / 100);
                        }
                        else if(Gear.IsWeapon(Type))
                        {
                            BaseStat.TryGetValue(GearPropType.incPAD, out int basePAD);
                            BaseStat.TryGetValue(GearPropType.incMAD, out int baseMAD);
                            bool useMAD = baseMAD > 0;
                            int weaponAtt = (type == GearPropType.incMAD) ? baseMAD : basePAD;
                            if(bossReward)
                            {
                                if(grade < 3) grade += 2;
                                double[] data = new double[] { 1, 1.4666, 2.0166, 2.663, 3.4166 };
                                value = (reqLevel > 160) ? 18 : (reqLevel > 150) ? 15 : (reqLevel > 110) ? 12 : 9;
                                statValue = (int)Math.Ceiling((useMAD ? weaponAtt : basePAD) * data[grade - 3] * value / 100);
                            }
                            else
                            {
                                double[] data = new double[] { 1, 2.2222, 3.63, 5.325, 7.32, 8.7777, 10.25 };
                                value = (reqLevel > 110) ? 4 : 3;
                                statValue = (int)Math.Ceiling((useMAD ? weaponAtt : basePAD) * data[grade - 1] * value / 100);
                            }
                        }
                        else
                            statValue = grade;
                        break;
                    case GearPropType.incMHP:
                    case GearPropType.incMMP:
                        statValue = reqLevel * 3 * grade;
                        break;
                    case GearPropType.incMHPr:
                    case GearPropType.incMMPr:
                    case GearPropType.incSpeed:
                    case GearPropType.incJump:
                    case GearPropType.imdR:
                    case GearPropType.damR:
                    case GearPropType.incAllStat:
                        statValue = grade;
                        break;
                    case GearPropType.bdR:
                        statValue = 2 * grade;
                        break;
                    case GearPropType.reduceReq:
                        statValue = Math.Min(5 * grade, reqLevel);
                        break;
                    default:
                        return;
                }
                if(!AdditionalStat.ContainsKey(type))
                    AdditionalStat.Add(type, 0);
                AdditionalStat[type] += statValue;
            }
        }

        public bool ApplyHammer()
        {
            if(GetPropValue(GearPropType.blockGoldHammer) > 0) return false;
            if(Tuc <= 0) return false;
            if(Hammer == 0)
            {
                Hammer = 1;
                return true;
            }
            return false;
        }

        public bool ApplyScroll(Scroll scroll, int count = 1)
        {
            if(GetPropValue(GearPropType.exceptUpgrade) > 0) return false;

            count = Math.Min(count, Tuc + Hammer - ScrollList.Count);
            if(count < 1) return false;

            if(ScrollType.yggdrasilSTR <= scroll.Type && scroll.Type <= ScrollType.yggdrasilLUK)
            {
                if(!ScrollList.Any(scr => scr.Type == ScrollType.yggdrasilSTR || scr.Type == ScrollType.yggdrasilDEX || scr.Type == ScrollType.yggdrasilINT || scr.Type == ScrollType.yggdrasilLUK))
                {
                    ScrollList.Add(scroll);
                    return true;
                }
                return false;
            }

            for(int i = 0; i < count; i++)
            {
                ScrollList.Add(scroll);
            }
            return true;
        }

        public bool ResetScroll()
        {
            if(Hammer == 0 && ScrollList.Count == 0) return false;
            Hammer = 0;
            ScrollList.Clear();
            return true;
        }

        public bool ApplyStar(int star, bool starTypeYellow = true, int reqLevel = -1, int bonusRate = 0, bool forceApply = false)
        {
            if(!forceApply && GetPropValue(GearPropType.exceptUpgrade) > 0) return false;

            if(reqLevel < 0) reqLevel = GetPropValue(GearPropType.reqLevel);
            int maxStar = CalculateMaxStar(reqLevel);

            StarStat.Clear();
            if(maxStar == 0)
            {
                Star = 0;
                StarTypeYellow = true;
                return false;
            }
            if(star <= 0)
            {
                Star = 0;
                StarTypeYellow = true;
                return true;
            }

            bool superiorEqp = GetPropValue(GearPropType.superiorEqp) > 0;
            bool isWeaponStat = Gear.IsWeapon(Type) || Type == GearType.katara;

            // superior
            if(superiorEqp)
            {
                int[][] data = new int[2][]
                {
                    GetStarStat(2, 0, isWeaponStat, reqLevel),
                    GetStarStat(2, 1, isWeaponStat, reqLevel),
                };
                star = Math.Min(15, star);
                star = Math.Min(maxStar, star);

                // stat
                GearPropType[] statTypes = new GearPropType[]
                {
                    GearPropType.incSTR, GearPropType.incDEX, GearPropType.incINT, GearPropType.incLUK
                };
                foreach(GearPropType type in statTypes)
                {
                    StarStat[type] = data[0].Skip(1).Take(star).Sum();
                }
                // attack
                StarStat[GearPropType.incPAD] = StarStat[GearPropType.incMAD] = data[1].Skip(1).Take(star).Sum();

                // PDD
                BaseStat.TryGetValue(GearPropType.incPDD, out int basePDD);
                GetScrollStat().TryGetValue(GearPropType.incPDD, out int scrollPDD);
                int initPDD = basePDD + scrollPDD, PDD = initPDD;
                for(int i = 0; i < star; i++)
                {
                    PDD += PDD / 20 + 1;
                }
                StarStat[GearPropType.incPDD] = PDD - initPDD;
            }
            // end superior
            // starforce
            else if(starTypeYellow)
            {
                int[][] data = new int[2][]
                {
                    GetStarStat(0, 0, isWeaponStat, reqLevel),
                    GetStarStat(0, 1, isWeaponStat, reqLevel),
                };
                int[] gloveAttackData = new int[] { 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] speedJumpData = new int[] { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] MHPData = new int[] { 5, 5, 5, 10, 10, 15, 15, 20, 20, 25, 25, 25, 25, 25, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int reqJob = GetPropValue(GearPropType.reqJob);

                star = Math.Min(25, star);
                maxStar = Math.Max(maxStar, star);

                // stat
                GearPropType[] statTypes = new GearPropType[]
                {
                    GearPropType.incSTR, GearPropType.incDEX, GearPropType.incINT, GearPropType.incLUK
                };
                GearPropType[][] jobStat = new GearPropType[][]
                {
                    new[]{ GearPropType.incSTR, GearPropType.incDEX },
                    new[]{ GearPropType.incINT, GearPropType.incLUK },
                    new[]{ GearPropType.incDEX, GearPropType.incSTR },
                    new[]{ GearPropType.incLUK, GearPropType.incDEX },
                    new[]{ GearPropType.incSTR, GearPropType.incDEX },
                };
                HashSet<GearPropType> statSet = new HashSet<GearPropType>();
                if(reqJob == 0) reqJob = 0x1f;
                for(int i = 0; i < 5; i++)
                    if((reqJob & (1 << i)) != 0)
                        foreach(GearPropType type in jobStat[i])
                            statSet.Add(type);

                Dictionary<GearPropType, int> scrollStat = GetScrollStat();
                foreach(var type in statTypes)
                {
                    int statValue = 0;
                    if(statSet.Contains(type))
                    {
                        statValue = data[0].Skip(1).Take(star).Sum();
                    }
                    else if((scrollStat.ContainsKey(type) && scrollStat[type] > 0)
                            || (BaseStat.ContainsKey(type) && BaseStat[type] > 0))
                    {
                        statValue = data[0].Skip(1 + 15).Take(star - 15).Sum();
                    }
                    StarStat[type] = statValue;
                }

                // attack
                // weapon
                if(isWeaponStat)
                {
                    BaseStat.TryGetValue(GearPropType.incPAD, out int basePAD);
                    GetScrollStat().TryGetValue(GearPropType.incPAD, out int scrollPAD);
                    BaseStat.TryGetValue(GearPropType.incMAD, out int baseMAD);
                    GetScrollStat().TryGetValue(GearPropType.incMAD, out int scrollMAD);
                    bool useMAD = (reqJob == 0) || (reqJob / 2 % 2 == 1) || (scrollMAD > 0);
                    int initPAD = basePAD + scrollPAD, initMAD = baseMAD + scrollMAD;
                    int PAD = initPAD, MAD = initMAD;

                    for(int i = 0; i < star; i++)
                    {
                        if(i < 15)
                        {
                            PAD += PAD > 0 ? PAD / 50 + 1 : 0;
                            MAD += useMAD ? (MAD / 50 + 1) : 0;
                        }
                        else
                        {
                            PAD += data[1][i + 1];
                            MAD += useMAD ? data[1][i + 1] : 0;
                        }
                    }
                    StarStat[GearPropType.incPAD] = PAD - initPAD;
                    StarStat[GearPropType.incMAD] = MAD - initMAD;
                }
                // not weapon
                else
                {
                    bool useMAD = (reqJob == 0) || (reqJob / 2 % 2 == 1);
                    int attackValue = data[1].Skip(1).Take(star).Sum();
                    int gloveBonus = (Type == GearType.glove) ? gloveAttackData.Take(star).Sum() : 0;
                    StarStat[GearPropType.incPAD] = StarStat[GearPropType.incMAD] = attackValue;
                    if(reqJob == 0)
                    {
                        StarStat[GearPropType.incPAD] += gloveBonus;
                        StarStat[GearPropType.incMAD] += gloveBonus;
                    }
                    else if(useMAD)
                        StarStat[GearPropType.incMAD] += gloveBonus;
                    else
                        StarStat[GearPropType.incPAD] += gloveBonus;
                }

                // PDD
                if(!isWeaponStat && Type != GearType.machineHeart)
                {
                    BaseStat.TryGetValue(GearPropType.incPDD, out int basePDD);
                    GetScrollStat().TryGetValue(GearPropType.incPDD, out int scrollPDD);
                    int initPDD = basePDD + scrollPDD, PDD = initPDD;
                    for(int i = 0; i < star; i++)
                    {
                        PDD += PDD / 20 + 1;
                    }
                    StarStat[GearPropType.incPDD] = PDD - initPDD;
                }

                // MHP
                GearType[] MHPgears = new GearType[]
                {
                    GearType.cap, GearType.coat, GearType.longcoat, GearType.pants, GearType.cape, GearType.ring, GearType.pendant, GearType.belt, GearType.shoulderPad, GearType.shield,
                };
                if(isWeaponStat)
                    StarStat[GearPropType.incMHP] = StarStat[GearPropType.incMMP] = MHPData.Take(star).Sum();
                else if(MHPgears.Contains(Type))
                    StarStat[GearPropType.incMHP] = MHPData.Take(star).Sum();

                // Speed, Jump
                if(Type == GearType.shoes)
                    StarStat[GearPropType.incJump] = StarStat[GearPropType.incSpeed] = speedJumpData.Take(star).Sum();
            }
            // end starforce
            // amazing scroll
            else
            {
                int[][] data = new int[2][]
                {
                    GetStarStat(1, 0, isWeaponStat, reqLevel),
                    GetStarStat(1, 1, isWeaponStat, reqLevel),
                };
                int[] bonusStatData = new int[] { 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };

                star = Math.Min(15, star);
                maxStar = Math.Min(15, maxStar);
                bonusRate = Math.Max(0, Math.Min(100, bonusRate));

                // stat
                GearPropType[] statTypes = new GearPropType[]
                {
                    GearPropType.incSTR, GearPropType.incDEX, GearPropType.incINT, GearPropType.incLUK
                };
                Dictionary<GearPropType, int> stats = MergeStatDict(BaseStat, MergeStatDict(AdditionalStat, GetScrollStat()));
                foreach(GearPropType type in statTypes)
                {
                    if(stats.ContainsKey(type) && stats[type] > 0)
                    {
                        int bonusStat = Gear.IsAccessory(Type) ? (bonusStatData.Take(star).Sum() * bonusRate / 100) : 0;
                        StarStat[type] = data[0].Skip(1).Take(star).Sum() + bonusStat;
                    }
                }

                // attack
                if(isWeaponStat)
                {
                    BaseStat.TryGetValue(GearPropType.incPAD, out int basePAD);
                    AdditionalStat.TryGetValue(GearPropType.incPAD, out int additionPAD);
                    GetScrollStat().TryGetValue(GearPropType.incPAD, out int scrollPAD);
                    BaseStat.TryGetValue(GearPropType.incMAD, out int baseMAD);
                    AdditionalStat.TryGetValue(GearPropType.incMAD, out int additionMAD);
                    GetScrollStat().TryGetValue(GearPropType.incMAD, out int scrollMAD);
                    int initPAD = basePAD + additionPAD + scrollPAD, initMAD = baseMAD + additionMAD + scrollMAD;
                    int PAD = initPAD, MAD = initMAD;

                    for(int i = 0; i < star; i++)
                    {
                        int bonusAttack = (bonusRate * i / 100 != bonusRate * (i + 1) / 100) ? 1 : 0;
                        if(initPAD > 0) PAD += PAD / 50 + 1 + data[1][i + 1] + bonusAttack;
                        if(initMAD > 0) MAD += MAD / 50 + 1 + data[1][i + 1] + bonusAttack;
                    }
                    StarStat[GearPropType.incPAD] = PAD - initPAD;
                    StarStat[GearPropType.incMAD] = MAD - initMAD;
                }
                else
                {
                    GearPropType[] attackTypes = new GearPropType[]
                    {
                        GearPropType.incPAD, GearPropType.incMAD
                    };
                    int bonusStat = (Type == GearType.shield) ? (star * bonusRate / 100) : 0;
                    foreach(GearPropType type in attackTypes)
                    {
                        if(stats.ContainsKey(type) && stats[type] > 0)
                        {
                            StarStat[type] = data[1].Skip(1).Take(star).Sum() + bonusStat;
                        }
                    }
                }

                // PDD
                BaseStat.TryGetValue(GearPropType.incPDD, out int basePDD);
                AdditionalStat.TryGetValue(GearPropType.incPDD, out int additionPDD);
                GetScrollStat().TryGetValue(GearPropType.incPDD, out int scrollPDD);
                int initPDD = basePDD + additionPDD + scrollPDD, PDD = initPDD;
                if(initPDD > 0)
                {
                    for(int i = 0; i < star; i++)
                    {
                        PDD += PDD / 20 + 1;
                    }
                    StarStat[GearPropType.incPDD] = PDD - initPDD;
                }
                // MHP bonus
                StarStat[GearPropType.incPDD] = Gear.IsArmor(Type) ? (50 * star * bonusRate / 100) : 0;
            }

            MaxStar = maxStar;
            Star = star;
            StarTypeYellow = starTypeYellow;
            return true;
        }

        // / <summary>
        // / Return star increment data in int[] format.
        // / </summary>
        // / <param name="starType"></param> 0: Starforce, 1: Amazing enchant, 2: Superior starforce
        // / <param name="propType"></param> 0: Stat, 1: Attack
        // / <param name="isWeapon"></param> True if GearType == Weapon or katara
        // / <param name="reqLevel"></param> ReqLevel prop of Gear
        // / <returns>int array containing data</returns>
        private int[] GetStarStat(int starType, int propType, bool isWeapon, int reqLevel)
        {
            int[][] superiorStatData = new int[][]
            {
                    new[]{ 0, 1, 2, 4, 7, 11, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 80, 2, 3, 5, 8, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 110, 9, 10, 12, 15, 19, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 150, 19, 20, 22, 25, 29, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };
            int[][] superiorAttackData = new int[][]
            {
                    new[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 110, 0, 0, 0, 0, 0, 5, 6, 7, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 150, 0, 0, 0, 0, 0, 9, 10, 11, 12, 13, 15, 17, 19, 21, 23 },
            };
            int[][] yellowStatData = new int[][]
            {
                    new[]{ 0, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
                    new[]{ 95, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
                    new[]{ 110, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0 },
                    new[]{ 120, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 5, 5, 5, 5, 5, 5, 5, 0, 0, 0 },
                    new[]{ 130, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 7, 7, 7, 7, 7, 7, 7, 0, 0, 0 },
                    new[]{ 140, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 9, 9, 9, 9, 9, 9, 9, 0, 0, 0 },
                    new[]{ 150, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 11, 11, 11, 11, 11, 11, 11, 0, 0, 0 },
                    new[]{ 160, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 13, 13, 13, 13, 13, 13, 13, 0, 0, 0 },
                    new[]{ 200, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 15, 15, 15, 15, 15, 15, 15, 0, 0, 0 },
            };
            int[][] yellowAttackData = new int[][]
            {
                    new[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 95, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 110, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 6, 7, 8, 9, 10, 12, 13, 15, 17 },
                    new[]{ 120, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 7, 8, 9, 10, 11, 13, 14, 16, 18 },
                    new[]{ 130, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 8, 9, 10, 11, 12, 14, 16, 18, 20 },
                    new[]{ 140, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 9, 10, 11, 12, 13, 15, 17, 19, 21 },
                    new[]{ 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9, 10, 11, 12, 13, 14, 16, 18, 20, 22 },
                    new[]{ 160, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 11, 12, 13, 14, 15, 17, 19, 21, 23 },
                    new[]{ 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 13, 14, 15, 16, 17, 19, 21, 23, 25 },
            };
            int[][] yellowWeaponAttackData = new int[][]
            {
                    new[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 95, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 110, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 5, 5, 6, 7, 8, 9, 27, 28, 29 },
                    new[]{ 120, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 6, 6, 7, 8, 9, 10, 28, 29, 30 },
                    new[]{ 130, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 7, 7, 8, 9, 10, 11, 29, 30, 31 },
                    new[]{ 140, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 8, 8, 9, 10, 11, 12, 30, 31, 32 },
                    new[]{ 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 9, 9, 10, 11, 12, 13, 31, 32, 33 },
                    new[]{ 160, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9, 9, 10, 11, 12, 13, 14, 32, 33, 34 },
                    new[]{ 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 13, 13, 14, 14, 15, 16, 17, 34, 35, 36 },
            };
            int[][] amazingStatData = new int[][] {
                    new[]{ 0, 1, 2, 4, 7, 11, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 80, 2, 3, 5, 8, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 85, 3, 4, 6, 9, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 90, 4, 5, 7, 10, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 95, 5, 6, 8, 11, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 100, 7, 8, 10, 13, 17, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 105, 8, 9, 11, 14, 18, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 110, 9, 10, 12, 15, 19, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 115, 10, 11, 13, 16, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 120, 12, 13, 15, 18, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 125, 13, 14, 16, 19, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 130, 14, 15, 17, 20, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 135, 15, 16, 18, 21, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 140, 17, 18, 20, 23, 27, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 145, 18, 19, 21, 24, 28, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[]{ 150, 19, 20, 22, 25, 29, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };
            int[][] amazingAttackData = new int[][] {
                    new[]{ 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 8, 10, 12, 14 },
                    new[]{ 80, 0, 0, 0, 0, 0, 2, 3, 4, 5, 6, 7, 9, 11, 13, 15 },
                    new[]{ 90, 0, 0, 0, 0, 0, 3, 4, 5, 6, 7, 8, 10, 12, 14, 16 },
                    new[]{ 100, 0, 0, 0, 0, 0, 4, 5, 6, 7, 8, 9, 11, 13, 15, 17 },
                    new[]{ 110, 0, 0, 0, 0, 0, 5, 6, 7, 8, 9, 10, 12, 14, 16, 18 },
                    new[]{ 120, 0, 0, 0, 0, 0, 6, 7, 8, 9, 10, 11, 13, 15, 17, 19 },
                    new[]{ 130, 0, 0, 0, 0, 0, 7, 8, 9, 10, 11, 12, 14, 16, 18, 20 },
                    new[]{ 140, 0, 0, 0, 0, 0, 8, 9, 10, 11, 12, 13, 15, 17, 19, 21 },
                    new[]{ 150, 0, 0, 0, 0, 0, 9, 10, 11, 12, 13, 14, 16, 18, 20, 22 },
            };
            int[][] data = null;
            int[] dataItem = null;
            switch(starType)
            {
                case 0:
                    if(propType == 0) data = yellowStatData;
                    else
                    {
                        if(isWeapon) data = yellowWeaponAttackData;
                        else data = yellowAttackData;
                    }
                    break;
                case 1:
                    if(propType == 0) data = amazingStatData;
                    else data = amazingAttackData;
                    break;
                case 2:
                    if(propType == 0) data = superiorStatData;
                    else data = superiorAttackData;
                    break;
            }
            foreach(int[] item in data)
            {
                if(reqLevel >= item[0]) dataItem = item;
                else break;
            }
            return dataItem;
        }

        public int GetPropValue(GearPropType propType)
        {
            if(Props.TryGetValue(propType, out int value)) return value;
            else return 0;
        }

        public int CalculateMaxStar(int reqLevel = -1)
        {
            if(Tuc <= 0) return 0;
            if(Gear.IsMechanicGear(Type) || Gear.IsDragonGear(Type)) return 0;


            int[][] starData = new int[][]
            {
                new[]{ 0, 5, 3 },
                new[]{ 95, 8, 5 },
                new[]{ 108, 10, 8 },
                new[]{ 118, 15, 10 },
                new[]{ 128, 20, 12 },
                new[]{ 138, 25, 15 },
            };

            if(reqLevel < 0) reqLevel = GetPropValue(GearPropType.reqLevel);
            int[] data = null;
            foreach(int[] item in starData)
            {
                if(reqLevel >= item[0]) data = item;
                else break;
            }
            if(data == null) return 0;

            return data[(GetPropValue(GearPropType.superiorEqp) > 0) ? 2 : 1];
        }

        public Dictionary<GearPropType, int> GetScrollStat()
        {
            Props.TryGetValue(GearPropType.reqLevel, out int reqLevel);
            Props.TryGetValue(GearPropType.reqJob, out int reqJob);
            int attFlag = (reqJob == 0) ? -1 : 1;

            Dictionary<GearPropType, int> scrollStat = new Dictionary<GearPropType, int>();
            for(int i = 0; i < ScrollList.Count; i++)
            {
                int fourthScrollFlag = (i == 3) ? attFlag : 0;
                ScrollList[i].GetStat(Type, reqLevel, fourthScrollFlag).ToList()
                    .ForEach(s => scrollStat[s.Key] = s.Value + (scrollStat.ContainsKey(s.Key) ? scrollStat[s.Key] : 0));
            }
            int ALL;
            if(scrollStat.ContainsKey(GearPropType.incAllStat) && (ALL = scrollStat[GearPropType.incAllStat]) != 0)
            {
                scrollStat[GearPropType.incSTR] = ALL + (scrollStat.ContainsKey(GearPropType.incSTR) ? scrollStat[GearPropType.incSTR] : 0);
                scrollStat[GearPropType.incDEX] = ALL + (scrollStat.ContainsKey(GearPropType.incDEX) ? scrollStat[GearPropType.incDEX] : 0);
                scrollStat[GearPropType.incINT] = ALL + (scrollStat.ContainsKey(GearPropType.incINT) ? scrollStat[GearPropType.incINT] : 0);
                scrollStat[GearPropType.incLUK] = ALL + (scrollStat.ContainsKey(GearPropType.incLUK) ? scrollStat[GearPropType.incLUK] : 0);
                scrollStat.Remove(GearPropType.incAllStat);
            }

            foreach(var type in scrollStat.ToList())
            {
                if(scrollStat[type.Key] == 0)
                {
                    scrollStat.Remove(type.Key);
                }
            }

            return scrollStat;
        }

        public Dictionary<GearPropType, int> GetEnchantStat()
        {
            var scrollStat = GetScrollStat();
            foreach(var stat in scrollStat.ToList())
            {
                if(stat.Value < 0)
                {
                    if(BaseStat.ContainsKey(stat.Key) && BaseStat[stat.Key] + stat.Value < 0)
                    {
                        scrollStat[stat.Key] = -BaseStat[stat.Key];
                    }
                }
            }
            return MergeStatDict(scrollStat, StarStat);

        }

        private static Dictionary<GearPropType, int> MergeStatDict(Dictionary<GearPropType, int> stat1, Dictionary<GearPropType, int> stat2)
        {
            Dictionary<GearPropType, int> resStat = new Dictionary<GearPropType, int>();
            stat1.ToList().ForEach(x => resStat[x.Key] = x.Value);
            stat2.ToList().ForEach(x => resStat[x.Key] = x.Value + (resStat.ContainsKey(x.Key) ? resStat[x.Key] : 0));

            return resStat;
        }

        public GearQuality GetGearQuality()
        {
            int diff = 0;
            foreach(var s in MergeStatDict(AdditionalStat, GetEnchantStat()))
            {
                diff += s.Value / GetPropTypeWeight(s.Key);
            }

            if(diff < 0)
                return GearQuality.Low;
            if(diff < 6)
            {
                // 업횟없으면 흰색
                if(ScrollList.Count(scr => scr.Type != ScrollType.fail) > 0)
                    return GearQuality.Premium;
                else
                    return GearQuality.Middle;
            }
            if(diff < 23)
                return GearQuality.High;
            if(diff < 40)
                return GearQuality.Top;
            if(diff < 55)
                return GearQuality.Premium;
            if(diff < 70)
                return GearQuality.Special;
            return GearQuality.Excellent;
        }

        private static int GetPropTypeWeight(GearPropType type)
        {
            if((int)type < 100)
            {
                switch(type)
                {
                    case GearPropType.incSTR:
                    case GearPropType.incDEX:
                    case GearPropType.incINT:
                    case GearPropType.incLUK:
                    case GearPropType.incPAD:
                    case GearPropType.incMAD:
                    case GearPropType.incSpeed:
                    case GearPropType.incJump:
                        return 1;
                    case GearPropType.incMHP:
                    case GearPropType.incMMP:
                        return 100;
                    case GearPropType.incPDD:
                        return 10;
                }
            }
            return int.MaxValue;
        }
    }
}
