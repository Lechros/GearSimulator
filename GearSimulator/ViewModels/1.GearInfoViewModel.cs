using Caliburn.Micro;
using GearManager;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GearSimulator.ViewModels
{
    public class GearInfoViewModel : Screen
    {
        private GearBuilderManager _gb;

        public GearInfoViewModel(GearBuilderManager gb)
        {
            _gb = gb;
            GearName = "-";
            GearCode = "-";
        }

        private string _gearName;
        public string GearName
        {
            get { return _gearName; }
            set
            {
                _gearName = value;
                NotifyOfPropertyChange(() => GearName);
            }
        }
        private string _gearCode;
        public string GearCode
        {
            get { return _gearCode; }
            set
            {
                _gearCode = value;
                NotifyOfPropertyChange(() => GearCode);
            }
        }
        public bool Reboot
        {
            get { return _gb.Reboot; }
            set
            {
                _gb.Reboot = value;
                NotifyOfPropertyChange(() => Reboot);
            }
        }
        private string _saveInfo;
        public string SaveInfo
        {
            get { return _saveInfo; }
            set
            {
                _saveInfo = value;
                NotifyOfPropertyChange(() => SaveInfo);
                NotifyOfPropertyChange(() => SaveInfoIsVisible);
            }
        }
        private bool _saveInfoIsVisible;
        public bool SaveInfoIsVisible
        {
            get { return !string.IsNullOrWhiteSpace(SaveInfo); }
            set
            {
                _saveInfoIsVisible = value;
                NotifyOfPropertyChange(() => SaveInfoIsVisible);
            }
        }

        public string NameToSearch { get; set; }

        private BindableCollection<KeyValuePair<int, string>> _gearSearchResult = new BindableCollection<KeyValuePair<int, string>>();
        public BindableCollection<KeyValuePair<int, string>> GearSearchResult
        {
            get { return _gearSearchResult; }
            set
            {
                _gearSearchResult = value;
                NotifyOfPropertyChange(() => GearSearchResult);
            }
        }

        private string _searchSummary;
        public string SearchSummary
        {
            get { return _searchSummary; }
            set
            {
                _searchSummary = value;
                NotifyOfPropertyChange(() => SearchSummary);
            }
        }

        public void ExecuteSearchGear(ActionExecutionContext context)
        {
            var keyArgs = context.EventArgs as KeyEventArgs;

            if(keyArgs != null && keyArgs.Key == Key.Enter)
            {
                SearchGear();
            }
        }

        public void SearchGear()
        {
            if(string.IsNullOrWhiteSpace(NameToSearch))
            {
                return;
            }
            else if(NameToSearch.Length < 2)
            {
                if(NameToSearch[0] < '가' || NameToSearch[0] > '힣')
                {
                    SearchSummary = "검색어는 두 글자 이상이어야 합니다.";
                    return;
                }
            }
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var rawResult = GearResource.SearchGear(NameToSearch, false);
            sw.Stop();
            if(rawResult.Count == 0)
            {
                SearchSummary = "검색 결과가 없습니다.";
                return;
            }
            else if(rawResult.Count > 128)
            {
                SearchSummary = "검색 결과가 너무 많습니다 (" + rawResult.Count + "개)";
                return;
            }
            sw.Start();
            GearSearchResult = new BindableCollection<KeyValuePair<int, string>>(rawResult.AsEnumerable());
            sw.Stop();
            SearchSummary = "검색 결과 " + _gearSearchResult.Count + "개 (" + sw.Elapsed.TotalSeconds.ToString("F3") + "초)";
        }

        public void SaveImage()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG (*.png)|*.png";
            saveFileDialog.DefaultExt = "*.png";
            if(saveFileDialog.ShowDialog() == true)
            {
                if(_gb.SaveImage(saveFileDialog.FileName))
                {
                    SaveInfo = saveFileDialog.SafeFileName + "가 저장되었습니다.";
                    return;
                }
                SaveInfo = "저장에 실패했습니다.";
            }
            SaveInfo = string.Empty;
        }

        public void CopyImage()
        {
            using(MemoryStream stream = new MemoryStream())
            {
                _gb.TooltipBitmap.Save(stream, ImageFormat.Png);
                DataObject data = new DataObject();
                data.SetData(DataFormats.Bitmap, _gb.TooltipBitmap, true);
                data.SetData("PNG", stream, false);
                Clipboard.SetDataObject(data, true);
                SaveInfo = "클립보드에 복사되었습니다.";
            }
        }

        public void CopyString()
        {
            Clipboard.SetText(_gb.GearString);
            SaveInfo = "클립보드에 복사되었습니다.";
        }

        public void ApplyGear(KeyValuePair<int, string> selectedItem)
        {
            _gb.SetGear(selectedItem.Key);
            GearCode = selectedItem.Key.ToString();
            GearName = selectedItem.Value;
        }
    }
}
