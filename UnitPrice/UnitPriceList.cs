using Microsoft.Maui.Controls.Shapes;
using UnitPrice.Model;
using UnitPrice.ViewModels;

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
            var viewModel = new UnitPriceViewModel();
            var items = viewModel.UnitPriceCollection;
            upCollection.ItemsSource = items;
            upCollection.CanReorderItems = true;

            upCollection.ItemTemplate = new DataTemplate(() =>
            {
                var emojiLabel = CreateEmojiLabel();
                var nameLabel = CreateNameLabel();
                var priceStack = CreatePriceStack();
                var unitFractionStack = CreateFractionStack();
                var priceForOneStack = CreatePriceForOneStack();
                var deleteButton = CreateDeleteButton();

                deleteButton.Clicked += async (sender, args) =>
                {
                    var answer = await DisplayAlert("Удаление", "Удалить выбранный элемент?", "ОК", "Отмена");
                    if (!answer) return;

                    var button = sender as Button;

                    if (button.BindingContext is UnitPriceModel obj)
                        items.Remove(obj);
                };

                var grid = new Grid() { BackgroundColor = Color.Parse("LightBlue") };
                grid.Add(emojiLabel, 0);
                grid.SetRowSpan(emojiLabel, 2);
                grid.Add(nameLabel, 1);
                grid.SetRowSpan(nameLabel, 2);
                grid.SetColumnSpan(nameLabel, 2);
                grid.Add(priceStack, 3);
                grid.SetColumnSpan(priceStack, 2);
                grid.Add(unitFractionStack, 3, 1);
                grid.SetColumnSpan(unitFractionStack, 2);
                grid.Add(priceForOneStack,5);
                grid.SetColumnSpan(priceForOneStack, 2);
                grid.SetRowSpan(priceForOneStack, 2);
                grid.Add(deleteButton, 7);
                grid.SetRowSpan(deleteButton, 2);
                grid.RowSpacing = -25;

                var border = new Border
                {
                    StrokeShape = new RoundRectangle() { CornerRadius = 15 }, Margin = 5,
                    Content = grid
                };

                return border;
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
                if (!string.IsNullOrWhiteSpace(name))
                    viewModel.AddCommand.Execute(name);
            };

            layout.Add(upCollection);
            layout.Add(addButton, 2, 1);
            layout.SetColumnSpan(upCollection, 3);
            layout.SetRowSpan(upCollection, 2);

            Content = layout;
        }

        private static Button CreateDeleteButton()
        {
            var deleteButton = new Button
            {
                Text = "❌",
                Margin = 5,
                FontSize = 10,
                MaximumWidthRequest = 50,
                MaximumHeightRequest = 50
            };
            return deleteButton;
        }

        private static VerticalStackLayout CreatePriceForOneStack()
        {
            var priceForOne = new Label() { VerticalOptions = LayoutOptions.Center };
            priceForOne.SetBinding(Label.TextProperty, "PriceForOne");
            var priceForOneDesc = new Label()
                { Text = "За шт.", FontSize = 10, VerticalOptions = LayoutOptions.Center };
            var priceForOneStack = new VerticalStackLayout
            {
                Children = { priceForOneDesc, priceForOne }, Margin = 10, VerticalOptions = LayoutOptions.Center,
                Spacing = 0
            };
            return priceForOneStack;
        }

        private static VerticalStackLayout CreateFractionStack()
        {
            var unitFraction = new Entry();
            unitFraction.Keyboard = Keyboard.Numeric;
            unitFraction.SetBinding(Entry.TextProperty, "UnitFraction");
            var unitFractionDesc = new Label { Text = "ед.", FontSize = 10, VerticalOptions = LayoutOptions.Center };
            var unitFractionStack = new VerticalStackLayout
            {
                Children = { unitFractionDesc, unitFraction }, Margin = 10,
                Spacing = -12
            };
            return unitFractionStack;
        }

        private static VerticalStackLayout CreatePriceStack()
        {
            var priceTb = new Entry();
            priceTb.Keyboard = Keyboard.Numeric;
            priceTb.SetBinding(Entry.TextProperty, "Price");
            var priceTbDesc = new Label() { Text = "Цена", FontSize = 10, VerticalOptions = LayoutOptions.Center };
            var priceStack = new VerticalStackLayout
            {
                Children = { priceTbDesc, priceTb }, Margin = 10,
                Spacing = -12
            };
            return priceStack;
        }

        private static Label CreateNameLabel()
        {
            var nameLabel = new Label() { VerticalOptions = LayoutOptions.Center };
            nameLabel.SetBinding(Label.TextProperty, "Name");
            return nameLabel;
        }

        private static Label CreateEmojiLabel()
        {
            var emojiLabel = new Label { FontSize = 20, Margin = 10, VerticalOptions = LayoutOptions.Center };
            emojiLabel.SetBinding(Label.TextProperty, "EmojiName");
            return emojiLabel;
        }
    }
}