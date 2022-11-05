using CommonLayer.Models;
using RepositoryLayer.AddContext;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class CollaboratorRL : ICollaboratorRL
    {

        private readonly Context Context;

        public CollaboratorRL(Context Context)
        {
            this.Context = Context;
        }
        public CollabResponseModel AddCollaborate(long notesId, long jwtUserId, CollaboratedModel model)
        {
            try
            {
                var validNotesAndUser = this.Context.Users.Where(e => e.UserId == jwtUserId);
                CollaboratorEntity collaborate = new CollaboratorEntity();

                collaborate.NoteID = notesId;
                collaborate.userid = jwtUserId;
                collaborate.CollaboratedEmail = model.Collaborated_Email;

               Context.Add(collaborate);
                Context.SaveChanges();

                CollabResponseModel responseModel = new CollabResponseModel();

                responseModel.CollaboratorID = collaborate.CollaboratorID;
                responseModel.noteID = collaborate.NoteID;
                responseModel.UserId = collaborate.userid;
                responseModel.CollaboratedEmail = collaborate.CollaboratedEmail;

                return responseModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteCollab(CollaboratorEntity collab)
        {
            try
            {

                this.Context.CollaboratorTable.Remove(collab);
                this.Context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public CollaboratorEntity GetCollabWithId(long collabId)
        {
            try
            {
                var result = this.Context.CollaboratorTable.FirstOrDefault(e => e.CollaboratorID == collabId);

                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public IEnumerable<CollaboratorEntity> GetCollab(long userID)
        {
            try
            {
                var result = Context.CollaboratorTable.Where(x => x.userid == userID);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
