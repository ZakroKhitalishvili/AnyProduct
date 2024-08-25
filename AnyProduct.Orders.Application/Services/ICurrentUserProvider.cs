using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyProduct.Orders.Application.Services;

public interface ICurrentUserProvider
{
    string UserId { get; }
}
