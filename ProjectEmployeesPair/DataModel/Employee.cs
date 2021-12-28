using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEmployeesPair.DataModel
{
    public class Employee
    {
        public Employee(int id, DateTime dateFrom, DateTime dateTo)
        {
            Id = id;
            DateFrom = dateFrom;
            DateTo = dateTo;
        }

        public int Id { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
