using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.EntityLayer.Concrete
{
    public class FolderInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FolderID { get; set; }
        public string FolderName { get; set; }
        public string FolderPath { get; set; }
        public DateTime FolderCreatedDate { get; set; }
        public virtual ICollection<FileInfos>? Files { get; set; } 
        public int UserID { get; set; }
        public virtual UserInfo? User { get; set; }

        public int? ParentFolderID { get; set; }
    }
}
