using Application.Stores;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public partial class SummaryViewModel : CoreViewModel
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BestCostPerStaninSchoolName))]
        private School _bestCostPerStaninIncrease;

        public string BestCostPerStaninSchoolName
        {
            get
            {
                return BestCostPerStaninIncrease.Name;
            }
        }

        [ObservableProperty]
        private decimal _bestCostPerStaninIncreaseDifference;

        private readonly ApplicationDataStore _applicationDataStore;
        public SummaryViewModel(ApplicationDataStore applicationDataStore, IServiceProvider serviceProvider) : base(serviceProvider) 
        {
            _applicationDataStore = applicationDataStore;
            CalculateBestCostPerStaninIncrease();
        }

        
        public void CalculateBestCostPerStaninIncrease()
        {
            var yearsByDate = _applicationDataStore.Years.OrderByDescending(i => i.Date).ToArray();
            var count = yearsByDate.Count();

            if (count < 2) return;

            for (int i = 0; i < yearsByDate[0].Schools.Count; i++)
            {
                if (BestCostPerStaninIncreaseDifference == null || BestCostPerStaninIncrease == null)
                {
                    BestCostPerStaninIncreaseDifference = decimal.Round(yearsByDate[1].Schools[i].CostPerStanin - yearsByDate[0].Schools[i].CostPerStanin, 2);
                    BestCostPerStaninIncrease = yearsByDate[0].Schools[i];
                }

                if (yearsByDate[count - 1].Schools[i].CostPerStanin - yearsByDate[0].Schools[i].CostPerStanin > BestCostPerStaninIncreaseDifference)
                {
                    BestCostPerStaninIncreaseDifference = decimal.Round(yearsByDate[1].Schools[i].CostPerStanin - yearsByDate[0].Schools[i].CostPerStanin, 2);
                    BestCostPerStaninIncrease = yearsByDate[0].Schools[i];
                }
            }
            


            //for (int i = 0; i < _applicationDataStore.Years.Count; i++)
            //{
            //    BestCostPerStaninIncrease
            //}
        }
    }
}
