using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using LiteDB;
using UnitPrice.Utils;

namespace UnitPrice.Model
{
    public class UnitPriceModel : INotifyPropertyChanged
    {
        public UnitPriceModel(string name)
        {
            Name = name;
            Id = Guid.NewGuid().ToString();
            EmojiName = Emoji.GetRandomEmoji();
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

        [BsonId]
        public string Id { get; }
        
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

        
        public string PriceForOne => (Price/UnitFraction).ToString("C", CultureInfo.InvariantCulture);
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