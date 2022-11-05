using BusinessLayer.Interfaces;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class NotesBL : INotesBL
    {
        INotesRL notesrl;
        public NotesBL(INotesRL NotesRL)
        {
            this.notesrl = NotesRL;


        }

        public NotesEntity AddNote(NotesModel note, long UserId)
        {
            try
            {
                return this.notesrl.AddNote(note, UserId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public NotesEntity DeleteNote(long NoteId)
        {
            try
            {
                return notesrl.DeleteNote(NoteId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<NotesEntity> GetNote(long NotesId)
        {
            try
            {
                return notesrl.GetNote(NotesId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<NotesEntity> GetNotebyUserId(long userId)
        {
            try
            {
                return notesrl.GetNotebyUserId(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<NotesEntity> GetAllNote()
        {
            try
            {
                return notesrl.GetAllNote();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotesEntity UpdateNote(NotesModel noteModel, long NoteId)
        {
            try
            {
                return notesrl.UpdateNote(noteModel, NoteId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool Pinned(long NoteID, long userId)
        {
            try
            {
                return notesrl.Pinned(NoteID, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool Trashed(long NoteID, long userId)
        {
            try
            {
                return notesrl.Trashed(NoteID, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool Archieved(long NoteID, long userId)
        {
            try
            {
                return notesrl.Archieved(NoteID, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NotesEntity ColorNote(long NoteId, string color)
        {
            try
            {
                return notesrl.ColorNote(NoteId, color);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string Imaged(long NoteID, long userId, IFormFile image)
        {
            try
            {
                return notesrl.Imaged(NoteID, userId, image);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
