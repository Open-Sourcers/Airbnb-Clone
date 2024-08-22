using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Airbnb.Domain
{
    public class BaseEntity<T>
    {
       public T Id { get; set; }
    }
}
