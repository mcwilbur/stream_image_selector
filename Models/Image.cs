using System.Collections.Generic;
namespace TCGStreamHelper.Models
{
    public class ImageVM
    {
        public string filename {get; set;}
        public int index {get; set;}
    }

    public class ImageSetVM
    {
        public string name {get; set;}
        public List<string> images {get; set;}
    }
}