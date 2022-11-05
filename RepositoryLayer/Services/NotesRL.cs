using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Services.Account;
using RepositoryLayer.AddContext;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Account = CloudinaryDotNet.Account;

namespace RepositoryLayer.Services
{
    public class NotesRL : INotesRL
    {
        public readonly Context context;
        private readonly IConfiguration Config;

        public NotesRL(Context context, IConfiguration Config)
        {
            this.context = context;
            this.Config = Config;
        }
        public NotesEntity AddNote(NotesModel notes, long userid)
        {
            try
            {
                NotesEntity notesEntity = new NotesEntity();
                notesEntity.Title = notes.Title;
                notesEntity.Note = notes.Note;
                notesEntity.Color = notes.Color;
                notesEntity.Image = notes.Image;
                notesEntity.IsArchive = notes.IsArchive;
                notesEntity.IsPin = notes.IsPin;
                notesEntity.userid = userid;
                this.context.Notes.Add(notesEntity);
                int result = this.context.SaveChanges();
                if (result > 0)
                {
                    return notesEntity;
                }
                return null;

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
                var deleteNote = context.Notes.Where(x => x.NoteID == NoteId).FirstOrDefault();
                if (deleteNote != null)
                {
                    context.Notes.Remove(deleteNote);
                    context.SaveChanges();
                    return deleteNote;
                }

                return null;


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
                var Note = context.Notes.Where(x => x.userid == userId).FirstOrDefault();
                if (Note != null)
                {
                    return context.Notes.Where(list => list.userid == userId).ToList();
                }

                return null;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<NotesEntity> GetNote(long NoteId)
        {
            try
            {
                var Note = context.Notes.Where(x => x.NoteID == NoteId).FirstOrDefault();

                if (Note != null)
                {
                    return context.Notes.Where(list => list.NoteID == NoteId).ToList();
                }

                return null;

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
                var Note = context.Notes.FirstOrDefault();

                if (Note != null)
                {
                    return context.Notes.ToList();
                }

                return null;

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
                var update = context.Notes.Where(x => x.NoteID == NoteId).FirstOrDefault();
                if (update != null)
                {
                    update.Title = noteModel.Title;
                    update.Note = noteModel.Note;
                    update.IsArchive = noteModel.IsArchive;
                    update.Color = noteModel.Color;
                    update.Image = noteModel.Image;
                    update.IsPin = noteModel.IsPin;
                    update.IsTrash = noteModel.IsTrash;
                    context.Notes.Update(update);
                    context.SaveChanges();
                    return update;

                }


                return null;

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
                var result = context.Notes.Where(r => r.userid == userId && r.NoteID == NoteID).FirstOrDefault();

                result.IsPin = !result.IsPin;
                context.SaveChanges();
                return result.IsPin;

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
                var result = context.Notes.Where(r => r.userid == userId && r.NoteID == NoteID).FirstOrDefault();

                result.IsTrash = !result.IsTrash;
                context.SaveChanges();
                return result.IsTrash;

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
                var result = context.Notes.Where(r => r.userid == userId && r.NoteID == NoteID).FirstOrDefault();
                result.IsArchive = !result.IsArchive;
                context.SaveChanges();
                return result.IsArchive;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotesEntity ColorNote(long NoteId, string color)
        {
            var result = context.Notes.Where(r => r.NoteID == NoteId).FirstOrDefault();
            if (result != null)
            {

                result.Color = color;
                context.Notes.Update(result);
                context.SaveChanges();
                return result;

            }
            else
            {
                return null;
            }
        }
        public string Imaged(long NoteID, long userId, IFormFile image)
        {
            try
            {
                var result = context.Notes.Where(x => x.userid == userId && x.NoteID == NoteID).FirstOrDefault();
                if (result != null)
                {
                    Account account = new Account(
                        "dm60ejstf",        // CLOUD_NAME,API_KEY,API_SECRET
                         "265125861619672",
                         "f7jpRrkJC5eoNJROkI5LJM6yyUE"
                        );

                    Cloudinary cloudinary = new Cloudinary(account);
                    var uploadParameters = new ImageUploadParams()
                    {
                        File = new FileDescription(image.FileName, image.OpenReadStream()),
                    };
                    var uploadResult = cloudinary.Upload(uploadParameters);
                    string imagePath = uploadResult.Url.ToString();
                    result.Image = image.FileName;
                    // result.Image = imagePath;
                    context.SaveChanges();
                    return "Image Upload Successfully";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
    }
