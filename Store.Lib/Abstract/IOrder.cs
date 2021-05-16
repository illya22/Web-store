using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Lib.Entities;

namespace Store.Lib.Abstract
{
   public interface IOrder
    {
        void ProcessOrder(ShoppingBasket shoppingBasket, ShippingDetails shippingDetails);
    }
}
