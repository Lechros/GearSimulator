using System;
using System.Collections.Generic;
using System.Xml;

namespace GearManager
{
    public class Potential
    {
        private static readonly string ItemOptionDataXml = "lib/Resources/ItemOptionData.xml";

        private Potential()
        {
            Props = new Dictionary<GearPropType, int>();
        }

        public int Code { get; private set; }
        public int OptionType { get; private set; }
        public int ReqLevel { get; private set; }
        public Dictionary<GearPropType, int> Props { get; private set; }
        public string StringSummary { get; private set; }

        public bool IsPotentialEx
        {
            get { return Code / 1000 % 10 == 2; }
        }

        public override string ToString()
        {
            return StringSummary;
        }

        public static int GetPotentialLevel(int gearReqLevel)
        {
            if(gearReqLevel <= 0) return 1;
            else if(gearReqLevel >= 200) return 20;
            else return (gearReqLevel + 9) / 10;
        }

        public static Potential CreateWithString(string summary)
        {
            Potential pot = new Potential();
            pot.StringSummary = summary;
            return pot;
        }

        public static Potential FromCode(int code, int gearReqLevel)
        {
            using(XmlReader reader = XmlReader.Create(ItemOptionDataXml))
            {
                XmlNode potentialNode = null;
                while(reader.ReadToFollowing("dir"))
                {
                    if(reader.GetAttribute("name") == code.ToString("D6"))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(reader.ReadOuterXml());
                        potentialNode = doc.DocumentElement;
                    }
                }
                if(potentialNode == null) return null;

                Potential pot = new Potential
                {
                    Code = code
                };
                if(potentialNode.SelectSingleNode("dir[@name='info']/int32[@name='optionType']") != null)
                    pot.OptionType = Convert.ToInt32(potentialNode.SelectSingleNode("dir[@name='info']/int32[@name='optionType']").Attributes["value"].Value);
                if(potentialNode.SelectSingleNode("dir[@name='info']/int32[@name='reqLevel']") != null)
                    pot.ReqLevel = Convert.ToInt32(potentialNode.SelectSingleNode("dir[@name='info']/int32[@name='reqLevel']").Attributes["value"].Value);

                string summary = potentialNode.SelectSingleNode("dir[@name='info']/string[@name='string']").Attributes["value"].Value;
                summary = summary.Replace("&lt;", "<").Replace("&gt;", ">");
                XmlNode propNode = potentialNode.SelectSingleNode("dir[@name='level']/dir[@name='" + GetPotentialLevel(gearReqLevel) + "']");
                foreach(XmlNode node in propNode.SelectNodes("int32"))
                {
                    summary = summary.Replace('#' + node.Attributes["name"].Value, node.Attributes["value"].Value);
                }
                pot.StringSummary = summary;

                return pot;
            }
        }

        public static SortedDictionary<int, string> AvailableOptions(GearType gearType, GearGrade gearGrade, int gearReqLevel, bool isAdOption, int optionIndex = 0)
        {
            XmlDocument data = new XmlDocument();
            data.Load(ItemOptionDataXml);

            SortedDictionary<int, string> res = new SortedDictionary<int, string>();
            foreach(XmlNode potentialNode in data.DocumentElement.SelectNodes("dir"))
            {
                int code = Convert.ToInt32(potentialNode.Attributes["name"].Value);
                if(100 < code && code <= 900) continue;
                if(2100 < code && code <= 2900) continue;

                // Check grade
                if(code / 10000 % 10 < (int)gearGrade - (optionIndex == 0 ? 0 : 1)) continue;
                if(code / 10000 % 10 > (int)gearGrade) break;

                // Check Additional
                if((code / 1000 % 10 == 2) != isAdOption) continue;

                // Check GearType
                XmlNode optionTypeNode = potentialNode.SelectSingleNode("dir[@name='info']/int32[@name='optionType']");
                if(optionTypeNode != null)
                {
                    int optionType = Convert.ToInt32(optionTypeNode.Attributes["value"].Value);
                    if(!CheckOptionType(optionType, gearType)) continue;
                }

                // Check ReqLevel
                XmlNode reqLevelNode = potentialNode.SelectSingleNode("dir[@name='info']/int32[@name='reqLevel']");
                if(reqLevelNode != null)
                {
                    int reqLevel = Convert.ToInt32(reqLevelNode.Attributes["value"].Value);
                    if(gearReqLevel < reqLevel) continue;
                }

                string summary = potentialNode.SelectSingleNode("dir[@name='info']/string[@name='string']").Attributes["value"].Value;
                XmlNode propNode = potentialNode.SelectSingleNode("dir[@name='level']/dir[@name='" + GetPotentialLevel(gearReqLevel) + "']");
                foreach(XmlNode node in propNode.SelectNodes("int32"))
                {
                    summary = summary.Replace('#' + node.Attributes["name"].Value, node.Attributes["value"].Value);
                }

                res.Add(code, summary);
            }

            return res;
        }

        public static bool CheckOptionType(int optionType, GearType gearType)
        {
            switch(optionType)
            {
                case 0: return true;
                case 10:
                    return Gear.IsWeapon(gearType)
                        || Gear.IsSubWeapon(gearType)
                        || gearType == GearType.katara
                        || gearType == GearType.emblem;
                case 11:
                    return !CheckOptionType(10, gearType);
                case 20:
                    return gearType == GearType.pants
                        || gearType == GearType.shoes
                        || gearType == GearType.cap
                        || gearType == GearType.coat
                        || gearType == GearType.longcoat
                        || gearType == GearType.glove
                        || gearType == GearType.cape;
                case 40:
                    return gearType == GearType.ring
                        || gearType == GearType.earrings
                        || gearType == GearType.pendant
                        || gearType == GearType.belt;
                case 51: return gearType == GearType.cap;
                case 52: return gearType == GearType.coat || gearType == GearType.longcoat;
                case 53: return gearType == GearType.pants || gearType == GearType.longcoat;
                case 54: return gearType == GearType.glove;
                case 55: return gearType == GearType.shoes;
                default: return false;
            }
        }
    }
}
