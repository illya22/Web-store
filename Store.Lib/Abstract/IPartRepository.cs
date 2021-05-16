using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Lib.Entities;

namespace Store.Lib.Abstract
{
   public interface IPartRepository
    {
        IEnumerable<Part> Parts { get; }
        void SavePart(Part part);
        Part DeletePart(int part_id);
    }
}
