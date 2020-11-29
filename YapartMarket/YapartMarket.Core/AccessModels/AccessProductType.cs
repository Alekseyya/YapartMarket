using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YapartMarket.Core.AccessModels
{
    public class AccessProductType
    {
        [Key]
        public int Tip { get; set; }
        public List<AccessProduct> AccessProducts { get; set; }
        public string Kategoria { get; set; }
        public string Opisanie { get; set; }
        public int KodRazd { get; set; }
        public int Nomer_rekl { get; set; }
        public int Nomer_spec { get; set; }
        public int Roditel { get; set; }
        /// <summary>
        /// МАТ,ТОВ,ПРД
        /// </summary>
        public string Vid_Tov { get; set; }
        public int Kf_zarpl1 { get; set; }
        public int Kf_zarpl2 { get; set; }
        public int Nacenka_gr { get; set; }
        public string Izgotovitel { get; set; }
        public string Strana { get; set; }
    }
}
