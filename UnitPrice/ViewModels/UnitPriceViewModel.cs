using CommunityToolkit.Maui.Core.Extensions;
using LiteDB;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UnitPrice.Model;

namespace UnitPrice.ViewModels;

public class UnitPriceViewModel
{
    public ObservableCollection<UnitPriceModel> UnitPriceCollection { get; set; }

    public ICommand AddCommand { get; set; }
    public ICommand DeleteCommand { get; set; }
    public ICommand UpdateCommand { get; set; }

    public UnitPriceViewModel()
    {
        using (var db = new LiteDatabase($"{FileSystem.Current.AppDataDirectory}UnitPrice.db"))
        {
            UnitPriceCollection = db.GetCollection<UnitPriceModel>().Query().ToList().ToObservableCollection();
            foreach (var unitPriceModel in UnitPriceCollection)
            {
                unitPriceModel.PropertyChanged += UnitPriceModel_PropertyChanged;
            }
        }

        AddCommand = new Command((args) =>
        {
            if (args is not string name) return;
            var model = new UnitPriceModel(name);
            model.PropertyChanged += UnitPriceModel_PropertyChanged;
            using var db = new LiteDatabase($"{FileSystem.Current.AppDataDirectory}UnitPrice.db");
            db.GetCollection<UnitPriceModel>().Insert(model);
            UnitPriceCollection.Add(model);
        });

        DeleteCommand = new Command((args) =>
        {
            if (args is not UnitPriceModel item) return;
            using var db = new LiteDatabase($"{FileSystem.Current.AppDataDirectory}UnitPrice.db");
            db.GetCollection<UnitPriceModel>().DeleteMany(it => it.Name == item.Name);
        });

        UpdateCommand = new Command((args) =>
        {
            if (args is not UnitPriceModel item) return;
            using var db = new LiteDatabase($"{FileSystem.Current.AppDataDirectory}UnitPrice.db");
            var res = db.GetCollection<UnitPriceModel>().Update(item);
        });

    }

    private void UnitPriceModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(UnitPriceModel.PriceForOne)) return;
        UpdateCommand.Execute(sender);
    }
}