using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YapartMarket.Core.Models;

namespace YapartMarket.MainApp.ViewModels
{
    public class CarViewModel
    {
        public ICollection<Mark> Marks { get; set; }
        public ICollection<Model> Models { get; set; }
        public ICollection<Modification> Modifications { get; set; }
    }
}
