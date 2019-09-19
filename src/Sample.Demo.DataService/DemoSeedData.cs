using Microsoft.EntityFrameworkCore;
using Sample.Demo.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Demo.DataService
{
    public static class DemoSeedData
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserType>().HasData(new UserType
            {
               Id=1,
               Name="Employee"
            }, new UserType
            {
                Id = 2,
                Name = "Manager"
            },new UserType
            {
                Id = 3,
                Name = "HR"
            });
        }
    }
}
