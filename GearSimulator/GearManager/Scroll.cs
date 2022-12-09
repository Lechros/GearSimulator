using System;
using System.Collections.Generic;

namespace GearManager
{
    public struct Scroll
    {
        public Scroll(ScrollType type, int optionValue, int[] stat)
        {
            Type = type;
            if(type == ScrollType.chaos || type == ScrollType.incredibleChaosOfGoodness)
            {
                OptionValue = 0;
                GearPropType[] typeData = new GearPropType[] {
                    GearPropType.incSTR, GearPropType.incDEX, GearPropType.incINT, GearPropType.incLUK, GearPropType.incMHP, GearPropType.incMMP, GearPropType.incPAD, GearPropType.incMAD, GearPropType.incPDD, GearPropType.incSpeed, GearPropType.incJump
                };
                ChaosStat = new Dictionary<GearPropType, int>();
                for(int i = 0; i < Math.Min(typeData.Length, stat.Length); i++)
                    if(stat[i] != 0)
                        ChaosStat.Add(typeData[i], stat[i]);
            }
            else
            {
                OptionValue = optionValue;
                ChaosStat = null;
            }
        }

        public Scroll(ScrollType type, int optionValue, Dictionary<GearPropType, int> stat)
        {
            Type = type;
            if(type == ScrollType.chaos || type == ScrollType.incredibleChaos || type == ScrollType.incredibleChaosOfGoodness)
            {
                OptionValue = 0;
                ChaosStat = stat;
            }
            else
            {
                OptionValue = optionValue;
                ChaosStat = null;
            }
        }

        public Scroll(ScrollType type, int[] stat)
        {
            Type = type;
            OptionValue = 0;
            if(type == ScrollType.chaos || type == ScrollType.incredibleChaos || type == ScrollType.incredibleChaosOfGoodness)
            {
                GearPropType[] typeData = new GearPropType[] {
                    GearPropType.incSTR, GearPropType.incDEX, GearPropType.incINT, GearPropType.incLUK, GearPropType.incMHP, GearPropType.incMMP, GearPropType.incPAD, GearPropType.incMAD, GearPropType.incPDD, GearPropType.incSpeed, GearPropType.incJump
                };
                ChaosStat = new Dictionary<GearPropType, int>();
                for(int i = 0; i < Math.Min(typeData.Length, stat.Length); i++)
                    if(stat[i] != 0)
                        ChaosStat.Add(typeData[i], stat[i]);
            }
            else
                ChaosStat = null;
        }

        public Scroll(ScrollType type, Dictionary<GearPropType, int> stat)
        {
            Type = type;
            OptionValue = 0;
            if(type == ScrollType.chaos || type == ScrollType.incredibleChaos || type == ScrollType.incredibleChaosOfGoodness)
                ChaosStat = stat;
            else
                ChaosStat = null;
        }

        public Scroll(ScrollType type, int optionValue)
        {
            Type = type;
            OptionValue = optionValue;
            if(type == ScrollType.chaos || type == ScrollType.incredibleChaos || type == ScrollType.incredibleChaosOfGoodness)
                ChaosStat = new Dictionary<GearPropType, int>();
            else
                ChaosStat = null;
        }

        public Scroll(ScrollType type)
        {
            Type = type;
            OptionValue = 0;
            if(type == ScrollType.chaos || type == ScrollType.incredibleChaos || type == ScrollType.incredibleChaosOfGoodness)
                ChaosStat = new Dictionary<GearPropType, int>();
            else
                ChaosStat = null;
        }

        public ScrollType Type { get; private set; }
        public int OptionValue { get; private set; }
        public Dictionary<GearPropType, int> ChaosStat { get; private set; }

        public Dictionary<GearPropType, int> GetStat(GearType gearType, int reqLevel, int isFourthScrollApply = 0)
        {
            switch(Type)
            {
                case ScrollType.spellTrace100:
                case ScrollType.spellTrace70:
                case ScrollType.spellTrace30:
                case ScrollType.spellTrace15:
                    int levelRange = reqLevel > 110 ? 2 : (reqLevel > 70 ? 1 : 0);
                    GearPropType statType;
                    switch(OptionValue)
                    {
                        case 0: statType = GearPropType.incSTR; break;
                        case 1: statType = GearPropType.incDEX; break;
                        case 2: statType = GearPropType.incINT; break;
                        case 3: statType = GearPropType.incLUK; break;
                        case 4: statType = GearPropType.incMHP; break;
                        default:
                            return new Dictionary<GearPropType, int>();
                    }
                    GearPropType attackType = OptionValue == 2 ? GearPropType.incMAD : GearPropType.incPAD;
                    if(Gear.IsWeapon(gearType) || gearType == GearType.katara)
                    {
                        int[][][] statData = new int[][][]
                        {
                            new int[][]
                            {
                                new int[]{ 1, 0 },
                                new int[]{ 2, 0 },
                                new int[]{ 3, 1 },
                                new int[]{ 5, 2 },
                            },
                            new int[][]
                            {
                                new int[]{ 2, 0 },
                                new int[]{ 3, 1 },
                                new int[]{ 5, 2 },
                                new int[]{ 7, 3 },
                            },
                            new int[][]
                            {
                                new int[]{ 3, 1 },
                                new int[]{ 5, 2 },
                                new int[]{ 7, 3 },
                                new int[]{ 9, 4 },
                            },
                        };
                        int[] data = statData[levelRange][(int)Type];

                        if(statType == GearPropType.incMHP)
                            return new Dictionary<GearPropType, int> { [attackType] = data[0], [GearPropType.incMHP] = data[1] * 50 };
                        else
                            return new Dictionary<GearPropType, int> { [attackType] = data[0], [statType] = data[1] };
                    }
                    else if(Type == ScrollType.spellTrace15) return new Dictionary<GearPropType, int>();
                    else if(gearType == GearType.glove)
                    {
                        levelRange = levelRange == 2 ? 1 : levelRange;
                        int[][] statData = new int[][] {
                            new int[]{ 0, 1, 2 },
                            new int[]{ 1, 2, 3 },
                        };
                        int value = statData[levelRange][(int)Type];

                        if(value == 0)
                            return new Dictionary<GearPropType, int> { [GearPropType.incPDD] = 3 };
                        else
                            return new Dictionary<GearPropType, int> { [attackType] = value };
                    }
                    else if(Gear.IsArmor(gearType))
                    {
                        int[][][] statData = new int[][][]
                        {
                            new int[][]
                            {
                                new int[]{ 1, 5, 1 },
                                new int[]{ 2, 15, 2 },
                                new int[]{ 3, 30, 4 },
                            },
                            new int[][]
                            {
                                new int[]{ 2, 20, 2 },
                                new int[]{ 3, 40, 4 },
                                new int[]{ 5, 70, 7 },
                            },
                            new int[][]
                            {
                                new int[]{ 3, 30, 3 },
                                new int[]{ 4, 70, 5 },
                                new int[]{ 7, 120, 10 },
                            },
                        };
                        int[] data = statData[levelRange][(int)Type];
                        Dictionary<GearPropType, int> scrollStat;
                        if(statType == GearPropType.incMHP)
                            scrollStat = new Dictionary<GearPropType, int> { [GearPropType.incMHP] = data[0] * 50 + data[1], [GearPropType.incPDD] = data[2] };
                        else
                            scrollStat = new Dictionary<GearPropType, int> { [statType] = data[0], [GearPropType.incMHP] = data[1], [GearPropType.incPDD] = data[2] };

                        if(isFourthScrollApply > 0)
                        {
                            scrollStat.Add(attackType, 1);
                        }
                        else if(isFourthScrollApply < 0)
                        {
                            scrollStat.Add(GearPropType.incPAD, 1);
                            scrollStat.Add(GearPropType.incMAD, 1);
                        }
                        return scrollStat;
                    }
                    else if(Gear.IsAccessory(gearType))
                    {
                        int[][] statData = new int[][]
                        {
                            new int[]{ 1, 2, 3 },
                            new int[]{ 1, 2, 4 },
                            new int[]{ 2, 3, 5 },
                        };
                        int value = statData[levelRange][(int)Type];

                        Dictionary<GearPropType, int> scrollStat;
                        if(statType == GearPropType.incMHP)
                            scrollStat = new Dictionary<GearPropType, int> { [GearPropType.incMHP] = value * 50 };
                        else
                            scrollStat = new Dictionary<GearPropType, int> { [statType] = value };

                        if(isFourthScrollApply > 0)
                        {
                            scrollStat.Add(attackType, 1);
                        }
                        else if(isFourthScrollApply < 0)
                        {
                            scrollStat.Add(GearPropType.incPAD, 1);
                            scrollStat.Add(GearPropType.incMAD, 1);
                        }
                        return scrollStat;
                    }
                    else if(gearType == GearType.machineHeart)
                    {
                        int[][] statData = new int[][]
                        {
                            new int[]{ 1, 2, 3 },
                            new int[]{ 2, 3, 5 },
                            new int[]{ 3, 5, 7 },
                        };
                        int value = statData[levelRange][(int)Type];

                        return new Dictionary<GearPropType, int> { [attackType] = value };
                    }
                    else return new Dictionary<GearPropType, int>();
                case ScrollType.chaos:
                case ScrollType.incredibleChaos:
                case ScrollType.incredibleChaosOfGoodness:
                    return ChaosStat;
                case ScrollType.enhance100:
                    return new Dictionary<GearPropType, int>
                    {
                        [GearPropType.incAllStat] = 1,
                        [GearPropType.incMHP] = 50,
                        [GearPropType.incMMP] = 50,
                    };
                case ScrollType.enhance50:
                    return new Dictionary<GearPropType, int>
                    {
                        [GearPropType.incAllStat] = 3,
                        [GearPropType.incMHP] = 100,
                        [GearPropType.incMMP] = 100,
                    };
                case ScrollType.yggdrasilSTR:
                case ScrollType.yggdrasilDEX:
                case ScrollType.yggdrasilINT:
                case ScrollType.yggdrasilLUK:
                    int min = 0, max = 0;
                    if(reqLevel >= 120) { min = 5; max = 15; }
                    else if(100 <= reqLevel && reqLevel < 120) { min = 3; max = 9; }
                    OptionValue = MinMax(OptionValue, min, max);
                    switch(Type)
                    {
                        case ScrollType.yggdrasilSTR: return new Dictionary<GearPropType, int> { [GearPropType.incSTR] = OptionValue };
                        case ScrollType.yggdrasilDEX: return new Dictionary<GearPropType, int> { [GearPropType.incDEX] = OptionValue };
                        case ScrollType.yggdrasilINT: return new Dictionary<GearPropType, int> { [GearPropType.incINT] = OptionValue };
                        case ScrollType.yggdrasilLUK: return new Dictionary<GearPropType, int> { [GearPropType.incLUK] = OptionValue };
                    }
                    return new Dictionary<GearPropType, int>();
                // armor
                case ScrollType.armorPAD:
                    OptionValue = MinMax(OptionValue, 1, 2);
                    return new Dictionary<GearPropType, int> { [GearPropType.incPAD] = OptionValue };
                case ScrollType.armorMAD:
                    OptionValue = MinMax(OptionValue, 1, 2);
                    return new Dictionary<GearPropType, int> { [GearPropType.incMAD] = OptionValue };
                case ScrollType.miracleArmorPAD:
                    OptionValue = MinMax(OptionValue, 2, 3);
                    return new Dictionary<GearPropType, int> { [GearPropType.incPAD] = OptionValue };
                case ScrollType.miracleArmorMAD:
                    OptionValue = MinMax(OptionValue, 2, 3);
                    return new Dictionary<GearPropType, int> { [GearPropType.incMAD] = OptionValue };
                case ScrollType.armorPADScroll:
                    return new Dictionary<GearPropType, int> { [GearPropType.incPAD] = 2 };
                case ScrollType.armorMADScroll:
                    return new Dictionary<GearPropType, int> { [GearPropType.incMAD] = 2 };
                case ScrollType.ultimateArmor:
                    return new Dictionary<GearPropType, int>
                    {
                        [GearPropType.incAllStat] = 9,
                    };
                case ScrollType.tenthAnnivArmor:
                    OptionValue = MinMax(OptionValue, 5, 6);
                    return new Dictionary<GearPropType, int>
                    {
                        [GearPropType.incAllStat] = OptionValue,
                        [GearPropType.incMHP] = 5,
                        [GearPropType.incMMP] = 5,
                        [GearPropType.incPDD] = 5,
                    };
                case ScrollType.tenthAnnivPrimeArmor:
                    return new Dictionary<GearPropType, int>
                    {
                        [GearPropType.incAllStat] = 10,
                        [GearPropType.incMHP] = 10,
                        [GearPropType.incMMP] = 10,
                        [GearPropType.incPDD] = 10,
                    };
                case ScrollType.happytimeArmorATT:
                    return new Dictionary<GearPropType, int> { [GearPropType.incPAD] = 5, [GearPropType.incMAD] = 5 };
                case ScrollType.frostyArmorEnhance:
                    return new Dictionary<GearPropType, int>
                    {
                        [GearPropType.incAllStat] = 3,
                    };
                // accessory
                case ScrollType.accPAD:
                    OptionValue = MinMax(OptionValue, 1, 2);
                    return new Dictionary<GearPropType, int> { [GearPropType.incPAD] = OptionValue };
                case ScrollType.accMAD:
                    OptionValue = MinMax(OptionValue, 1, 2);
                    return new Dictionary<GearPropType, int> { [GearPropType.incMAD] = OptionValue };
                case ScrollType.miracleAccPAD:
                    OptionValue = MinMax(OptionValue, 1, 4);
                    return new Dictionary<GearPropType, int> { [GearPropType.incPAD] = OptionValue };
                case ScrollType.miracleAccMAD:
                    OptionValue = MinMax(OptionValue, 1, 4);
                    return new Dictionary<GearPropType, int> { [GearPropType.incMAD] = OptionValue };
                case ScrollType.accPADScroll:
                    OptionValue = MinMax(OptionValue, 2, 4);
                    return new Dictionary<GearPropType, int> { [GearPropType.incPAD] = OptionValue };
                case ScrollType.accMADScroll:
                    OptionValue = MinMax(OptionValue, 2, 4);
                    return new Dictionary<GearPropType, int> { [GearPropType.incMAD] = OptionValue };
                case ScrollType.premiumAccPADScroll:
                    OptionValue = MinMax(OptionValue, 4, 5);
                    return new Dictionary<GearPropType, int> { [GearPropType.incPAD] = OptionValue };
                case ScrollType.premiumAccMADScroll:
                    OptionValue = MinMax(OptionValue, 4, 5);
                    return new Dictionary<GearPropType, int> { [GearPropType.incMAD] = OptionValue };
                case ScrollType.tenthAnnivAcc:
                    OptionValue = MinMax(OptionValue, 5, 6);
                    return new Dictionary<GearPropType, int>
                    {
                        [GearPropType.incAllStat] = OptionValue,
                    };
                case ScrollType.tenthAnnivPrimeAcc:
                    return new Dictionary<GearPropType, int>
                    {
                        [GearPropType.incAllStat] = 10,
                    };
                // weapon
                case ScrollType.magicalPAD:
                    OptionValue = MinMax(OptionValue, 9, 11);
                    return new Dictionary<GearPropType, int>
                    {
                        [GearPropType.incAllStat] = 3,
                        [GearPropType.incPAD] = OptionValue,
                    };
                case ScrollType.magicalMAD:
                    OptionValue = MinMax(OptionValue, 9, 11);
                    return new Dictionary<GearPropType, int>
                    {
                        [GearPropType.incAllStat] = 3,
                        [GearPropType.incMAD] = OptionValue,
                    };
                case ScrollType.earringINT10:
                    return new Dictionary<GearPropType, int> { [GearPropType.incINT] = 3, [GearPropType.incMAD] = 5 };
                case ScrollType.dragonStone:
                    return new Dictionary<GearPropType, int>
                    {
                        [GearPropType.incAllStat] = 15,
                        [GearPropType.incPDD] = 350,
                    };
                case ScrollType.fragmentOfTwistedTime:
                    return new Dictionary<GearPropType, int>
                    {
                        [GearPropType.incAllStat] = 3,
                        [GearPropType.incMHP] = 40,
                        [GearPropType.incMMP] = 40,
                        [GearPropType.incPAD] = 3,
                        [GearPropType.incMAD] = 3,
                        [GearPropType.incPDD] = 25,
                        [GearPropType.incSpeed] = 3,
                        [GearPropType.incJump] = 2,
                    };
                default:
                    return new Dictionary<GearPropType, int>();
            }
        }

        public string ToString(GearType gearType = GearType.cap, int reqLevel = 120)
        {
            // 장갑은 따로
            string[] data = new string[] { "힘", "민첩", "지력", "운", "체력" };
            switch(Type)
            {
                case ScrollType.spellTrace100:
                    if(0 > OptionValue || 4 < OptionValue) return null;
                    if(Gear.IsWeapon(gearType) || gearType == GearType.katara)
                        return "100% " + (OptionValue == 2 ? "마력" : "공격력") + (reqLevel > 110 ? "(" + data[OptionValue] + ")" : null) + " 주문서";
                    else if(gearType == GearType.glove)
                        return "100% " + (reqLevel > 70 ? (OptionValue == 2 ? "마력" : "공격력") : "방어력") + " 주문서";
                    else
                        return "100% " + data[OptionValue] + " 주문서";
                case ScrollType.spellTrace70:
                    if(0 > OptionValue || 4 < OptionValue) return null;
                    if(Gear.IsWeapon(gearType) || gearType == GearType.katara)
                        return "70% " + (OptionValue == 2 ? "마력" : "공격력") + (reqLevel > 70 ? "(" + data[OptionValue] + ")" : null) + " 주문서";
                    else if(gearType == GearType.glove)
                        return "70% " + (OptionValue == 2 ? "마력" : "공격력") + " 주문서";
                    else
                        return "70% " + data[OptionValue] + " 주문서";
                case ScrollType.spellTrace30:
                    if(0 > OptionValue || 4 < OptionValue) return null;
                    if(Gear.IsWeapon(gearType) || gearType == GearType.katara)
                        return "30% " + (OptionValue == 2 ? "마력" : "공격력") + "(" + data[OptionValue] + ")" + " 주문서";
                    else if(gearType == GearType.glove)
                        return "30% " + (OptionValue == 2 ? "마력" : "공격력") + " 주문서";
                    else
                        return "30% " + data[OptionValue] + " 주문서";
                case ScrollType.spellTrace15:
                    if(0 > OptionValue || 4 < OptionValue) return null;
                    if(Gear.IsWeapon(gearType) || gearType == GearType.katara)
                        return "15% " + (OptionValue == 2 ? "마력" : "공격력") + "(" + data[OptionValue] + ")" + " 주문서";
                    else
                        return null;
                case ScrollType.chaos: return "혼돈의 주문서";
                case ScrollType.incredibleChaos: return "놀라운 혼돈의 주문서";
                case ScrollType.incredibleChaosOfGoodness: return "놀라운 긍정의 혼돈 주문서";
                case ScrollType.enhance100:
                    if(Gear.IsArmor(gearType))
                        return "방어구 증폭 주문서 100%";
                    else if(Gear.IsAccessory(gearType))
                        return "악세서리 증폭 주문서 100%";
                    else
                        return null;
                case ScrollType.enhance50:
                    if(Gear.IsArmor(gearType))
                        return "방어구 증폭 주문서 50%";
                    else if(Gear.IsAccessory(gearType))
                        return "악세서리 증폭 주문서 50%";
                    else
                        return null;
                case ScrollType.yggdrasilSTR: return "힘의 이그드라실의 축복";
                case ScrollType.yggdrasilDEX: return "민첩성의 이그드라실의 축복";
                case ScrollType.yggdrasilINT: return "지력의 이그드라실의 축복";
                case ScrollType.yggdrasilLUK: return "행운의 이그드라실의 축복";
                // armor
                case ScrollType.armorPAD: return "방어구 공격력 주문서 70%";
                case ScrollType.armorMAD: return "방어구 마력 주문서 70%";
                case ScrollType.miracleArmorPAD: return "미라클 방어구 공격력 주문서 50%";
                case ScrollType.miracleArmorMAD: return "미라클 방어구 마력 주문서 50%";
                case ScrollType.armorPADScroll: return "방어구 공격력 스크롤 70%";
                case ScrollType.armorMADScroll: return "방어구 마력 스크롤 70%";
                case ScrollType.ultimateArmor: return "얼티밋 방어구 강화 주문서 20%";
                case ScrollType.tenthAnnivArmor: return "10주년 방어구 주문서";
                case ScrollType.tenthAnnivPrimeArmor: return "10주년 프라임 방어구 주문서";
                case ScrollType.happytimeArmorATT: return "방어구 공격력 주문서70%(해피타임)";
                case ScrollType.frostyArmorEnhance: return "프로스티 방어구 증폭 주문서50%";
                // accessory
                case ScrollType.accPAD: return "악세서리 공격력 주문서 70%";
                case ScrollType.accMAD: return "악세서리 마력 주문서 70%";
                case ScrollType.miracleAccPAD: return "미라클 악세서리 공격력 주문서 50%";
                case ScrollType.miracleAccMAD: return "미라클 악세서리 마력 주문서 50%";
                case ScrollType.accPADScroll: return "악세서리 공격력 스크롤 100%";
                case ScrollType.accMADScroll: return "악세서리 마력 스크롤 100%";
                case ScrollType.premiumAccPADScroll: return "프리미엄 악세서리 공격력 스크롤 100%";
                case ScrollType.premiumAccMADScroll: return "프리미엄 악세서리 마력 스크롤 100%";
                case ScrollType.tenthAnnivAcc: return "10주년 악세서리 주문서";
                case ScrollType.tenthAnnivPrimeAcc: return "10주년 프라임 악세서리 주문서";
                // weapon
                case ScrollType.magicalPAD:
                    if(Gear.IsDoubleHandWeapon(gearType))
                        return "매지컬 두손무기 공격력 주문서";
                    else if(Gear.IsLeftWeapon(gearType) || gearType == GearType.katara || gearType == GearType.machineHeart)
                        return "매지컬 한손무기 공격력 주문서";
                    else
                        return null;
                case ScrollType.magicalMAD:
                    if(Gear.IsWeapon(gearType) || gearType == GearType.katara || gearType == GearType.machineHeart)
                        return "매지컬 한손무기 마력 주문서";
                    else
                        return null;
                case ScrollType.earringINT10: return "귀 장식 지력 주문서 10%";
                case ScrollType.dragonStone: return "드래곤의 돌";
                case ScrollType.fragmentOfTwistedTime: return "뒤틀린 시간의 파편";
                default:
                    return null;
            }
        }

        private static int MinMax(int value, int min, int max)
        {
            if(value < min) value = min;
            if(value > max) value = max;
            return value;
        }
    }
}
