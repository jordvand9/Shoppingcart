using System;
using System.ComponentModel.DataAnnotations;
using Eduportal.Models;
 
namespace Eduportal.Models
{
    public class Cart 
    {
        public string   CartId      { get; set; }
        public int      RecordId    { get; set; }
        public int      AlbumId     { get; set; }
        public int      Count       { get; set; }
        public virtual Album Album  { get; set; }
    }
}