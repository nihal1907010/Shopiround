﻿using Shopiround.DataAccess.Data;
using Shopiround.DataAccess.Repository.IRepository;
using Shopiround.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopiround.DataAccess.Repository
{
    public class ShopRepository : Repository<Shop>, IShopRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public ShopRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
    }
}
