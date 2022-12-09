using Caliburn.Micro;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace GearSimulator.ViewModels
{
    public class InfoViewModel : Screen
    {
        public InfoViewModel()
        {

        }

        public string Version { get; } = "버전 " + MenuItems.Version;
        private string _versionInfo;
        public string VersionInfo
        {
            get { return _versionInfo; }
            set
            {
                _versionInfo = value;
                NotifyOfPropertyChange(() => VersionInfo);
            }
        }

        public void CheckUpdate()
        {
            string url = @"https://sygunhas.github.io/GearTooltipMakerVersion.html";
            string result = DownloadVersion(url);
            if(result == "0.0.0.0")
            {
                VersionInfo = "최신 버전 확인에 실패했습니다.";
            }
            else if(CompareVersion(MenuItems.Version, result))
            {
                VersionInfo = "현재 최신 버전입니다. (" + result + ")";
            }
            else
            {
                VersionInfo = result + "이 최신 버전입니다.";
            }
        }

        private string DownloadVersion(string url)
        {
            using(WebClient client = new WebClient())
            {
                var result = client.DownloadString(url);
                return result.TrimEnd('\n');
            }
        }

        /// <returns>True if original >= update</returns>
        private bool CompareVersion(string original, string update)
        {
            var oNums = original.Split('.');
            var uNums = update.Split('.');
            if(uNums.Length != 4) return false;

            for(int i = 0; i < 4; i++)
            {
                int oVer = int.Parse(oNums[i]);
                int uVer = int.Parse(uNums[i]);
                if(oVer > uVer) return true;
                else if(oVer < uVer) return false;
            }
            // All same
            return true;
        }
    }
}
