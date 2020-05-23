using CQRSPerson.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CQRSPerson.Domain.Repositories
{
    public interface IPersonCommandRepository
    {
        Task<int> InsertAsync(Person person);
    }
}
