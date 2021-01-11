using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System.Linq;

namespace BulkyBook.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company company)
        {
            var companyFromDb = _db.Companies.FirstOrDefault(x => x.Id == company.Id);
            if (companyFromDb != null)
            {
                companyFromDb.Name = company.Name;
                companyFromDb.StreetAddress = company.StreetAddress;
                companyFromDb.City = company.City;
                companyFromDb.PostalCode = company.PostalCode;
                companyFromDb.State = company.State;
                companyFromDb.PhoneNumber = company.PhoneNumber;
                companyFromDb.IsAuthorizedCompany = company.IsAuthorizedCompany;
            }
        }
    }
}
