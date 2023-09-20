using Shopiround.DataAccess.Data;
using Shopiround.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopiround.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext applicationDbContext;
        public IShopRepository ShopRepository { get; private set; }
        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
            ShopRepository = new ShopRepository(applicationDbContext);
        }

        public void Save()
        {
            applicationDbContext.SaveChanges();
        }
    }
}
