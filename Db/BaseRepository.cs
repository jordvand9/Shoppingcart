using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Eduportal.Models;
using Eduportal.Models.Security;

namespace Eduportal.Db
{
    public abstract class BaseRepository 
    {        
        public ApplicationDbContext ApplicationDbContext { get; set; }
        public UserManager<ApplicationUser> ApplicationUserManager  { get; set; }
        public RoleManager<ApplicationRole> ApplicationRoleManager  { get; set; }
        
        public BaseRepository() 
        {
        }
        public BaseRepository([FromServices]ApplicationDbContext applicationDbContext) 
        {
            ApplicationDbContext = applicationDbContext;
        }

        public BaseRepository([FromServices]ApplicationDbContext applicationDbContext, [FromServices]UserManager<ApplicationUser>  ApplicationUserManager, [FromServices]RoleManager<ApplicationRole>  ApplicationRoleManager) 
        {
            ApplicationDbContext = applicationDbContext;
        }
    }
}