using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.EntityLayer.Concrete
{
    public class FileInfos
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public double FileSize { get; set; }
        public DateTime FileCreationDate { get; set; }
        public int FolderID { get; set; }
        public virtual FolderInfo? Folder { get; set; }  
    }
}
