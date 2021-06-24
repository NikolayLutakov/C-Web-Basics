using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.ViewModels.Issues
{
    public class IssueViewModel
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public string IsFixed { get; set; }
    }
}
