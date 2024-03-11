using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT_company.Model
{
    public class Position
    {
        public int PositionId { get; set; }
        public string Title { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
