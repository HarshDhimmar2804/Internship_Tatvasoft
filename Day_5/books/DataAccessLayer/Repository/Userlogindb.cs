using DataAccessLayer.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class Userlogindb : DbContext
    {
       
         public Userlogindb(DbContextOptions<Userlogindb> options) : base(options)
            {
            }
        public DbSet<UserLogin> UserLogins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserLogin>().HasNoKey();
        }
    }
}
