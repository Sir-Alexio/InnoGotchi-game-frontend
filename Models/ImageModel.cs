using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InnoGotchi_frontend.Models
{
    public class ImageModel
    {
        public string ImagePath { get; set; }
        public bool IsSelected { get; set; } = false;
        public string Name { get; set; }
    }
}
