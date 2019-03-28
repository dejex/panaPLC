using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shippment.Models;
using System.Data;
using System.Data.Entity;

namespace Shippment.BLL
{
    public class BoardingPhotoBLL
    {
        public bool Add(BoardingPhoto obj)
        {
            ShippmentEntities db = new ShippmentEntities();
            db.BoardingPhotoes.Add(obj);
            return db.SaveChanges() > 0;
        }
        public IQueryable<BoardingPhoto> GetAllPhotos()
        {
            ShippmentEntities db = new ShippmentEntities();
            IQueryable<BoardingPhoto> list = from p in db.BoardingPhotoes
                                             select p;
            return list;
        }
        public BoardingPhoto GetPhoto(int id)
        {
            ShippmentEntities db = new ShippmentEntities();
            BoardingPhoto photo = (from p in db.BoardingPhotoes
                                   where p.Id == id
                                   select p).FirstOrDefault();
            return photo;
        }

        public bool EditBoardingPhoto(int id, BoardingPhoto obj)
        {
            if (id != obj.Id) return false;
            ShippmentEntities db = new ShippmentEntities();
            BoardingPhoto photo = db.BoardingPhotoes.FirstOrDefault(p => p.Id == id);
            if (photo != null)
            {
                
                photo.FullPath = obj.FullPath;                
                photo.RegTime = obj.RegTime;
                //photo.PhotoDefine = obj.PhotoDefine;//常规修改不该修改导航属性吧？
                //photo.Carriage = obj.Carriage;//常规修改不该修改导航属性吧？
                db.Entry(photo).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
            return false;
        }
        public bool DeletePhoto(int id)
        {
            ShippmentEntities db = new ShippmentEntities();
            BoardingPhoto photo = db.BoardingPhotoes.FirstOrDefault(p => p.Id == id);
            if (photo != null)
            {
                db.Entry(photo).State = EntityState.Deleted;
                return db.SaveChanges() > 0;
            }
            else
            {
                return false;
            }
        }
    }
}
