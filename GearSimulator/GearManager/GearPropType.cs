namespace GearManager
{
    public enum GearPropType
    {
        incSTR = 1,
        incSTRr,
        incDEX,
        incDEXr,
        incINT,
        incINTr,
        incLUK,
        incLUKr,
        incMHP_incMMP,
        incMHPr_incMMPr,
        incMHP,
        incMHPr,
        incMMP,
        incMMPr,
        incMDF,
        incPAD_incMAD,
        incPAD,
        incMAD,
        incAD,
        incPDD_incMDD,
        incPDD,
        incMDD,
        incACC_incEVA,
        incACC,
        incEVA,
        incSpeed,
        incJump,
        incARC,
        incCraft,
        knockback,
        incPVPDamage,
        bdR,
        incBDR,
        imdR,
        incIMDR,
        damR,
        nbdR,
        statR,
        incCHUC,
        incAllStat,

        //潜能属性
        incPADr = 100,
        incMADr,
        incPDDr,
        incMDDr,
        incACCr,
        incEVAr,
        incCr,
        incDAMr,
        RecoveryHP,
        RecoveryMP,
        face,
        prop,
        time,
        HP,
        MP,
        attackType,
        ignoreTargetDEF,
        ignoreDAM,
        ignoreDAMr,
        DAMreflect,
        mpconReduce,
        mpRestore,
        incMesoProp,
        incRewardProp,
        incAllskill,
        RecoveryUP,
        boss,
        level,
        incTerR,
        incAsrR,
        incEXPr,
        reduceCooltime,
        incCriticaldamageMax,
        incCriticaldamageMin,
        @sealed,
        incSTRlv,
        incDEXlv,
        incINTlv,
        incLUKlv,
        incMaxDamage,
        incPADlv,
        incMADlv,
        incCriticaldamage,

        attackSpeed = 200,
        tuc,
        setItemID,
        durability,
        bossReward,
        reduceReq,
        growthLevel,
        growthExp,

        reqLevel = 1000,
        reqSTR,
        reqDEX,
        reqINT,
        reqLUK,
        reqJob,
        reqSpecJob,
        pddDiff,
        BDrDiff,
        IMDrDiff,

        // 고유 아이템
        only = 1100,
        // 퀘스트 아이템
        quest,
        // 교환 불가
        tradeBlock,
        // 장착 시 교환 불가
        equipTradeBlock,
        // 1회 교환가능 (거래 후 교환불가)
        tradeOnce,
        // 월드 내 나의 캐릭터간 이동만 가능
        accountSharable,
        // 계정 내 1회 이동 가능(이동 후 교환 불가)
        sharableOnce,
        // 고유장착 아이템
        onlyEquip,
        // 기간 한정 능력치
        abilityTimeLimited,
        // 유효기간 연장 불가
        notExtend,
        // 황금 망치 사용 불가
        blockGoldHammer,
        // 잠재능력 설정 불가
        noPotential,
        // 잠재능력 재설정 불가
        fixedPotential,
        specialGrade,
        fixedGrade,
        // 수리 불가
        cantRepair,
        // 슈페리얼
        superiorEqp,
        // 강화 불가
        exceptUpgrade,
        // 가위 사용 가능 횟수
        karmaLeft,
        // 카르마의 가위를 사용하면 1회 교환이 가능하게 할 수 있습니다.
        tradeAvailable,
        // 쉐어 네임 텍을 사용하면 1회 같은 계정 내 캐릭터로 이동할 수 있습니다.
        accountShareTag,
        // 3개 이상 착용하고 있는 모든 세트 아이템에 포함되는 럭키 아이템!
        jokerToSetItem,
        // 전용 주문서만 사용 가능
        onlyUpgrade,
        // 장착 시 1회에 한해 {성향} {value}(, {성향} {value})의 경험치를 얻으실 수 있습니다.
        // 카리스마
        charismaEXP,
        // 감성
        senseEXP,
        // 통찰력
        insightEXP,
        // 의지
        willEXP,
        // 손재주
        craftEXP,
        // 매력
        charmEXP,
        // (힘/지력/민첩성/행운)의 이그드라실의 축복
        yggdrasil,
        // 프로텍트 주문서가 적용되어 있습니다. (12성 이상의 장비에는 효과 없음)
        scrollProtect,
        // 주문서 보호 효과가 적용되어 있습니다.
        scrollRecover,
        // 리턴 주문서 효과가 적용되어 있습니다.
        scrollReturn,
        // 슬롯 사용 만료일 YYYY년 MM월 DD일 HH시 mm분 이후에는 아이템 장착효과가 사라집니다.
        // slot_expire,

        _null,
    }
}
