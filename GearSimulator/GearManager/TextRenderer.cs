using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using TR = System.Windows.Forms.TextRenderer;
using System.Windows.Forms;
using System;

namespace GearManager
{
    public static class TextRenderer
    {
        // Prevent emSize convertion error when DPI != 96
        public static readonly Font ItemNameFont = new Font("돋움", 14f - 0.01f, FontStyle.Bold, GraphicsUnit.Pixel);
        public static readonly Font SoulFont = new Font("돋움", 12f - 0.01f, GraphicsUnit.Pixel);
        public static readonly Font ItemDetailFont = new Font("돋움", 11f - 0.01f, GraphicsUnit.Pixel);
        public static readonly Font NameTagFont = new Font("돋움", 11f - 0.01f, FontStyle.Bold, GraphicsUnit.Pixel);

        public static readonly Color WhiteColor = Color.FromArgb(255, 255, 255);
        public static readonly Color RedColor = Color.FromArgb(255, 0, 102);
        public static readonly Color OrangeColor = Color.FromArgb(255, 170, 0);
        public static readonly Color OrangeColor2 = Color.FromArgb(255, 204, 0);
        public static readonly Color OrangeColor3 = Color.FromArgb(255, 136, 17);
        public static readonly Color YellowColor = Color.FromArgb(255, 255, 68);
        public static readonly Color GreenColor = Color.FromArgb(204, 255, 0);
        public static readonly Color BlueColor = Color.FromArgb(102, 255, 255);
        public static readonly Color PurpleColor = Color.FromArgb(153, 102, 255);
        public static readonly Color GrayColor = Color.FromArgb(187, 187, 187);
        public static readonly Color GrayColor2 = Color.FromArgb(153, 153, 153);

        public static int DrawText(Graphics g, string s, int x, int y, StringAlignment flag = StringAlignment.Near)
        {
            return DrawText(g, s, ItemDetailFont, x, y, flag);
        }

        public static int DrawText(Graphics g, string s, Font font, int x, int y, StringAlignment flag = StringAlignment.Near)
        {
            if(string.IsNullOrWhiteSpace(s))
            {
                return font.Height + 2;
            }
            return DrawFormattedText(g, s, font, x, y, flag);
        }

        public static int DrawText(Graphics g, string s, int x, int y, Color color, StringAlignment flag = StringAlignment.Near)
        {
            return DrawText(g, s, ItemDetailFont, x, y, color, flag);
        }

        public static int DrawText(Graphics g, string s, Font font, int x, int y, Color color, StringAlignment flag = StringAlignment.Near)
        {
            if(string.IsNullOrWhiteSpace(s))
            {
                return font.Height + 2;
            }
            return DrawPlainText(g, s, font, x, y, color, flag);
        }

        private static int DrawPlainText(Graphics g, string s, Font font, int x, int y, Color color, StringAlignment flag = StringAlignment.Near)
        {
            int strWidth, textWidth, curX, i0, padding = (font == ItemNameFont) ? 9 : 7;
            int y0 = y;

            switch(flag)
            {
                case StringAlignment.Center:
                    strWidth = 0;
                    i0 = 0;
                    for(int i = 0; i < s.Length; i++)
                    {
                        if(s[i] == '\0') continue;
                        textWidth = TR.MeasureText(s[i].ToString(), font).Width - padding;
                        if(strWidth + 2 + textWidth >= 261 - padding)
                        {
                            curX = (x / 2) - (strWidth / 2) - 1;
                            for(int j = i0; j < i; j++)
                            {
                                TR.DrawText(g, s[j].ToString(), font, new Point(curX, y), color);
                                textWidth = TR.MeasureText(s[j].ToString(), font).Width - padding;
                                curX += textWidth + 2;
                            }
                            strWidth = 0;
                            y += font.Height + 2;
                            i0 = i;
                        }
                        strWidth += (strWidth > 0) ? 2 : 0;
                        strWidth += textWidth;
                    }
                    if(strWidth > 0)
                    {
                        curX = (x / 2) - (strWidth / 2) - (padding / 2);
                        for(int i = i0; i < s.Length; i++)
                        {
                            TR.DrawText(g, s[i].ToString(), font, new Point(curX, y), color);
                            textWidth = TR.MeasureText(s[i].ToString(), font).Width - padding;
                            curX += textWidth + 2;
                        }
                        y += font.Height + 2;
                    }
                    break;
                case StringAlignment.Near:
                default:
                    curX = x;
                    for(int i = 0; i < s.Length; i++)
                    {
                        if(s[i] == '\0') continue;
                        if(s[i] == '\n')
                        {
                            curX = x;
                            y += font.Height + 2;
                        }
                        textWidth = TR.MeasureText(s[i].ToString(), font).Width - padding;
                        if(curX + textWidth >= 261 - padding - 1 - (s[i] == ' ' ? 5 : 0))
                        {
                            curX = x;
                            y += font.Height + 2;
                        }
                        TR.DrawText(g, s[i].ToString(), font, new Point(curX, y), color);
                        curX += textWidth + 2;
                    }
                    y += font.Height + 2;
                    break;
            }
            return y - y0;
        }

        private static int DrawFormattedText(Graphics g, string s, Font font, int x, int y, StringAlignment flag = StringAlignment.Near)
        {
            string trimS = s.Trim();
            if(s.Length > 3 && trimS.StartsWith("#") && trimS.EndsWith("#"))
            {
                if(s.Count(c => c == '#') == 2)
                {
                    switch(trimS[1])
                    {
                        case 'c': return DrawPlainText(g, s.Replace("#c"," ").Replace("#"," "), font, x, y, OrangeColor, flag);
                        case 'o': return DrawPlainText(g, s.Replace("#o", " ").Replace("#", " "), font, x, y, OrangeColor2, flag);
                        case 'g': return DrawPlainText(g, s.Replace("#g", " ").Replace("#", " "), font, x, y, GreenColor, flag);
                        case 'b': return DrawPlainText(g, s.Replace("#b", " ").Replace("#", " "), font, x, y, BlueColor, flag);
                        case 'r': return DrawPlainText(g, s.Replace("#r", " ").Replace("#", " "), font, x, y, RedColor, flag);
                    }
                }
            }

            var TaCs = ParseString(s);
            int strWidth, textWidth, curX, i0, padding = (font == ItemNameFont) ? 9 : 7;
            int y0 = y;

            switch(flag)
            {
                case StringAlignment.Center:
                    strWidth = 0;
                    i0 = 0;
                    for(int i = 0; i < TaCs.Count; i++)
                    {
                        if(TaCs[i].Text == '\0') continue;
                        textWidth = TR.MeasureText(TaCs[i].Text.ToString(), font).Width - padding;
                        if(strWidth + 2 + textWidth >= 261 - padding)
                        {
                            curX = (x / 2) - (strWidth / 2) - 1;
                            for(int j = i0; j < i; j++)
                            {
                                TR.DrawText(g, TaCs[j].Text.ToString(), font, new Point(curX, y), TaCs[j].Color);
                                textWidth = TR.MeasureText(TaCs[j].Text.ToString(), font).Width - padding;
                                curX += textWidth + 2;
                            }
                            strWidth = 0;
                            y += font.Height + 2;
                            i0 = i;
                        }
                        strWidth += (strWidth > 0) ? 2 : 0;
                        strWidth += textWidth;
                    }
                    if(strWidth > 0)
                    {
                        curX = (x / 2) - (strWidth / 2) - (padding / 2);
                        for(int i = i0; i < TaCs.Count; i++)
                        {
                            TR.DrawText(g, TaCs[i].Text.ToString(), font, new Point(curX, y), TaCs[i].Color);
                            textWidth = TR.MeasureText(TaCs[i].Text.ToString(), font).Width - padding;
                            curX += textWidth + 2;
                        }
                        y += font.Height + 2;
                    }
                    break;
                case StringAlignment.Near:
                default:
                    curX = x;
                    for(int i = 0; i < TaCs.Count; i++)
                    {
                        if(TaCs[i].Text == '\0') continue;
                        if(TaCs[i].Text == '\n')
                        {
                            curX = x;
                            y += font.Height + 2;
                        }
                        textWidth = TR.MeasureText(TaCs[i].Text.ToString(), font).Width - padding;
                        if(curX + textWidth >= 261 - padding - 1 - (TaCs[i].Text == ' ' ? 5 : 0))
                        {
                            curX = x;
                            y += font.Height + 2;
                        }
                        TR.DrawText(g, TaCs[i].Text.ToString(), font, new Point(curX, y), TaCs[i].Color);
                        curX += textWidth + 2;
                    }
                    y += font.Height + 2;
                    break;
            }
            return y - y0;
        }

        public static void DrawTitleText(Graphics g, string s, int x, ref int y, Color color)
        {
            int stringWidth = TR.MeasureText(s, ItemNameFont).Width;
            // Overflow
            if(stringWidth + 2 >= 261)
            {
                string[] sp = s.Split(' ');
                if(sp.Length >= 2)
                {
                    int minI = 0;
                    int minDiff = s.Length;
                    for(int i = 1; i < sp.Length; i++)
                    {
                        int frontLength = sp.Take(i).Sum(str => str.Length) + (i - 1);
                        int backLength = sp.Skip(i).Take(sp.Length).Sum(str => str.Length) + (sp.Length - i - 1);
                        int diff = Math.Abs(frontLength - backLength);
                        if(diff < minDiff)
                        {
                            minI = i;
                            minDiff = diff;
                        }
                    }
                    if(minI != 0)
                    {
                        string frontString = string.Join(" ", sp.Take(minI));
                        string backString = string.Join(" ", sp.Skip(minI).Take(sp.Length));
                        y += DrawPlainText(g, frontString, ItemNameFont, x, y, color, StringAlignment.Center);
                        y += DrawPlainText(g, backString, ItemNameFont, x, y, color, StringAlignment.Center);
                        y += 4;
                        return;
                    }
                }
            }
            // Normal
            y += DrawPlainText(g, s, ItemNameFont, x, y, color, StringAlignment.Center);
            y += 4;
        }

        public static void DrawStatText(Graphics g, GearPropType propType, int[] s, int x, ref int y)
        {
            string rate = null;
            string propStr;
            int curX = x;

            if(s.Sum() <= 0)
            {
                return;
            }

            switch(propType)
            {
                case GearPropType.knockback:
                    TR.DrawText(g, StringHelper.GetStatString(propType, s[0]), ItemDetailFont, new Point(x, y), WhiteColor);
                    y += 15;
                    return;
                case GearPropType.reduceReq:
                    TR.DrawText(g, StringHelper.GetStatString(propType, s[0] + s[1]), ItemDetailFont, new Point(x, y), GreenColor);
                    y += 15;
                    return;
                case GearPropType.incMHPr:
                case GearPropType.incMMPr:
                case GearPropType.bdR:
                case GearPropType.imdR:
                case GearPropType.damR:
                case GearPropType.incAllStat:
                    rate = "%";
                    break;
            }

            if(s[1] == 0 && s[2] == 0)
            {
                propStr = StringHelper.GetStatString(propType, 0);
                TR.DrawText(g, propStr, ItemDetailFont, new Point(curX, y), WhiteColor);
                curX += TR.MeasureText(propStr, ItemDetailFont).Width - 7 + 2;
                TR.DrawText(g, " +" + s[0] + rate, ItemDetailFont, new Point(curX, y), WhiteColor);
            }
            else
            {
                propStr = StringHelper.GetStatString(propType, 0) + " +" + (s[0] + s[1] + s[2]) + rate;
                TR.DrawText(g, propStr, ItemDetailFont, new Point(curX, y), BlueColor);
                curX += TR.MeasureText(propStr, ItemDetailFont).Width - 7 + 2;

                propStr = " (" + s[0] + rate;
                TR.DrawText(g, propStr, ItemDetailFont, new Point(curX, y), WhiteColor);
                curX += TR.MeasureText(propStr, ItemDetailFont).Width - 7 + 2;

                if(s[1] > 0)
                {
                    propStr = " +" + s[1] + rate;
                    TR.DrawText(g, propStr, ItemDetailFont, new Point(curX, y), GreenColor);
                    curX += TR.MeasureText(propStr, ItemDetailFont).Width - 7 + 2;
                }
                if(s[2] > 0)
                {
                    propStr = " +" + s[2] + rate;
                    TR.DrawText(g, propStr, ItemDetailFont, new Point(curX, y), BlueColor);
                    curX += TR.MeasureText(propStr, ItemDetailFont).Width - 7 + 2;
                }
                else if(s[2] < 0)
                {
                    propStr = " " + s[2] + rate;
                    TR.DrawText(g, propStr, ItemDetailFont, new Point(curX, y), RedColor);
                    curX += TR.MeasureText(propStr, ItemDetailFont).Width - 7 + 2;
                }
                TR.DrawText(g, ")", ItemDetailFont, new Point(curX, y), WhiteColor);
            }
            y += 15;
        }

        private static List<TextAndColor> ParseString(string s)
        {
            s = s.Replace("\\r", "");
            Color curColor = WhiteColor;
            List<TextAndColor> TaCs = new List<TextAndColor>();
            for(int i = 0; i < s.Length; i++)
            {
                if(s[i] == '#')
                {
                    if(i + 1 < s.Length)
                    {
                        if(curColor == WhiteColor)
                        {
                            switch(s[i + 1])
                            {
                                case 'c':
                                    curColor = OrangeColor;
                                    break;
                                case 'o':
                                    curColor = OrangeColor2;
                                    break;
                                case 'g':
                                    curColor = GreenColor;
                                    break;
                                case 'b':
                                    curColor = BlueColor;
                                    break;
                                case 'r':
                                    curColor = RedColor;
                                    break;
                            }
                            i++;
                        }
                        else
                        {
                            curColor = WhiteColor;
                        }
                    }
                    TaCs.Add(new TextAndColor(' ', curColor));
                }
                else if(s[i] == '\\')
                {
                    if(i + 1 < s.Length)
                    {
                        if(s[i + 1] == 'n')
                        {
                            TaCs.Add(new TextAndColor('\n', curColor));
                        }
                        i++;
                    }
                }
                else
                {
                    TaCs.Add(new TextAndColor(s[i], curColor));
                }
            }
            return TaCs;
        }

        private struct TextAndColor
        {
            public TextAndColor(char text, Color color)
            {
                Text = text;
                Color = color;
            }
            public char Text { get; }
            public Color Color { get; }
        }
    }
}
