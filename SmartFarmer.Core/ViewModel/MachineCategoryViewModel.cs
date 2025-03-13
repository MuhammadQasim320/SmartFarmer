using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Core.ViewModel
{
    public class MachineCategoryResponseViewModel
    {
        public Guid MachineCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
    }
    public class MachineCategoryRequestViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class SearchMachineCategoryRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchKey { get; set; }
    }

    public class MachineCategoryCountRequestViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<MachineCategoryResponseViewModel> List { get; set; }
    }

    public class MachineCategoryNameListViewModel
    {
        public List<MachineCategoryNameViewModel> List { get; set; }
    }
    public class MachineCategoryNameViewModel
    {
        public Guid MachineCategoryId { get; set; }
        public string MachineCategoryName { get; set; }
    }
}
