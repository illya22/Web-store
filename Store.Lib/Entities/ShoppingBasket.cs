using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Lib.Entities
{
    public class ShoppingBasket
    {
        private List<Basket_Line> basket_lines = new List<Basket_Line>();

        public void Add_Item(Part part, int quantity)
        {
            Basket_Line line = basket_lines
                .Where(p => p.Part.Part_Id == part.Part_Id)
                .FirstOrDefault();

            if(line == null)
            {
                basket_lines.Add(new Basket_Line
                {
                    Part = part,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void Remove_Item(Part part)
        {
            basket_lines.RemoveAll(l => l.Part.Part_Id == part.Part_Id);
        }

        public void Clear()
        {
            basket_lines.Clear();
        }
        
        public decimal Total_Value()
        {
            return basket_lines.Sum(s => s.Part.Price * s.Quantity);
        }

        public IEnumerable<Basket_Line> Lines
        {
            get { return basket_lines; }
        }
    }

    public class Basket_Line
    {
        public Part Part { get; set; }
        public int Quantity { get; set; }
    }
}
