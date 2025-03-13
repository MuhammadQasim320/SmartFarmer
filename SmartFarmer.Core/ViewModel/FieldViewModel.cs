using System.ComponentModel.DataAnnotations;

namespace SmartFarmer.Core.ViewModel
{
    public class FieldResponseViewModel : FieldRequestViewModel
    {
        public Guid FieldId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string FarmName { get; set; }
    }

    public class FieldRequestViewModel
    {
        [MaxLength(1000)]
        public string Name { get; set; }
    }

    public class SearchFieldRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchKey { get; set; }
    }

    public class FieldCountRequestViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<FieldDetailViewModel> List { get; set; }
    }



    public class FieldNameSearchRequestViewModel
    {
        public string SearchKey { get; set; }
    }


    public class FieldNameViewModel
    {
        public Guid FieldId { get; set; }
        public string Name { get; set; }
    }

    public class FieldNameListViewModel
    {
        public List<FieldNameViewModel> List { get; set; }
    }


    public class FieldCenterViewModel
    {
        public Guid FieldId { get; set; }
        public CenterViewModel Center { get; set; }
    }

    public class CenterViewModel
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }


    public class FieldBoundaryViewModel
    {
        public Guid FieldId { get; set; }
        public List<BoundaryViewModel> Boundaries { get; set; }
    }

    public class BoundaryViewModel
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }



    public class FieldDetailViewModel 
    {
        public Guid FieldId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string FarmName { get; set; }
        public string Name { get; set; }
        public List<BoundaryViewModel> Boundaries { get; set; }
        public CenterViewModel Center { get; set; }

    }

}
