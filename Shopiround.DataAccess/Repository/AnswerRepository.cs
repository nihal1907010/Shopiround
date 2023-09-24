using Shopiround.DataAccess.Data;
using Shopiround.DataAccess.Repository.IRepository;
using Shopiround.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopiround.DataAccess.Repository
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public AnswerRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
    }
}
