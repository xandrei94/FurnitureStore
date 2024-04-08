using FurnitureStore.DataAccess.Data;
using FurnitureStore.DataAccess.Repository.IRepository;
using FurnitureStore.Models;

namespace FurnitureStore.DataAccess.Repository
{
    public class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        public readonly ApplicationDbContext _db;
        public DiscountRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public override void Update(Discount obj)
        {
            _db.Update(obj);
        }
    }
}
