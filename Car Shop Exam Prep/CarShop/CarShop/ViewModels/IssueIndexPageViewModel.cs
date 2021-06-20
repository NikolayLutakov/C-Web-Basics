using System.Collections.Generic;

namespace CarShop.ViewModels
{
    public class IssueIndexPageViewModel
    {
        public string IssueId { get; set; }

        public string CarId { get; set; }

        public string Model { get; set; }

        public string Year { get; set; }

        public string UserId { get; set; }

        public bool IsMechanic { get; set; }

        public ICollection<IssueViewModel> Issues { get; set; }
    }
}
