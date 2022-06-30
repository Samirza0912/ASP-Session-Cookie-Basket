using Friello.Models;
using System;
using System.Collections.Generic;

namespace Friello.ViewModels
{
    public class HomeVM
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public List<Slider> Sliders { get; set; }
        public SliderContent SliderContents { get; set; }
    }
}
