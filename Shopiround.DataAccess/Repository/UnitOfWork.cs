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
        public IProductRepository ProductRepository { get; private set; }

        public IReviewRepository ReviewRepository { get; private set; }

        public IQuestionRepository QuestionRepository { get; private set; }

        public IAnswerRepository AnswerRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
            ShopRepository = new ShopRepository(applicationDbContext);
            ProductRepository = new ProductRepository(applicationDbContext);
            ReviewRepository = new ReviewRepository(applicationDbContext);
            QuestionRepository = new QuestionRepository(applicationDbContext);
            AnswerRepository = new AnswerRepository(applicationDbContext);
        }

        public void Save()
        {
            applicationDbContext.SaveChanges();
        }
    }
}
