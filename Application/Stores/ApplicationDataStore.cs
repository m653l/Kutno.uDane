using Application.ViewModels.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.Aggregates;
using System.Collections.ObjectModel;

namespace Application.Stores
{
    public partial class ApplicationDataStore: ObservableObject
    {
        // Raw data (not anymore)
        public ObservableCollection<YearViewModel> Years { get; set; } = new();
        [ObservableProperty]
        private YearViewModel activeYear;

    }
}
