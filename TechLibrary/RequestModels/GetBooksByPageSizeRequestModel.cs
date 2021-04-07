using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechLibrary.RequestModels
{
    public class GetBooksByPageSizeRequestModel
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public List<string> FilterOn { get; set; }
        public string FilterBy { get; set; }


        public GetBooksByPageSizeRequestModel()
        {
            FilterOn = new List<string>();
        }
        
    }
}
