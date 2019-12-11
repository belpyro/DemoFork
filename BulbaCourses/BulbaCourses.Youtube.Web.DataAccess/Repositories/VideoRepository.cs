﻿using BulbaCourses.Youtube.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BulbaCourses.Youtube.Web.DataAccess.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        private YoutubeContext _db;
        private bool _isDisposed = false;

        public VideoRepository(YoutubeContext youtubeContext)
        {
            _db = youtubeContext;
        }
        //GetById
        public ResultVideoDb GetById(int? id)
        {
            return _db.Videos.Find(id);
        }
        //GetAll
        public IEnumerable<ResultVideoDb> GetAll()
        {
            return _db.Videos.ToList().AsReadOnly();
        }

        //Create
        public void Create(ResultVideoDb videoEntity)
        {
            _db.Videos.Add(videoEntity);
        }

        //Update
        public void Update(ResultVideoDb videoEntity)
        {
            _db.Entry(videoEntity).State = EntityState.Modified;
        }

        //Delete
        public void Delete(int? id)
        {
            var video = _db.Videos.Find(id);
            if (video != null)
                _db.Videos.Remove(video);
        }

        //Save
        public void Save()
        {
            _db.SaveChanges();
        }
        
        public virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            _isDisposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}