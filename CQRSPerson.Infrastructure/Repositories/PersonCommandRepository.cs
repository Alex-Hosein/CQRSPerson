using CQRSPerson.Domain.Entities;
using CQRSPerson.Domain.Repositories;
using System.Threading.Tasks;

namespace CQRSPerson.Infrastructure.Repositories
{
    public class PersonCommandRepository : IPersonCommandRepository
    {
        private DBContext _dbContext;
        public PersonCommandRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> InsertAsync(Person person)
        {
            await _dbContext.Person.AddAsync(person);
            await _dbContext.SaveChangesAsync();
            return person.PersonId;
        }
    }
}
