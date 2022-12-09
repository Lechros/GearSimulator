using System.Windows;

namespace GearSimulator
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
    }

    public static class MenuItems
    {
        public static string Version { get; } = "1.0.2.0";

        public static string GearInfo { get; } = "아이템 관리";
        public static string Properties { get; } = "아이템 속성";
        public static string AddOption { get; } = "추가옵션";
        public static string Upgrade { get; } = "업그레이드";
        public static string Starforce { get; } = "스타포스 강화";
        public static string Potential { get; } = "잠재능력";
        public static string AddPotential { get; } = "에디셔널 잠재능력";
        public static string SoulWeapon { get; } = "소울웨폰";
        public static string Info { get; } = "정보";
    }
}
