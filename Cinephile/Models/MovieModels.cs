using System.ComponentModel.DataAnnotations;

namespace Cinephile.Models
{
    public class MovieModels
    {
        [Required]
        public string Title
        {
            get;
            set;
        }
        [Required]
        public string Description
        {
            get;
            set;
        }
        [Required]
        public int Length
        {
            get;
            set;
        }
        [Required]
        public int Year
        {
            get;
            set;
        }
        [Required]
        public string Genre
        {
            get;
            set;
        }
        [Required]
        public bool HasSeen
        {
            get;
            set;
        }
        [Required]
        public bool IsFavourite
        {
            get;
            set;
        }
    }
}