using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.ViewModels.Issues
{
    public class IssueCarViewModel
    {
        public string Id { get; set; }

        public string Model { get; set; }

        public string Year { get; set; }

        public bool IsMechanic { get; set; }

        public ICollection<IssueViewModel> Issues { get; set; }
    }
}
