using Caliburn.Micro;
using System.Collections.Generic;

namespace GearSimulator.ViewModels
{
    public class AddOptionViewModel : Screen
    {
        private GearBuilderManager _gb;

        public AddOptionViewModel(GearBuilderManager gb)
        {
            _gb = gb;
            AddOptionType1 = new BindableCollection<string>
            { "STR", "DEX", "INT", "LUK", "STR+DEX", "STR+INT", "STR+LUK", "DEX+INT", "DEX+LUK", "INT+LUK", "최대 HP", "최대 MP", "공격력", "마력", "방어력", "이동속도", "점프력", "보스 데미지", "데미지", "올스탯%", "착용 레벨 감소", };
            AddOptionType2 = new BindableCollection<string>
            { "STR", "DEX", "INT", "LUK", "STR+DEX", "STR+INT", "STR+LUK", "DEX+INT", "DEX+LUK", "INT+LUK", "최대 HP", "최대 MP", "공격력", "마력", "방어력", "이동속도", "점프력", "보스 데미지", "데미지", "올스탯%", "착용 레벨 감소", };
            AddOptionType3 = new BindableCollection<string>
            { "STR", "DEX", "INT", "LUK", "STR+DEX", "STR+INT", "STR+LUK", "DEX+INT", "DEX+LUK", "INT+LUK", "최대 HP", "최대 MP", "공격력", "마력", "방어력", "이동속도", "점프력", "보스 데미지", "데미지", "올스탯%", "착용 레벨 감소", };
            AddOptionType4 = new BindableCollection<string>
            { "STR", "DEX", "INT", "LUK", "STR+DEX", "STR+INT", "STR+LUK", "DEX+INT", "DEX+LUK", "INT+LUK", "최대 HP", "최대 MP", "공격력", "마력", "방어력", "이동속도", "점프력", "보스 데미지", "데미지", "올스탯%", "착용 레벨 감소", };
            AddOptionValue1 = new BindableCollection<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
            AddOptionValue2 = new BindableCollection<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
            AddOptionValue3 = new BindableCollection<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
            AddOptionValue4 = new BindableCollection<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
        }
        private string _selectedAddOptionType1;
        private string _selectedAddOptionType2;
        private string _selectedAddOptionType3;
        private string _selectedAddOptionType4;
        private int _selectedAddOptionValue1;
        private int _selectedAddOptionValue2;
        private int _selectedAddOptionValue3;
        private int _selectedAddOptionValue4;
        public BindableCollection<string> AddOptionType1 { get; }
        public BindableCollection<string> AddOptionType2 { get; }
        public BindableCollection<string> AddOptionType3 { get; }
        public BindableCollection<string> AddOptionType4 { get; }
        public BindableCollection<int> AddOptionValue1 { get; }
        public BindableCollection<int> AddOptionValue2 { get; }
        public BindableCollection<int> AddOptionValue3 { get; }
        public BindableCollection<int> AddOptionValue4 { get; }
        public string SelectedAddOptionType1
        {
            get { return _selectedAddOptionType1; }
            set
            {
                _selectedAddOptionType1 = value;
                NotifyOfPropertyChange(() => _selectedAddOptionType1);
            }
        }
        public string SelectedAddOptionType2
        {
            get { return _selectedAddOptionType2; }
            set
            {
                _selectedAddOptionType2 = value;
                NotifyOfPropertyChange(() => _selectedAddOptionType2);
            }
        }
        public string SelectedAddOptionType3
        {
            get { return _selectedAddOptionType3; }
            set
            {
                _selectedAddOptionType3 = value;
                NotifyOfPropertyChange(() => _selectedAddOptionType3);
            }
        }
        public string SelectedAddOptionType4
        {
            get { return _selectedAddOptionType4; }
            set
            {
                _selectedAddOptionType4 = value;
                NotifyOfPropertyChange(() => _selectedAddOptionType4);
            }
        }
        public int SelectedAddOptionValue1
        {
            get { return _selectedAddOptionValue1; }
            set
            {
                _selectedAddOptionValue1 = value;
                NotifyOfPropertyChange(() => _selectedAddOptionValue1);
            }
        }
        public int SelectedAddOptionValue2
        {
            get { return _selectedAddOptionValue2; }
            set
            {
                _selectedAddOptionValue2 = value;
                NotifyOfPropertyChange(() => _selectedAddOptionValue2);
            }
        }
        public int SelectedAddOptionValue3
        {
            get { return _selectedAddOptionValue3; }
            set
            {
                _selectedAddOptionValue3 = value;
                NotifyOfPropertyChange(() => _selectedAddOptionValue3);
            }
        }
        public int SelectedAddOptionValue4
        {
            get { return _selectedAddOptionValue4; }
            set
            {
                _selectedAddOptionValue4 = value;
                NotifyOfPropertyChange(() => _selectedAddOptionValue4);
            }
        }

        private string _addOptionSummary;
        public string AddOptionSummary
        {
            get { return _addOptionSummary; }
            set
            {
                _addOptionSummary = value;
                NotifyOfPropertyChange(() => AddOptionSummary);
            }
        }

        public void ApplyAddOptions()
        {
            bool duplicate = false;
            Dictionary<string, int> typeGradePairs = new Dictionary<string, int>();
            if(!string.IsNullOrEmpty(SelectedAddOptionType1))
            {
                typeGradePairs.Add(SelectedAddOptionType1, SelectedAddOptionValue1);
            }
            if(!string.IsNullOrEmpty(SelectedAddOptionType2))
            {
                if(typeGradePairs.ContainsKey(SelectedAddOptionType2)) duplicate = true;
                else typeGradePairs.Add(SelectedAddOptionType2, SelectedAddOptionValue2);
            }
            if(!string.IsNullOrEmpty(SelectedAddOptionType3))
            {
                if(typeGradePairs.ContainsKey(SelectedAddOptionType3)) duplicate = true;
                else typeGradePairs.Add(SelectedAddOptionType3, SelectedAddOptionValue3);
            }
            if(!string.IsNullOrEmpty(SelectedAddOptionType4))
            {
                if(typeGradePairs.ContainsKey(SelectedAddOptionType4)) duplicate = true;
                else typeGradePairs.Add(SelectedAddOptionType4, SelectedAddOptionValue4);
            }
            if(duplicate)
            {
                AddOptionSummary = "중복된 추가옵션이 있습니다."; return;
            }
            _gb.ApplyAdditionalOption(typeGradePairs);
            AddOptionSummary = "추가옵션이 적용되었습니다.";
        }

        public void RemoveAddOptions()
        {
            _gb.ApplyAdditionalOption(new Dictionary<string, int>());
            AddOptionSummary = "추가옵션이 제거되었습니다.";
        }
    }
}
