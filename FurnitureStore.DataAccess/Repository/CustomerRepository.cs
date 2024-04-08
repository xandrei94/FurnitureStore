using FurnitureStore.DataAccess.Data;
using FurnitureStore.DataAccess.Repository.IRepository;
using FurnitureStore.Models;


namespace FurnitureStore.DataAccess.Repository
{
    public class CustomerRepository : Repository<CustomerUser>, ICustomerRepository
    {
        public readonly ApplicationDbContext _db;
        public CustomerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public override void Update(CustomerUser obj)
        {
            var objFromDb = _db.Customers.FirstOrDefault(x => x.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name ?? objFromDb.Name;
                objFromDb.PhoneNumber = obj.PhoneNumber ?? objFromDb.PhoneNumber;
                objFromDb.Adress = obj.Adress ?? objFromDb.Adress;
                objFromDb.City = obj.City ?? objFromDb.City;
                objFromDb.Country = obj.Country ?? objFromDb.Country;
                objFromDb.DiscountId = obj.DiscountId;

                _db.Update(objFromDb);
            }
        }
    }
}
