using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechLibrary.RequestModels
{
    public class UpdateBookRequestModel
    {
        public string ShortDescription { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Title { get; set; }
        public int BookId { get; set; }
    }
}
