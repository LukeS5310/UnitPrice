using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UnitPrice.Model
{
    internal class UnitPriceModel : INotifyPropertyChanged
    {
        public UnitPriceModel()
        {
            
        }

        public UnitPriceModel(string name)
        {
            Name = name;
            EmojiName = "🤑";
            Price = 0;
            UnitFraction = 1;
        }

        private string? _name;
        public string Name
        {
            get => _name ?? "";
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public string? EmojiName { get; set; }
        
        private decimal _price;

        public decimal Price
        {
            get => _price;
            set
            {
                if (_price == value || value <= 0) return;
                _price = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PriceForOne));
            }
        }

        private decimal _unitFraction;
        public decimal UnitFraction
        {
            get => _unitFraction;
            set
            {
                if (value <= 0) return;
                _unitFraction = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PriceForOne));
            }
        }

        
        public string PriceForOne => (Price/UnitFraction).ToString("C");
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}