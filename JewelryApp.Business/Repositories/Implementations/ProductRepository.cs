using JewelryApp.Data;
using JewelryApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryApp.Business.Repositories.Implementations;

public class ProductRepository : RepositoryBase<Product>
{
    public ProductRepository(AppDbContext context) : base(context)
    {

    }
}
