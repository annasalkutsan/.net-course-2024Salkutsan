using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    /// <summary>
    /// Структура "Валюта"
    /// </summary>
    public struct Currency
    {
        /// <summary>
        /// Код согласно международному стандарту
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// Полное название
        /// </summary>
        public string Name { get; set; }

        public Currency(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
