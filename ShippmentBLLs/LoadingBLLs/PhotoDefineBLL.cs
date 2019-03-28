using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shippment.Models;
using System.Data;
using System.Data.Entity;
using Shippment.Common;


namespace Shippment.BLL
{
    public class PhotoDefineBLL
    {
        public ErrorInfo Add(PhotoDefine photoDefine)
        {
            ShippmentEntities db = new ShippmentEntities();
            var photo = (from p in db.PhotoDefines
                         where p.Name == photoDefine.Name
                         select p).ToList();
            if (photo.Count>0) return ErrorInfo.Err_Resource_Exist;
            db.PhotoDefines.Add(photoDefine);
            if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
            else return ErrorInfo.Err_Internal_Error;
        }
        public IQueryable<PhotoDefine> GetAll()
        {
            ShippmentEntities db = new ShippmentEntities();
            IQueryable<PhotoDefine> list = from p in db.PhotoDefines
                                        select p;
            return list;
        }
        public PhotoDefine Get(int id)
        {
            ShippmentEntities db = new ShippmentEntities();
            PhotoDefine photo = (from p in db.PhotoDefines
                            where p.Id == id 
                            select p).FirstOrDefault();
            return photo;
        }
        public ErrorInfo Edit(int id, PhotoDefine photoDefine)
        {

            if (id != photoDefine.Id)
            {
                return ErrorInfo.Err_Bad_Request_Information;
            }
            ShippmentEntities db = new ShippmentEntities();
            PhotoDefine photo = db.PhotoDefines.FirstOrDefault(p => p.Id == id);
            if (photo != null)
            {
                
                photo.BoardingPhotoes = photoDefine.BoardingPhotoes;
                photo.Height = photoDefine.Height;
                photo.Name = photoDefine.Name;
                photo.Width = photoDefine.Width;
                db.Entry(photo).State = EntityState.Modified;
                if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
            }
            return ErrorInfo.Err_Internal_Error;
        }
        public ErrorInfo Delete(int id)
        {
            ShippmentEntities db = new ShippmentEntities();
            PhotoDefine photo = db.PhotoDefines.FirstOrDefault(p => p.Id == id);
            if (photo != null)
            {
                db.Entry(photo).State = EntityState.Deleted;
                if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
            }
            return ErrorInfo.Err_Internal_Error;
        }
    }
}
