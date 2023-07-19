using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.EntityLayer.Concrete
{
    public class UserInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [StringLength(50)]
        [Required]
        public string UserName { get; set; }

        [StringLength(50)]
        [Required]
        public string Password { get; set; }

        //public virtual ICollection<FolderInfo>? Folders { get; set; }

    }
}
