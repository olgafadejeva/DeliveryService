using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models
{
    public class DeliveryItemDimensions
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }

        public DeliveryItemDimensions(double height, double width, double length) {
            this.Height = height;
            this.Width = width;
            this.Length = length;
        }
    }
}
