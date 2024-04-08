using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureStore.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        IDiscountRepository DiscountRepository { get; }
        void Save();
        void SaveChangesAsync();
    }
}
