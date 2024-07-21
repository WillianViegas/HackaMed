using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.DatabaseConfig
{
    public class DatabaseConfig : IDatabaseConfig
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public string CollectionName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
