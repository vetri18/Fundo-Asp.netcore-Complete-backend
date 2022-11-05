using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface ILabelBL
    {

        public LabelResponseModel CreateLable(long notesId, long jwtUserId, LabelModel model);
        public IEnumerable<LabelEntity> GetAllLable(long jwtUserId);
        public LabelResponseModel GetLableWithId(long lableId, long jwtUserId);
        public LabelEntity GetLablesWithId(long lableId, long jwtUserId);
        public LabelResponseModel UpdateLable(LabelEntity updateLable, UpdateLableModel model, long jwtUserId);
        public void DeleteLable(LabelEntity lable, long jwtUserId);

    }
}
