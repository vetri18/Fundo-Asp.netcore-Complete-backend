using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class CollaboratorEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CollaboratorID { get; set; }
        public string CollaboratedEmail { get; set; }

        [ForeignKey("note")]
        public long NoteID { get; set; }
        public virtual NotesEntity note { get; set; }

        [ForeignKey("user")]
        public long userid { get; set; }
        public virtual UserEntity user { get; set; }


    }


}

