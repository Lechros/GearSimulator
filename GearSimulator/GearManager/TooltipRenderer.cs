using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using TR = System.Windows.Forms.TextRenderer;
using GearSimulator.GearManager;

namespace GearManager
{
    public class TooltipRenderer
    {
        static Dictionary<string, TextureBrush> InitializeRes()
        {
            Dictionary<string, TextureBrush> res = new Dictionary<string, TextureBrush>
            {
                ["t"] = new TextureBrush(Resource.UIToolTip_img_Item_Frame_top, WrapMode.Clamp),
                ["line"] = new TextureBrush(Resource.UIToolTip_img_Item_Frame_line, WrapMode.Tile),
                ["dotline"] = new TextureBrush(Resource.UIToolTip_img_Item_Frame_dotline, WrapMode.Clamp),
                ["b"] = new TextureBrush(Resource.UIToolTip_img_Item_Frame_bottom, WrapMode.Clamp),
                ["cover"] = new TextureBrush(Resource.UIToolTip_img_Item_Frame_cover, WrapMode.Clamp)
            };
            return res;
        }
        private static readonly Dictionary<string, TextureBrush> res = InitializeRes();
        private static readonly int absoluteDpi = 96;

        public TooltipRenderer(Gear gear)
        {
            Gear = gear;
        }

        public TooltipRenderer()
        {
        }

        public Gear Gear { get; set; }

        public static Bitmap Render(Gear gear, bool reboot = false)
        {
            TooltipRenderer tr = new TooltipRenderer(gear);
            return tr.Render(reboot);
        }

        public Bitmap Render(bool reboot = false)
        {
            if(Gear == null)
            {
                return null;
            }

            Bitmap tooltip;
            int width = 261, height;
            using(Bitmap temp = RenderBase(out height, reboot))
            {
                tooltip = new Bitmap(width, height);
                tooltip.SetResolution(absoluteDpi, absoluteDpi);
                Graphics g = Graphics.FromImage(tooltip);

                g.DrawImage(res["t"].Image, 0, 0);
                g.FillRectangle(res["line"], 0, 13, width, height - 26);
                g.DrawImage(res["b"].Image, 0, height - 13);

                g.DrawImage(res["cover"].Image, 3, 3);

                g.DrawImage(temp, 0, 0);
            }

            return tooltip;
        }
        private Bitmap RenderBase(out int picH, bool reboot)
        {
            int width = 261;
            int height = 2000;
            int value;

            Bitmap tooltip = new Bitmap(width, height);
            tooltip.SetResolution(absoluteDpi, absoluteDpi);
            Graphics g = Graphics.FromImage(tooltip);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            g.Clear(Color.Transparent);

            width = 261;
            picH = 13;
            // 이름 새기기
            if(Gear.NameTag != null && Gear.NameTag.Length > 0)
            {
                string nameTagStr = "#c" + Gear.NameTag + "#의";
                TextRenderer.DrawText(g, nameTagStr, TextRenderer.NameTagFont, width, picH, StringAlignment.Center);
                picH += 24;
            }

            // 스타포스
            if(Gear.Star > 0 || Gear.MaxStar > 0)
            {
                DrawStar(g, ref picH);
            }

            // 소울
            if(Gear.SoulEnchanted && !string.IsNullOrEmpty(Gear.Soul.Name))
            {
                string soulName = Gear.Soul.Name.Replace(" 소울", "");
                TextRenderer.DrawText(g, soulName, TextRenderer.ItemNameFont, width, picH, TextRenderer.GreenColor, StringAlignment.Center);
                picH += 19;
            }

            // 아이템명
            string gearName = Gear.Name + ((!reboot && (Gear.ScrollUp > 0)) ? (" (+" + Gear.ScrollUp + ")") : null);
            TextRenderer.DrawTitleText(g, gearName, width, ref picH, GetGearNameColor(Gear.Quality));
            // TextRenderer.DrawText(g, gearName, TextRenderer.ItemNameFont, width, picH, GetGearNameColor(Gear.Quality), StringAlignment.Center);

            // 잠재능력 등급
            if(Gear.Props.TryGetValue(GearPropType.specialGrade, out value) && value > 0)
            {
                TextRenderer.DrawText(g, StringHelper.GetGearGradeString(GearGrade.Special), width, picH, StringAlignment.Center);
                picH += 15;
            }
            else if(Gear.Grade > 0)
            {
                TextRenderer.DrawText(g, StringHelper.GetGearGradeString(Gear.Grade), width, picH, StringAlignment.Center);
                picH += 15;
            }

            // 교환 등
            var attrList = GetGearAttributeString();
            if(attrList.Count > 0)
            {
                var font = TextRenderer.ItemDetailFont;
                string attrStr = null;
                for(int i = 0; i < attrList.Count; i++)
                {
                    var newStr = (attrStr != null ? (attrStr + ", ") : null) + attrList[i];
                    if(TR.MeasureText(newStr, font).Width > 261 - 7)
                    {
                        TextRenderer.DrawText(g, attrStr, font, width, picH, TextRenderer.OrangeColor, StringAlignment.Center);
                        picH += 15;
                        attrStr = attrList[i];
                    }
                    else
                    {
                        attrStr = newStr;
                    }
                }
                if(!string.IsNullOrEmpty(attrStr))
                {
                    TextRenderer.DrawText(g, attrStr, font, width, picH, TextRenderer.OrangeColor, StringAlignment.Center);
                    picH += 15;
                }
            }

            // 기간제 아이템
            if(Gear.TimeLimited >= DateTime.Parse("2003/4/29"))
            {
                string expireStr = Gear.TimeLimited.ToString("yyyy년 M월 d일 HH시 mm분까지 사용가능");
                TextRenderer.DrawText(g, expireStr, width, picH, TextRenderer.WhiteColor, StringAlignment.Center);
                picH += 15;
            }
            // 봉인의 자물쇠
            if(Gear.Lock >= DateTime.Parse("2003/4/29"))
            {
                string expireStr = Gear.Lock.ToString("yyyy년 M월 d일 HH시 mm분 봉인해제");
                TextRenderer.DrawText(g, expireStr, width, picH, TextRenderer.WhiteColor, StringAlignment.Center);
                picH += 15;
            }

            // 구분선 1
            picH += 8;
            g.DrawImage(res["dotline"].Image, 0, picH);

            // 아이콘 테두리
            if(0 < Gear.Grade && (int)Gear.Grade <= 4)
            {
                Image border = Resource.ResourceManager.GetObject("UIToolTip_img_Item_ItemIcon_" + (int)Gear.Grade) as Image;
                if(border != null)
                {
                    g.DrawImage(border, 13, picH + 11);
                }
            }
            // 아이콘 배경
            g.DrawImage(Resource.UIToolTip_img_Item_ItemIcon_base, 12, picH + 10);

            if(Gear.IconRaw.Bitmap != null) {
                var attr = new ImageAttributes();
                var matrix = new ColorMatrix(
                    new[] {
                        new float[] { 1, 0, 0, 0, 0 },
                        new float[] { 0, 1, 0, 0, 0 },
                        new float[] { 0, 0, 1, 0, 0 },
                        new float[] { 0, 0, 0, 0.5f, 0 },
                        new float[] { 0, 0, 0, 0, 1 },
                        });
                attr.SetColorMatrix(matrix);

                var shade = Resource.UIToolTip_img_Item_ItemIcon_shade;
                g.DrawImage(shade,
                    new Rectangle(18 + 9, picH + 15 + 54, shade.Width, shade.Height),
                    0, 0, shade.Width, shade.Height,
                    GraphicsUnit.Pixel,
                    attr);
                g.DrawImage(EnlargeBitmap(Gear.IconRaw.Bitmap),
                    18 + (1 - Gear.IconRaw.Origin.X) * 2,
                    picH + 16 + (33 - Gear.IconRaw.Origin.Y) * 2);
                attr.Dispose();
            }
            // 아이콘 커버
            g.DrawImage(Resource.UIToolTip_img_Item_ItemIcon_old, 14 - 2 + 5, picH + 9 + 5);
            // 봉인의 자물쇠
            if(Gear.Lock >= DateTime.Parse("2003/4/29"))
            {
                g.DrawImage(EnlargeBitmap(Resource.ItemProtector_Icon_0), 60, picH + 58);
            }
            // 그린 PC 잠금
            else if(Gear.GreenLock)
            {
                g.DrawImage(EnlargeBitmap(Resource.ItemProtector_Icon_1), 60, picH + 58);
            }
            // 추옵 동그라미
            if(Gear.AdditionalStat.Sum(x => x.Value) > 0)
            {
                g.DrawImage(Resource.UIToolTip_img_Item_ItemIcon_new, 14 - 2 + 7, picH + 9 + 7);
            }
            g.DrawImage(Resource.UIToolTip_img_Item_ItemIcon_cover, 16, picH + 14);

            // 공격력 증가량
            TextRenderer.DrawText(g, "공격력 증가량", 177, picH + 10, TextRenderer.GrayColor2);
            DrawIncline(g, picH + 27, false);

            // 장비 REQ
            DrawGearReq(g, 97, picH + 59);
            picH += 94;

            DrawPropDiffEx(g, 12, picH);
            picH += 20;

            DrawJobReq(g, ref picH);

            g.DrawImage(res["dotline"].Image, 0, picH);
            picH += 8;

            bool hasPart2 = false;
            // 슈페리얼
            if(Gear.Props.TryGetValue(GearPropType.superiorEqp, out value) && value > 0)
            {
                TextRenderer.DrawText(g, "슈페리얼", width, picH, TextRenderer.GreenColor, StringAlignment.Center);
                picH += 15;
            }

            // 성장
            int level, exp;
            if(Gear.Props.TryGetValue(GearPropType.growthLevel, out level) && level > 0)
            {
                string levelStr, expStr;
                if(level > 5)
                {
                    levelStr = "MAX";
                    expStr = "MAX";
                }
                else
                {
                    Gear.Props.TryGetValue(GearPropType.growthLevel, out exp);
                    levelStr = level.ToString();
                    expStr = exp.ToString();
                }

                TextRenderer.DrawText(g, "성장 레벨 : " + levelStr, 11, picH, TextRenderer.OrangeColor2);
                picH += 15;
                TextRenderer.DrawText(g, "성장 경험치 : " + expStr, 11, picH, TextRenderer.OrangeColor2);
                picH += 15;
            }

            // 내구도
            if(Gear.Props.TryGetValue(GearPropType.durability, out value))
            {
                TextRenderer.DrawText(g, "내구도 : " + value.ToString() + "%", 11, picH, TextRenderer.GreenColor);
                picH += 15;
            }

            bool isWeapon = Gear.IsWeapon(Gear.Type);
            string typeStr = StringHelper.GetGearTypeString(Gear.Type);
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
                if(Gear.IsLeftWeapon(Gear.Type) || Gear.Type == GearType.katara)
                {
                    typeStr += " (한손무기)";
                }
                else if(Gear.IsDoubleHandWeapon(Gear.Type))
                {
                    typeStr += " (두손무기)";
                }
                TextRenderer.DrawText(g, typeStr, 11, picH, TextRenderer.WhiteColor);
                picH += 15;
                hasPart2 = true;
            }

            // 공격속도
            if(!Gear.Props.TryGetValue(GearPropType.attackSpeed, out value) && (Gear.IsWeapon(Gear.Type) || Gear.Type == GearType.katara))
            {
                value = 6;
            }
            if(value > 0)
            {
                TextRenderer.DrawText(g, "공격속도 : " + StringHelper.GetAttackSpeedString(value), 11, picH, TextRenderer.WhiteColor);
                picH += 15;
                hasPart2 = true;
            }

            // 기본 스탯
            List<GearPropType> props = new List<GearPropType>();
            foreach(KeyValuePair<GearPropType, int> p in Gear.BaseStat)
            {
                if(((int)p.Key < 100 || p.Key == GearPropType.reduceReq) && p.Value != 0)
                {
                    props.Add(p.Key);
                }
            }
            foreach(KeyValuePair<GearPropType, int> p in Gear.AdditionalStat)
            {
                if(!props.Contains(p.Key))
                {
                    if(((int)p.Key < 100 || p.Key == GearPropType.reduceReq) && p.Value != 0)
                    {
                        props.Add(p.Key);
                    }
                }
            }
            foreach(KeyValuePair<GearPropType, int> p in Gear.EnchantStat)
            {
                if(!props.Contains(p.Key))
                {
                    if(((int)p.Key < 100 || p.Key == GearPropType.reduceReq) && p.Value != 0)
                    {
                        props.Add(p.Key);
                    }
                }
            }
            props.Sort();
            foreach(GearPropType type in props)
            {
                int[] s = new int[3];
                Gear.BaseStat.TryGetValue(type, out s[0]);
                Gear.AdditionalStat.TryGetValue(type, out s[1]);
                Gear.EnchantStat.TryGetValue(type, out s[2]);
                TextRenderer.DrawStatText(g, type, s, 11, ref picH);
                hasPart2 = true;
            }

            if(!reboot)
            {
                // 주문서
                if(Gear.Props.TryGetValue(GearPropType.exceptUpgrade, out value) && value > 0)
                {
                    TextRenderer.DrawText(g, "강화불가", 11, picH, TextRenderer.WhiteColor);
                    picH += 15;
                }
                else if(Gear.HasTuc)
                {
                    TextRenderer.DrawText(g, "업그레이드 가능 횟수 : " + Gear.ScrollAvailable + "#o(복구 가능 횟수 : " + Gear.ScrollRestore + ")#", 11, picH);
                    picH += 15;
                    hasPart2 = true;
                }

                // 황금 망치
                if(Gear.HasTuc && Gear.Hammer > 0)
                {
                    TextRenderer.DrawText(g, "황금망치 제련 적용", 11, picH, TextRenderer.WhiteColor);
                    picH += 15;
                }

                // 가위 사용 가능 횟수
                bool tradable = false;
                if(Gear.Props.TryGetValue(GearPropType.tradeBlock, out value) && value > 0)
                {
                    tradable = true;
                }
                if(Gear.Props.TryGetValue(GearPropType.equipTradeBlock, out value) && value > 0)
                {
                    tradable = true;
                }
                if(Gear.Props.TryGetValue(GearPropType.tradeOnce, out value) && value > 0)
                {
                    tradable = true;
                }
                if(tradable && Gear.Props.TryGetValue(GearPropType.karmaLeft, out value) && value >= 0)
                {
                    TextRenderer.DrawText(g, StringHelper.GetGearPropString(GearPropType.karmaLeft, value), 11, picH, TextRenderer.OrangeColor2);
                    picH += 15;
                }
            }

            // 슈페리얼 설명
            if(Gear.Props.TryGetValue(GearPropType.superiorEqp, out value) && value > 0)
            {
                TextRenderer.DrawText(g, StringHelper.GetGearPropString(GearPropType.superiorEqp, value), 11, picH);
                picH += 30;
            }

            picH += 16;

            // 잠재옵션
            int optionCount = 0;
            Gear.Props.TryGetValue(GearPropType.noPotential, out value);
            bool hasPotential = value <= 0;
            foreach(Potential potential in Gear.Options)
            {
                if(potential != null)
                {
                    optionCount++;
                }
            }
            if(hasPotential && Gear.Grade > GearGrade.Normal && optionCount > 0)
            {
                if(hasPart2)
                {
                    g.DrawImage(res["dotline"].Image, 0, picH - 8);
                }
                g.DrawImage(GetOptionIcon(Gear.Grade), 9, picH - 2);
                TextRenderer.DrawText(g, "잠재옵션", 25, picH, GetPotentialTextColor(Gear.Grade));
                picH += 15;
                foreach(Potential potential in Gear.Options)
                {
                    if(potential != null && !string.IsNullOrEmpty(potential.ToString()))
                    {
                        picH += TextRenderer.DrawText(g, potential.ToString(), 11, picH, TextRenderer.WhiteColor);
                    }
                }
                picH += 8;
            }

            // 에디셔널 잠재옵션
            int adOptionCount = 0;
            foreach(Potential potential in Gear.AdditionalOptions)
            {
                if(potential != null)
                {
                    adOptionCount++;
                }
            }
            if(hasPotential && Gear.AdditionGrade > GearGrade.Normal && adOptionCount > 0)
            {
                if(hasPart2)
                {
                    g.DrawImage(res["dotline"].Image, 0, picH - 8);
                }
                g.DrawImage(GetOptionIcon(Gear.AdditionGrade), 9, picH - 2);
                TextRenderer.DrawText(g, "에디셔널 잠재옵션", 25, picH, GetPotentialTextColor(Gear.AdditionGrade));
                picH += 15;
                foreach(Potential potential in Gear.AdditionalOptions)
                {
                    if(potential != null && !string.IsNullOrEmpty(potential.ToString()))
                    {
                        picH += TextRenderer.DrawText(g, "+ " + potential.ToString(), 11, picH, TextRenderer.WhiteColor);
                    }
                }
                picH += 8;
            }

            // 소울
            if(Gear.SoulEnchanted)
            {
                if(hasPart2)
                {
                    g.DrawImage(res["dotline"].Image, 0, picH - 8);
                }
                DrawSoul(g, 11, ref picH);
                picH += 8;
            }

            // 설명
            List<string> desc = new List<string>();
            if(!string.IsNullOrEmpty(Gear.Desc))
            {
                desc.Add(Gear.Desc);
            }
            if(Gear.Props.TryGetValue(GearPropType.tradeBlock, out value) && value > 0 && Gear.Props.TryGetValue(GearPropType.tradeAvailable, out value) && value > 0)
            {
                desc.Add(StringHelper.GetGearPropString(GearPropType.tradeAvailable, value));
            }
            GearPropType[] descTypes = new GearPropType[]{
                GearPropType.accountShareTag,
                GearPropType.jokerToSetItem,
            };
            foreach(GearPropType type in descTypes)
            {
                if(Gear.Props.TryGetValue(type, out value) && value > 0)
                {
                    desc.Add(StringHelper.GetGearPropString(type, value));
                }
            }

            // 성향
            string incline = null;
            GearPropType[] inclineTypes = new GearPropType[]{
                GearPropType.charismaEXP,
                GearPropType.insightEXP,
                GearPropType.willEXP,
                GearPropType.craftEXP,
                GearPropType.senseEXP,
                GearPropType.charmEXP
            };
            string[] inclineString = new string[]{
                "카리스마","통찰력","의지","손재주","감성","매력"
            };
            for(int i = 0; i < inclineTypes.Length; i++)
            {
                if(Gear.Props.TryGetValue(inclineTypes[i], out value) && value > 0)
                {
                    incline += ", " + inclineString[i] + " " + value;
                }
            }
            if(!string.IsNullOrEmpty(incline))
            {
                desc.Add("");
                desc.Add("#c장착 시 1회에 한해 " + incline.Substring(2) + "의 경험치를 얻으실 수 있습니다.#");
            }
            // 놀장
            if(!Gear.StarTypeYellow && Gear.Star > 0)
            {
                desc.Add("#c놀라운 장비강화 주문서가 사용되었습니다.#");
            }
            // 주문서 효과
            GearPropType[] scrollTypes = new GearPropType[]{
                GearPropType.scrollProtect,
                GearPropType.scrollRecover,
                GearPropType.scrollReturn,
            };
            foreach(var type in scrollTypes)
            {
                if(Gear.Props.TryGetValue(type, out value) && value > 0)
                {
                    desc.Add(StringHelper.GetGearPropString(type, value));
                }
            }
            switch(Gear.Yggdrasil)
            {
                case 1: desc.Add("#c힘의 이그드라실의 축복 성공#"); break;
                case 2: desc.Add("#c지력의 이그드라실의 축복 성공#"); break;
                case 3: desc.Add("#c민첩성의 이그드라실의 축복 성공#"); break;
                case 4: desc.Add("#c행운의 이그드라실의 축복 성공#"); break;
            }

            if(desc.Count > 0)
            {
                if(hasPart2)
                {
                    g.DrawImage(res["dotline"].Image, 0, picH - 8);
                }
                foreach(string s in desc)
                {
                    picH += TextRenderer.DrawText(g, s, 8, picH);
                }
                picH += 8;
            }

            // 신비의 모루
            if(!string.IsNullOrEmpty(Gear.FusionAnvil))
            {
                picH += 7;
                picH += TextRenderer.DrawText(g, "신비의 모루에 의해 [" + Gear.FusionAnvil + "]의 외형이 합성됨", 11, picH, TextRenderer.GreenColor);
                picH += 8;
            }

            return tooltip;
        }

        private void DrawStar(Graphics g, ref int picH)
        {
            Bitmap star = Gear.StarTypeYellow ? Resource.UIToolTip_img_Item_Equip_Star_Star : Resource.UIToolTip_img_Item_Equip_Star_Star1;
            Bitmap emptyStar = Resource.UIToolTip_img_Item_Equip_Star_Star0;

            int maxStar = Gear.MaxStar;
            maxStar = Math.Max(maxStar, Gear.Star);

            if(maxStar > 0)
            {
                for(int i = 0; i < maxStar; i += 15)
                {
                    int starLine = Math.Min(maxStar - i, 15);
                    int totalWidth = starLine * 10 + (starLine / 5 - 1) * 6;
                    int dx = 130 - totalWidth / 2;
                    for(int j = 0; j < starLine; j++)
                    {
                        g.DrawImage((i + j < Gear.Star) ? star : emptyStar, dx, picH);
                        dx += 10;
                        if(j > 0 && j % 5 == 4)
                        {
                            dx += 6;
                        }
                    }
                    picH += 18;
                }
                // picH -= 1;
            }
        }

        private void DrawIncline(Graphics g, int y, bool noIncline)
        {
            int incValue = Gear.Incline % 100000000;
            var res = new Dictionary<string, BitmapOrigin>();
            var BOstack = new Stack<BitmapOrigin>();

            res["+"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_incline_plus, new Point(1, 4));
            res["0"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_incline_0, new Point(1, 0));
            res["1"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_incline_1, new Point(4, 0));
            res["2"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_incline_2, new Point(1, 0));
            res["3"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_incline_3, new Point(2, 0));
            res["4"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_incline_4, new Point(1, 0));
            res["5"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_incline_5, new Point(2, 0));
            res["6"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_incline_6, new Point(1, 0));
            res["7"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_incline_7, new Point(2, 0));
            res["8"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_incline_8, new Point(1, 0));
            res["9"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_incline_9, new Point(1, 0));
            res["-"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_decline_minus, new Point(2, 9));
            res["-0"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_decline_0, new Point(1, 0));
            res["-1"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_decline_1, new Point(4, 0));
            res["-2"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_decline_2, new Point(1, 0));
            res["-3"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_decline_3, new Point(2, 0));
            res["-4"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_decline_4, new Point(1, 0));
            res["-5"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_decline_5, new Point(2, 0));
            res["-6"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_decline_6, new Point(1, 0));
            res["-7"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_decline_7, new Point(2, 0));
            res["-8"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_decline_8, new Point(1, 0));
            res["-9"] = new BitmapOrigin(Resource.UIToolTip_img_Item_Equip_Summary_decline_9, new Point(1, 0));

            if(noIncline)
            {
                BOstack.Push(res["-"]);
            }
            else if(incValue == 0)
            {
                BOstack.Push(res["0"]);
            }
            else if(incValue > 0)
            {
                string incStr = incValue.ToString();

                BOstack.Push(res["+"]);
                foreach(char c in incStr)
                {
                    BOstack.Push(res[c.ToString()]);
                }
            }
            else
            {
                string incStr = incValue.ToString();

                BOstack.Push(res["-"]);
                foreach(char c in incStr)
                {
                    BOstack.Push(res["-" + c]);
                }
            }

            int dx = 261 - 8;
            if(Math.Abs(incValue) > 9999999)
            {
                foreach(var BO in BOstack)
                {
                    Image newImg;
                    if(BO.Equals(res["+"]) || BO.Equals(res["-"]))
                    {
                        newImg = BO.Bitmap;
                    }
                    else
                    {
                        newImg = ResizeImage(BO.Bitmap, BO.Bitmap.Width - 1, BO.Bitmap.Height - 2);
                    }
                    dx -= newImg.Width;
                    g.DrawImage(newImg, dx, y + BO.Origin.Y);
                    dx -= BO.Origin.X - 1;
                }
            }
            else
            {
                foreach(var IaO in BOstack)
                {
                    if(IaO.Equals(res["+"]) || IaO.Equals(res["-"]))
                    {
                        dx -= 5;
                    }
                    dx -= IaO.Bitmap.Width;
                    g.DrawImage(IaO.Bitmap, dx, y + IaO.Origin.Y);
                    dx -= IaO.Origin.X;
                }
            }
        }

        private void DrawGearReq(Graphics g, int x, int y)
        {
            int value, iX = 54;
            bool can;
            NumberType type;
            int reqLevel, reduceReq = 0;

            // 레벨 제한
            Gear.Props.TryGetValue(GearPropType.reqLevel, out value);
            reqLevel = value;
            Gear.BaseStat.TryGetValue(GearPropType.reduceReq, out reduceReq);
            Gear.AdditionalStat.TryGetValue(GearPropType.reduceReq, out value);
            value = reduceReq + value;
            if(value > 0)
            {
                reduceReq = value > reqLevel ? reqLevel : value;
                value = reqLevel - reduceReq;
            }
            else
            {
                value = reqLevel;
            }
            can = true;
            type = GetReqType(can, value);
            g.DrawImage(FindReqImage(type, "reqLEV", out _), x, y);
            iX += DrawReqNum(g, value.ToString().PadLeft(3), type == NumberType.Can ? NumberType.YellowNumber : type, x + 54, y, StringAlignment.Near);
            if(reduceReq > 0)
            {
                iX += DrawReqNum(g, "(" + reqLevel, NumberType.Can, x + iX, y, StringAlignment.Near);
                iX += DrawReqNum(g, "-" + reduceReq, NumberType.YellowNumber, x + iX, y, StringAlignment.Near);
                DrawReqNum(g, ")", NumberType.Can, x + iX, y, StringAlignment.Near);
            }

            y += 15;

            // STR
            this.Gear.Props.TryGetValue(GearPropType.reqSTR, out value);
            can = true;
            type = GetReqType(can, value);
            g.DrawImage(FindReqImage(type, "reqSTR", out _), x, y);
            DrawReqNum(g, value.ToString("D3"), type, x + 54, y, StringAlignment.Near);


            // LUK
            this.Gear.Props.TryGetValue(GearPropType.reqLUK, out value);
            can = true;
            type = GetReqType(can, value);
            g.DrawImage(FindReqImage(type, "reqLUK", out _), x + 80, y);
            DrawReqNum(g, value.ToString("D3"), type, x + 80 + 54, y, StringAlignment.Near);

            y += 9;

            // DEX
            this.Gear.Props.TryGetValue(GearPropType.reqDEX, out value);
            can = true;
            type = GetReqType(can, value);
            g.DrawImage(FindReqImage(type, "reqDEX", out _), x, y);
            DrawReqNum(g, value.ToString("D3"), type, x + 54, y, StringAlignment.Near);

            // INT
            this.Gear.Props.TryGetValue(GearPropType.reqINT, out value);
            can = true;
            type = GetReqType(can, value);
            g.DrawImage(FindReqImage(type, "reqINT", out _), x + 80, y);
            DrawReqNum(g, value.ToString("D3"), type, x + 80 + 54, y, StringAlignment.Near);

        }

        private void DrawPropDiffEx(Graphics g, int x, int y)
        {
            int value;
            string numValue;

            // 방어력
            g.DrawImage(Resource.UIToolTip_img_Item_Equip_Summary_icon_pdd, x, y);
            x += 62;
            Gear.Props.TryGetValue(GearPropType.pddDiff, out value);
            numValue = (value > 0 ? "+ " : null) + (value < 0 ? "- " : null) + Math.Abs(value);
            DrawReqNum(g, numValue, NumberType.LookAhead, x - 5, y + 6, StringAlignment.Far);

            // 보스 데미지
            g.DrawImage(Resource.UIToolTip_img_Item_Equip_Summary_icon_bdr, x, y);
            x += 62;
            Gear.Props.TryGetValue(GearPropType.BDrDiff, out value);
            numValue = (value > 0 ? "+ " : null) + (value < 0 ? "- " : null) + Math.Abs(value) + " % ";
            DrawReqNum(g, numValue, NumberType.LookAhead, x - 5 + 3, y + 6, StringAlignment.Far);

            // 방어율 무시
            g.DrawImage(Resource.UIToolTip_img_Item_Equip_Summary_icon_igpddr, x, y);
            x += 62;
            Gear.Props.TryGetValue(GearPropType.IMDrDiff, out value);
            numValue = (value > 0 ? "+ " : null) + (value < 0 ? "- " : null) + Math.Abs(value) + " % ";
            DrawReqNum(g, numValue, NumberType.LookAhead, x - 5 - 1, y + 6, StringAlignment.Far);
        }

        private void DrawJobReq(Graphics g, ref int picH)
        {
            int value;
            string extraReq = StringHelper.GetExtraJobReqString(Gear.Type) ??
                (Gear.Props.TryGetValue(GearPropType.reqSpecJob, out value) ? StringHelper.GetExtraJobReqString(value) : null);
            Image jobImage = extraReq == null ? Resource.UIToolTip_img_Item_Equip_Job_normal : Resource.UIToolTip_img_Item_Equip_Job_expand;
            g.DrawImage(jobImage, 12, picH);

            Gear.Props.TryGetValue(GearPropType.reqJob, out int reqJob);
            int[] origin = new int[] { 15, 7, 60, 7, 93, 7, 137, 7, 171, 7, 204, 7 };
            int[] origin2 = new int[] { 15, 7, 60, 7, 93, 7, 137, 7, 171, 7, 204, 7 };
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
                    Image jobImage2 = Resource.ResourceManager.GetObject("UIToolTip_img_Item_Equip_Job_" + (enable ? "enable" : "disable") + "_" + i.ToString()) as Image;
                    if(jobImage != null)
                    {
                        if(enable)
                            g.DrawImage(jobImage2, 12 + origin[i * 2], picH + origin[i * 2 + 1]);
                        else
                            g.DrawImage(jobImage2, 12 + origin2[i * 2], picH + origin2[i * 2 + 1]);
                    }
                }
            }
            if(extraReq != null)
            {
                TextRenderer.DrawText(g, extraReq, 261, picH + 24, TextRenderer.OrangeColor2, StringAlignment.Center);
            }
            picH += jobImage.Height + 9;
        }

        private void DrawSoul(Graphics g, int x, ref int y)
        {
            if(!Gear.SoulEnchanted)
            {
                return;
            }
            if(!string.IsNullOrEmpty(Gear.Soul.Name))
            {
                int chargeAD = Gear.Soul.GetChargeAD();
                y += TextRenderer.DrawText(g, Gear.Soul.Name + " 적용", TextRenderer.SoulFont, x, y, TextRenderer.YellowColor);
                y += TextRenderer.DrawText(g, "소울 충전량 " + Gear.Soul.Charge + "/1000 (공:+" + chargeAD + ",마:+" + chargeAD + ")", TextRenderer.SoulFont, x, y, TextRenderer.GrayColor);
                y += TextRenderer.DrawText(g, Gear.Soul.OptionString, TextRenderer.SoulFont, x, y, TextRenderer.WhiteColor);
                y += TextRenderer.DrawText(g, "소울 충전 시 '" + Gear.Soul.SkillName + "' 사용가능", TextRenderer.SoulFont, x, y, TextRenderer.OrangeColor3);
            }
        }

        private int DrawReqNum(Graphics g, string numString, NumberType type, int x, int y, StringAlignment align)
        {
            int x0 = x;
            if(g == null || numString == null || align == StringAlignment.Center)
            {
                return 0;
            }
            int spaceWidth = type == NumberType.LookAhead ? 3 : 6;
            bool near = align == StringAlignment.Near;

            for(int i = 0; i < numString.Length; i++)
            {
                char c = near? numString[i] : numString[numString.Length - i - 1];
                Image image = null;
                Point origin = Point.Empty;
                switch(c)
                {
                    case ' ':
                        break;
                    case '+':
                        image = Resource.ResourceManager.GetObject("UIToolTip_img_Item_Equip_" + type.ToString() + "_" + "plus") as Image;
                        break;
                    case '-':
                        image = Resource.ResourceManager.GetObject("UIToolTip_img_Item_Equip_" + type.ToString() + "_" + "minus") as Image;
                        origin.Y = 2;
                        break;
                    case '%':
                        image = Resource.ResourceManager.GetObject("UIToolTip_img_Item_Equip_" + type.ToString() + "_" + "percent") as Image;
                        break;
                    case '(':
                        image = Resource.ResourceManager.GetObject("UIToolTip_img_Item_Equip_" + type.ToString() + "_" + "open") as Image;
                        break;
                    case ')':
                        image = Resource.ResourceManager.GetObject("UIToolTip_img_Item_Equip_" + type.ToString() + "_" + "close") as Image;
                        break;
                    default:
                        if('0' <= c && c <= '9')
                        {
                            image = Resource.ResourceManager.GetObject("UIToolTip_img_Item_Equip_" + type.ToString() + "_" + c) as Image;
                            if(c == '1' && (type == NumberType.LookAhead || type == NumberType.Can))
                            {
                                origin.X = 1;
                            }
                        }
                        break;
                }

                if(image != null)
                {
                    if(near)
                    {
                        x += c == '(' ? 2 : 0;
                        g.DrawImage(image, x + origin.X, y + origin.Y);
                        x += image.Width + origin.X + 1;
                    }
                    else
                    {
                        x -= image.Width + origin.X;
                        g.DrawImage(image, x + origin.X, y + origin.Y);
                        x -= 1;
                    }
                }
                else
                {
                    x += spaceWidth * (near ? 1 : -1);
                }
            }
            return x - x0;
        }

        private Image FindReqImage(NumberType type, string req, out Size size)
        {
            Image image = Resource.ResourceManager.GetObject("UIToolTip_img_Item_Equip_" + type.ToString() + "_" + req) as Image;
            if(image != null)
                size = image.Size;
            else
                size = Size.Empty;
            return image;
        }

        private NumberType GetReqType(bool can, int reqValue)
        {
            if(reqValue <= 0)
                return NumberType.Disabled;
            if(can)
                return NumberType.Can;
            else
                return NumberType.Cannot;
        }

        private Bitmap EnlargeBitmap(Bitmap bitmap)
        {
            return ResizeImage(bitmap, bitmap.Width * 2, bitmap.Height * 2);
        }

        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(absoluteDpi, absoluteDpi);

            using(var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using(var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private List<string> GetGearAttributeString()
        {
            int value;
            List<string> tags = new List<string>();

            if(Gear.Props.TryGetValue(GearPropType.only, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.only, value));
            }
            if(Gear.Props.TryGetValue(GearPropType.quest, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.quest, value));
            }
            if(Gear.Props.TryGetValue(GearPropType.tradeBlock, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.tradeBlock, value));
            }
            if(Gear.Props.TryGetValue(GearPropType.equipTradeBlock, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.equipTradeBlock, value));
            }
            if(Gear.Props.TryGetValue(GearPropType.tradeOnce, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.tradeOnce, value));
            }
            if(Gear.Props.TryGetValue(GearPropType.accountSharable, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.accountSharable, value));
            }
            if(Gear.Props.TryGetValue(GearPropType.sharableOnce, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.sharableOnce, value));
            }
            if(Gear.Props.TryGetValue(GearPropType.onlyEquip, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.onlyEquip, value));
            }
            if(Gear.Props.TryGetValue(GearPropType.abilityTimeLimited, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.abilityTimeLimited, value));
                tags.Add(StringHelper.GetGearPropString(GearPropType.notExtend, value));
            }
            if(Gear.Props.TryGetValue(GearPropType.notExtend, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.notExtend, value));
            }
            if(Gear.Props.TryGetValue(GearPropType.blockGoldHammer, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.blockGoldHammer, value));
            }
            if(Gear.Props.TryGetValue(GearPropType.noPotential, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.noPotential, value));
            }
            if(Gear.Props.TryGetValue(GearPropType.fixedPotential, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.fixedPotential, value));
            }
            if(Gear.Props.TryGetValue(GearPropType.cantRepair, out value) && value != 0)
            {
                tags.Add(StringHelper.GetGearPropString(GearPropType.cantRepair, value));
            }

            return tags;
        }

        private Color GetGearNameColor(GearQuality quality)
        {
            switch(quality)
            {
                case GearQuality.Low:
                    return TextRenderer.GrayColor;
                case GearQuality.Middle:
                    return TextRenderer.WhiteColor;
                case GearQuality.High:
                    return TextRenderer.BlueColor;
                case GearQuality.Top:
                    return TextRenderer.PurpleColor;
                case GearQuality.Premium:
                    return TextRenderer.OrangeColor2;
                case GearQuality.Special:
                    return TextRenderer.GreenColor;
                case GearQuality.Excellent:
                    return TextRenderer.RedColor;
                default:
                    return TextRenderer.WhiteColor;
            }
        }

        private Image GetOptionIcon(GearGrade grade)
        {
            switch(grade)
            {
                case GearGrade.Rare: return Resource.AdditionalOptionTooltip_rare;
                case GearGrade.Epic: return Resource.AdditionalOptionTooltip_epic;
                case GearGrade.Unique: return Resource.AdditionalOptionTooltip_unique;
                case GearGrade.Legendary: return Resource.AdditionalOptionTooltip_legendary;
                default: return null;
            }
        }

        private Color GetPotentialTextColor(GearGrade grade)
        {
            switch(grade)
            {
                case GearGrade.Rare: return TextRenderer.BlueColor;
                case GearGrade.Epic: return TextRenderer.PurpleColor;
                case GearGrade.Unique: return TextRenderer.OrangeColor2;
                case GearGrade.Legendary: return TextRenderer.GreenColor;
                default: return TextRenderer.WhiteColor;
            }
        }

        private enum NumberType
        {
            Can,
            Cannot,
            Disabled,
            LookAhead,
            YellowNumber,
        }
    }
}
