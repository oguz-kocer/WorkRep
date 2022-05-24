using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework
{
	public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
		where TEntity : class, IEntity, new()//TEntity class olmalı , IEntity türünde olmalı , newlenebilmeli.
		where TContext : DbContext, new() //TContext DbContext türünde olmalı , newlenebilmeli 

	{
		public void Add(TEntity entity)
		{
			using (var context = new TContext())//using içindeki context garbage collector yardımıyla belleği hızlıca temizler.Performans için yazdık.
			{
				var addedEntity = context.Entry(entity);
				addedEntity.State = EntityState.Added;//Ekleme işlemi yapılacağını bildirdik. 
				context.SaveChanges();
			}
		}

		public void Delete(TEntity entity)
		{
			using (var context = new TContext())
			{
				var deletedEntity = context.Entry(entity);
				deletedEntity.State = EntityState.Deleted;
				context.SaveChanges();
			}
		}

		public TEntity Get(Expression<Func<TEntity, bool>> filter = null)
		{
			using (var context = new TContext())
			{
				return context.Set<TEntity>().SingleOrDefault(filter);
			}
		}

		public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
		{
			using (var context = new TContext())
			{
				return filter == null
					? context.Set<TEntity>().ToList()
					: context.Set<TEntity>().Where(filter).ToList();
			}
		}

		public void Update(TEntity entity)
		{
			using (var context = new TContext())
			{
				var updatedEntity = context.Entry(entity);
				updatedEntity.State = EntityState.Modified;
				context.SaveChanges();
			}
		}
	}
}
