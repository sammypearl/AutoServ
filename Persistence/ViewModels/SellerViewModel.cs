using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ViewModels
{
    public class SellerViewModel
    {
        public Seller Seller { get; set; }
        public IFormFile Photo { get; set; }
        public string ExistingPhotoPath { get; set; }
        public IEnumerable<Vehicle> Vehicles { get; set; }
        public IEnumerable<Make> Makes { get; set; }
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


    }
}
