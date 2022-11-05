using BusinessLayer.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class LabelBL : ILabelBL
    {

        ILabelRL ilabelRL;

        public LabelBL(ILabelRL ilabelRL)
        {
            this.ilabelRL = ilabelRL;
        }



        public LabelResponseModel CreateLable(long notesId, long jwtUserId, LabelModel model)
        {

            try
            {

                return ilabelRL.CreateLable(notesId, jwtUserId, model);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public void DeleteLable(LabelEntity lable, long jwtUserId)
        {

            try
            {
                ilabelRL.DeleteLable(lable, jwtUserId);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IEnumerable<LabelEntity> GetAllLable(long jwtUserId)
        {

            try
            {

                return ilabelRL.GetAllLable(jwtUserId);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public LabelEntity GetLablesWithId(long lableId, long jwtUserId)
        {

            try
            {

                return ilabelRL.GetLablesWithId(lableId, jwtUserId);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public LabelResponseModel GetLableWithId(long lableId, long jwtUserId)
        {

            try
            {

                return ilabelRL.GetLableWithId(lableId, jwtUserId);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public LabelResponseModel UpdateLable(LabelEntity updateLable, UpdateLableModel model, long jwtUserId)
        {

            try
            {

                return ilabelRL.UpdateLable(updateLable, model, jwtUserId);

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
