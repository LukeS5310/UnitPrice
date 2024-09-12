using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Core.Extensions;
using LiteDB;
using UnitPrice.Model;

namespace UnitPrice.ViewModels;

public class UnitPriceViewModel
{
    public ObservableCollection<UnitPriceModel> UnitPriceCollection { get; set; }

    public ICommand AddCommand { get; set; }

    public UnitPriceViewModel()
    {
        using (var db = new LiteDatabase($"{FileSystem.Current.AppDataDirectory}UnitPrice.db"))
        {
            UnitPriceCollection = db.GetCollection<UnitPriceModel>().Query().ToList().ToObservableCollection();
        }
       
        AddCommand = new Command((args) =>
        {
            if (args is not string name) return;
            var model = new UnitPriceModel(name);
            UnitPriceCollection.Add(model);
            using var db = new LiteDatabase($"{FileSystem.Current.AppDataDirectory}UnitPrice.db");
            db.GetCollection<UnitPriceModel>().Insert(model);
        });

    }
}