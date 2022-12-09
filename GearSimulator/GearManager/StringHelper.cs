namespace GearManager
{
    public static class StringHelper
    {
        public static string GetGearPropString(GearPropType propType, int value)
        {
            switch(propType)
            {
                case GearPropType.incSTR: return "STR : +" + value;
                case GearPropType.incDEX: return "DEX : +" + value;
                case GearPropType.incINT: return "INT : +" + value;
                case GearPropType.incLUK: return "LUK : +" + value;
                case GearPropType.incMHP: return "최대 HP : +" + value;
                case GearPropType.incMMP: return "최대 MP : +" + value;
                case GearPropType.incMHPr: return "최대 HP : +" + value + "%";
                case GearPropType.incMMPr: return "최대 MP : +" + value + "%";
                case GearPropType.incMDF: return "MaxDF : +" + value;
                case GearPropType.incPAD: return "공격력 : +" + value;
                case GearPropType.incMAD: return "마력 : +" + value;
                case GearPropType.incPDD: return "방어력 : +" + value;
                case GearPropType.incSpeed: return "이동속도 : +" + value;
                case GearPropType.incJump: return "점프력 : +" + value;
                case GearPropType.knockback: return "직접 타격시 " + value + "%의 확률로 넉백";
                case GearPropType.bdR: return "보스 몬스터 공격 시 데미지 : +" + value + "%";
                case GearPropType.imdR: return "몬스터 방어율 무시 : +" + value + "%";
                case GearPropType.damR: return "데미지 : +" + value + "%";
                case GearPropType.incAllStat: return "올스탯: +" + value;
                case GearPropType.reduceReq: return "착용 레벨 감소 : - " + value;

                case GearPropType.incCr: return "크리티컬 확률 : +" + value + "%";
                case GearPropType.incAllskill: return "모든 스킬레벨 : +" + value + "(5차 제외, 스킬의 마스터 레벨까지만 증가)";

                case GearPropType.only: return value == 0 ? null : "고유 아이템";
                case GearPropType.quest: return value == 0 ? null : "퀘스트 아이템";
                case GearPropType.tradeBlock: return value == 0 ? null : "교환 불가";
                case GearPropType.equipTradeBlock: return value == 0 ? null : "장착 시 교환 불가";
                case GearPropType.tradeOnce: return value == 0 ? null : "1회 교환가능 (거래 후 교환불가)";
                case GearPropType.accountSharable: return value == 0 ? null : "월드 내 나의 캐릭터간 이동만 가능";
                case GearPropType.sharableOnce: return value == 0 ? null : "계정 내 1회 이동 가능(이동 후 교환불가)";
                case GearPropType.onlyEquip: return value == 0 ? null : "고유장착 아이템";
                case GearPropType.abilityTimeLimited: return value == 0 ? null : "기간 한정 능력치";
                case GearPropType.notExtend: return value == 0 ? null : "유효기간 연장 불가";
                case GearPropType.blockGoldHammer: return value == 0 ? null : "황금망치 사용 불가";
                case GearPropType.noPotential: return value == 0 ? null : "잠재능력 설정 불가";
                case GearPropType.fixedPotential: return value == 0 ? null : "잠재능력 재설정 불가";
                case GearPropType.cantRepair: return value == 0 ? null : "수리 불가";
                case GearPropType.exceptUpgrade: return value == 0 ? null : "강화불가";
                case GearPropType.karmaLeft: return value < 0 ? null : "가위 사용 가능 횟수 : " + value + "회";
                case GearPropType.tradeAvailable:
                    switch (value)
                    {
                        case 1: return "#c실버 카르마의 가위를 사용하면 1회 교환이 가능하게 할 수 있습니다.#";
                        case 2: return "#c플래티넘 카르마의 가위를 사용하면 1회 교환이 가능하게 할 수 있습니다.#";
                        default: return null;
                    }
                case GearPropType.accountShareTag: return value == 0 ? null : "#c쉐어 네임 텍을 사용하면 1회 같은 계정 내 캐릭터로 이동할 수 있습니다.#";
                case GearPropType.superiorEqp: return value == 0 ? null : "#g아이템 강화 성공시 더욱 높은 효과를 받을 수 있습니다.#";
                case GearPropType.jokerToSetItem: return value == 0 ? null : "#c3개 이상 착용하고 있는 모든 세트 아이템에 포함되는 럭키 아이템!#";
                case GearPropType.scrollProtect: return value == 0 ? null : "#c프로텍트 주문서가 적용되어 있습니다. (12성 이상의 장비에는 효과 없음)#";
                case GearPropType.scrollRecover: return value == 0 ? null : "#c주문서 보호 효과가 적용되어 있습니다.#";
                case GearPropType.scrollReturn: return value == 0 ? null : "#c리턴 주문서 효과가 적용되어 있습니다.#";

                default: return null;
            }
        }

        public static string GetStatString(GearPropType propType, int value)
        {
            switch(propType)
            {
                case GearPropType.incSTR: return "STR :";
                case GearPropType.incDEX: return "DEX :";
                case GearPropType.incINT: return "INT :";
                case GearPropType.incLUK: return "LUK :";
                case GearPropType.incMHP: return "최대 HP :";
                case GearPropType.incMMP: return "최대 MP :";
                case GearPropType.incMHPr: return "최대 HP :";
                case GearPropType.incMMPr: return "최대 MP :";
                case GearPropType.incMDF: return "MaxDF :";
                case GearPropType.incPAD: return "공격력 :";
                case GearPropType.incMAD: return "마력 :";
                case GearPropType.incPDD: return "방어력 :";
                case GearPropType.incSpeed: return "이동속도 :";
                case GearPropType.incJump: return "점프력 :";
                case GearPropType.knockback: return "직접 타격시 " + value + "%의 확률로 넉백";
                case GearPropType.bdR: return "보스 몬스터 공격 시 데미지";
                case GearPropType.imdR: return "몬스터 방어율 무시";
                case GearPropType.damR: return "데미지 :";
                case GearPropType.incAllStat: return "올스탯:";
                case GearPropType.reduceReq: return "착용 레벨 감소 : - " + value;

                default: return null;
            }
        }

        public static string GetGearGradeString(GearGrade rank)
        {
            switch(rank)
            {
                case GearGrade.Rare: return "(레어 아이템)";
                case GearGrade.Epic: return "(에픽 아이템)";
                case GearGrade.Unique: return "(유니크 아이템)";
                case GearGrade.Legendary: return "(레전드리 아이템)";
                case GearGrade.Special: return "(스페셜 아이템)";
                default: return null;
            }
        }

        public static string GetGearTypeString(GearType type)
        {
            switch(type)
            {
                case GearType.cap: return "모자";
                case GearType.faceAccessory: return "얼굴장식";
                case GearType.eyeAccessory: return "눈장식";
                case GearType.earrings: return "귀고리";
                case GearType.coat: return "상의";
                case GearType.longcoat: return "한벌옷";
                case GearType.pants: return "하의";
                case GearType.shoes: return "신발";
                case GearType.glove: return "장갑";
                case GearType.cape: return "망토";
                case GearType.ring: return "반지";
                case GearType.pendant: return "펜던트";
                case GearType.belt: return "벨트";
                case GearType.medal: return "훈장";
                case GearType.shoulderPad: return "어깨장식";
                case GearType.pocket: return "포켓 아이템";
                case GearType.badge: return "뱃지";
                case GearType.android: return "안드로이드";
                case GearType.machineHeart: return "기계 심장";

                //case GearType.weapon: return "무기";
                case GearType.shiningRod: return "샤이닝 로드";
                case GearType.tuner: return "튜너";
                case GearType.soulShooter: return "소울 슈터";
                case GearType.desperado: return "데스페라도";
                case GearType.energySword: return "에너지소드";
                case GearType.espLimiter: return "ESP 리미터";
                case GearType.chain2: return "체인";
                case GearType.magicGauntlet: return "매직 건틀렛";
                case GearType.handFan: return "부채";
                case GearType.ohSword: return "한손검";
                case GearType.ohAxe: return "한손도끼";
                case GearType.ohBlunt: return "한손둔기";
                case GearType.dagger: return "단검";
                case GearType.katara: return "블레이드";
                case GearType.cane: return "케인";
                case GearType.wand: return "완드";
                case GearType.staff: return "스태프";
                case GearType.thSword: return "두손검";
                case GearType.thAxe: return "두손도끼";
                case GearType.thBlunt: return "두손둔기";
                case GearType.spear: return "창";
                case GearType.polearm: return "폴암";
                case GearType.bow: return "활";
                case GearType.crossbow: return "석궁";
                case GearType.throwingGlove: return "아대";
                case GearType.knuckle: return "너클";
                case GearType.gun: return "건";
                case GearType.dualBow: return "듀얼 보우건";
                case GearType.handCannon: return "핸드캐논";
                case GearType.swordZB: return "대검";
                case GearType.swordZL: return "태도";
                case GearType.GauntletBuster: return "건틀렛 리볼버";
                case GearType.ancientBow: return "에인션트 보우";

                case GearType.shield: return "방패";
                case GearType.soulShield: return "소울실드";
                case GearType.demonShield: return "포스실드";
                //case GearType.subWeapon: return "보조무기";
                case GearType.magicArrow: return "마법화살";
                case GearType.card: return "카드";
                case GearType.heroMedal: return "메달";
                case GearType.rosario: return "로자리오";
                case GearType.chain: return "쇠사슬";
                case GearType.book1:
                case GearType.book2:
                case GearType.book3: return "마도서";
                case GearType.bowMasterFeather: return "화살깃";
                case GearType.crossBowThimble: return "활골무";
                case GearType.shadowerSheath: return "단검용 검집";
                case GearType.nightLordPoutch: return "부적";
                case GearType.orb: return "오브";
                case GearType.novaMarrow: return "용의 정수";
                case GearType.soulBangle: return "소울링";
                case GearType.mailin: return "매그넘";
                case GearType.viperWristband: return "리스트밴드";
                case GearType.captainSight: return "조준기";
                case GearType.cannonGunPowder: return "화약통";
                case GearType.aranPendulum: return "무게추";
                case GearType.evanPaper: return "문서";
                case GearType.battlemageBall: return "마법구슬";
                case GearType.wildHunterArrowHead: return "화살촉";
                case GearType.cygnusGem: return "보석";
                case GearType.controller: return "컨트롤러";
                case GearType.foxPearl: return "여우 구슬";
                case GearType.chess: return "체스피스";
                case GearType.transmitter: return "무기 전송장치";
                case GearType.ExplosivePill: return "장약";
                case GearType.magicWing: return "매직윙";
                case GearType.pathOfAbyss: return "패스 오브 어비스";
                case GearType.relic: return "렐릭";
                case GearType.fanTassel: return "선추";

                case GearType.emblem: return "엠블렘";
                case GearType.powerSource: return "파워소스";

                case GearType.dragonMask: return "드래곤 모자";
                case GearType.dragonPendant: return "드래곤 펜던트";
                case GearType.dragonWings: return "드래곤 날개장식";
                case GearType.dragonTail: return "드래곤 꼬리장식";

                case GearType.machineEngine: return "메카닉 엔진";
                case GearType.machineArms: return "메카닉 암";
                case GearType.machineLegs: return "메카닉 다리";
                case GearType.machineBody: return "메카닉 프레임";
                case GearType.machineTransistors: return "메카닉 트랜지스터";

                default: return null;
            }
        }
        public static string GetAttackSpeedString(int attackSpeed)
        {
            switch(attackSpeed)
            {
                case 2:
                case 3: return "매우 빠름";
                case 4:
                case 5: return "빠름";
                case 6: return "보통";
                case 7:
                case 8: return "느림";
                case 9: return "매우 느림";
                default:
                    return attackSpeed.ToString();
            }
        }

        public static string GetExtraJobReqString(GearType type)
        {
            switch(type)
            {
                //0xxx
                case GearType.heroMedal: return "히어로 직업군 착용 가능";
                case GearType.rosario: return "팔라딘 직업군 착용 가능";
                case GearType.chain: return "다크나이트 직업군 착용 가능";
                case GearType.book1: return "불,독 계열 마법사 착용 가능";
                case GearType.book2: return "얼음,번개 계열 마법사 착용 가능";
                case GearType.book3: return "비숍 계열 마법사 착용 가능";
                case GearType.bowMasterFeather: return "보우마스터 직업군 착용 가능";
                case GearType.crossBowThimble: return "신궁 직업군 착용 가능";
                case GearType.shadowerSheath: return "섀도어 직업군 착용 가능";
                case GearType.nightLordPoutch: return "나이트로드 직업군 착용 가능";
                case GearType.katara: return "듀얼블레이드 직업군 착용 가능";
                case GearType.viperWristband: return "바이퍼 직업군 착용 가능";
                case GearType.captainSight: return "캡틴 직업군 착용 가능";
                case GearType.cannonGunPowder: return "캐논 슈터 직업군 착용 가능";
                case GearType.relic: return "패스파인더 직업군 착용 가능";

                //1xxx
                case GearType.cygnusGem: return "시그너스 기사단 착용 가능";

                //2xxx
                case GearType.aranPendulum: return GetExtraJobReqString(21);
                case GearType.evanPaper: return GetExtraJobReqString(22);
                case GearType.magicArrow: return GetExtraJobReqString(23);
                case GearType.card: return GetExtraJobReqString(24);
                case GearType.foxPearl: return GetExtraJobReqString(25);
                case GearType.orb:
                case GearType.shiningRod: return GetExtraJobReqString(27);

                //3xxx
                case GearType.demonShield: return GetExtraJobReqString(31);
                case GearType.desperado: return "데몬 어벤져 착용 가능";
                case GearType.battlemageBall: return "배틀메이지 직업군 착용 가능";
                case GearType.wildHunterArrowHead: return "와일드헌터 직업군 착용 가능";
                case GearType.mailin: return "메카닉 착용 가능";
                case GearType.controller:
                case GearType.powerSource:
                case GearType.energySword: return GetExtraJobReqString(36);
                case GearType.GauntletBuster:
                case GearType.ExplosivePill: return GetExtraJobReqString(37);

                //5xxx
                case GearType.soulShield: return "미하일 착용 가능";

                //6xxx
                case GearType.novaMarrow: return GetExtraJobReqString(61);
                //case GearType.chain2:
                case GearType.transmitter: return GetExtraJobReqString(64);
                case GearType.soulBangle:
                case GearType.soulShooter: return GetExtraJobReqString(65);

                //10xxx
                case GearType.swordZB:
                case GearType.swordZL: return GetExtraJobReqString(101);

                case GearType.espLimiter:
                case GearType.chess: return GetExtraJobReqString(142);

                case GearType.magicGauntlet:
                case GearType.magicWing: return GetExtraJobReqString(152);

                case GearType.pathOfAbyss: return GetExtraJobReqString(155);

                case GearType.fanTassel: return GetExtraJobReqString(164);

                default: return null;
            }
        }

        public static string GetExtraJobReqString(int specJob)
        {
            switch(specJob)
            {
                case 21: return "아란 직업군 착용 가능";
                case 22: return "에반 직업군 착용 가능";
                case 23: return "메르세데스 착용가능";
                case 24: return "팬텀 착용 가능";
                case 25: return "은월 착용 가능";
                case 27: return "루미너스 착용 가능";
                case 31: return "데몬 직업군 착용 가능";
                case 36: return "제논 착용 가능";
                case 37: return "블래스터 착용 가능";
                case 61: return "카이저 착용 가능";
                case 64: return "카데나 직업군 착용 가능";
                case 65: return "엔젤릭 버스터 착용 가능";
                case 101: return "제로 착용 가능";
                case 142: return "키네시스 착용 가능";
                case 152: return "일리움 착용 가능";
                case 155: return "아크 착용 가능";
                case 164: return "호영 직업군 착용 가능";
                default: return null;
            }
        }
    }
}
