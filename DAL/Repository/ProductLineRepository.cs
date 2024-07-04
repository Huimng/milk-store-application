using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    
    public class ProductLineRepository 
    {
        public BSADBContext dbContext = new BSADBContext();

        public void CreateProductLine(ProductLine productLine)
        {
            dbContext.ProductLines.Add(productLine);
            dbContext.SaveChanges();
        }

        public List<ProductLine> GetAllProductLine()
        {
            return dbContext.ProductLines.Include(p => p.Product).ToList();
        }

        public ProductLine? GetProductLineById(int id)
        {
            var x = dbContext.ProductLines.FirstOrDefault(x => x.ProductLineId == id);
            return x;
        }
        public void DeleteProductLine(int id)
        {
            var x = GetProductLineById(id);
            dbContext.ProductLines.Remove(x);
            dbContext.SaveChanges();
        }

        public void UpdateProductLine(ProductLine productLine)
        {
            var trackedTag = dbContext.ChangeTracker.Entries<ProductLine>()
                                  .FirstOrDefault(e => e.Entity.ProductLineId == productLine.ProductLineId);
            if (trackedTag != null)
            {
                dbContext.Entry(trackedTag.Entity).State = EntityState.Detached;
            }

            // Cập nhật thực thể
            dbContext.ProductLines.Update(productLine);
            dbContext.SaveChanges();
        }

    }
}
