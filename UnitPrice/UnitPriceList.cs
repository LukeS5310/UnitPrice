using System.Collections.ObjectModel;
using UnitPrice.Model;

namespace UnitPrice
{
    internal class UnitPriceList : ContentPage
    {
        public UnitPriceList()
        {
            var layout = new Grid()
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(4, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition(),
                    new ColumnDefinition()
                }
            };

            var upCollection = new CollectionView();
            var items = new ObservableCollection<UnitPriceModel>();
            upCollection.ItemsSource = items;
            upCollection.CanReorderItems = true;

            upCollection.ItemTemplate = new DataTemplate(() =>
            {
                var emojiLabel = new Label { FontSize = 20, Margin = 10 , VerticalOptions = LayoutOptions.Center};
                emojiLabel.SetBinding(Label.TextProperty, "EmojiName");
                var nameLabel = new Label(){VerticalOptions = LayoutOptions.Center};
                nameLabel.SetBinding(Label.TextProperty, "Name");
                
                var priceTb = new Entry();
                priceTb.Keyboard = Keyboard.Numeric;
                priceTb.SetBinding(Entry.TextProperty, "Price");
                var priceTbDesc = new Label() { Text = "Цена", FontSize = 10, VerticalOptions = LayoutOptions.Center};
                var priceStack = new VerticalStackLayout() { Children = { priceTbDesc, priceTb }, Margin = 10};
                priceStack.Spacing = -12;

                var unitFraction = new Entry();
                unitFraction.Keyboard = Keyboard.Numeric;
                unitFraction.SetBinding(Entry.TextProperty, "UnitFraction");
                var unitFractionDesc = new Label { Text = "ед.", FontSize = 10, VerticalOptions = LayoutOptions.Center};
                var unitFractionStack = new VerticalStackLayout()
                    { Children = { unitFractionDesc, unitFraction }, Margin = 10};
                unitFractionStack.Spacing = -12;

                var priceForOne = new Label(){VerticalOptions = LayoutOptions.Center};
                priceForOne.SetBinding(Label.TextProperty, "PriceForOne");
                var priceForOneDesc = new Label()
                    { Text = "За шт.", FontSize = 10, VerticalOptions = LayoutOptions.Center };
                var priceForOneStack = new VerticalStackLayout() { Children = { priceForOneDesc, priceForOne }, Margin = 10, VerticalOptions = LayoutOptions.Center};
                priceForOneStack.Spacing = 0;

                var editButton = new Button
                {
                    Text = "❌",
                    Margin = 5,
                    FontSize = 20,
                    MinimumWidthRequest = 20
                };
                editButton.Clicked += async (sender, args) =>
                {
                    var answer = await DisplayAlert("Удаление", "Удалить выбранный элемент?","Нет","Да");
                    if (answer) return;

                    var button = sender as Button;

                    if(button.BindingContext is UnitPriceModel obj)
                        items.Remove(obj);
                };

                var grid = new Grid(){BackgroundColor = Color.Parse("LightBlue"), Margin = 1};
                
                grid.Add(emojiLabel,0);
                grid.SetRowSpan(emojiLabel, 2);
                grid.Add(nameLabel,1);
                grid.SetRowSpan(nameLabel,2);
                grid.Add(priceStack,2);
                grid.SetColumnSpan(priceStack,2);
                grid.Add(unitFractionStack,2,1);
                grid.SetColumnSpan(unitFractionStack,2);
                grid.Add(priceForOneStack,4);
                grid.SetColumnSpan(priceForOneStack,2);
                grid.SetRowSpan(priceForOneStack,2);
                grid.Add(editButton,6);
                grid.SetRowSpan(editButton,2);
                grid.RowSpacing = -25;
                return grid;
            });

            var addButton = new Button()
            {
                MaximumHeightRequest = 60,
                MaximumWidthRequest = 60,
                ZIndex = 5,
                Text = "+",
                FontAttributes = FontAttributes.Bold,
                FontSize = 30,
                CornerRadius = 30
            };

            addButton.Clicked += async (sender, e) =>
            {
                var name = await DisplayPromptAsync("Добавление", "Введите наименование:", "OK", "Отмена");
                if(!string.IsNullOrWhiteSpace(name))
                    items.Add(new UnitPriceModel(name));
            };

            layout.Add(upCollection);
            layout.Add(addButton, 2, 1);
            layout.SetColumnSpan(upCollection, 3);
            layout.SetRowSpan(upCollection, 2);

            Content = layout;
        }
    }
}