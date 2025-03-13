using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Core.ViewModel
{
    public class HazardKeyViewModel
    {

    }


    public class HazardKeyResponseViewModel: HazardKeyRequestViewModel
    {
        public Guid HazardKeyId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string HazartType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class HazardKeyRequestViewModel
    {

        public string Name { get; set; }
        public string Color { get; set; }
        public int HazardTypeId { get; set; }
    }




    public class SearchHazardKeyRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchKey { get; set; }
        public int? HazardTypeId { get; set; }
    }

    public class HazardKeyCountRequestViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<HazardKeyResponseViewModel> List { get; set; }
    }



    public class HazardKeyFieldResponseViewModel: HazardKeyFieldViewModel
    {
        public Guid HazardKeyFieldMappingId { get; set; }
        public DateTime CreatedDate { get; set; }

    }
    
    public class HazardKeyFieldViewModel
    {
        public Guid FieldId { get; set; }
        public Guid HazardKeyId { get; set; }
        public List<LocationViewModel> Locations { get; set; }
    }

    public class LocationViewModel
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
    
    public class HazardKeyNameSearchRequestViewModel
    {
        public string SearchKey { get; set; }
    }


    public class HazardKeyNameViewModel
    {
        public Guid HazardKeyId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int HazardTypeId { get; set; }
        public string HazardType { get; set; }
    }

    public class HazardKeyNameListViewModel
    {
        public List<HazardKeyNameViewModel> List { get; set; }
    }

    public class FieldHazardListViewModel
    {
        public IEnumerable<FieldHazardViewModel> List { get; set; }
    }
    public class FieldHazardViewModel
    {
        public Guid HazardKeyId { get; set; }
        public Guid HazardKeyFieldMappingId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Color { get; set; }
        public int HazardTypeId { get; set; }
        public string HazartType { get; set; }
        public List<LocationViewModel>? Locations { get; set; }
    }



    public class HazardFieldResponseViewModel : HazardKeyLocationListViewModel
    {
        public Guid HazardKeyFieldMappingId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid FieldId { get; set; }

    }
    public class HazardKeyLocationListViewModel
    {
        public List<HazardFieldViewModel> List { get; set; }
    }
   
    public class HazardFieldViewModel
    {
        public Guid HazardKeyId { get; set; }
        public List<LocationViewModel> Locations { get; set; }
    }


}
