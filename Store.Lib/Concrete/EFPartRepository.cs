using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Lib.Entities;
using Store.Lib.Abstract;
 

namespace Store.Lib.Concrete
{
   public class EFPartRepository : IPartRepository 
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Part> Parts
        {
            get { return context.Parts; }
        }

        public void SavePart(Part part)
        {
            if(part.Part_Id == 0)
            {
                context.Parts.Add(part);
            }
            else
            {
                Part dbEntry = context.Parts.Find(part.Part_Id);
                if(dbEntry != null)
                {
                    dbEntry.Name = part.Name;
                    dbEntry.Description = part.Description;
                    dbEntry.Type = part.Type;
                    dbEntry.Bran_Car = part.Bran_Car;
                    dbEntry.Price = part.Price;
                    dbEntry.ImageData = part.ImageData;
                    dbEntry.ImageType = part.ImageType;
                }
            }
            context.SaveChanges();
        }

        public Part DeletePart(int part_id)
        {
            Part dbEntry = context.Parts.Find(part_id);
            if(dbEntry != null)
            {
                context.Parts.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;

        }
    }

    
}
