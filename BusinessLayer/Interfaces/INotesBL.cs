using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface INotesBL
    {
        public NotesEntity AddNote(NotesModel note, long UserId);
        public NotesEntity DeleteNote(long NoteId);
        public List<NotesEntity> GetNote(long NoteId);
        public List<NotesEntity> GetNotebyUserId(long userId);
        public List<NotesEntity> GetAllNote();
        public NotesEntity UpdateNote(NotesModel noteModel, long NoteId);
        public bool Pinned(long NoteID, long userId);
        public bool Trashed(long NoteID, long userId);
        public bool Archieved(long NoteID, long userId);
        public NotesEntity ColorNote(long NoteId, string color);
        public string Imaged(long NoteID, long userId, IFormFile image);

    }
}
