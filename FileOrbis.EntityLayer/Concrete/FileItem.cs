using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.EntityLayer.Concrete
{
    public class FileItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsFolder { get; set; }
        public DateTime CreatedAt { get; set; }
        public long SizeInBytes { get; set; }

        public int? ParentFolderId { get; set; }
        public FileItem ParentFolder { get; set; }

        public int UserID { get; set; }
        public UserInfo User { get; set; }
    }
}
