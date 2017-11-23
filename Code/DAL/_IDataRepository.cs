using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neolab.Code.DAL
{
    public interface IDataRepository<T>
        where T:class
    {
        T Select(int id);
        List<T> SelectList();
        T Insert(T dataObj);
        T Update(T dataObj);
        void Delete(int id);
    }
}