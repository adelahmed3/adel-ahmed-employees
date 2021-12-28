using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEmployeesPair.DataModel
{
    public class ProjectPairedEmployees
    {
        public int ProjectId { get; set; }
        public int FirstEmpId { get; set; }
        public int SecondEmpId { get; set; }
        public int WorkingDays { get; set; }
        public string Description { get; set; }
    }
}
