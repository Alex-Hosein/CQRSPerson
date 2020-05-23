using CQRSPerson.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CQRSPerson.Domain.Repositories
{
    public interface IPersonQueryRepository
    {
        Task<List<Person>> GetAllAsync();
    }
}
