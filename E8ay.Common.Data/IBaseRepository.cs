using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace E8ay.Common.Data
{
    public interface IBaseRepository<T>
    {
        T GetById(string id);

        IEnumerable<T> GetAll();

        Task Create(T data);

        Task Update(T data);

        Task Delete(string id);

        Task DeleteAll();
    }
}
