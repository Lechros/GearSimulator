using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace GearManager
{
    public static class GearResource
    {
        private static readonly string GearDataXml = "lib/Resources/GearData.xml";
        private static readonly string GearStringXml = "lib/Resources/GearString.xml";

        public static Gear CreateGear(string name)
        {
            return CreateGear(FirstGearCodeByGearName(name));
        }

        public static Gear CreateGear(int code)
        {
            XmlNode gearNode = FindGearNode(code);
            if(gearNode == null) return new Gear();

            int value;
            Gear gear = new Gear();
            GetNameDescByCode(code, out string gearName, out string gearDesc);
            gear.Name = gearName;
            gear.Desc = gearDesc;
            gear.Type = Gear.GetGearType(code);

            gear.IconRaw = GetGearIcon(code);
            XmlNode tempNode = gearNode.SelectSingleNode("dir[@name='info']/dir[@name='onlyUpgrade']");
            if(tempNode != null && tempNode.HasChildNodes)
            {
                gear.Props[GearPropType.onlyUpgrade] = 1;
            }
            foreach(XmlNode node in gearNode.SelectNodes("dir[@name='info']/int32|dir[@name='info']/string"))
            {
                if(!Enum.TryParse(node.Attributes["name"].Value, out GearPropType type)) continue;
                value = Convert.ToInt32(node.Attributes["value"].Value);
                switch(type)
                {
                    case GearPropType.incSTR:
                    case GearPropType.incDEX:
                    case GearPropType.incINT:
                    case GearPropType.incLUK:
                    case GearPropType.incMHP:
                    case GearPropType.incMMP:
                    case GearPropType.incMHPr:
                    case GearPropType.incMMPr:
                    case GearPropType.incPAD:
                    case GearPropType.incMAD:
                    case GearPropType.incPDD:
                    case GearPropType.incSpeed:
                    case GearPropType.incJump:
                    case GearPropType.knockback:
                    case GearPropType.bdR:
                    case GearPropType.imdR:
                    case GearPropType.damR:
                    case GearPropType.reduceReq:
                        gear.BaseStat.Add(type, value);
                        break;
                    case GearPropType.reqLevel:
                    case GearPropType.reqSTR:
                    case GearPropType.reqDEX:
                    case GearPropType.reqINT:
                    case GearPropType.reqLUK:
                    case GearPropType.reqJob:
                    case GearPropType.reqSpecJob:
                    case GearPropType.attackSpeed:
                    case GearPropType.bossReward:
                    case GearPropType.only:
                    case GearPropType.tradeBlock:
                    case GearPropType.equipTradeBlock:
                    case GearPropType.accountSharable:
                    case GearPropType.sharableOnce:
                    case GearPropType.onlyEquip:
                    case GearPropType.abilityTimeLimited:
                    case GearPropType.notExtend:
                    case GearPropType.blockGoldHammer:
                    case GearPropType.noPotential:
                    case GearPropType.fixedPotential:
                        gear.Props.Add(type, value);
                        break;
                    case GearPropType.fixedGrade:
                        gear.Props.Add(type, value);
                        gear.Grade = (GearGrade)((value + 1) / 2);
                        break;
                    case GearPropType.specialGrade:
                    case GearPropType.cantRepair:
                    case GearPropType.superiorEqp:
                    case GearPropType.exceptUpgrade:
                    case GearPropType.tradeAvailable:
                    case GearPropType.accountShareTag:
                    case GearPropType.jokerToSetItem:
                    case GearPropType.charismaEXP:
                    case GearPropType.senseEXP:
                    case GearPropType.insightEXP:
                    case GearPropType.willEXP:
                    case GearPropType.craftEXP:
                    case GearPropType.charmEXP:
                        gear.Props.Add(type, value);
                        break;
                    case GearPropType.tuc:
                        gear.ScrollAvailable = value;
                        gear.HasTuc = value > 0;
                        break;
                }
            }
            gear.MaxStar = Gear.CalculateMaxStar(gear);
            gear.Quality = GearQuality.Middle;
            if(gear.Props.TryGetValue(GearPropType.specialGrade, out value) && value > 0)
            {
                tempNode = gearNode.SelectSingleNode("dir[@name='info']/dir[@name='option']");
                if(tempNode != null)
                {
                    foreach(XmlNode opNode in tempNode.SelectNodes("dir"))
                    {
                        int opIdx = Convert.ToInt32(opNode.Attributes["name"].Value);
                        int opCode = Convert.ToInt32(opNode.SelectSingleNode("int32[@name='option']").Attributes["value"].Value);
                        gear.Options[opIdx] = Potential.FromCode(opCode, gear.Props[GearPropType.reqLevel]);
                    }
                }
            }

            return gear;
        }
        public static BitmapOrigin GetGearIcon(string name)
        {
            return GetGearIcon(FirstGearCodeByGearName(name));
        }

        public static BitmapOrigin GetGearIcon(int code)
        {
            XmlNode gearNode = FindGearNode(code);
            if(gearNode == null) return new BitmapOrigin();

            XmlNode iconRawNode = gearNode.SelectSingleNode("dir[@name='info']/png[@name='iconRaw']");
            XmlNode iconNode = gearNode.SelectSingleNode("dir[@name='info']/png[@name='icon']");
            if(iconRawNode != null)
            {
                XmlNode linkNode = iconRawNode.SelectSingleNode("string[not(@name='_hash')]");
                if(linkNode != null)
                {
                    string linkNodeValue = linkNode.Attributes["name"].Value;
                    if(linkNodeValue == "_inlink")
                    {
                        if(iconRawNode.SelectSingleNode("string").Attributes["value"].Value == "info/icon")
                        {
                            return BitmapOriginFromIconNode(iconNode);
                        }
                    }
                    else if(linkNodeValue == "_outlink")
                    {
                        string[] outlink = linkNode.Attributes["value"].Value.Split('/');
                        int newCode = Convert.ToInt32(outlink[2].Replace(".img", ""));
                        return GetGearIcon(newCode);
                    }
                }
                return BitmapOriginFromIconNode(iconRawNode);
            }
            else
            {
                return BitmapOriginFromIconNode(iconNode);
            }
        }

        public static SortedDictionary<int, string> SearchGear(string name, bool exactMatch = false)
        {
            using(XmlReader reader = XmlReader.Create(GearStringXml))
            {
                SortedDictionary<int, string> res = new SortedDictionary<int, string>();
                name = (name ?? "").Replace(" ", "");
                if(name.Length < 1) return res;

                while(reader.ReadToFollowing("dir"))
                {
                    if(reader.Depth != 2) continue;

                    int gearCode = Convert.ToInt32(reader.GetAttribute("name"));
                    if(reader.ReadToDescendant("string"))
                    {
                        string gearName = reader.GetAttribute("value");
                        if(exactMatch)
                        {
                            if(gearName.Replace(" ", "") == name)
                            {
                                res.Add(gearCode, gearName);
                            }
                        }
                        else
                        {
                            if(gearName.Replace(" ", "").Contains(name))
                            {
                                res.Add(gearCode, gearName);
                            }
                        }
                    }
                }

                return res;
            }
        }

        private static BitmapOrigin BitmapOriginFromIconNode(XmlNode iconNode)
        {
            Bitmap icon = DecodeImage(iconNode.Attributes["value"].Value);

            string originStr = iconNode.SelectSingleNode("vector[@name='origin']").Attributes["value"].Value;
            int X = Convert.ToInt32(originStr.Split(',')[0]);
            int Y = Convert.ToInt32(originStr.Split(',')[1]);

            return new BitmapOrigin(icon, X, Y);
        }

        public static int FirstGearCodeByGearName(string name)
        {
            using(XmlReader reader = XmlReader.Create(GearStringXml))
            {
                name = (name ?? "").Replace(" ", "");

                while(reader.ReadToFollowing("dir"))
                {
                    if(reader.Depth != 2) continue;

                    int gearCode = Convert.ToInt32(reader.GetAttribute("name"));
                    if(reader.ReadToDescendant("string"))
                    {
                        string gearName = reader.GetAttribute("value");
                        if(gearName.Replace(" ", "") == name)
                        {
                            return gearCode;
                        }
                    }
                }
                return -1;
            }
        }

        public static bool GetNameDescByCode(int code, out string name, out string desc)
        {
            using(XmlReader reader = XmlReader.Create(GearStringXml))
            {
                name = null;
                desc = null;
                while(reader.ReadToFollowing("dir"))
                {
                    string codeStr = code.ToString("D7");

                    if(reader.GetAttribute("name") == codeStr)
                    {
                        using(XmlReader inner = reader.ReadSubtree())
                        {
                            while(inner.ReadToFollowing("string"))
                            {
                                switch(inner.GetAttribute("name"))
                                {
                                    case "name": name = inner.GetAttribute("value"); break;
                                    case "desc": desc = inner.GetAttribute("value"); break;
                                }
                            }
                        }
                        if(!string.IsNullOrWhiteSpace(name)) return true;
                    }
                }
                return false;
            }
        }

        private static XmlNode FindGearNode(int code)
        {
            using(XmlReader reader = XmlReader.Create(GearDataXml))
            {
                string codeStr = code.ToString("D7");

                while(reader.ReadToFollowing("dir"))
                {
                    if(reader.GetAttribute("name") == codeStr)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(reader.ReadOuterXml());
                        return doc.DocumentElement;
                    }
                }
                return null;
            }
        }

        private static Bitmap DecodeImage(string base64String)
        {
            byte[] data = System.Convert.FromBase64String(base64String);
            using(var stream = new System.IO.MemoryStream(data, 0, data.Length))
            {
                Bitmap image = Image.FromStream(stream) as Bitmap;
                return image;
            }
        }
    }
}