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
    public class LabelRL : ILabelRL
    {

        Context Context;

        public LabelRL(Context Context)
        {
            this.Context = Context;
        }




        public LabelResponseModel CreateLable(long notesId, long jwtUserId, LabelModel model)
        {
            try
            {
                var validNotesAndUser = this.Context.Users.Where(e => e.UserId == jwtUserId);

                if (validNotesAndUser != null)
                {
                    LabelEntity label = new LabelEntity();

                    label.noteID = notesId;
                    label.UserId = jwtUserId;
                    label.LabelName = model.LabelName;

                    this.Context.Add(label);
                    this.Context.SaveChanges();

                    LabelResponseModel responseModel = new LabelResponseModel();

                    responseModel.LabelID = label.LabelID;
                    responseModel.NoteID = label.noteID;
                    responseModel.UserID = label.UserId;
                    responseModel.LabelName = label.LabelName;

                    return responseModel;

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



        public IEnumerable<LabelEntity> GetAllLable(long jwtUserId)
        {
            try
            {

                var result = this.Context.lableTable.Where(x => x.UserId == jwtUserId);
                return result;

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public LabelResponseModel GetLableWithId(long lableId, long jwtUserId)
        {
            try
            {
                var validUserId = this.Context.Users.Where(e => e.UserId == jwtUserId);

                var response = this.Context.lableTable.FirstOrDefault(e => e.LabelID == lableId && e.UserId == jwtUserId);

                if (validUserId != null && response != null)
                {


                    LabelResponseModel model = new LabelResponseModel();

                    model.LabelID = response.LabelID;
                    model.NoteID = response.noteID;
                    model.UserID = response.UserId;
                    model.LabelName = response.LabelName;

                    return model;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public LabelEntity GetLablesWithId(long lableId, long jwtUserId)
        {
            try
            {
                var validUserId = this.Context.Users.Where(e => e.UserId == jwtUserId);
                if (validUserId != null)
                {
                    return this.Context.lableTable.FirstOrDefault(e => e.LabelID == lableId);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public LabelResponseModel UpdateLable(LabelEntity updateLable, UpdateLableModel model, long jwtUserId)
        {
            try
            {
                var validUserId = this.Context.Users.Where(e => e.UserId == jwtUserId);

                var response = this.Context.lableTable.FirstOrDefault(e => e.LabelID == updateLable.LabelID);

                if (validUserId != null && response != null)
                {
                    updateLable.LabelName = model.LabelName;
                    updateLable.noteID = model.NoteID;

                    this.Context.SaveChanges();


                    LabelResponseModel models = new LabelResponseModel();

                    models.LabelID = response.LabelID;
                    models.NoteID = response.noteID;
                    models.UserID = response.UserId;
                    models.LabelName = response.LabelName;

                    return models;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public void DeleteLable(LabelEntity lable, long jwtUserId)
        {
            try
            {
                var validUserId = this.Context.Users.Where(e => e.UserId == jwtUserId);
                if (validUserId != null)
                {
                    this.Context.lableTable.Remove(lable);
                    this.Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
