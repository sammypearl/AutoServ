
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ViewModels
{
    public class VehicleViewModel
    {
        public IFormFile Photo { get; set; }
        public string ExistingPhotoPath { get; set; }
        public Vehicle Vehicle { get; set; }
        public IEnumerable<Make> Makes { get; set; }
        public IEnumerable<Seller> Sellers { get; set; }
        public IEnumerable<Model> Models { get; set; }
        public IEnumerable<SelectListItem> CSelectListItem<T>(IEnumerable<T> Items)
        {
            List<SelectListItem> List = new List<SelectListItem>();
            SelectListItem sli = new SelectListItem
            {
                Text = "---Select---",
                Value = "0"
            };
            List.Add(sli);
            foreach (var item in Items)
            {
                sli = new SelectListItem
                {
                    Text = item.GetType().GetProperty("Name").GetValue(item, null).ToString(),
                    Value = item.GetType().GetProperty("Id").GetValue(item, null).ToString()
                };
                List.Add(sli);
            }
            return List;
        }

        public IEnumerable<Currency> Currencies { get; set; }

        private List<Currency> CList = new List<Currency>();
        private List<Currency> CreateList()
        {
            CList.Add(new Currency("USD", "USD"));
            CList.Add(new Currency("NGN", "NGN"));
            CList.Add(new Currency("CAD", "CAD"));
            CList.Add(new Currency("EUR", "EUR"));
            return CList;
        }

        public VehicleViewModel()
        {
            Currencies = CreateList();
        }

    }


    public class Currency
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public Currency(String id, String value)
        {
            Id = id;
            Name = value;
        }
    }
}
