using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("tb_t_Role")]
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        [JsonIgnore]
        public virtual ICollection<Account> Account { get; set; }
    }
    
}
