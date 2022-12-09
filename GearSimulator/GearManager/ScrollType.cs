using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GearManager
{
    public enum ScrollType
    {
        fail = -1,
        // 주문의 흔적
        spellTrace100 = 0,
        spellTrace70,
        spellTrace30,
        spellTrace15,

        chaos,
        incredibleChaos,
        incredibleChaosOfGoodness,
        enhance100,
        enhance50,
        yggdrasilSTR,
        yggdrasilDEX,
        yggdrasilINT,
        yggdrasilLUK,

        armorPAD = 100,
        armorMAD,
        miracleArmorPAD,
        miracleArmorMAD,
        armorPADScroll,
        armorMADScroll,
        ultimateArmor,
        tenthAnnivArmor,
        tenthAnnivPrimeArmor,
        happytimeArmorATT,
        frostyArmorEnhance,

        accPAD = 200,
        accMAD,
        miracleAccPAD,
        miracleAccMAD,
        accPADScroll,
        accMADScroll,
        premiumAccPADScroll,
        premiumAccMADScroll,
        tenthAnnivAcc,
        tenthAnnivPrimeAcc,

        magicalPAD = 300,
        magicalMAD,

        earringINT10 = 1000,
        dragonStone,
        fragmentOfTwistedTime,
    }
}
