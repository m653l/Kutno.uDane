using Application.Stores;
using CommunityToolkit.Mvvm.ComponentModel;
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
        Tuple<decimal, string> _bestCostPerStaninIncrease;

        private readonly ApplicationDataStore _applicationDataStore;
        public SummaryViewModel(ApplicationDataStore applicationDataStore, IServiceProvider serviceProvider) : base(serviceProvider) 
        {
            applicationDataStore = _applicationDataStore;
        }

        
        public void CalculateBestCostPerStaninIncrease()
        {
            var yearsByDate = _applicationDataStore.Years.OrderByDescending(i => i.Date);

            yearsByDate[0].

            for (int i = 0; i < length; i++)
            {

            }
            


            //for (int i = 0; i < _applicationDataStore.Years.Count; i++)
            //{
            //    BestCostPerStaninIncrease
            //}
        }
    }
}
