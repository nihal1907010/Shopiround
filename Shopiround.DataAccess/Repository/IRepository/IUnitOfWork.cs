using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopiround.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IShopRepository ShopRepository { get; }
        void Save();
    }
}
