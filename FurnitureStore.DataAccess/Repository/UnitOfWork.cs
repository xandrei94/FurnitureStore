using FurnitureStore.DataAccess.Data;
using FurnitureStore.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureStore.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ApplicationDbContext _db;
        public IProductRepository ProductRepository { get; private set; }
        public ICustomerRepository CustomerRepository { get; private set; }
        public IDiscountRepository DiscountRepository { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ProductRepository = new ProductRepository(_db);
            CustomerRepository = new CustomerRepository(_db);
            DiscountRepository = new DiscountRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void SaveChangesAsync()
        {
            _db.SaveChangesAsync();
        }
    }
}
