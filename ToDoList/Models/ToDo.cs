using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class ToDo
    {
        /// <summary>
        /// The unique Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Description of ToDo's 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Show if ToDo done or not
        /// </summary>
        public bool IsDone { get; set; }

        /// <summary>
        /// User identifier
        /// </summary>
        public virtual IdentityUser User { get; set; }
    }
}
