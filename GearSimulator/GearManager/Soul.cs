using System;
using System.Collections.Generic;
using System.Linq;

namespace GearManager
{
    public class Soul
    {
        static Soul()
        {
            SoulMobNameData = new string[]
            {
                "시그너스",
                "매그너스",
                "무르무르",
                "블러디퀸",
                "벨룸",
                "스우",
                "데미안",
                "루시드",
                "윌",
                "진 힐라",
                "듄켈",
                "핑크빈",
                "피에르",
                "반반",
                "우르스",
                "아카이럼",
                "모카딘",
                "카리아인",
                "CQ57",
                "줄라이",
                "플레드",
                "반 레온",
                "힐라",
                "파풀라투스",
                "자쿰",
                "발록",
                "돼지바",
                "프리미엄PC방",
                "무공",
                "피아누스",
                "드래곤 라이더",
                "렉스",
                "에피네아",
                "핑크몽",
                "락 스피릿",
                "교도관 아니",
                "크세르크세스",
                "블랙 슬라임",
            };
            SoulNameData = new string[]
            {
                "기운찬 시그너스의 소울",
                "날렵한 시그너스의 소울",
                "총명한 시그너스의 소울",
                "놀라운 시그너스의 소울",
                "화려한 시그너스의 소울",
                "강력한 시그너스의 소울",
                "빛나는 시그너스의 소울",
                "강인한 시그너스의 소울",
                "위대한 시그너스의 소울",
                "기운찬 매그너스의 소울",
                "날렵한 매그너스의 소울",
                "총명한 매그너스의 소울",
                "놀라운 매그너스의 소울",
                "화려한 매그너스의 소울",
                "강력한 매그너스의 소울",
                "빛나는 매그너스의 소울",
                "강인한 매그너스의 소울",
                "위대한 매그너스의 소울",
                "기운찬 무르무르의 소울",
                "날렵한 무르무르의 소울",
                "총명한 무르무르의 소울",
                "놀라운 무르무르의 소울",
                "화려한 무르무르의 소울",
                "강력한 무르무르의 소울",
                "빛나는 무르무르의 소울",
                "강인한 무르무르의 소울",
                "위대한 무르무르의 소울",
                "기운찬 블러디퀸의 소울",
                "날렵한 블러디퀸의 소울",
                "총명한 블러디퀸의 소울",
                "놀라운 블러디퀸의 소울",
                "화려한 블러디퀸의 소울",
                "강력한 블러디퀸의 소울",
                "빛나는 블러디퀸의 소울",
                "강인한 블러디퀸의 소울",
                "위대한 블러디퀸의 소울",
                "기운찬 벨룸의 소울",
                "날렵한 벨룸의 소울",
                "총명한 벨룸의 소울",
                "놀라운 벨룸의 소울",
                "화려한 벨룸의 소울",
                "강력한 벨룸의 소울",
                "빛나는 벨룸의 소울",
                "강인한 벨룸의 소울",
                "위대한 벨룸의 소울",
                "기운찬 스우의 소울",
                "날렵한 스우의 소울",
                "총명한 스우의 소울",
                "놀라운 스우의 소울",
                "화려한 스우의 소울",
                "강력한 스우의 소울",
                "빛나는 스우의 소울",
                "강인한 스우의 소울",
                "위대한 스우의 소울",
                "기운찬 데미안의 소울",
                "날렵한 데미안의 소울",
                "총명한 데미안의 소울",
                "놀라운 데미안의 소울",
                "화려한 데미안의 소울",
                "강력한 데미안의 소울",
                "빛나는 데미안의 소울",
                "강인한 데미안의 소울",
                "위대한 데미안의 소울",
                "기운찬 루시드의 소울",
                "날렵한 루시드의 소울",
                "총명한 루시드의 소울",
                "놀라운 루시드의 소울",
                "화려한 루시드의 소울",
                "강력한 루시드의 소울",
                "빛나는 루시드의 소울",
                "강인한 루시드의 소울",
                "위대한 루시드의 소울",
                "기운찬 윌의 소울",
                "날렵한 윌의 소울",
                "총명한 윌의 소울",
                "놀라운 윌의 소울",
                "화려한 윌의 소울",
                "강력한 윌의 소울",
                "빛나는 윌의 소울",
                "강인한 윌의 소울",
                "위대한 윌의 소울",
                "기운찬 진 힐라의 소울",
                "날렵한 진 힐라의 소울",
                "총명한 진 힐라의 소울",
                "놀라운 진 힐라의 소울",
                "화려한 진 힐라의 소울",
                "강력한 진 힐라의 소울",
                "빛나는 진 힐라의 소울",
                "강인한 진 힐라의 소울",
                "위대한 진 힐라의 소울",
                "기운찬 듄켈의 소울",
                "날렵한 듄켈의 소울",
                "총명한 듄켈의 소울",
                "놀라운 듄켈의 소울",
                "화려한 듄켈의 소울",
                "강력한 듄켈의 소울",
                "빛나는 듄켈의 소울",
                "강인한 듄켈의 소울",
                "위대한 듄켈의 소울",
                "기운찬 핑크빈의 소울",
                "날렵한 핑크빈의 소울",
                "총명한 핑크빈의 소울",
                "놀라운 핑크빈의 소울",
                "화려한 핑크빈의 소울",
                "강력한 핑크빈의 소울",
                "빛나는 핑크빈의 소울",
                "강인한 핑크빈의 소울",
                "날카로운 핑크빈의 소울",
                "파괴하는 핑크빈의 소울",
                "위대한 핑크빈의 소울",
                "기운찬 피에르의 소울",
                "날렵한 피에르의 소울",
                "총명한 피에르의 소울",
                "놀라운 피에르의 소울",
                "화려한 피에르의 소울",
                "강력한 피에르의 소울",
                "빛나는 피에르의 소울",
                "강인한 피에르의 소울",
                "위대한 피에르의 소울",
                "기운찬 반반의 소울",
                "날렵한 반반의 소울",
                "총명한 반반의 소울",
                "놀라운 반반의 소울",
                "화려한 반반의 소울",
                "강력한 반반의 소울",
                "빛나는 반반의 소울",
                "강인한 반반의 소울",
                "위대한 반반의 소울",
                "기운찬 우르스의 소울",
                "날렵한 우르스의 소울",
                "총명한 우르스의 소울",
                "놀라운 우르스의 소울",
                "화려한 우르스의 소울",
                "강력한 우르스의 소울",
                "빛나는 우르스의 소울",
                "강인한 우르스의 소울",
                "위대한 우르스의 소울",
                "기운찬 아카이럼의 소울",
                "날렵한 아카이럼의 소울",
                "총명한 아카이럼의 소울",
                "놀라운 아카이럼의 소울",
                "화려한 아카이럼의 소울",
                "강력한 아카이럼의 소울",
                "빛나는 아카이럼의 소울",
                "강인한 아카이럼의 소울",
                "위대한 아카이럼의 소울",
                "기운찬 모카딘의 소울",
                "날렵한 모카딘의 소울",
                "총명한 모카딘의 소울",
                "놀라운 모카딘의 소울",
                "화려한 모카딘의 소울",
                "강력한 모카딘의 소울",
                "빛나는 모카딘의 소울",
                "강인한 모카딘의 소울",
                "위대한 모카딘의 소울",
                "기운찬 카리아인의 소울",
                "날렵한 카리아인의 소울",
                "총명한 카리아인의 소울",
                "놀라운 카리아인의 소울",
                "화려한 카리아인의 소울",
                "강력한 카리아인의 소울",
                "빛나는 카리아인의 소울",
                "강인한 카리아인의 소울",
                "위대한 카리아인의 소울",
                "기운찬 CQ57의 소울",
                "날렵한 CQ57의 소울",
                "총명한 CQ57의 소울",
                "놀라운 CQ57의 소울",
                "화려한 CQ57의 소울",
                "강력한 CQ57의 소울",
                "빛나는 CQ57의 소울",
                "강인한 CQ57의 소울",
                "위대한 CQ57의 소울",
                "기운찬 줄라이의 소울",
                "날렵한 줄라이의 소울",
                "총명한 줄라이의 소울",
                "놀라운 줄라이의 소울",
                "화려한 줄라이의 소울",
                "강력한 줄라이의 소울",
                "빛나는 줄라이의 소울",
                "강인한 줄라이의 소울",
                "위대한 줄라이의 소울",
                "기운찬 플레드의 소울",
                "날렵한 플레드의 소울",
                "총명한 플레드의 소울",
                "놀라운 플레드의 소울",
                "화려한 플레드의 소울",
                "강력한 플레드의 소울",
                "빛나는 플레드의 소울",
                "강인한 플레드의 소울",
                "위대한 플레드의 소울",
                "기운찬 반 레온의 소울",
                "날렵한 반 레온의 소울",
                "총명한 반 레온의 소울",
                "놀라운 반 레온의 소울",
                "화려한 반 레온의 소울",
                "강력한 반 레온의 소울",
                "빛나는 반 레온의 소울",
                "강인한 반 레온의 소울",
                "날카로운 반 레온의 소울",
                "파괴하는 반 레온의 소울",
                "위대한 반 레온의 소울",
                "기운찬 힐라의 소울",
                "날렵한 힐라의 소울",
                "총명한 힐라의 소울",
                "놀라운 힐라의 소울",
                "화려한 힐라의 소울",
                "강력한 힐라의 소울",
                "빛나는 힐라의 소울",
                "강인한 힐라의 소울",
                "위대한 힐라의 소울",
                "기운찬 파풀라투스의 소울",
                "날렵한 파풀라투스의 소울",
                "총명한 파풀라투스의 소울",
                "놀라운 파풀라투스의 소울",
                "화려한 파풀라투스의 소울",
                "강력한 파풀라투스의 소울",
                "빛나는 파풀라투스의 소울",
                "강인한 파풀라투스의 소울",
                "위대한 파풀라투스의 소울",
                "기운찬 자쿰의 소울",
                "날렵한 자쿰의 소울",
                "총명한 자쿰의 소울",
                "놀라운 자쿰의 소울",
                "화려한 자쿰의 소울",
                "강력한 자쿰의 소울",
                "빛나는 자쿰의 소울",
                "강인한 자쿰의 소울",
                "위대한 자쿰의 소울",
                "기운찬 발록의 소울",
                "날렵한 발록의 소울",
                "총명한 발록의 소울",
                "놀라운 발록의 소울",
                "화려한 발록의 소울",
                "강력한 발록의 소울",
                "빛나는 발록의 소울",
                "강인한 발록의 소울",
                "날카로운 발록의 소울",
                "파괴하는 발록의 소울",
                "위대한 발록의 소울",
                "기운찬 돼지바 소울",
                "날렵한 돼지바 소울",
                "총명한 돼지바 소울",
                "놀라운 돼지바 소울",
                "화려한 돼지바 소울",
                "강력한 돼지바 소울",
                "빛나는 돼지바 소울",
                "강인한 돼지바 소울",
                "위대한 돼지바 소울",
                "기운찬 프리미엄PC방 소울",
                "날렵한 프리미엄PC방 소울",
                "총명한 프리미엄PC방 소울",
                "놀라운 프리미엄PC방 소울",
                "화려한 프리미엄PC방 소울",
                "강력한 프리미엄PC방 소울",
                "빛나는 프리미엄PC방 소울",
                "강인한 프리미엄PC방 소울",
                "위대한 프리미엄PC방 소울",
                "기운찬 무공의 소울",
                "날렵한 무공의 소울",
                "총명한 무공의 소울",
                "놀라운 무공의 소울",
                "강인한 무공의 소울",
                "풍부한 무공의 소울",
                "화려한 무공의 소울",
                "기운찬 피아누스의 소울",
                "날렵한 피아누스의 소울",
                "총명한 피아누스의 소울",
                "놀라운 피아누스의 소울",
                "강인한 피아누스의 소울",
                "풍부한 피아누스의 소울",
                "화려한 피아누스의 소울",
                "기운찬 드래곤 라이더의 소울",
                "날렵한 드래곤 라이더의 소울",
                "총명한 드래곤 라이더의 소울",
                "놀라운 드래곤 라이더의 소울",
                "강인한 드래곤 라이더의 소울",
                "풍부한 드래곤 라이더의 소울",
                "화려한 드래곤 라이더의 소울",
                "기운찬 렉스의 소울",
                "날렵한 렉스의 소울",
                "총명한 렉스의 소울",
                "놀라운 렉스의 소울",
                "강인한 렉스의 소울",
                "풍부한 렉스의 소울",
                "화려한 렉스의 소울",
                "기운찬 에피네아의 소울",
                "날렵한 에피네아의 소울",
                "총명한 에피네아의 소울",
                "놀라운 에피네아의 소울",
                "강인한 에피네아의 소울",
                "풍부한 에피네아의 소울",
                "화려한 에피네아의 소울",
                "기운찬 핑크몽의 소울",
                "날렵한 핑크몽의 소울",
                "총명한 핑크몽의 소울",
                "놀라운 핑크몽의 소울",
                "강인한 핑크몽의 소울",
                "풍부한 핑크몽의 소울",
                "화려한 핑크몽의 소울",
                "기운찬 락 스피릿의 소울",
                "날렵한 락 스피릿의 소울",
                "총명한 락 스피릿의 소울",
                "놀라운 락 스피릿의 소울",
                "강인한 락 스피릿의 소울",
                "풍부한 락 스피릿의 소울",
                "화려한 락 스피릿의 소울",
                "기운찬 교도관 아니의 소울",
                "날렵한 교도관 아니의 소울",
                "총명한 교도관 아니의 소울",
                "놀라운 교도관 아니의 소울",
                "강인한 교도관 아니의 소울",
                "풍부한 교도관 아니의 소울",
                "화려한 교도관 아니의 소울",
                "기운찬 크세르크세스의 소울",
                "날렵한 크세르크세스의 소울",
                "총명한 크세르크세스의 소울",
                "놀라운 크세르크세스의 소울",
                "강인한 크세르크세스의 소울",
                "풍부한 크세르크세스의 소울",
                "화려한 크세르크세스의 소울",
                "기운찬 블랙 슬라임의 소울",
                "날렵한 블랙 슬라임의 소울",
                "총명한 블랙 슬라임의 소울",
                "놀라운 블랙 슬라임의 소울",
                "강인한 블랙 슬라임의 소울",
                "풍부한 블랙 슬라임의 소울",
                "화려한 블랙 슬라임의 소울",
            };
            GreatSoulOptionStrings = new string[]
            {
                "공격력",
                "마력",
                "올스탯",
                "최대 HP",
                "크리티컬 확률",
                "몬스터 방어율 무시",
                "보스 몬스터 공격 시 데미지",
                "모든 스킬레벨",
            };
            GreatSoulOptionTypes = new GearPropType[]
            {
                GearPropType.incPAD,
                GearPropType.incMAD,
                GearPropType.incAllStat,
                GearPropType.incMHP,
                GearPropType.incCr,
                GearPropType.imdR,
                GearPropType.bdR,
                GearPropType.incAllskill,
            };
        }

        private Soul(string soulName, string skillName, string optionString, int charge)
        {
            Name = soulName;
            SkillName = skillName;
            OptionString = optionString;
            Charge = charge;
        }

        private Soul(string soulName, string greatSoulOptionString)
        {
            Name = soulName;
            SkillName = GetSkillName(soulName);
            SetSoulOption(greatSoulOptionString);
        }

        public static string[] SoulMobNameData { get; }
        public static string[] SoulNameData { get; }
        public static string[] GreatSoulOptionStrings { get; }
        private static GearPropType[] GreatSoulOptionTypes { get; }

        public string Name { get; }
        public string SkillName { get; }
        public GearPropType OptionType { get; private set; }
        public int OptionValue { get; private set; }
        public string OptionString { get; private set; }
        public int Charge { get; set; }

        public int GetChargeAD()
        {
            if(GetSoulGrade(Name) <= 6) return Math.Min(500, Charge) / 100 * 4;
            else return Math.Min(500, Charge) / 100 * 3;
        }

        private void SetSoulOption(string greatSoulOptionString)
        {
            Dictionary<string, GearPropType> typeData = new Dictionary<string, GearPropType>
            {
                { "기운찬", GearPropType.incSTR },
                { "날렵한", GearPropType.incDEX },
                { "총명한", GearPropType.incINT },
                { "놀라운", GearPropType.incLUK },
                { "화려한", GearPropType.incAllStat },
                { "강력한", GearPropType.incPAD },
                { "빛나는", GearPropType.incMAD },
                { "강인한", GearPropType.incMHP },
                { "풍부한", GearPropType.incMMP },
                { "날카로운", GearPropType.imdR },
                { "파괴하는", GearPropType.bdR },
                { "위대한", GearPropType._null },
            };
            int[][] normalData = new int[][]
             {
                new int[] { 24, 15, 6, 960 },
                new int[] { 20, 12, 5, 800 },
                new int[] { 18, 10, 4, 700 },
                new int[] { 15, 8, 3, 600 },
                new int[] { 12, 8, 3, 500 },
                new int[] { 10, 7, 3, 400 },
                new int[] { 7, 5, 0, 300 },
                new int[] { 5, 3, 0, 200 },
                new int[] { 4, 2, 0, 180 },
                new int[] { 3, 2, 0, 150 },
             };
            int[][] greatData = new int[][]
            {
                new int[] { 3, 5, 2000, 12, 7, 2 },
                new int[] { 10, 20, 1500, 10, 5, 1 },
                new int[] { 8, 17, 1300, 8, 4, 1 },
                new int[] { 7, 15, 1200, 7, 4, 1 },
                new int[] { 6, 12, 1100, 6, 3, 1 },
                new int[] { 5, 10, 1000, 5, 3, 1 },
            };

            int soulGrade = GetSoulGrade(Name);
            if(Name.Contains("위대한"))
            {
                int optionIdx;
                if(!string.IsNullOrWhiteSpace(greatSoulOptionString) && (optionIdx = Array.IndexOf(GreatSoulOptionStrings, greatSoulOptionString)) >= 0)
                {
                    OptionType = GreatSoulOptionTypes[optionIdx];
                }
                else
                {
                    OptionType = GearPropType.incPAD;
                }
                if(soulGrade == 1)
                {
                    bool isR = false;
                    switch(OptionType)
                    {
                        case GearPropType.incPAD:
                        case GearPropType.incMAD:
                            OptionValue = greatData[soulGrade - 1][0];
                            isR = true;
                            break;
                        case GearPropType.incAllStat:
                            OptionType = GearPropType.incAllStat;
                            OptionValue = greatData[soulGrade - 1][1];
                            isR = true;
                            break;
                        case GearPropType.incMHP:
                            OptionValue = greatData[soulGrade - 1][2];
                            break;
                        case GearPropType.incCr:
                            OptionValue = greatData[soulGrade - 1][3];
                            break;
                        case GearPropType.imdR:
                        case GearPropType.bdR:
                            OptionValue = greatData[soulGrade - 1][4];
                            break;
                        case GearPropType.incAllskill:
                            OptionValue = greatData[soulGrade - 1][5];
                            break;
                    }
                    OptionString = GetOptionString(OptionType, OptionValue, isR);
                }
                else
                {
                    switch(OptionType)
                    {
                        case GearPropType.incPAD:
                        case GearPropType.incMAD:
                            OptionValue = greatData[soulGrade - 1][0];
                            break;
                        case GearPropType.incAllStat:
                            OptionValue = greatData[soulGrade - 1][1];
                            break;
                        case GearPropType.incMHP:
                            OptionValue = greatData[soulGrade - 1][2];
                            break;
                        case GearPropType.incCr:
                            OptionValue = greatData[soulGrade - 1][3];
                            break;
                        case GearPropType.imdR:
                        case GearPropType.bdR:
                            OptionValue = greatData[soulGrade - 1][4];
                            break;
                        case GearPropType.incAllskill:
                            OptionValue = greatData[soulGrade - 1][5];
                            break;
                    }
                    OptionString = GetOptionString(OptionType, OptionValue);
                }
            }
            else
            {
                foreach(var curType in typeData)
                {
                    if(Name.Contains(curType.Key))
                    {
                        OptionType = curType.Value;
                    }
                }
                switch(OptionType)
                {
                    case GearPropType.incSTR:
                    case GearPropType.incDEX:
                    case GearPropType.incINT:
                    case GearPropType.incLUK:
                        OptionValue = normalData[soulGrade - 1][0];
                        break;
                    case GearPropType.incAllStat:
                        OptionValue = normalData[soulGrade - 1][1];
                        break;
                    case GearPropType.incPAD:
                    case GearPropType.incMAD:
                        OptionValue = normalData[soulGrade - 1][2];
                        break;
                    case GearPropType.incMHP:
                    case GearPropType.incMHP_incMMP:
                        OptionValue = normalData[soulGrade - 1][3];
                        break;
                    case GearPropType.imdR:
                    case GearPropType.bdR:
                        OptionValue = 5;
                        break;
                }
                OptionString = GetOptionString(OptionType, OptionValue);
            }
        }

        private static string GetSkillName(string soulName)
        {
            Dictionary<string, string[]> data = new Dictionary<string, string[]>
            {
                { "시그너스", new string[] { "불꽃 여제", "폭풍 여제" } },
                { "매그너스", new string[] { "진격! 그게 바로 나다", "폭격! 그게 바로 나다" } },
                { "무르무르", new string[] { "무르무르의 이상한 동행", "무르무르의 수상한 동행" } },
                { "블러디퀸", new string[] { "여왕의 마음은 갈대", "여왕님이 함께 하셔!" } },
                { "벨룸", new string[] { "기가 벨룸 레이저", "주니어 벨룸 소환!" } },
                { "스우", new string[] { "때렸스우~", "화났스우~" } },
                { "데미안", new string[] { "사냥 개시", "파멸의 검" } },
                { "루시드", new string[] { "악몽으로의 초대", "악몽의 지배자" } },
                { "윌", new string[] { "파괴의 손아귀", "거미의 왕" } },
                { "진 힐라", new string[] { "영혼 찢기", "붉은 마녀" } },
                { "듄켈", new string[] { "지면 절단", "지면 파쇄" } },
                { "핑크빈", new string[] { "까칠한 귀여움", "치명적인 귀여움" } },
                { "피에르", new string[] { "피에르의 모자선물", "깜짝 피에르!" } },
                { "반반", new string[] { "불닭의 따끔한 맛", "치킨 날다!" } },
                { "우르스", new string[] { "파왕의 포효", "파왕의 거친 포효" } },
                { "아카이럼", new string[] { "스네이크 사우론", "메두사카이럼" } },
                { "모카딘", new string[] { "검은 기사 모카딘", "어둠 기사 모카딘" } },
                { "카리아인", new string[] { "미친 마법사 카리아인", "폭주 마법사 카리아인" } },
                { "CQ57", new string[] { "돌격형 CQ57", "상급 돌격형 CQ57" } },
                { "줄라이", new string[] { "인간 사냥꾼 줄라이", "피의 사냥꾼 줄라이" } },
                { "플레드", new string[] { "싸움꾼 플레드", "거친 싸움꾼 플레드" } },
                { "반 레온", new string[] { "야옹이 권법 : 할퀴기 초식", "야옹이 권법 : 크로스 따귀 어택" } },
                { "힐라", new string[] { "마른 하늘에 번개 어택", "마른 하늘에 벼락 어택" } },
                { "파풀라투스", new string[] { "공간의 지배자", "시간의 지배자" } },
                { "자쿰", new string[] { "뜨거운 토템 투하", "화끈한 토템 투하" } },
                { "발록", new string[] { "지옥불 트림", "지옥불 재채기" } },
                { "돼지바", new string[] { "돼지바 스윙!", "돼지바 드랍!" } },
                { "프리미엄PC방", new string[] { "PC방에서 메이플을 켰다!", "프리미엄 PC방은 빵빵해" } },
                { "무공", new string[] { "회춘신공", "" } },
                { "피아누스", new string[] { "공포의 마빡생선", "" } },
                { "드래곤 라이더", new string[] { "손바닥 장풍", "" } },
                { "렉스", new string[] { "내 앞길을 막지마", "" } },
                { "에피네아", new string[] { "여왕의 향기는 나빌레라", "" } },
                { "핑크몽", new string[] { "해피 뉴 에브리데이!", "" } },
                { "락 스피릿", new string[] { "로큰롤 베이비", "" } },
                { "교도관 아니", new string[] { "난 한놈만 패", "" } },
                { "크세르크세스", new string[] { "특공 염소 어택", "" } },
                { "블랙 슬라임", new string[] { "핑크빛 독안개", "" } },
            };

            foreach(var curName in data)
            {
                if(soulName.Contains(curName.Key))
                {
                    if(soulName.Contains("위대한")) return curName.Value[1];
                    else return curName.Value[0];
                }
            }

            return string.Empty;
        }

        private static int GetSoulGrade(string soulName)
        {
            Dictionary<string, int> data = new Dictionary<string, int>
            {
                { "시그너스", 1 },
                { "매그너스", 1 },
                { "무르무르", 1 },
                { "블러디퀸", 1 },
                { "벨룸", 1 },
                { "스우", 1 },
                { "데미안", 1 },
                { "루시드", 1 },
                { "윌", 1 },
                { "진 힐라", 1 },
                { "듄켈", 1 },
                { "핑크빈", 2 },
                { "피에르", 2 },
                { "반반", 2 },
                { "우르스", 2 },
                { "아카이럼", 3 },
                { "모카딘", 3 },
                { "카리아인", 3 },
                { "CQ57", 3 },
                { "줄라이", 3 },
                { "플레드", 3 },
                { "반 레온", 4 },
                { "힐라", 4 },
                { "파풀라투스", 4 },
                { "자쿰", 5 },
                { "발록", 6 },
                { "돼지바", 6 },
                { "프리미엄PC방", 6 },
                { "무공", 7 },
                { "피아누스", 7 },
                { "드래곤 라이더", 8 },
                { "렉스", 8 },
                { "에피네아", 9 },
                { "핑크몽", 9 },
                { "락 스피릿", 10 },
                { "교도관 아니", 10 },
                { "크세르크세스", 10 },
                { "블랙 슬라임", 10 },
            };

            foreach(var curGrade in data)
            {
                if(soulName.Contains(curGrade.Key))
                {
                    return curGrade.Value;
                }
            }

            return 1;
        }

        private static string GetOptionString(GearPropType optionType, int optionValue, bool isR = false)
        {
            return StringHelper.GetGearPropString(optionType, optionValue) + (isR ? "%" : null);
        }

        public static Soul CreateFromName(string soulName, string greatOptionString = null)
        {
            return new Soul(soulName, greatOptionString);
        }

        public static Soul CreateCustomSoul(string name, string skillName, string optionString, int charge)
        {
            return new Soul(name, skillName, optionString, charge);
        }

        public static string[] GetSoulNameContains(string value)
        {
            return SoulNameData.Where(x => x.Contains(value)).ToArray();
        }
    }
}
