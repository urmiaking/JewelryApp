using JewelryApp.Business.Repositories.Interfaces;
using JewelryApp.Data;
using JewelryApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryApp.Business.Repositories.Implementations;

public class InvoiceRepository : RepositoryBase<Invoice>
{
    public InvoiceRepository(AppDbContext context) : base(context)
    {

    }
}
