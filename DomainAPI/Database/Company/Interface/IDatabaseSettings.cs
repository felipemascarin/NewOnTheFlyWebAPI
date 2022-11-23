using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainAPI.Database.Company.Interface
{
    public interface IDatabaseSettings
    {
        public string CompanyCollectionName { get; set; }
        public string RestrictedCollectionName { get; set; }
        public string DeadfileCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
